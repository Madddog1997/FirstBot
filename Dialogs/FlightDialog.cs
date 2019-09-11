using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Dialogs
{
    public class FlightDialog : InterDialog
    {
        public FlightDialog() : base(nameof(FlightDialog))
        {
        }

        public async override Task<DialogTurnResult> BeginDialogAsync2(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "IntroStep";
            dc.ActiveDialog.State["Order"] = "Flight order";

            await dc.Context.SendActivityAsync("Welcome to Flight Booker");
            return await this.ContinueDialogAsync(dc, cancellationToken);
        }

        public async Task<DialogTurnResult> OriginStep(DialogContext dc, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "OriginStep";

            var options = new string[] { "Kosice", "Bratislava", "Budapest", }.ToList();
            var sizeOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Where would you like to go?"),
                Choices = ChoiceFactory.ToChoices(options),
            };
            return await dc.PromptAsync(nameof(ChoicePrompt), sizeOptions, cancellationToken);
        }

        public async Task<DialogTurnResult> DestinationStep(DialogContext dc, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "DestinationStep";

            var options = new string[] { "London", "Wiena", "Krakow", }.ToList();
            var sizeOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("From where would you like to go?"),
                Choices = ChoiceFactory.ToChoices(options),
            };
            return await dc.PromptAsync(nameof(ChoicePrompt), sizeOptions, cancellationToken);
        }

        public async Task<DialogTurnResult> CardDestination(DialogContext dc)
        {
            dc.ActiveDialog.State["stepID"] = "DestinationConfirmation";

            dc.ActiveDialog.State["Intent"] = "Flight";
            dc.ActiveDialog.State["IntentAction"] = "SelectDestination";

            var reply = MessageFactory.Attachment(new List<Attachment>());
            reply.Attachments.Add(Cards.GetThumbnailCard(dc.ActiveDialog.State).ToAttachment());
            await dc.Context.SendActivityAsync(reply);

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public async Task<DialogTurnResult> DateStep(DialogContext dc, CancellationToken cancellationToken = default)
        {
            dc.ActiveDialog.State["stepID"] = "DateStep";

            var cheeseMessage = MessageFactory.Text("When would you like to take the fligh", "When would you like to take the fligh", InputHints.ExpectingInput);
            return await dc.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = cheeseMessage }, cancellationToken);
        }

        public async Task<DialogTurnResult> ConfirmationStep(DialogContext dc)
        {
            dc.ActiveDialog.State["stepID"] = "ConfirmationStep";
            dc.ActiveDialog.State["Intent"] = "Flight";
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
                    return await OriginStep(dc, cancellationToken);
                case "OriginStep":
                    return await DestinationStep(dc, cancellationToken);
                case "DestinationStep":
                    return await CardDestination(dc);
                case "DestinationConfirmation":
                    return await DateStep(dc, cancellationToken);
                case "DateStep":
                    return await ConfirmationStep(dc);
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

        [IntentActionAtribute(DialogNames.FLIGHT, IntentActions.FlightActions.Confirm)]
        public async Task JsonConfirmFlight(DialogContext dc) {
            await CardConfirmed(dc);
        }

        [IntentActionAtribute(DialogNames.FLIGHT, IntentActions.FlightActions.SelectDestination)]
        public async Task JsonConfirmDestination(DialogContext dc)
        {
            var msg = dc.Context.Activity.Value.ToString();
            var parse = JsonConvert.DeserializeObject<FlightDetails>(msg);

            dc.ActiveDialog.State[nameof(OriginStep)] = parse.OriginStep;
            dc.ActiveDialog.State[nameof(DestinationStep)] = parse.DestinationStep;

            await DateStep(dc);
        }

        [IntentActionAtribute(DialogNames.FLIGHT, IntentActions.FlightActions.Cancel)]
        public async Task JsonCancelFlight(DialogContext dc)
        {
            dc.Context.Activity.Value = null;

            await CardConfirmed(dc);
        }

        public override Task EndDialogAsync(ITurnContext turnContext, DialogInstance instance, DialogReason reason, CancellationToken cancellationToken = default)
        {
            return base.EndDialogAsync(turnContext, instance, reason, cancellationToken);
        }
    }
}
