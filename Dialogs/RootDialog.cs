using FirstBot.Dialogs;
using FirstBot.State;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Dialogs
{
    public class RootDialog : Dialog
    {
        private readonly LuisRecognizer _recognizer;

        public RootDialog(string dialogId, LuisRecognizer recognizer) : base(dialogId)
        {
            _recognizer = recognizer;
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            await dc.Context.SendActivityAsync("Food or Flight?");
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(DialogContext dc, CancellationToken cancellationToken = default)
        {
            RecognizerResult result = await _recognizer.RecognizeAsync(dc.Context, cancellationToken);
            string intent = result.GetTopScoringIntent().intent;

            return await dc.BeginDialogAsync(nameof(TestDialog));

            //if(intent == "Food")
            //{
            //    return await dc.BeginDialogAsync(nameof(FoodDialog));
            //}
            //else if (intent == "Flight")
            //{
            //    return await dc.BeginDialogAsync(nameof(FlightDialog));
            //}
            //else
            //{
            //    await dc.Context.SendActivityAsync("I don't understand");
            //    return new DialogTurnResult(DialogTurnStatus.Waiting);
            //}
        }

        public override async Task<DialogTurnResult> ResumeDialogAsync(DialogContext dc, DialogReason reason, object result = null, CancellationToken cancellationToken = default)
        {
            if (result != null)
            {
                return await BeginDialogAsync(dc);
            }
            else
            {
                await dc.Context.SendActivityAsync("End");
                return await dc.EndDialogAsync("End root");
            }
        }
    }
}
