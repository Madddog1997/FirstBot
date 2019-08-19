using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Middleware
{
    public class CancelAndHelp : IMiddleware
    {
        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default)
        {
            string res = turnContext.Activity.Text;

            if (res != null)
            {
                if (res.ToLower().Contains("cancel"))
                {
                    //turnContext.
                    await turnContext.SendActivityAsync("Canceling Dialog");
                    //turnContext.Activity.Value = new Details(null, Intent);
                }
                else if (res.ToLower().Contains("help"))
                {
                    await turnContext.SendActivityAsync("Help Dialog");
                    new DialogTurnResult(DialogTurnStatus.Waiting);
                    return;
                }
            }

            await next(cancellationToken);
        }
    }
}
