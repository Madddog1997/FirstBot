using FirstBot.BotExtensions;
using FirstBot.State;
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
//Pridat na Azure Table Storage
//-Ak sa resumne child dialog tak dokončiť resumeDialogAsync
//Vytvoriť delegáta pre názvy funckii 
namespace FirstBot.Dialogs
{
    public class TestDialog : InterDialog
    { 
        public TestDialog(string dialogId) : base(dialogId)
        {
        }

        public async override Task<DialogTurnResult> BeginDialogAsync2(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {            
            await dc.Context.SendActivityAsync("Step One");
            return await dc.Wait("StepTwo", nameof(FuckDialog));
        }

        public async Task<DialogTurnResult> StepTwo(DialogContext dc, object options = null)
        {
            await dc.Context.SendActivityAsync("Step Two, " + (string)options + " You wrote : " + dc.Context.Activity.Text);
            return dc.Wait("Confirm");
        }

        public async Task<DialogTurnResult> Confirm(DialogContext dc, object options = null)
        {
            await dc.Context.SendActivityAsync("Confirm, " + (string)options + " You wrote : " + dc.Context.Activity.Text);
            return await dc.EndDialogAsync();
        }
    }
}
