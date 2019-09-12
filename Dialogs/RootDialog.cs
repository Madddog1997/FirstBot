using FirstBot.Dialogs.DialogRework;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace FirstBot.Dialogs
{
    public class RootDialog : LuisDialog
    {
        public RootDialog(LuisRecognizer recognizer) : base(recognizer, nameof(RootDialog))
        {
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
