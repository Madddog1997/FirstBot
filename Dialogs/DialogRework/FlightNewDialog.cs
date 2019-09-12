using FirstBot.BotExtensions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Dialogs.DialogRework
{
    public class FlightNewDialog : InterDialog
    {
        public FlightNewDialog() : base(nameof(FlightNewDialog))
        {
        }

        public async override Task<DialogTurnResult> BeginDialogAsync2(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "OriginStep";

            await dc.Context.SendActivityAsync("Welcome to Flight Booker");
            await dc.Context.SendActivityAsync("Where would you like to go?");
            return dc.Wait(DestinationStep);
        }

        public async Task<DialogTurnResult> DestinationStep(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "DestinationStep";

            await dc.Context.SendActivityAsync("From where would you like to go?");
            return dc.Wait(CardDestination);
        }

        public async Task<DialogTurnResult> CardDestination(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "";
            dc.ActiveDialog.State["Intent"] = nameof(FlightNewDialog);
            dc.ActiveDialog.State["IntentAction"] = "SelectDestination";

            var reply = MessageFactory.Attachment(new List<Attachment>());
            reply.Attachments.Add(Cards.GetThumbnailCard(dc.ActiveDialog.State).ToAttachment());
            await dc.Context.SendActivityAsync(reply);

            return dc.Wait(DateStep);
        }

        public async Task<DialogTurnResult> DateStep(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        { 
            dc.ActiveDialog.State["stepID"] = "DateStep";

            await dc.Context.SendActivityAsync("When would you like to take the fligh?");
            return dc.Wait(ConfirmationStep);
        }

        public async Task<DialogTurnResult> ConfirmationStep(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "";
            dc.ActiveDialog.State["Intent"] = nameof(FlightNewDialog);
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

        [IntentActionAtribute(DialogNames.FLIGHT, IntentActions.FlightActions.Confirm)]
        public async Task<DialogTurnResult> JsonConfirmFlight(DialogContext dc) {
            return await CardConfirmed(dc);
        }

        [IntentActionAtribute(DialogNames.FLIGHT, IntentActions.FlightActions.SelectDestination)]
        public async Task<DialogTurnResult> JsonConfirmDestination(DialogContext dc)
        {
            var msg = dc.Context.Activity.Value.ToString();
            //dc.Context.Activity.Value = null;

            var parse = JsonConvert.DeserializeObject<FlightDetails>(msg);

            dc.ActiveDialog.State["OriginStep"] = parse.OriginStep;
            dc.ActiveDialog.State["DestinationStep"] = parse.DestinationStep;

            return await DateStep(dc);
        }

        [IntentActionAtribute(DialogNames.FLIGHT, IntentActions.FlightActions.Cancel)]
        public async Task<DialogTurnResult> JsonCancelFlight(DialogContext dc)
        {
            dc.Context.Activity.Value = null;

            return await CardConfirmed(dc);
        }
    }
}
