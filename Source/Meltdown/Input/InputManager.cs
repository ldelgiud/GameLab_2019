using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Meltdown.Utilities;

namespace Meltdown.Input
{
    interface IInputEvent { }

    struct PressEvent : IInputEvent {
        public override string ToString()
        {
            return String.Format("PressEvent {{ }}");
        }
    }

    struct HoldEvent : IInputEvent
    {
        public float start;
        public float duration;

        public HoldEvent(float start, float duration)
        {
            this.start = start;
            this.duration = duration;
        }

        public override string ToString()
        {
            return String.Format("HoldEvent {{ start: {0}, duration: {1} }}", this.start, this.duration);
        }
    }

    struct ReleaseEvent : IInputEvent {
        public override string ToString()
        {
            return String.Format("ReleaseEvent {{ }}");
        }
    }

    struct ValueEvent<T> : IInputEvent
    {
        public T old;
        public T current;

        public ValueEvent(T old, T current)
        {
            this.old = old;
            this.current = current;
        }

        public override string ToString()
        {
            return String.Format("ValueEvent {{ old: {0}, current: {1}, }}", this.old, this.current);
        }
    }

    interface IInputState<T, S>
        where S: struct
    {
        void Update(Time time, T type, ref S state, Dictionary<T, IInputEvent> events);
    }

    class BooleanInputState<T> : IInputState<T, bool>
    {
        static readonly float PRESS_THRESHOLD = 0.25f;

        Nullable<float> timestamp;

        public void Update(Time time, T type, ref bool active, Dictionary<T, IInputEvent> events)
        {
            if (this.timestamp.HasValue)
            {
                // Is active
                if (active)
                {
                    // Still active
                    if (time.Absolute - this.timestamp >= PRESS_THRESHOLD)
                    {
                        // Above press threshold, is hold
                        events[type] = new HoldEvent(this.timestamp.Value, time.Absolute);
                    }
                }
                else
                {
                    // No longer active
                    if (time.Absolute - this.timestamp >= PRESS_THRESHOLD)
                    {
                        // Above press threshold, is hold
                        events[type] = new ReleaseEvent();
                    }
                    else
                    {
                        events[type] = new PressEvent();
                    }

                    this.timestamp = null;
                }
            }
            else
            {
                // Not active
                if (active)
                {
                    
                    // Become active
                    this.timestamp = time.Absolute;
                }
            }
        }
    }

    class ValueInputState<T, V> : IInputState<T, V>
        where V: struct
    {
        V value;

        public void Update(Time time, T type, ref V value, Dictionary<T, IInputEvent> events)
        {
            events[type] = new ValueEvent<V>(this.value, value);
            this.value = value;
        }
    }

    enum ThumbSticks
    {
        Left,
        Right
    }

    enum Triggers
    {
        Left,
        Right
    }

    class InputManager
    {

        // States
        Dictionary<Keys, BooleanInputState<Keys>> keyboardStates = new Dictionary<Keys, BooleanInputState<Keys>>();
        Dictionary<Buttons, BooleanInputState<Buttons>>[] buttonStates = new Dictionary<Buttons, BooleanInputState<Buttons>>[GamePad.MaximumGamePadCount];
        Dictionary<ThumbSticks, ValueInputState<ThumbSticks, Vector2>>[] thumbStickStates = new Dictionary<ThumbSticks, ValueInputState<ThumbSticks, Vector2>>[GamePad.MaximumGamePadCount];
        Dictionary<Triggers, ValueInputState<Triggers, float>>[] triggerStates = new Dictionary<Triggers, ValueInputState<Triggers, float>>[GamePad.MaximumGamePadCount];

        // Events
        public Dictionary<Keys, IInputEvent> keyboardEvents = new Dictionary<Keys, IInputEvent>();
        Dictionary<Buttons, IInputEvent>[] buttonEvents = new Dictionary<Buttons, IInputEvent>[GamePad.MaximumGamePadCount];
        Dictionary<ThumbSticks, IInputEvent>[] thumbStickEvents = new Dictionary<ThumbSticks, IInputEvent>[GamePad.MaximumGamePadCount];
        Dictionary<Triggers, IInputEvent>[] triggerEvents = new Dictionary<Triggers, IInputEvent>[GamePad.MaximumGamePadCount];

