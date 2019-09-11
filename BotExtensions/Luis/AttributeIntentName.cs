using System;

namespace FirstBot
{
    [Serializable]
    public abstract class AttributeIntentName : Attribute, IEquatable<AttributeIntentName>
    {
        protected abstract string dialogName { get; }

        public override string ToString()
        {
            return $"{GetType().Name}({dialogName})";
        }

        bool IEquatable<AttributeIntentName>.Equals(AttributeIntentName other)
        {
            return other != null
                && string.Equals(dialogName, other.dialogName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object other)
        {
            var casted = other as AttributeIntentName;
            return casted != null
                && string.Equals(dialogName, casted.dialogName, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return dialogName.GetHashCode();
        }
    }
}