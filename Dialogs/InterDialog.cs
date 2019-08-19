using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Dialogs
{
    public abstract class InterDialog : Dialog
    {
        public InterDialog(string dialogId) : base(dialogId)
        {
        }

        public abstract Task<DialogTurnResult> BeginDialogAsync2(DialogContext outerDc, object options = null, CancellationToken cancellationToken = default);

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext outerDc, object options = null, CancellationToken cancellationToken = default)
        {
            if(await outerDc.HandleValueRedirection(this))
            {
                return new DialogTurnResult(DialogTurnStatus.Complete);
            }

            return await BeginDialogAsync2(outerDc, options, cancellationToken);
        }
    }
}
