using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.Models
{
    using Enums;

    public class HistoryAction
    {
        private LockerActionRequestsEnum _action;
        private string _description;
        public LockerActionRequestsEnum Action {
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
