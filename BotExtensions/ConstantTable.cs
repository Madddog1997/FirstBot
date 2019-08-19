using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBot
{
    public static class ConstantTable
    {

    }

    public static class IntentNames
    {
        public const string FOOD = "Food";
        public const string FLIGHT = "Flight";
        public const string NONE = "Root";
        public const string CANCEL = "Cancel";
    }

    public static class IntentActions
    {
        public static class FoodActions
        {
            public const string SelectSauce = "SelectSauce";
            public const string Confirm = "Confirm";
            public const string Cancel = "Cancel";
        }

        public static class FlightActions
        {
            public const string SelectDestination = "SelectDestination";
            public const string Confirm = "Confirm";
            public const string Cancel = "Cancel";
        }
    }
}
