using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Middlewares
{
    public class Logger : ITranscriptLogger
    {
        public Task LogActivityAsync(IActivity activity)
        {
            if (activity.Type == ActivityTypes.Message)
                Console.WriteLine(activity.AsMessageActivity().Text);
            return Task.CompletedTask;
        }      
    }
}
