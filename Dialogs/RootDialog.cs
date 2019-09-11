using FirstBot.Dialogs.DialogRework;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Dialogs
{
    public class RootDialog : LuisDialog
    {
        public RootDialog(LuisRecognizer recognizer) : base(recognizer, nameof(RootDialog))
        {
        }
        
        public override async Task<DialogTurnResult> ResumeDialogAsync(DialogContext dc, DialogReason reason, object result = null, CancellationToken cancellationToken = default)
        {
            await dc.Context.SendActivityAsync("End");
            return await dc.EndDialogAsync("End root");
        }

        [DialogNameAtribute(DialogNames.FLIGHT)]
        public async Task<DialogTurnResult> LuisCallFlight(DialogContext dc, RecognizerResult result)
        {
            return await dc.BeginDialogAsync(nameof(FlightNewDialog));
        }

        [DialogNameAtribute(DialogNames.FOOD)]
        public async Task<DialogTurnResult> LuisCallFood(DialogContext dc, RecognizerResult result)
        {
            return await dc.BeginDialogAsync(nameof(FoodNewDialog));
        }
        [DialogNameAtribute(DialogNames.NONE)]
        public async Task<DialogTurnResult> LuisCallNone(DialogContext dc, RecognizerResult result)
        {
            await dc.Context.SendActivityAsync("Type som normal shit");
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }
    }
}
