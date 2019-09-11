using FirstBot.BotExtensions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Dialogs.DialogRework
{
    public class FoodNewDialog : InterDialog
    {
        public FoodNewDialog() : base(nameof(FoodNewDialog))
        {
        }

        public async override Task<DialogTurnResult> BeginDialogAsync2(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "SizeStep";

            await dc.Context.SendActivityAsync("Welcome to Pizza maker");
            await dc.Context.SendActivityAsync("What size would you like?");
            return dc.Wait(SauceStep);
        }

        public async Task<DialogTurnResult> SauceStep(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "SauceStep";

            await dc.Context.SendActivityAsync("What sauce would you like?");
            return dc.Wait(CardSauce);
        }

        public async Task<DialogTurnResult> CardSauce(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "";
            dc.ActiveDialog.State["Intent"] = nameof(FoodNewDialog);
            dc.ActiveDialog.State["IntentAction"] = "SelectSauce";

            var reply = MessageFactory.Attachment(new List<Attachment>());
            reply.Attachments.Add(Cards.GetThumbnailCard(dc.ActiveDialog.State).ToAttachment());
            await dc.Context.SendActivityAsync(reply);

            return dc.Wait(CheeseStep);
        }

        public async Task<DialogTurnResult> CheeseStep(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "CheeseStep";

            await dc.Context.SendActivityAsync("What cheese would you like?");
            return dc.Wait(ToppingStep);
        }

        public async Task<DialogTurnResult> ToppingStep(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "ToppingStep";

            await dc.Context.SendActivityAsync("What topping would you like?");
            return dc.Wait(ConfirmationStep);
        }

        public async Task<DialogTurnResult> ConfirmationStep(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "";
            dc.ActiveDialog.State["Intent"] = nameof(FoodNewDialog);
            dc.ActiveDialog.State["IntentAction"] = "Confirm";

            var reply = MessageFactory.Attachment(new List<Attachment>());
            reply.Attachments.Add(Cards.GetThumbnailCard(dc.ActiveDialog.State).ToAttachment());
            await dc.Context.SendActivityAsync(reply);

            return dc.Wait(CardConfirmed);
        }

        public async Task<DialogTurnResult> CardConfirmed(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
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

        public override Task<DialogTurnResult> ContinueDialogAsync(DialogContext dc, CancellationToken cancellationToken = default)
        {
            string nextStep = (string)dc.ActiveDialog.State["stepID"];
            if (nextStep != "" && dc.Context.Activity.Text != null)
            {
                dc.ActiveDialog.State[nextStep] = dc.Context.Activity.Text;
            }
            return base.ContinueDialogAsync(dc, cancellationToken);
        }

        [IntentActionAtribute(DialogNames.FOOD, IntentActions.FoodActions.Confirm)]
        public async Task<DialogTurnResult> JsonConfirmFlight(DialogContext dc)
        {
            return await CardConfirmed(dc);
        }

        [IntentActionAtribute(DialogNames.FOOD, IntentActions.FoodActions.SelectSauce)]
        public async Task<DialogTurnResult> JsonSelectSauce(DialogContext dc)
        {
            var msg = dc.Context.Activity.Value.ToString();
            var parse = JsonConvert.DeserializeObject<FoodDetails>(msg);

            dc.ActiveDialog.State["SizeStep"] = parse.SizeStep;
            dc.ActiveDialog.State["SauceStep"] = parse.SauceStep;

            return await CheeseStep(dc);
        }

        [IntentActionAtribute(DialogNames.FOOD, IntentActions.FoodActions.Cancel)]
        public async Task<DialogTurnResult> JsonCancelFlight(DialogContext dc)
        {
            dc.Context.Activity.Value = null;

            return await CardConfirmed(dc);
        }
    }
}
