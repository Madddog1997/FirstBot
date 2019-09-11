using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FirstBot
{
    public delegate Task<DialogTurnResult> LuisCommandHandler(DialogContext context, RecognizerResult result);
    public static class LuisExtensions
    {
        public static async Task<DialogTurnResult> LuisValueRedirection<T>(this DialogContext context, T instanceReference, RecognizerResult result) where T : class
        {
            var dialogName = result.GetTopScoringIntent().intent;
            
            if (!string.IsNullOrEmpty(dialogName))
            {
                var comparisonAttribute = new DialogNameAtribute(dialogName);
                var methods = instanceReference.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(m => m.GetCustomAttributes()
                    .OfType<DialogNameAtribute>()
                    .Any(n => n.Equals(comparisonAttribute)));

                if(methods.Count() > 1)
                {
                    throw new Exception("Multiple methods found");
                }
                
                var method = methods.FirstOrDefault();

                var handler = (LuisCommandHandler)Delegate.CreateDelegate(typeof(LuisCommandHandler), instanceReference, method, throwOnBindFailure: false);

                if (handler == null)
                {
                    throw new Exception("No method Found");
                }

                if (handler != null)
                {
                    return await handler(context, result);
                }
            }
            throw new Exception("Intent handler Not Found");
        }
    }
}
