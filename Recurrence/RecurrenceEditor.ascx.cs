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

namespace Engage.Dnn.Events.Recurrence
{
    using System;

    public partial class RecurrenceEditor : ModuleBase
    {
        public void Add(RecurrenceControlBase control)
        {
            this.RecurrencePatternPlaceHolder.Controls.Clear();
            this.RecurrencePatternPlaceHolder.Controls.Add(control);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.LocalResourceFile = "~" + DesktopModuleFolderName + "Recurrence/App_LocalResources/RecurrenceEditor";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.RecurrenceSelector.Editor = this;
        }

        internal string RecurrenceRule
        {
            get
            {
                RecurrenceControlBase recurrencePatternControl = (RecurrenceControlBase)this.RecurrencePatternPlaceHolder.Controls[0];
                return recurrencePatternControl.RecurrenceRule;
            }
        }
    }
}