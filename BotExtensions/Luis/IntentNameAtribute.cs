﻿using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBot
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DialogNameAtribute : AttributeIntentName
    {
        private readonly string _dialogName;
       
        /// <param name="dialogName">The Luis intent name</param>
        public DialogNameAtribute(string dialogName)
        {
            _dialogName = dialogName;
        }

        protected override string dialogName
        {
            get
            {
                return _dialogName;
            }
        }
    }
}