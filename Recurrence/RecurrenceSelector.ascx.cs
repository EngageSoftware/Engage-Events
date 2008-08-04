// <copyright file="RecurrenceSelector.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Recurrence
{
    using System;

    public partial class RecurrenceSelector : ModuleBase
    {
        private RecurrenceEditor editor;
        public RecurrenceEditor Editor
        {
            set { this.editor = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.RecurrenceOptionsList.SelectedIndexChanged += this.RecurrenceOptionsList_SelectedIndexChanged;
            this.LocalResourceFile = "~" + DesktopModuleFolderName + "Recurrence/App_LocalResources/RecurrenceSelector";
        }

        private void RecurrenceOptionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRecurrenceControl();
        }

        private void Page_Load(object sender, EventArgs e)
        {
            this.LoadRecurrenceControl();
        }

        private void LoadRecurrenceControl()
        {
            string control = "DailyRecurrence.ascx";

            switch (this.RecurrenceOptionsList.SelectedIndex)
            {
                case 0:
                    control = "DailyRecurrence.ascx";
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

            RecurrenceControlBase c = (RecurrenceControlBase)this.LoadControl(control);
            c.EnableViewState = false;
            this.editor.Add(c);
        }
    }
}