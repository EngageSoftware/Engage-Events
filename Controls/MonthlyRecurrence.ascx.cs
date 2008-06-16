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

namespace Engage.Dnn.Events.Controls
{
    using System;

    public partial class MonthlyRecurrence : System.Web.UI.UserControl
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.FillCombos();
        }

        private void FillCombos()
        {
            this.monthlyNthOccurrenceCombo.DataTextField = "DisplayMember";
            this.monthlyNthOccurrenceCombo.DataValueField = "ValueMember";
            ////monthlyNthOccurrenceCombo.DataSource = Recurrence.RecurrenceHelper.CreateNthOccurrenceList();
            this.monthlyNthOccurrenceCombo.DataBind();
        }
    }
}