// <copyright file="EventDetail.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    using System.IO;
    using System.Web.UI;
    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;
    using Framework.Templating;
    using Engage.Events;
    using Templating;

    /// <summary>
    /// Event Detail view.
    /// </summary>
    public partial class EventDetail : ModuleBase
    {
        /// <summary>
        /// Method used to process a tag. This method is invoked from the <see cref="TemplateEngine"/> class. Since this control knows
        /// best on how to construct the control. ListingItem templates are used here.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="tag">The tag that is being processed.</param>
        /// <param name="engageObject">The engage object.</param>
        /// <param name="resourceFile">The resource file.</param>
        internal static void ProcessTag(Control container, Tag tag, object engageObject, string resourceFile)
        {
            // do nothing here, handled up in TemplateEngine - for now.
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        internal void BindData()
        {
            Event ev = Event.Load(EventId);
            string templateName = Utility.GetStringSetting(this.Settings, Framework.Setting.DetailTemplate.PropertyName, "Detail.Item.html");

            Template template = TemplateEngine.GetTemplate(PhysicialTemplatesFolderName, templateName);
            TemplateEngine.ProcessTags(this.DetailPlaceHolder, template.ChildTags, ev, this.LocalResourceFile, ProcessTag);

            this.BackHyperLink.NavigateUrl = Globals.NavigateURL();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.LocalResourceFile = this.AppRelativeTemplateSourceDirectory + "App_LocalResources/" + Path.GetFileNameWithoutExtension(this.TemplateControl.AppRelativeVirtualPath);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}

