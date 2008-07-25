// <copyright file="EventListing.ascx.cs" company="Engage Software">
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
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.LocalResourceFile = "~" + DesktopModuleFolderName + "Display/App_LocalResources/EventDetail.ascx.resx";
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
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

        /// <summary>
        /// Binds the data.
        /// </summary>
        internal void BindData()
        {
            Event ev = Event.Load(EventId);
            string templateName = Utility.GetStringSetting(Settings, Framework.Setting.DetailTemplate.PropertyName);
            if (templateName == null) templateName = "Detail.Item.html";

            Engage.Templating.Template template = TemplateEngine.GetTemplate(PhysicialTemplatesFolderName, templateName);
            TemplateEngine.ProcessTags(this.DetailPlaceHolder, template.ChildTags, ev, LocalResourceFile, ProcessTag);

            BackHyperLink.NavigateUrl = Globals.NavigateURL();
        }

        internal static void ProcessTag(Control container, Tag tag, object engageObject, string resourceFile)
        {
            //do nothing here, handled up in TemplateEngine - for now.
        }

    }
}

