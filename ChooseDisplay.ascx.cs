// <copyright file="ChooseDisplay.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2010
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
    using System.Threading;
    using System.Web.UI.WebControls;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Framework.Templating;

    /// <summary>
    /// A control allowing the user to set settings pertaining to display and templates.
    /// </summary>
    public partial class ChooseDisplay : ModuleBase
    {
        /// <summary>
        /// Gets the template used to display the event listing.
        /// </summary>
        /// <value>The template used to display the event listing.</value>
        private string TemplateSetting
        {
            get
            {
                return ModuleSettings.Template.GetValueAsStringFor(this);
            }
        }

        /// <summary>
        /// Gets the template used to display a single event.
        /// </summary>
        /// <value>The template used to display a single event</value>
        private string SingleItemTemplateSetting
        {
            get
            {
                return ModuleSettings.SingleItemTemplate.GetValueAsStringFor(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!this.PermissionsService.CanManageDisplay)
            {
                this.DenyAccess();
                return;
            }

            base.OnInit(e);
            this.Load += this.Page_Load;
            this.ChooseDisplayDropDown.SelectedIndexChanged += this.ChooseDisplayDropDown_SelectedIndexChanged;
            this.CancelButton.Click += this.CancelButton_Click;
            this.SubmitButton.Click += this.SubmitButton_Click;
        }

        /// <summary>
        /// Handles the <see cref="ListControl.SelectedIndexChanged"/> event of the <see cref="ChooseDisplayDropDown"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ChooseDisplayDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetDisplayTypeOptions();
        }

        /// <summary>
        /// Displays the proper options for the chosen display type
        /// </summary>
        private void SetDisplayTypeOptions()
        {
            this.TemplatePickersSection.Visible = this.ChooseDisplayDropDown.SelectedValue.Equals("LIST", StringComparison.OrdinalIgnoreCase);
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
                if (!this.IsPostBack)
                {
                    this.ChooseDisplayDropDown.Items.Add(new ListItem(Localization.GetString("EventListingTemplate", this.LocalResourceFile), "LIST"));
                    this.ChooseDisplayDropDown.Items.Add(new ListItem(Localization.GetString("EventCalendar", this.LocalResourceFile), "CALENDAR"));

                    string displayType = ModuleSettings.DisplayType.GetValueAsStringFor(this).ToUpperInvariant();
                    ListItem li = this.ChooseDisplayDropDown.Items.FindByValue(displayType);
                    if (li != null)
                    {
                        li.Selected = true;
                    }

                    this.SetDisplayTypeOptions();
                    this.ListTemplatePicker.SelectedTemplateFolderName = this.TemplateSetting;
                    this.SingleItemTemplatePicker.SelectedTemplateFolderName = this.SingleItemTemplateSetting;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the <see cref="Button.Click"/> event of the <see cref="CancelButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(Globals.NavigateURL(this.TabId), false);
        }

        /// <summary>
        /// Handles the <see cref="Button.Click"/> event of the <see cref="SubmitButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.TemplatePickersSection.Visible)
                {
                    ModuleSettings.Template.Set(this, this.ListTemplatePicker.SelectedTemplateFolderName);
                    this.ApplyTemplateSettings(this.ListTemplatePicker.SelectedTemplateFolderName);

                    ModuleSettings.SingleItemTemplate.Set(this, this.SingleItemTemplatePicker.SelectedTemplateFolderName);
                    this.ApplyTemplateSettings(this.SingleItemTemplatePicker.SelectedTemplateFolderName);
                }

                ModuleSettings.DisplayType.Set(this, this.ChooseDisplayDropDown.SelectedValue);

                this.Response.Redirect(Globals.NavigateURL(this.TabId));
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Applies the settings for the given template.
        /// </summary>
        /// <param name="templateFolderName">Name of the template folder for the selected template.</param>
        private void ApplyTemplateSettings(string templateFolderName)
        {
            TemplateInfo manifest = this.GetTemplate(templateFolderName);
            if (manifest.Settings != null)
            {
                foreach (KeyValuePair<string, string> setting in manifest.Settings)
                {
                    new ModuleController().UpdateTabModuleSetting(this.TabModuleId, setting.Key, setting.Value);
                }
            }
        }
    }
}