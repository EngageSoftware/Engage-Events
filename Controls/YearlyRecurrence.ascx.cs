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

namespace Engage.Dnn.Events.Controls
{
    using System;
    using DotNetNuke.Services.Exceptions;
    using Framework.Recurrence;

    public partial class YearlyRecurrence : System.Web.UI.UserControl
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
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                FillCombos();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void FillCombos()
        {
            Framework.Utility.SetDataSource(yearlyDayCombo, RecurrenceHelper.CreateDayList());
            Framework.Utility.SetDataSource(yearlyDayOfWeekCombo, RecurrenceHelper.CreateDayOfWeekList());
            Framework.Utility.SetDataSource(yearlyDayMonthCombo, RecurrenceHelper.CreateMonthList());
            Framework.Utility.SetDataSource(yearlyDayOfWeekMonthCombo, RecurrenceHelper.CreateMonthList());
            Framework.Utility.SetDataSource(yearlyNthOccurrenceCombo, RecurrenceHelper.CreateNthOccurrenceList());
            Framework.Utility.SetDataSource(yearlyDayOccurrenceCombo, RecurrenceHelper.CreateWeekDayOccurrenceList());
            Framework.Utility.SetDataSource(yearlyWeekDayTypeCombo, RecurrenceHelper.CreateWeekDayTypeList());
            Framework.Utility.SetDataSource(yearlyWeekDayMonthCombo, RecurrenceHelper.CreateMonthList());

        }
    }
}