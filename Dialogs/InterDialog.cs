using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static FirstBot.BotExtensions.ContextExtensions;

namespace FirstBot.Dialogs
{
    public abstract class InterDialog : Dialog
    {
        public InterDialog(string id) : base(id)
        {
        }

        public abstract Task<DialogTurnResult> BeginDialogAsync2(DialogContext outerDc, object options = null, CancellationToken cancellationToken = default);

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext outerDc, object options = null, CancellationToken cancellationToken = default)
        {
            if (await outerDc.HandleValueRedirection(this))
            {
                return new DialogTurnResult(DialogTurnStatus.Complete);
            }

            return await BeginDialogAsync2(outerDc, options, cancellationToken);
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(DialogContext dc, CancellationToken cancellationToken = default)
        {
            if (dc.ActiveDialog.State.ContainsKey("Handler") && dc.ActiveDialog.State["Handler"] != null)
            {
                string methodName = (string)dc.ActiveDialog.State["Handler"];
                MethodInfo mi = this.GetType().GetMethod(methodName);
                DialogCommandHandler Handler = (DialogCommandHandler)Delegate.CreateDelegate(typeof(DialogCommandHandler), this, mi);

                if (Handler != null)
                {
                    dc.ActiveDialog.State["Handler"] = null;
                    return await Handler(dc);
                }
            }

            await dc.Context.SendActivityAsync("No next step defined");
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ResumeDialogAsync(DialogContext dc, DialogReason reason, object result = null, CancellationToken cancellationToken = default)
        {
            if (dc.ActiveDialog.State.ContainsKey("Handler") && dc.ActiveDialog.State["Handler"] != null)
            {
                string methodName = (string)dc.ActiveDialog.State["Handler"];
                MethodInfo mi = this.GetType().GetMethod(methodName);
                DialogCommandHandler Handler = (DialogCommandHandler)Delegate.CreateDelegate(typeof(DialogCommandHandler), this, mi);

                if (Handler != null)
                {
                    dc.ActiveDialog.State["Handler"] = null;
                    return await Handler(dc, result);
                }
            }

            await dc.Context.SendActivityAsync("No next step defined");
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }
    }
}
