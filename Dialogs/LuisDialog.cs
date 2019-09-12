using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;

namespace FirstBot.Dialogs
{
    public abstract class LuisDialog : InterDialog
    {
        private readonly LuisRecognizer _recognizer;

        public LuisDialog(LuisRecognizer recognizer, string id) : base(id)
        {
            _recognizer = recognizer;
        }

        public override async Task<DialogTurnResult> BeginDialogAsync2(DialogContext outerDc, object options = null, CancellationToken cancellationToken = default)
        {
            RecognizerResult result = await _recognizer.RecognizeAsync(outerDc.Context, cancellationToken);
            result = await LuisIntentChange(outerDc, result, options);
            return await outerDc.LuisValueRedirection(this, result);
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(DialogContext dc, CancellationToken cancellationToken = default)
        {
            return await BeginDialogAsync2(dc);
        }

        public virtual Task<RecognizerResult> LuisIntentChange(DialogContext outerDc, RecognizerResult result, object options = null, CancellationToken cancellationToken = default)
        {
            //Kvoli volaniu NONE intentu kedže NONE intent nie je dialog
            if(result.GetTopScoringIntent().intent.ToLower() == "none")
            {
                result.Intents.Clear();
                IntentScore score = new IntentScore
                {
                    Score = 1
                };
                result.Intents.Add(nameof(RootDialog), score);
            }
            return Task.FromResult(result);
        }
    }
}
