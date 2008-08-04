// <copyright file="YearlyRecurrence.ascx.cs" company="Engage Software">
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

    public partial class YearlyRecurrence : RecurrenceControlBase
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
            this.LocalResourceFile = "~" + DesktopModuleFolderName + "Recurrence/App_LocalResources/YearlyRecurrence";
        }

        private void Page_Load(object sender, EventArgs e)
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
            Framework.Utility.SetDataSource(this.DaySequenceList, RecurrenceHelper.CreateNthOccurrenceList());
            Framework.Utility.SetDataSource(this.DayOfWeekList, RecurrenceHelper.CreateDayOfWeekList());
            Framework.Utility.SetDataSource(this.SpecificMonthList, RecurrenceHelper.CreateMonthList());
            Framework.Utility.SetDataSource(this.RelativeMonthList, RecurrenceHelper.CreateMonthList());
            //Framework.Utility.SetDataSource(this.yearlyNthOccurrenceCombo, RecurrenceHelper.CreateNthOccurrenceList());
            //Framework.Utility.SetDataSource(this.yearlyDayOccurrenceCombo, RecurrenceHelper.CreateWeekDayOccurrenceList());
            //Framework.Utility.SetDataSource(this.yearlyWeekDayTypeCombo, RecurrenceHelper.CreateWeekDayTypeList());
            //Framework.Utility.SetDataSource(this.yearlyWeekDayMonthCombo, RecurrenceHelper.CreateMonthList());

        }

        public override string RecurrenceRule
        {
            get { return "Yearly Recurrence"; }
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