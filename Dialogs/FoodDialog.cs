using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Dialogs
{
    public class FoodDialog : InterDialog
    {
        public FoodDialog(string dialogId) : base(dialogId)
        {
        }

        //private static FoodDetails foodDetails;

        public async override Task<DialogTurnResult> BeginDialogAsync2(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "IntroStep";

            await dc.Context.SendActivityAsync("Welcome to Pizza maker");
            return await this.ContinueDialogAsync(dc, cancellationToken);
        }

        public async Task<DialogTurnResult> SizeStep(DialogContext dc, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "SizeStep";

            var options = new string[] { "Small", "Medium", "Big", }.ToList();
            var sizeOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("What size would you like"),
                Choices = ChoiceFactory.ToChoices(options),
            };
            return await dc.PromptAsync(nameof(ChoicePrompt), sizeOptions, cancellationToken);
        }

        public async Task<DialogTurnResult> SauceStep(DialogContext dc, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "SauceStep";

            var sauceMessage = MessageFactory.Text("What sauce would you like", "What sauce would you like", InputHints.ExpectingInput);
            return await dc.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = sauceMessage }, cancellationToken);
        }

        public async Task<DialogTurnResult> CardSauce(DialogContext dc, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "SauceConfirmation";

            dc.ActiveDialog.State["Intent"] = "Food";
            dc.ActiveDialog.State["IntentAction"] = "SelectSauce";

            var reply = MessageFactory.Attachment(new List<Attachment>());
            reply.Attachments.Add(Cards.GetThumbnailCard(dc.ActiveDialog.State).ToAttachment());
            await dc.Context.SendActivityAsync(reply);

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public async Task<DialogTurnResult> CheeseStep(DialogContext dc, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "CheeseStep";

            var cheeseMessage = MessageFactory.Text("What cheese would you like", "What cheese would you like", InputHints.ExpectingInput);
            return await dc.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = cheeseMessage }, cancellationToken);
        }

        public async Task<DialogTurnResult> ToppingStep(DialogContext dc, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "ToppingStep";

            var toppingMessage = MessageFactory.Text("What topping would you like", "What topping would you like", InputHints.ExpectingInput);
            return await dc.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = toppingMessage }, cancellationToken);
        }

        public async Task<DialogTurnResult> ConfirmationStep(DialogContext dc, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "ConfirmationStep";
            dc.ActiveDialog.State["Intent"] = "Food";
            dc.ActiveDialog.State["IntentAction"] = "Confirm";

            var reply = MessageFactory.Attachment(new List<Attachment>());
            reply.Attachments.Add(Cards.GetThumbnailCard(dc.ActiveDialog.State).ToAttachment());
            await dc.Context.SendActivityAsync(reply);

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public async Task<DialogTurnResult> CardConfirmed(DialogContext dc)
        {
            dc.ActiveDialog.State["stepID"] = "FinalStep";

            if (dc.Context.Activity.Value == null)
            {
                await dc.Context.SendActivityAsync("Order canceled");
                return await dc.EndDialogAsync("Order canceled");
            }
            else
            {
                await dc.Context.SendActivityAsync("Order confirmed");
                return await dc.EndDialogAsync("Order confirmed");
            }
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(DialogContext dc, CancellationToken cancellationToken = default)
        {
            switch ((string)dc.ActiveDialog.State["stepID"])
            {
                case "IntroStep":
                    return await SizeStep(dc, cancellationToken);
                case "SizeStep":
                    return await SauceStep(dc, cancellationToken);
                case "SauceStep":
                    return await CardSauce(dc, cancellationToken);
                case "SauceConfirmation":
                    return await CheeseStep(dc, cancellationToken);
                case "CheeseStep":
                    return await ToppingStep(dc, cancellationToken);
                case "ToppingStep":
                    return await ConfirmationStep(dc, cancellationToken);
                case "ConfirmationStep":
                    return await CardConfirmed(dc);
            }
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ResumeDialogAsync(DialogContext dc, DialogReason reason, object result = null, CancellationToken cancellationToken = default)
        {
            string nextStep = (string)dc.ActiveDialog.State["stepID"];

            dc.ActiveDialog.State[nextStep] = dc.Context.Activity.Text;
            return await ContinueDialogAsync(dc, cancellationToken);
        }

        [IntentActionAtribute(IntentNames.FOOD, IntentActions.FoodActions.Confirm)]
        public async Task JsonConfirmFlight(DialogContext dc)
        {
            var msg = dc.Context.Activity.Value.ToString();
            var parse = JsonConvert.DeserializeObject<FoodDetails>(msg);

            await CardConfirmed(dc);
        }

        [IntentActionAtribute(IntentNames.FOOD, IntentActions.FoodActions.SelectSauce)]
        public async Task JsonSelectSauce(DialogContext dc)
        {
            var msg = dc.Context.Activity.Value.ToString();
            var parse = JsonConvert.DeserializeObject<FoodDetails>(msg);

            dc.ActiveDialog.State[nameof(SizeStep)] = parse.SizeStep;
            dc.ActiveDialog.State[nameof(SauceStep)] = parse.SauceStep;

            await CheeseStep(dc);
        }

        [IntentActionAtribute(IntentNames.FOOD, IntentActions.FoodActions.Cancel)]
        public async Task JsonCancelFlight(DialogContext dc)
        {
            var msg = dc.Context.Activity.Value.ToString();
            var parse = JsonConvert.DeserializeObject<FoodDetails>(msg);

            dc.Context.Activity.Value = null;

            await CardConfirmed(dc);
        }
    }
}
