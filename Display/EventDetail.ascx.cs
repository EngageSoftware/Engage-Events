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
    using System.Web.UI.WebControls;
    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;
    using Engage.Events.Templating;
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

        private string t = string.Empty;

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
            string templateName = Utility.GetStringSetting(Settings, Setting.DetailTemplate.PropertyName);
            if (templateName == null) templateName = "Detail.Item.html";

            Template template = TemplateEngine.GetTemplate(PhysicialTemplatesFolderName, templateName);
            EventTemplateEngine.ProcessEventTags(this.DetailPlaceHolder, template.ChildTags, ev, ProcessEventTag);

            BackHyperLink.NavigateUrl = Globals.NavigateURL();
        }

        internal static void ProcessEventTag(Control container, Tag tag, Event ev)
        {
            switch (tag.LocalName.ToUpperInvariant())
            {
                case "EVENTTITLE":
                    Literal literalTitle = new Literal();
                    literalTitle.Text = ev.Title;
                    container.Controls.Add(literalTitle);
                    break;
                case "EVENTDATE":
                    Literal literalWhen = new Literal();
                    string format = tag.GetAttributeValue("Format");
                    literalWhen.Text = ev.EventStart.ToString(format);
                    container.Controls.Add(literalWhen);
                    break;
                case "EVENTMONTHSHORT":
                    Literal literalMonth = new Literal();
                    literalMonth.Text = ev.EventStart.ToString("MMM");
                    container.Controls.Add(literalMonth);
                    break;
                case "EVENTMONTHLONG":
                    Literal literalMonthLong = new Literal();
                    literalMonthLong.Text = ev.EventStart.ToString("MMMM");
                    container.Controls.Add(literalMonthLong);
                    break;
                case "EVENTDAYSHORT":
                    Literal literalDay = new Literal();
                    literalDay.Text = ev.EventStart.ToString("%d");
                    container.Controls.Add(literalDay);
                    break;
                case "EVENTDAYLONG":
                    Literal literalDayLong = new Literal();
                    literalDayLong.Text = ev.EventStart.ToString("dddd");
                    container.Controls.Add(literalDayLong);
                    break;
                case "EVENTYEARSHORT":
                    Literal literalYear = new Literal();
                    literalYear.Text = ev.EventStart.ToString("yy");
                    container.Controls.Add(literalYear);
                    break;
                case "EVENTYEARLONG":
                    Literal literalYearLong = new Literal();
                    literalYearLong.Text = ev.EventStart.ToString("yyyy");
                    container.Controls.Add(literalYearLong);
                    break;
                case "EVENTLOCATION":
                    Literal literalLocation = new Literal();
                    literalLocation.Text = ev.Location;
                    container.Controls.Add(literalLocation);
                    break;
                case "EVENTOVERVIEW":
                    Literal literalLocationOverview = new Literal();
                    literalLocationOverview.Text = ev.Overview;
                    container.Controls.Add(literalLocationOverview);
                    break;
                case "EVENTDESCRIPTION":
                    Literal literalLocationDescription = new Literal();
                    literalLocationDescription.Text = ev.Description;
                    container.Controls.Add(literalLocationDescription);
                    break;
            }
        }

        /// <summary>
        /// Sets the name of the detail template. 
        /// </summary>
        /// <value>The name of the header template.</value>
        internal string T
        {
            set { this.t = value; }
        }
       
    }
}

