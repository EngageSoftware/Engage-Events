// <copyright file="EventDisplay.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Display
{
    using System;
    using System.Web.UI;

    /// <summary>
    /// Custom event listing
    /// </summary>
    public partial class EventDisplay : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            ModuleBase displayControl;
            if (ModuleSettings.DisplayType.GetValueAsStringFor(this).Equals("LIST", StringComparison.OrdinalIgnoreCase))
            {
                displayControl = (ModuleBase)this.LoadControl("~" + this.DesktopModuleFolderName + "Display/EventListingItem.ascx");
            }
            else
            {
                displayControl = (ModuleBase)this.LoadControl("~" + this.DesktopModuleFolderName + "Display/EventCalendar.ascx");
            }

            displayControl.ModuleConfiguration = this.ModuleConfiguration;
            this.Controls.Add(displayControl);
        }
    }
}