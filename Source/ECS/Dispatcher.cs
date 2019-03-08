using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
    /// <summary>
    /// Builder class for the dispatcher.
    /// </summary>
    public class DispatcherBuilder
    {
        /// <summary>
        /// Node class for dependency resolution
        /// </summary>
        struct Node
        {
            /// <summary>
            /// Longest dependency chain for this node.
            /// </summary>
            public uint depth;

            /// <summary>
            /// System associated with the node.
            /// </summary>
            public SystemBase value;

            /// <summary>
            /// Set of systems that depend on this system
            /// </summary>
            public HashSet<Node> children;

            public Node(SystemBase value)
            {
                this.depth = 1;
                this.value = value;
                this.children = new HashSet<Node>();

            }

            /// <summary>
            /// Add a node to this node as a child
            /// </summary>
            /// <param name="child">Node to add</param>
            public void Add(Node child)
            {
                this.children.Add(child);
            }

            /// <summary>
            /// Check if the graph is cyclic
            /// </summary>
            /// <returns>Whether the graph is cyclic</returns>
            public bool IsCyclic()
            {
                return this.IsCyclic(new List<Node>());
            }

            /// <summary>
            /// Check if the graph is cyclic
            /// </summary>
            /// <param name="visited">List of nodes that were visited</param>
            /// <returns>Whether the graph is cyclic</returns>
            private bool IsCyclic(List<Node> visited)
            {
                // Check if this node has already been visited
                if (visited.Contains(this))
                {
                    return true;
                }
                else
                {
                    // Add this node to visited nodes
                    visited.Add(this);

                    // Check all children
                    foreach (var child in this.children)
                    {
                        if (child.IsCyclic(visited))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            /// <summary>
            /// Mark this node and its children with the appropriate depth
            /// </summary>
            /// <returns></returns>
            public uint Mark()
            {
                return this.Mark(0);
            }

            /// <summary>
            /// Mark this node and its children with the appropriate depth
            /// </summary>
            /// <param name="depth">Depth of the previous node</param>
            /// <returns></returns>
            private uint Mark(uint depth)
            {
                uint max = this.depth;
                this.depth = Math.Max(this.depth, depth + 1);
                foreach (var child in this.children)
                {
                    // Mark children and get max depth
                    max = Math.Max(max, child.Mark(this.depth));
                }

                return max;
            }
        }

        /// <summary>
        /// Nodes in the dependency graph, indexed by the system's type
        /// </summary>
        Dictionary<Type, Node> nodes = new Dictionary<Type, Node>();

        /// <summary>
        /// Edges in the dependency graph, indexed by the system's type
        /// </summary>
        Dictionary<Type, IEnumerable<Type>> edges = new Dictionary<Type, IEnumerable<Type>>();

        HashSet<Type> actionTypes = new HashSet<Type>();

        public DispatcherBuilder Add(SystemBase system, IEnumerable<Type> dependencies)
        {
            this.nodes.Add(system.GetType(), new Node(system));
            this.edges.Add(system.GetType(), dependencies);
            this.actionTypes.UnionWith(system.GetActionTypes());

            return this;
        }

        /// <summary>
        /// Finalize the dispatcher from the builder by resolving dependencies.
        /// </summary>
        /// <returns>A finalized dispatcher</returns>
        public Dispatcher Build()
        {
            // Construct graph
            foreach (var kv in this.edges)
            {
                var type = kv.Key;
                var enumerable = kv.Value;
                foreach(var dependency in enumerable)
                {
                    // Check if dependencies exist in graph
                    if (!this.nodes.Keys.Contains(dependency))
                    {
                        throw new Exception("Listed dependency that wasn't part of this dispatcher!");
                    }
                    this.nodes[type].Add(this.nodes[dependency]);
                }
            }

            // Assert that graph is acyclic and mark nodes
            uint depth = 0;
            foreach (var node in this.nodes.Values)
            {
                if (node.IsCyclic())
                {
                    throw new Exception("Dispatcher contains cyclic dependencies!");
                }

                // Mark nodes and get max depth
                depth = Math.Max(depth, node.Mark());
            }


            var temp = new List<SystemBase>[depth];
            var stages = new SystemBase[depth][];

            // Initialize lists
            for (uint stage = 0; stage < depth; ++stage)
            {
                temp[stage] = new List<SystemBase>();
            }

            // Fill in lists from nodes
            foreach (var node in this.nodes.Values)
            {
                // Convert depth to index
                temp[node.depth - 1].Add(node.value);
            }

            // Flatten lists into arrays
            for (uint stage = 0; stage < depth; ++stage)
            {
                stages[stage] = temp[stage].ToArray();
            }

            return new Dispatcher(stages, this.actionTypes);
        }
    }

    /// <summary>
    /// Class to store and dispatch systems.
    /// </summary>
    public class Dispatcher
    {

        public static DispatcherBuilder Builder()
        {
            return new DispatcherBuilder();
        }

        ActionStore actionStore;
        readonly SystemBase[][] stages;

        internal Dispatcher(SystemBase[][] stages, IEnumerable<Type> actionTypes)
        {
            this.stages = stages;
            this.actionStore = new ActionStore(actionTypes);
        }

        
        /// <summary>
        /// Run all stages on the context and apply actions after each stage.
        /// </summary>
        /// <param name="ctx">Context</param>
        public void Tick(Context ctx)
        {
            foreach (var stage in this.stages)
            {
                stage.AsParallel().ForAll(system => system.Tick(this.actionStore, ctx));
                this.actionStore.Apply(ctx);
            }
        }

    }
}