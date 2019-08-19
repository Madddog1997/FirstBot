using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FirstBot
{
    public class Cards
    {
        public static ThumbnailCard GetThumbnailCard(object obj)
        {
            string output = JsonConvert.SerializeObject(obj);

            if (output.ToLower().Contains("flight"))
            {
                FlightDetails details = JsonConvert.DeserializeObject<FlightDetails>(output);

                var card = new ThumbnailCard
                {
                    Title = "Confirm Your Order",
                    Subtitle = $"Here is your {details.Intent} : ",
                    Text = $"Origin : {details.OriginStep}, Destination : {details.DestinationStep}, Time : {details.DateStep}",
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.MessageBack, "Yes", value: output), new CardAction(ActionTypes.MessageBack, "No", value: new Details("Cancel", details.Intent)) },
                };
                return card;
            }
            else
            {
                FoodDetails details = JsonConvert.DeserializeObject<FoodDetails>(output);

                var card = new ThumbnailCard
                {
                    Title = "Confirm Your Order",
                    Subtitle = $"Here is your {details.Intent} : ",
                    Text = $"Size : {details.SizeStep}, Sauce : {details.SauceStep}, Cheese : {details.CheeseStep}, Topping : {details.ToppingStep}",
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.MessageBack, "Yes", value: output), new CardAction(ActionTypes.MessageBack, "No", value: new Details("Cancel", details.Intent)) },
                };
                return card;
            }
        }
    }

    public class FlightDetails : Details
    {
        public string StepID { get; set; }

        public string OriginStep { get; set; }
        public string DestinationStep { get; set; }
        public string DateStep { get; set; }
    }

    public class Details
    {
        public Details(string intentAction = null, string intent = null)
        {
            IntentAction = intentAction;
            Intent = intent;
        }

        public string IntentAction { get; set; }
        public string Intent { get; set; }
    }

    public class FoodDetails : Details
    //{"stepID":"ConfirmationStep","Order":"Pizza order","SizeStep":"Medium","SauceStep":"tomato","CheeseStep":"eidam","ToppingStep":"salami"}
    {
        public string StepID { get; set; }

        public string SizeStep { get; set; }
        public string SauceStep { get; set; }
        public string CheeseStep { get; set; }
        public string ToppingStep { get; set; }
    }
}
