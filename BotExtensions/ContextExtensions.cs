using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace FirstBot.BotExtensions
{ 
    public static class ContextExtensions
    {
        public delegate Task<DialogTurnResult> DialogCommandHandler(DialogContext context, object options = null);

        public static DialogTurnResult Wait(this DialogContext dc, string methodName)
        {
            dc.ActiveDialog.State["Handler"]  = methodName;
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public static async Task<DialogTurnResult> Wait(this DialogContext dc, string methodName, string dialogName)
        {
            dc.ActiveDialog.State["Handler"] = methodName;
            await dc.BeginDialogAsync(dialogName);
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }
    }
}
