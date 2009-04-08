// <copyright file="Settings.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2009
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
    using System.Globalization;
    using System.IO;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    /// <summary>
    /// This is the settings code behind for Event related Settings.
    /// </summary>
    public partial class Settings : ModuleSettingsBase
    {
        /// <summary>
        /// Backing field for the current module settings base selected.
        /// </summary>
        private ModuleSettingsBase currentSettingsBase;

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();
            try
            {
                if (!this.IsPostBack)
                {
                    this.DropDownChooseDisplay.Items.Add(new ListItem(Localization.GetString("EventListingTemplate", this.LocalResourceFile), "LIST"));
                    this.DropDownChooseDisplay.Items.Add(new ListItem(Localization.GetString("EventCalendar", this.LocalResourceFile), "CALENDAR"));

                    Dnn.Utility.LocalizeGridView(ref this.DetailsDisplayModuleGrid, this.LocalResourceFile);
                    this.DetailsDisplayModuleGrid.DataSource = new ModuleController().GetModulesByDefinition(this.PortalId, Utility.ModuleDefinitionFriendlyName);
                    this.DetailsDisplayModuleGrid.DataBind();

                    this.SetOptions();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Updates the settings.
        /// </summary>
        public override void UpdateSettings()
        {
            if (this.Page.IsValid)
            {
                try
                {
                    ModuleController modules = new ModuleController();
                    modules.UpdateTabModuleSetting(this.TabModuleId, "DisplayType", this.DropDownChooseDisplay.SelectedValue);
                    modules.UpdateTabModuleSetting(this.TabModuleId, "FeaturedOnly", this.FeaturedCheckBox.Checked.ToString(CultureInfo.InvariantCulture));
                    modules.UpdateTabModuleSetting(this.TabModuleId, "DetailsDisplayTabId", this.GetSelectedDetailsDisplayTabId().ToString(CultureInfo.InvariantCulture));
                    modules.UpdateTabModuleSetting(this.TabModuleId, "DetailsDisplayModuleId", this.GetSelectedDetailsDisplayModuleId().ToString(CultureInfo.InvariantCulture));

                    this.currentSettingsBase.UpdateSettings();
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += this.Page_Load;
            this.DropDownChooseDisplay.SelectedIndexChanged += this.DropDownChooseDisplay_SelectedIndexChanged;
            this.DetailsDisplayModuleValidator.ServerValidate += this.DetailsDisplayModuleValidator_ServerValidate;
        }

        /// <summary>
        /// Handles the <see cref="CheckBox.CheckedChanged"/> event of the <c>DetailsDisplayModuleRadioButton</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void DetailsDisplayModuleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            TableRow selectedRow = Engage.Utility.FindParentControl<TableRow>((RadioButton)sender);            
            this.SetDetailsDisplayModuleGridSelection(GetTabIdFromRow(selectedRow), GetModuleIdFromRow(selectedRow));
        }

        /// <summary>
        /// Gets the tab ID of the given <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The row from which to get the tab ID.</param>
        /// <returns>The tab ID of the given <paramref name="row"/>.</returns>
        private static int GetTabIdFromRow(TableRow row)
        {
            return int.Parse(row.Cells[2].Text, NumberStyles.Integer, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the module ID of the given <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The row from which to get the module ID.</param>
        /// <returns>The module ID of the given <paramref name="row"/>.</returns>
        private static int GetModuleIdFromRow(TableRow row)
        {
            return int.Parse(row.Cells[4].Text, NumberStyles.Integer, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.DisplaySettingsControl();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the DropDownChooseDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DropDownChooseDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DisplaySettingsControl();
        }

        /// <summary>
        /// Handles the <see cref="CustomValidator.ServerValidate"/> event of the <see cref="DetailsDisplayModuleValidator"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        private void DetailsDisplayModuleValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int checkedCount = 0;
            foreach (GridViewRow row in this.DetailsDisplayModuleGrid.Rows)
            {
                RadioButton detailsDisplayModuleRadioButton = (RadioButton)row.FindControl("DetailsDisplayModuleRadioButton");
                if (detailsDisplayModuleRadioButton.Checked)
                {
                    checkedCount++;
                }
            }

            args.IsValid = checkedCount == 1;
        }

        /// <summary>
        /// Sets the options.
        /// </summary>
        private void SetOptions()
        {
            string displayType = Dnn.Utility.GetStringSetting(this.Settings, "DisplayType").ToUpperInvariant();

            ListItem li = this.DropDownChooseDisplay.Items.FindByValue(displayType);
            if (li != null)
            {
                li.Selected = true;
            }

            this.FeaturedCheckBox.Checked = Dnn.Utility.GetBoolSetting(this.Settings, "FeaturedOnly", false);
            this.SetDetailsDisplayModuleGridSelection(Dnn.Utility.GetIntSetting(this.Settings, "DetailsDisplayTabId", this.TabId), Dnn.Utility.GetIntSetting(this.Settings, "DetailsDisplayModuleId", this.ModuleId));
        }

        /// <summary>
        /// Selects the <see cref="RadioButton"/> for the row in <see cref="DetailsDisplayModuleGrid"/> with the given <paramref name="selectedTabId"/> and <paramref name="selectedModuleId"/>.
        /// </summary>
        /// <param name="selectedTabId">The Tab ID of the row to select.</param>
        /// /// <param name="selectedModuleId">The Module ID of the row to select.</param>
        private void SetDetailsDisplayModuleGridSelection(int selectedTabId, int selectedModuleId)
        {
            foreach (GridViewRow row in this.DetailsDisplayModuleGrid.Rows)
            {
                RadioButton detailsDisplayModuleRadioButton = (RadioButton)row.FindControl("DetailsDisplayModuleRadioButton");

                detailsDisplayModuleRadioButton.Checked = GetTabIdFromRow(row) == selectedTabId && GetModuleIdFromRow(row) == selectedModuleId;
            }
        }

        /// <summary>
        /// Gets the selected Tab ID for the DetailsDisplayTabId setting.
        /// </summary>
        /// <returns>The TabID that was selected for the Details Display TabID setting</returns>
        /// <exception cref="InvalidOperationException">This method cannot be called until validation has run, ensuring that a Tab ID has been selected</exception>
        private int GetSelectedDetailsDisplayTabId()
        {
            foreach (GridViewRow row in this.DetailsDisplayModuleGrid.Rows)
            {
                RadioButton detailsDisplayModuleRadioButton = (RadioButton)row.FindControl("DetailsDisplayModuleRadioButton");
                if (detailsDisplayModuleRadioButton.Checked)
                {
                    return GetTabIdFromRow(row);
                }
            }

            throw new InvalidOperationException("This method cannot be called until validation has run, ensuring that a Tab ID has been selected");
        }

        /// <summary>
        /// Gets the selected Module ID for the DetailsDisplayModuleId setting.
        /// </summary>
        /// <returns>The ModuleID that was selected for the Details Display ModuleID setting</returns>
        /// <exception cref="InvalidOperationException">This method cannot be called until validation has run, ensuring that a Module ID has been selected</exception>
        private int GetSelectedDetailsDisplayModuleId()
        {
            foreach (GridViewRow row in this.DetailsDisplayModuleGrid.Rows)
            {
                RadioButton detailsDisplayModuleRadioButton = (RadioButton)row.FindControl("DetailsDisplayModuleRadioButton");
                if (detailsDisplayModuleRadioButton.Checked)
                {
                    return GetModuleIdFromRow(row);
                }
            }

            throw new InvalidOperationException("This method cannot be called until validation has run, ensuring that a Module ID has been selected");
        }

        /// <summary>
        /// Displays the settings control.
        /// </summary>
        private void DisplaySettingsControl()
        {
            string selectedDisplayType = this.DropDownChooseDisplay.SelectedValue;
            switch (selectedDisplayType.ToUpperInvariant())
            {
                case "LIST":
                    this.LoadSettingsControl("Display/TemplateDisplayOptions.ascx");
                    break;
                case "CALENDAR":
                    this.LoadSettingsControl("Display/CalendarDisplayOptions.ascx");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Loads the settings control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        private void LoadSettingsControl(string controlName)
        {
            this.ControlsPlaceholder.EnableViewState = false;
            this.ControlsPlaceholder.Controls.Clear();

            this.currentSettingsBase = this.CreateSettingsControl(controlName);
            this.ControlsPlaceholder.Controls.Add(this.currentSettingsBase);
        }

        /// <summary>
        /// Creates the settings control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        /// <returns>Module Settings Base</returns>
        private ModuleSettingsBase CreateSettingsControl(string controlName)
        {
            ModuleSettingsBase settingsControl = (ModuleSettingsBase)this.LoadControl(controlName);
            ModuleController mc = new ModuleController();
            ModuleInfo mi = mc.GetModule(this.ModuleId, this.TabId);
            settingsControl.ModuleConfiguration = mi;
            settingsControl.ID = Path.GetFileNameWithoutExtension(controlName);
            settingsControl.LoadSettings();

            return settingsControl;
        }
    }
}