using Microsoft.Bot.Builder.Dialogs;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.BotExtensions
{ 
    public static class ContextExtensions
    {
        public delegate Task<DialogTurnResult> DialogCommandHandler(DialogContext context, object options = null, CancellationToken cancellationToken = default);

        public static DialogTurnResult Wait(this DialogContext dc, DialogCommandHandler method)
        {
            MethodInfo mi = method.Method;
            dc.ActiveDialog.State["Handler"] = mi.Name;
                       
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public static async Task<DialogTurnResult> Wait(this DialogContext dc, DialogCommandHandler method, string dialogName)
        {
            MethodInfo mi = method.Method;
            dc.ActiveDialog.State["Handler"] = mi.Name;

            await dc.BeginDialogAsync(dialogName);
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }
    }
}
