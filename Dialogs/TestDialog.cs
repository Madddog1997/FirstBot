using FirstBot.BotExtensions;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using static FirstBot.BotExtensions.ContextExtensions;
//Pridat na Azure Table Storage
//-Ak sa resumne child dialog tak dokončiť resumeDialogAsync
//-Vytvoriť delegáta pre názvy funckii 
namespace FirstBot.Dialogs
{
    public class TestDialog : InterDialog
    {
        public delegate Task<DialogTurnResult> DialogFuckHandler(DialogContext context, object options = null);

        public TestDialog() : base(nameof(TestDialog))
        {
        }

        public async override Task<DialogTurnResult> BeginDialogAsync2(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            await dc.Context.SendActivityAsync("Step One");
            return await dc.Wait(StepTwo, nameof(FuckDialog));
            //return dc.Wait(StepTwo);
        }

        public async Task<DialogTurnResult> StepTwo(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            await dc.Context.SendActivityAsync("Step Two, " + (string)options + " You wrote : " + dc.Context.Activity.Text);
            return dc.Wait(Confirm);
        }

        public async Task<DialogTurnResult> Confirm(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            await dc.Context.SendActivityAsync("Confirm, " + (string)options + " You wrote : " + dc.Context.Activity.Text);
            return await dc.EndDialogAsync();
        }
    }
}
