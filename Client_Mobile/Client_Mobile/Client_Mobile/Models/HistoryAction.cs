using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    using Enums;

    public class HistoryAction
    {
        private LockerActionEnum _action;
        private string _description;
        public LockerActionEnum Action {
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
