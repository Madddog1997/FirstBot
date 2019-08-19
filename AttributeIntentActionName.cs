using System;

namespace FirstBot
{
    [Serializable]
    public abstract class AttributeIntentActionName : Attribute, IEquatable<AttributeIntentActionName>
    {
        protected abstract string IntentAction { get; }
        protected abstract string IntentName { get; }

        public override string ToString()
        {
            return $"{GetType().Name}({IntentName})({IntentAction})";
        }

        bool IEquatable<AttributeIntentActionName>.Equals(AttributeIntentActionName other)
        {
            return other != null
                && string.Equals(IntentName, other.IntentName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(IntentAction, other.IntentAction, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object other)
        {
            var casted = other as AttributeIntentActionName;
            return casted != null
                && string.Equals(IntentName, casted.IntentName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(IntentAction, casted.IntentAction, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (IntentAction + IntentName).GetHashCode();
        }
    }
}