﻿using Microsoft.Bot.Builder;
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
    public delegate Task JsonCommandHandler(DialogContext context);
    public static class ActivityExtensions
    {
        public static async Task<bool> HandleValueRedirection<T>(this DialogContext context, T instanceReference, IMessageActivity optionalMessage = null) where T : class
        {
            var messageActivity = optionalMessage ?? context.Context.Activity.AsMessageActivity();
            var intentName = messageActivity?.Value.GetIntentFromMessageValue();
            var intentAction = messageActivity?.Value.GetIntentActionFromMessageValue();

            if (!string.IsNullOrEmpty(intentName) && !string.IsNullOrEmpty(intentAction))
            {
                var comparisonAttribute = new IntentActionAtribute(intentName, intentAction);
                var methods = instanceReference.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(m => m.GetCustomAttributes()
                    .OfType<IntentActionAtribute>()
                    .Any(n => n.Equals(comparisonAttribute)));
                var method = methods.FirstOrDefault();

                if(method == null)
                {
                    return false;
                }

                var handler = (JsonCommandHandler)Delegate.CreateDelegate(typeof(JsonCommandHandler), instanceReference, method, throwOnBindFailure: false);

                if(handler != null)
                {
                    await handler(context);
                }
                return true;
            }
            return false;
        }
    }
}