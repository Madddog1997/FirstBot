using Microsoft.Bot.Builder.AI.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstBot.LUIS
{
    public class TestLuisRecognizer : LuisRecognizer
    {
        public TestLuisRecognizer(LuisApplication application, LuisPredictionOptions predictionOptions = null, bool includeApiResults = false, HttpClientHandler clientHandler = null) : base(application, predictionOptions, includeApiResults, clientHandler)
        {
        }
    }
}
