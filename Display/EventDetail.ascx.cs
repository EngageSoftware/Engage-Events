// <copyright file="EventDetail.ascx.cs" company="Engage Software">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Common;

    using Engage.Dnn.Events.Components;
    using Engage.Events;
    using Framework.Templating;
    using Templating;

    /// <summary>
    /// Event Detail view.
    /// </summary>
    public partial class EventDetail : TemplatedDisplayModuleBase
    {
        /// <summary>
        /// Processes a tag for a template
        /// </summary>
        /// <param name="container">The container into which created controls should be added</param>
        /// <param name="tag">The tag to process</param>
        /// <param name="templateItem">The object to query for data to implement the given tag</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>Whether to process the tag's ChildTags collection</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "backHyperlink passed to controls collection and we do not need to manually dispose")]
        internal bool ProcessTag(Control container, Tag tag, ITemplateable templateItem, string resourceFile)
        {
            if (tag.TagType == TagType.Open && tag.LocalName.Equals("BACKHYPERLINK", StringComparison.OrdinalIgnoreCase))
            {
                var backHyperlink = new HyperLink
                    {
                        NavigateUrl = Globals.NavigateURL(),
                        CssClass = TemplateEngine.GetAttributeValue(tag, templateItem, null, resourceFile, "CssClass", "class"),
                        Text = TemplateEngine.GetAttributeValue(tag, templateItem, (ITemplateable)null, resourceFile, "Text")
                    };

                container.Controls.Add(backHyperlink);
            }
            else
            {
                return this.ProcessCommonTag(container, tag, (Event)templateItem, resourceFile);
            }

            return true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.SetupTemplateProvider();
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the <see cref="DeleteAction.Delete"/> and <see cref="CancelAction.Cancel"/> events,
        /// reloading this page to reflect the changes made by those controls
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void ReloadPage(object sender, EventArgs args)
        {
            if (this.EventId.HasValue)
            {
                this.Response.Redirect(this.BuildLinkUrl(this.TabId, this.ModuleId, "EventDetail", Dnn.Events.Utility.GetEventParameters(this.EventId.Value, this.EventStart)));
            }
            else
            {
                this.ReturnToList(sender, args);
            }
        }

        /// <summary>
        /// Handles the <see cref="DeleteAction.Delete"/> event,
        /// reloading the list of events to reflect the changes made by those controls
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void ReturnToList(object sender, EventArgs args)
        {
            this.Response.Redirect(Globals.NavigateURL(), true);
        }

        /// <summary>
        /// Sets up the <see cref="Dnn.Framework.ModuleBase.TemplateProvider"/> for this control.
        /// </summary>
        private void SetupTemplateProvider()
        {
            int? eventId = this.EventId;
            if (!eventId.HasValue)
            {
                return;
            }

            var ev = Event.Load(eventId.Value);
            if (!this.CanShowEvent(ev))
            {
                return;
            }

            if (ev.IsRecurring)
            {
                ev = ev.CreateOccurrence(this.EventStart);
            }

            this.TemplateProvider = new SingleItemTemplateProvider(
                this.GetTemplate(ModuleSettings.SingleItemTemplate.GetValueAsStringFor(this)),
                this,
                this.ProcessTag,
                null,
                ev,
                new GlobalTemplateContext(this.ModuleContext));
        }
    }
}