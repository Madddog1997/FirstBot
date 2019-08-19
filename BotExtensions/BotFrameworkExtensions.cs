using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
