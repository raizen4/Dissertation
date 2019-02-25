using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class HistoryAction
    {
        private string _action;
        private string _description;
        public string Action {
            get =>this._action;
            set => this._action = value;
        }
        public string Description
        {
            get => this._description;
            set => this._description = value;
        }
        
    }
}
