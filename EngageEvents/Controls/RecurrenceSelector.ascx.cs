using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Engage.Dnn.Events.Controls
{
    public partial class RecurrenceSelector : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            _editor.Clear();
            Control c = null;
            string control = "DialyRecurrence.ascx";

            switch (RecurrenceRadio.SelectedIndex)
            {
                case 0:
                    control = "DialyRecurrence.ascx";
                    break;
                case 1:
                    control = "WeeklyRecurrence.ascx";
                    break;
                case 2:
                    control = "MonthlyRecurrence.ascx";
                    break;
                case 3:
                    control = "YearlyRecurrence.ascx";
                    break;
                default:
                    break;
            }

            c = LoadControl(control);
            c.ID = Path.GetFileNameWithoutExtension(control);
            _editor.Add(c);

        }

        RecurrenceEditor _editor;
        public RecurrenceEditor Editor
        {
            set { _editor = value; }
        }

    }
}