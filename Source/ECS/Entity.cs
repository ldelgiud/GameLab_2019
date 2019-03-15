using System;

namespace ECS
{

    /// <summary>
    /// The <c>Entity</c> struct acts as a handle to some entry in the <see cref="Context" />.
    /// </summary>
    public struct Entity
    {
        public const Int32 MAX_INDEX = 1 << Entity.INDEX_BITS;

        const Int32 INDEX_BITS = 24;
        const Int32 INDEX_OFFSET = 0;
        const UInt32 INDEX_MASK = (UInt32)((1 << Entity.INDEX_BITS) - 1) << Entity.INDEX_OFFSET;

        const Int32 GENERATION_BITS = 8;
        const Int32 GENERATION_OFFSET = 24;
        const UInt32 GENERATION_MASK = (UInt32)((1 << Entity.GENERATION_BITS) - 1) << Entity.GENERATION_OFFSET;

        /// <summary>
        /// Bitfield for the <see cref="Entity.Generation" /> and <see cref="Entity.Index" />. First 8 bits contain the <see cref="Entity.Generation" />. Following 24 bits represent the <see cref="Entity.Index" />.
        /// </summary>
        private UInt32 bitfield;

        /// <summary>
        /// The <c>Generation</c> is used to differentiate entities with the same <see cref="Entity.Index">. This is needed when one entity is deleted and another is placed at the same index, since the <see cref="Context"> will reuse indices to save space.
        /// </summary>
        internal UInt32 Generation
        {
            get
            {
                return (UInt32)(this.bitfield & Entity.GENERATION_MASK);
            }

            set
            {
                this.bitfield &= ~Entity.GENERATION_MASK;
                this.bitfield |= ((value % (1 << Entity.GENERATION_BITS)) << Entity.GENERATION_OFFSET);
            }
        }

        /// <summary>
        /// The <c>Index</c> is used to indicate where an entity's data is stored inside the <see cref="Context" />.
        /// </summary>
        internal UInt32 Index
        {
            get
            {
                return (UInt32)(this.bitfield & Entity.INDEX_MASK);
            }

            set
            {
                this.bitfield &= ~Entity.INDEX_MASK;
                this.bitfield |= ((value % (1 << Entity.INDEX_BITS)) << Entity.INDEX_OFFSET);
            }
        }

        public override String ToString() 
        {
            return String.Format("Entity {{ generation: {0}, index: {1} }}", this.Generation, this.Index);
        }
    }
}