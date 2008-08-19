// <copyright file="EventListingTemplate.ascx.cs" company="Engage Software">
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
    using System.Web.UI;
    using Framework.Templating;
    using Templating;
    using Utility = Engage.Dnn.Utility;

    /// <summary>
    /// Custom event listing
    /// </summary>
    public partial class EventListingTemplate : TemplateModuleBase
    {
        /// <summary>
        /// Method used to process a tag. This method is invoked from the <see cref="TemplateEngine"/> class. Since this control knows
        /// best on how to construct the control. ListingItem templates are used here.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="tag">The tag that is being processed.</param>
        /// <param name="engageObject">The engage object.</param>
        /// <param name="resourceFile">The resource file.</param>
        protected override void ProcessTag(Control container, Tag tag, object engageObject, string resourceFile)
        {
            if (tag.TagType == TagType.Open)
            {
                switch (tag.LocalName.ToUpperInvariant())
                {
                    case "EVENTLISTING":
                        EventListingItem listingCurrent = (EventListingItem)this.LoadControl("~" + DesktopModuleFolderName + "Display/EventListingItem.ascx");

                        // need to default to all and set if attribute is defined.
                        if (tag.HasAttribute("ListingMode"))
                        {
                            listingCurrent.SetListingMode(tag.GetAttributeValue("ListingMode"));
                        }

                        if (tag.HasAttribute("HeaderTemplate"))
                        {
                            listingCurrent.HeaderTemplateName = tag.GetAttributeValue("HeaderTemplate");
                        }

                        if (tag.HasAttribute("ItemTemplate"))
                        {
                            listingCurrent.ItemTemplateName = tag.GetAttributeValue("ItemTemplate");
                        }

                        if (tag.HasAttribute("FooterTemplate"))
                        {
                            listingCurrent.FooterTemplateName = tag.GetAttributeValue("FooterTemplate");
                        }

                        listingCurrent.IsFeatured = Utility.GetBoolSetting(this.Settings, Setting.FeaturedOnly.PropertyName, false);

                        listingCurrent.ModuleConfiguration = this.ModuleConfiguration;
                        container.Controls.Add(listingCurrent);
                        break;
                    case "CALENDAR":
                        EventCalendar calendar = (EventCalendar)this.LoadControl("~" + DesktopModuleFolderName + "Display/EventCalendar.ascx");
                        calendar.ModuleConfiguration = this.ModuleConfiguration;
                        calendar.IsFeatured = Utility.GetBoolSetting(this.Settings, Setting.FeaturedOnly.PropertyName, false);

                        container.Controls.Add(calendar);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

