using FirstBot.Dialogs;
using FirstBot.Dialogs.DialogRework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBot
{
    public static class ConstantTable
    {

    }

    public static class DialogNames
    {
        public const string FOOD = nameof(FoodNewDialog);
        public const string FLIGHT = nameof(FlightNewDialog);
        public const string NONE = nameof(RootDialog);
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
