// <copyright file="RecurrenceEditor.ascx.cs" company="Engage Software">
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
    
    public partial class RecurrenceEditor : ModuleBase
    {
        //public void Clear()
        //{
        //    this.phRecurrencePattern.Controls.Clear();
        //}

        public void Add(System.Web.UI.Control control)
        {
            this.phRecurrencePattern.Controls.Clear();
            this.phRecurrencePattern.Controls.Add(control);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.RecurringCheckbox.CheckedChanged += this.RecurringCheckbox_CheckedChanged;
        }

        private void RecurringCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            RecurrenceEditorDiv.Visible = RecurringCheckbox.Checked;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.RecurrenceSelector1.Editor = this;
        }
    }
}

