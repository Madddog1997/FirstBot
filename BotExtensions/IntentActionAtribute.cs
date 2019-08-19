using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBot
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class IntentActionAtribute : AttributeIntentActionName
    {
        private readonly string _intentName;
        private readonly string _intentAction;
       
        /// <param name="intentName">The Luis intent name</param>
        public IntentActionAtribute(string intentName, string intentAction)
        {
            _intentAction = intentAction;
            _intentName = intentName;
        }

        protected override string IntentName
        {
            get
            {
                return _intentName;
            }
        }

        protected override string IntentAction
        {
            get
            {
                return _intentAction;
            }
        }
    }
}
