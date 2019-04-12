using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    using Enums;

    public class HistoryAction
    {
        private LockerActionEnum _actionType;
        private string _message;
        public LockerActionEnum ActionType
        {
            get =>this._actionType;
            set => this._actionType = value;
        }
        public string Message
        {
            get => this._message;
            set => this._message = value;
        }
        
    }
}
