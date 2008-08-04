// <copyright file="MonthlyRecurrence.ascx.cs" company="Engage Software">
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
    using DotNetNuke.Services.Exceptions;
    using Engage.Dnn.Framework.Recurrence;

    public partial class MonthlyRecurrence : RecurrenceControlBase
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
            this.LocalResourceFile = "~" + DesktopModuleFolderName + "Recurrence/App_LocalResources/MonthlyRecurrence";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.FillCombos();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void FillCombos()
        {
            Framework.Utility.SetDataSource(this.DayOfMonthList, RecurrenceHelper.CreateDayList());
            Framework.Utility.SetDataSource(this.DayOfWeekList, RecurrenceHelper.CreateDayOfWeekList());
            Framework.Utility.SetDataSource(this.DaySequenceList, RecurrenceHelper.CreateNthOccurrenceList());
            //Framework.Utility.SetDataSource(this.DaySequenceList, RecurrenceHelper.CreateWeekDayOccurrenceList());
            //Framework.Utility.SetDataSource(this.DaySequenceList, RecurrenceHelper.CreateWeekDayTypeList());
        }



        public override string RecurrenceRule
        {
            get { return "Monthly Recurrence"; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
            }
        }
    }
}