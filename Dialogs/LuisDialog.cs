using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FirstBot.Dialogs.DialogRework;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;

//urobiť classu ktorá dedi od interMed a v nej urobiť tuto logiku a následne nech dedí ju RootDialog
//V nej pridať funkciu ako pozmeniť intent ale to pozrieť od niekoho ako funguje 

namespace FirstBot.Dialogs
{
    public class LuisDialog : InterDialog
    {
        private readonly LuisRecognizer _recognizer;

        public LuisDialog(LuisRecognizer recognizer, string id) : base(id)
        {
            _recognizer = recognizer;
        }

        public override async Task<DialogTurnResult> BeginDialogAsync2(DialogContext outerDc, object options = null, CancellationToken cancellationToken = default)
        {
            RecognizerResult result = await _recognizer.RecognizeAsync(outerDc.Context, cancellationToken);

            return await outerDc.LuisValueRedirection(this, result);
        }

        //public override async Task<DialogTurnResult> ContinueDialogAsync(DialogContext dc, CancellationToken cancellationToken = default)
        //{
        //    RecognizerResult result = await _recognizer.RecognizeAsync(dc.Context, cancellationToken);
        //    return await dc.LuisValueRedirection(this, result);
        //}
    }
}