        public InputManager()
        {
            for (uint i = 0; i < GamePad.MaximumGamePadCount; ++i)
            {
                this.buttonStates[i] = new Dictionary<Buttons, BooleanInputState<Buttons>>();
                this.thumbStickStates[i] = new Dictionary<ThumbSticks, ValueInputState<ThumbSticks, Vector2>>();
                this.triggerStates[i] = new Dictionary<Triggers, ValueInputState<Triggers, float>>();

                this.buttonEvents[i] = new Dictionary<Buttons, IInputEvent>();
                this.thumbStickEvents[i] = new Dictionary<ThumbSticks, IInputEvent>();
                this.triggerEvents[i] = new Dictionary<Triggers, IInputEvent>();
            }
        }

        public void Register(Keys key)
        {
            this.keyboardStates[key] = new BooleanInputState<Keys>();
        }

        public void Register(Buttons button)
        {
            foreach (var buttonStates in this.buttonStates)
            {
                buttonStates[button] = new BooleanInputState<Buttons>();
            }
        }

        public void Register(ThumbSticks thumbStick)
        {
            foreach (var thumbStickStates in this.thumbStickStates)
            {
                thumbStickStates[thumbStick] = new ValueInputState<ThumbSticks, Vector2>();
            }
        }

        public void Register(Triggers trigger)
        {
            foreach (var triggerStates in this.triggerStates)
            {
                triggerStates[trigger] = new ValueInputState<Triggers, float>();
            }
        }

        public IInputEvent GetEvent(Keys key)
        {
            return this.keyboardEvents.TryGetValue(key, out IInputEvent _event) ? _event : null;
        }

        public IInputEvent GetEvent(int index, Buttons button)
        {
            return this.buttonEvents[index].TryGetValue(button, out IInputEvent _event) ? _event : null;
        }

        public IInputEvent GetEvent(int index, ThumbSticks thumbStick)
        {
            return this.thumbStickEvents[index].TryGetValue(thumbStick, out IInputEvent _event) ? _event : null;
        }

        public IInputEvent GetEvent(int index, Triggers trigger)
        {
            return this.triggerEvents[index].TryGetValue(trigger, out IInputEvent _event) ? _event : null;
        }

        public void RemoveEvent(Keys key)
        {
            this.keyboardEvents.Remove(key);
        }

        public void RemoveEvent(int index, Buttons button)
        {
            this.buttonEvents[index].Remove(button);
        }

        public void RemoveEvent(int index, ThumbSticks thumbStick)
        {
            this.thumbStickEvents[index].Remove(thumbStick);
        }

        public void RemoveEvent(int index, Triggers trigger)
        {
            this.triggerEvents[index].Remove(trigger);
        }

        public void Update(Time time)
        {
            // Keyboard events
            {
                this.keyboardEvents.Clear();
                KeyboardState state = Keyboard.GetState();
                foreach (var entry in this.keyboardStates)
                {
                    bool active = state.IsKeyDown(entry.Key);
                    entry.Value.Update(time, entry.Key, ref active, this.keyboardEvents);
                }
            }

            // Controller events
            {
                for (int i = 0; i < GamePad.MaximumGamePadCount; ++i)
                {
                    this.buttonEvents[i].Clear();
                    this.thumbStickEvents[i].Clear();
                    this.triggerEvents[i].Clear();

                    GamePadState state = GamePad.GetState(i);

                    // Skip if controller is not connected
                    if (!state.IsConnected)
                    {
                        continue;
                    }

                    foreach (var entry in this.buttonStates[i])
                    {
                        bool active = state.IsButtonDown(entry.Key);
                        entry.Value.Update(time, entry.Key, ref active, this.buttonEvents[i]);
                    }

                    foreach (var entry in this.thumbStickStates[i])
                    {
                        Vector2 value;
                        switch (entry.Key)
                        {
                            case ThumbSticks.Left:
                                value = state.ThumbSticks.Left;
                                break;
                            case ThumbSticks.Right:
                                value = state.ThumbSticks.Right;
                                break;
                            default:
                                throw new Exception("Not possible");
                        }
                        entry.Value.Update(time, entry.Key, ref value, this.thumbStickEvents[i]);

                    }

                    foreach (var entry in this.triggerStates[i])
                    {
                        float value;
                        switch (entry.Key)
                        {
                            case Triggers.Left:
                                value = state.Triggers.Left;
                                break;
                            case Triggers.Right:
                                value = state.Triggers.Right;
                                break;
                            default:
                                throw new Exception("Not possible");
                        }
                        entry.Value.Update(time, entry.Key, ref value, this.triggerEvents[i]);
                    }
                }
                
            }
            
        }
    }
}
