using FirstBot.Dialogs;
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
        internal static Dictionary<string, Func<DialogContext, Task>> dispatchDict;

        public static async Task SendAsync(this Dialog dialog, ITurnContext turnContext, IStatePropertyAccessor<DialogState> accessor, CancellationToken cancellationToken = default)
        {
            DialogSet dialogSet = new DialogSet(accessor);
            dialogSet.Add(dialog);

            //Dialogs
            dialogSet.Add(new FoodDialog(nameof(FoodDialog)));
            dialogSet.Add(new FlightDialog(nameof(FlightDialog)));
            dialogSet.Add(new TestDialog(nameof(TestDialog)));
            dialogSet.Add(new FuckDialog(nameof(FuckDialog)));
            
            //Prompts
            dialogSet.Add(new TextPrompt(nameof(TextPrompt)));
            dialogSet.Add(new ChoicePrompt(nameof(ChoicePrompt)));

            DialogContext dialogContext = await dialogSet.CreateContextAsync(turnContext, cancellationToken);

            InitState();
           
            try
            {
                #region Cancel And Help
                if (dialogContext.Context.Activity.Text != null)
                {
                    string res = dialogContext.Context.Activity.Text;

                    if (res.ToLower().Contains("cancel"))
                    {
                        await dialogContext.Context.SendActivityAsync("Cancel Dialog");
                        await dialogContext.CancelAllDialogsAsync();
                        return;
                    }
                    else if (res.ToLower().Contains("help"))
                    {
                        await dialogContext.Context.SendActivityAsync("Help Dialog");
                        new DialogTurnResult(DialogTurnStatus.Waiting);
                    }
                }
                #endregion

                #region Dispatch
                if (turnContext.Activity.Text == null && turnContext.Activity.Value != null)
                {
                    await dialogContext.CancelAllDialogsAsync();
                    var intent = turnContext.Activity.Value.GetIntentFromMessageValue();

                    if (dispatchDict.ContainsKey(intent))
                    {
                        await dispatchDict[intent](dialogContext);
                    }
                    else
                    {
                        await turnContext.SendActivityAsync("I don't understand");
                    }
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

        private static void InitState()
        {
            if (dispatchDict == null)
            {
                dispatchDict = new Dictionary<string, Func<DialogContext, Task>>();

                Func<DialogContext, Task<DialogTurnResult>> foodDialogFunc = async (context) =>
                {
                    return await context.BeginDialogAsync(nameof(FoodDialog));
                };
                dispatchDict.Add(IntentNames.FOOD, foodDialogFunc);

                Func<DialogContext, Task<DialogTurnResult>> flightDialogFunc = async (context) =>
                {
                    return await context.BeginDialogAsync(nameof(FlightDialog));
                };
                dispatchDict.Add(IntentNames.FLIGHT, flightDialogFunc);
            }
        }
    }
}