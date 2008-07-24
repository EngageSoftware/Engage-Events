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
    using System.Web.UI;
    using Framework.Templating;
    using Templating;

    /// <summary>
    /// Custom event listing
    /// </summary>
    public partial class EventListingTemplate : TemplateModuleBase
    {
        /// <summary>
        /// Method used to process a tag. This method is invoked from the TemplateEngine class. Since this control knows
        /// best on how to contruct the control. ListingItem templates are used here.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="tag">The tag.</param>
        protected override void ProcessTag(Control container, Tag tag, object engageObject, string localResourceFile)
        {
            switch (tag.LocalName.ToUpperInvariant())
            {
                case "EVENTLISTING":
                    EventListingItem listingCurrent = (EventListingItem)LoadControl("~" + DesktopModuleFolderName + "Display/EventListingItem.ascx");
                    //need to default to all and set if attribute is defined.
                    if (tag.HasAttribute("ListingMode"))
                    {
                        listingCurrent.Mode = tag.GetAttributeValue("ListingMode");
                    }
                    if (tag.HasAttribute("HeaderTemplate"))
                    {
                        listingCurrent.HeaderTemplateName = tag.GetAttributeValue("HeaderTemplate");
                    }
                    if (tag.HasAttribute("ItemTemplate"))
                    {
                        listingCurrent.ItememplateName = tag.GetAttributeValue("ItemTemplate");
                    }
                    if (tag.HasAttribute("FooterTemplate"))
                    {
                        listingCurrent.FooterTemplateName = tag.GetAttributeValue("FooterTemplate");
                    }

                    listingCurrent.ModuleConfiguration = ModuleConfiguration;
                    container.Controls.Add(listingCurrent);
                    break;
                case "CALENDAR":
                    EventCalendar calendar = (EventCalendar)LoadControl("~" + DesktopModuleFolderName + "Display/EventCalendar.ascx");
                    container.Controls.Add(calendar);
                    break;
                default:
                    break;
            }
        }
    }
}

