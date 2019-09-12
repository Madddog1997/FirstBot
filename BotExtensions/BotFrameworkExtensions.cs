using Newtonsoft.Json.Linq;
using System;

namespace FirstBot
{
    public static class BotFrameworkExtensions
    {
        public static string GetIntentFromMessageValue(this object value)
        {
            if (value == null)
                return null;

            return Convert.ToString((JObject.Parse(value.ToString()))?.GetValue("Intent"));
        }

        public static string GetIntentActionFromMessageValue(this object value)
        {
            if (value == null)
                return null;

            return Convert.ToString((JObject.Parse(value.ToString()))?.GetValue("IntentAction"));
        }
    }
}
