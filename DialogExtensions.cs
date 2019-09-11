using FirstBot.Dialogs;
using FirstBot.Dialogs.DialogRework;
using FirstBot.State;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot
{
    public static class Conversation
    {
        public static async Task SendAsync(this Dialog dialog, ITurnContext turnContext, IStatePropertyAccessor<DialogState> accessor, CancellationToken cancellationToken = default)
        {
            DialogSet dialogSet = new DialogSet(accessor);
            dialogSet.Add(dialog);

            //Dialogs
            dialogSet.Add(new FoodDialog());
            dialogSet.Add(new FlightDialog());
            dialogSet.Add(new TestDialog());
            dialogSet.Add(new FuckDialog());

            ////Test Dialogs
            dialogSet.Add(new FlightNewDialog());
            dialogSet.Add(new FoodNewDialog());

            //Prompts
            dialogSet.Add(new TextPrompt(nameof(TextPrompt)));
            dialogSet.Add(new ChoicePrompt(nameof(ChoicePrompt)));

            DialogContext dialogContext = await dialogSet.CreateContextAsync(turnContext, cancellationToken);

            try
            {
                #region Cancel And Help
                if (turnContext.Activity.Text != null)
                {
                    string res = turnContext.Activity.Text;

                    if (res.ToLower().Contains("cancel"))
                    {
                        await dialogContext.CancelAllDialogsAsync();
                        await turnContext.SendActivityAsync("Cancel Dialog");
                        return;
                    }
                    else if (res.ToLower().Contains("help"))
                    {
                        await turnContext.SendActivityAsync("Help Dialog");
                    }
                }
                #endregion

                #region Dispatch
                if (turnContext.Activity.Text == null && turnContext.Activity.Value != null)
                {
                    await dialogContext.CancelAllDialogsAsync();
                    var intent = turnContext.Activity.Value.GetIntentFromMessageValue();

                    await dialogContext.BeginDialogAsync(intent);
                    return;
                    
                }
                #endregion

                DialogTurnResult results = await dialogContext.ContinueDialogAsync(cancellationToken);

                if (results.Status == DialogTurnStatus.Empty)
                {
                    await dialogContext.BeginDialogAsync(dialog.Id, null, cancellationToken);
                }

            }
            catch (Exception ex)
            {
                await turnContext.SendActivityAsync($"Pohubilo SE: {ex.ToString()}");
            }
        }
    }
}