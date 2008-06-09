//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Diagnostics;
//using System.Reflection;
//using System.Text;

//namespace Engage.Dnn.Events
//{
//    public delegate void ActionEventHandler(object sender, ActionEventArg e);

//    public enum Action
//    {
//        Failed = 0,
//        Success = 1
//    }

//    public class ActionEventArg: System.EventArgs
//    {
        
//        public ActionEventArg(Action action)
//        {
//            this._action = action;
//        }

//        private Action _action;
//        public Action ActionStatus
//        {
//            get { return _action; }
//        }
//    }
//}
