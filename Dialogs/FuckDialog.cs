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

namespace FirstBot.Dialogs
{
    public class FuckDialog : InterDialog
    { 
        public FuckDialog(string dialogId) : base(dialogId)
        {
        }

        public async override Task<DialogTurnResult> BeginDialogAsync2(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {            
            await dc.Context.SendActivityAsync("Fuck One");
            return dc.Wait("Confirm");
        }

        public async Task<DialogTurnResult> Confirm(DialogContext dc, object options = null)
        {
            await dc.Context.SendActivityAsync("Fuck Confirm, " + (string)options + " You wrote : " + dc.Context.Activity.Text);
            return await dc.EndDialogAsync("Ended FuckDialog");
        }
    }
}
