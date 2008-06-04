using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Engage.Dnn.Events.Controls
{
    public partial class WeeklyRecurrence : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //_editor.Clear();
            //Literal l = new Literal();

            //switch (RecurrenceRadio.SelectedIndex)
            //{
            //    case 0:
            //        l.Text = "Daily";
            //        break;
            //    case 1:
            //        l.Text = "Weekly";
            //        break;
            //    case 2:
            //        l.Text = "Monthly";
            //        break;
            //    case 3:
            //        l.Text = "Yearly";
            //        break;
            //    default:
            //        break;
            //}

            //_editor.Add(l);

        }

        RecurrenceEditor _editor;
        public RecurrenceEditor Editor
        {
            set { _editor = value; }
        }

    }
}