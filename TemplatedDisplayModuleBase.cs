// <copyright file="TemplatedDisplayModuleBase.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text; 
    using System.Web;
    using System.Web.UI;
    using Engage.Dnn.Framework.Templating;
    using Engage.Events;
    using Engage.Templating;

    /// <summary>
    /// A base class for modules which display templated event information
    /// </summary>
    public abstract class TemplatedDisplayModuleBase : ModuleBase
    {
        /// <summary>
        /// Relative path to the folder where the action controls are located in this module
        /// </summary>
        protected readonly string ActionsControlsFolder;

        /// <summary>
        /// Holds tag names and their corresponding logic
        /// </summary> 
        private static readonly Dictionary<string, TokenProcessor> TokenImplementations = new Dictionary<string, TokenProcessor>
                {
                    {
                        "EDITEVENTBUTTON", 
                        (@this, container, tag, currentEvent, resourceFile) => 
                        {
                            if (@this.PermissionsService.CanManageEvents)
                            {
                                var editEventAction = (ButtonAction)@this.LoadControl(@this.ActionsControlsFolder + "ButtonAction.ascx");
                                editEventAction.CurrentEvent = currentEvent;
                                editEventAction.ModuleConfiguration = @this.ModuleConfiguration;
                                editEventAction.Href = @this.BuildLinkUrl(@this.ModuleId, "EventEdit", Utility.GetEventParameters(currentEvent));
                                editEventAction.ResourceKey = "EditEventButton";
                                editEventAction.CssClass = TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "class", "CssClass") ?? editEventAction.CssClass;
                                    
                                container.Controls.Add(editEventAction);
                            }

                            return false;
                        }
                    },
                    {
                        "VIEWRESPONSESBUTTON", 
                        (@this, container, tag, currentEvent, resourceFile) => 
                        {
                            if (@this.PermissionsService.CanViewResponses)
                            {
                                var responsesEventAction = (ButtonAction)@this.LoadControl(@this.ActionsControlsFolder + "ButtonAction.ascx");
                                responsesEventAction.CurrentEvent = currentEvent;
                                responsesEventAction.ModuleConfiguration = @this.ModuleConfiguration;
                                responsesEventAction.Href = @this.BuildLinkUrl(@this.ModuleId, "ResponseDetail", Utility.GetEventParameters(currentEvent));
                                responsesEventAction.ResourceKey = "ResponsesButton";
                                responsesEventAction.CssClass = TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "class", "CssClass") ?? responsesEventAction.CssClass;

                                container.Controls.Add(responsesEventAction);
                            }

                            return false;
                        }
                    },
                    {
                        "REGISTERBUTTON", 
                        (@this, container, tag, currentEvent, resourceFile) => 
                        {
                            // must be an active event and has not ended
                            if (currentEvent != null && currentEvent.AllowRegistrations && !currentEvent.Canceled && currentEvent.EventEndUtc > DateTime.UtcNow)
                            {
                                var registerEventAction = (RegisterAction)@this.LoadControl(@this.ActionsControlsFolder + "RegisterAction.ascx");
                                registerEventAction.CurrentEvent = currentEvent;
                                registerEventAction.ModuleConfiguration = @this.ModuleConfiguration;
                                registerEventAction.LocalResourceFile = resourceFile;
                                registerEventAction.CssClass = TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "class", "CssClass") ?? registerEventAction.CssClass;

                                container.Controls.Add(registerEventAction);
                            }

                            return false;
                        }
                    },
                    {
                        "ADDTOCALENDARBUTTON", 
                        (@this, container, tag, currentEvent, resourceFile) => 
                        {
                            // must be an active event and has not ended
                            if (currentEvent != null && !currentEvent.Canceled && currentEvent.EventEndUtc > DateTime.UtcNow)
                            {
                                var addToCalendarAction = (AddToCalendarAction)@this.LoadControl(@this.ActionsControlsFolder + "AddToCalendarAction.ascx");
                                addToCalendarAction.CurrentEvent = currentEvent;
                                addToCalendarAction.ModuleConfiguration = @this.ModuleConfiguration;
                                addToCalendarAction.LocalResourceFile = resourceFile;
                                addToCalendarAction.CssClass = TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "class", "CssClass") ?? addToCalendarAction.CssClass;

                                container.Controls.Add(addToCalendarAction);
                            }

                            return false;
                        }
                    },
                    {
                        "DELETEBUTTON", 
                        (@this, container, tag, currentEvent, resourceFile) => 
                        {
                            var deleteAction = (DeleteAction)@this.LoadControl(@this.ActionsControlsFolder + "DeleteAction.ascx");
                            deleteAction.CurrentEvent = currentEvent;
                            deleteAction.ModuleConfiguration = @this.ModuleConfiguration;
                            deleteAction.LocalResourceFile = resourceFile;
                            deleteAction.Delete += @this.ReturnToList;
                            deleteAction.CssClass = TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "class", "CssClass") ?? deleteAction.CssClass;

                            container.Controls.Add(deleteAction);

                            return false;
                        }
                    },
                    {
                        "CANCELBUTTON", 
                        (@this, container, tag, currentEvent, resourceFile) => 
                        {
                            var cancelAction = (CancelAction)@this.LoadControl(@this.ActionsControlsFolder + "CancelAction.ascx");
                            cancelAction.CurrentEvent = currentEvent;
                            cancelAction.ModuleConfiguration = @this.ModuleConfiguration;
                            cancelAction.LocalResourceFile = resourceFile;
                            cancelAction.Cancel += @this.ReloadPage;
                            cancelAction.CssClass = TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "class", "CssClass") ?? cancelAction.CssClass;

                            container.Controls.Add(cancelAction);

                            return false;
                        }
                    },
                    {
                        "RECURRENCESUMMARY", 
                        (@this, container, tag, currentEvent, resourceFile) => 
                        {
                            if (currentEvent != null)
                            {
                                container.Controls.Add(new LiteralControl(Utility.GetRecurrenceSummary(currentEvent.RecurrenceRule)));
                            }

                            return false;
                        }
                    },
                    {
                        "EVENTWRAPPER", 
                        (@this, container, tag, currentEvent, resourceFile) => 
                        {
                            if (currentEvent != null)
                            {
                                var cssClassBuilder = new StringBuilder(TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "CssClass", "class"));
                                if (currentEvent.IsRecurring)
                                {
                                    AppendCssClassAttribute(cssClassBuilder, TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "RecurringEventCssClass"));
                                }

                                if (currentEvent.IsFeatured)
                                {
                                    AppendCssClassAttribute(cssClassBuilder, TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "FeaturedEventCssClass"));
                                }

                                if (currentEvent.Canceled)
                                {
                                    AppendCssClassAttribute(cssClassBuilder, TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "CanceledEventCssClass", "CancelledEventCssClass"));
                                }

                                if (currentEvent.IsFull)
                                {
                                    AppendCssClassAttribute(cssClassBuilder, TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "FullEventCssClass"));
                                }

                                if (@this.isAlternatingEvent)
                                {
                                    AppendCssClassAttribute(cssClassBuilder, TemplateEngine.GetAttributeValue(tag, currentEvent, @this.TemplateProvider.GlobalItem, resourceFile, "AlternatingCssClass"));
                                }

                                container.Controls.Add(
                                    new LiteralControl(
                                        string.Format(
                                            CultureInfo.InvariantCulture,
                                            "<div class=\"{0}\">",
                                            HttpUtility.HtmlAttributeEncode(cssClassBuilder.ToString()))));
                            }

                            return true;
                        }
                    },
                    { 
                        "DURATION", 
                        (@this, container, tag, currentEvent, resourceFile) => 
                        {
                            if (currentEvent != null)
                            {
                                container.Controls.Add(
                                    new LiteralControl(
                                        HttpUtility.HtmlEncode(Utility.GetFormattedEventDate(currentEvent.EventStart, currentEvent.EventEnd))));
                            }

                            return false;
                        }
                    }
                };

        /// <summary>
        /// Keeps track of whether the current event being processed in <see cref="ProcessCommonTag"/> is an alternating (even number) event
        /// </summary>
        private bool isAlternatingEvent = true;

        /// <summary>
        /// Keeps track of the last event processed in <see cref="ProcessCommonTag"/> to enable alternating behavior
        /// </summary>
        private Event lastEventProcessed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatedDisplayModuleBase"/> class.
        /// </summary>
        protected TemplatedDisplayModuleBase()
        {
            this.ActionsControlsFolder = "~" + this.DesktopModuleFolderName + "Actions/";
        }

        /// <summary>
        /// A delegate that processes a single template token
        /// </summary>
        /// <param name="this">The current instance of this class</param>
        /// <param name="container">The container control</param>
        /// <param name="tag">The template tag to process</param>
        /// <param name="currentEvent">The current event</param>
        /// <param name="resourceFile">A resource file with localized settings</param>
        /// <returns>Whether to process the tag's ChildTags collection</returns>
        private delegate bool TokenProcessor(TemplatedDisplayModuleBase @this, Control container, Tag tag, Event currentEvent, string resourceFile);

        /// <summary>
        /// Processes the tokens that are common between the listing and details views
        /// </summary>
        /// <param name="container">The container into which created controls should be added</param>
        /// <param name="tag">The tag to process</param>
        /// <param name="currentEvent">The current event.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>
        /// Whether to process the tag's ChildTags collection
        /// </returns>
        protected bool ProcessCommonTag(Control container, Tag tag, Event currentEvent, string resourceFile)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            if (currentEvent != null && currentEvent != this.lastEventProcessed)
            {
                this.isAlternatingEvent = !this.isAlternatingEvent;
                this.lastEventProcessed = currentEvent;
            }

            if (tag.TagType == TagType.Open)
            {
                TokenProcessor process;
                if (TokenImplementations.TryGetValue(tag.LocalName.ToUpperInvariant(), out process))
                {
                    return process(this, container, tag, currentEvent, resourceFile);
                }

                return false;
            }
            
            if (tag.TagType == TagType.Close && tag.LocalName.Equals("EVENTWRAPPER", StringComparison.OrdinalIgnoreCase))
            {
                container.Controls.Add(new LiteralControl("</div>"));
            }

            return false;
        }

        /// <summary>
        /// Handles the <see cref="CancelAction.Cancel"/> event, 
        /// reloading the current page to reflect the changes made by those controls
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected abstract void ReloadPage(object sender, EventArgs args);

        /// <summary>
        /// Handles the <see cref="DeleteAction.Delete"/> event, 
        /// reloading the list of events to reflect the changes made by those controls
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected abstract void ReturnToList(object sender, EventArgs args);

        /// <summary>
        /// Appends the given attribute to <paramref name="cssClassBuilder"/>, adding a space beforehand if necessary.
        /// </summary>
        /// <param name="cssClassBuilder">The <see cref="StringBuilder"/> which will contain the appended CSS class.</param>
        /// <param name="cssClass">The class to add to the <see cref="cssClassBuilder"/></param>
        private static void AppendCssClassAttribute(StringBuilder cssClassBuilder, string cssClass)
        {
            if (cssClassBuilder.Length > 0)
            {
                cssClassBuilder.Append(" ");
            }

            cssClassBuilder.Append(cssClass);
        }
    }
}