 // <copyright file="RespondPage.aspx.cs" company="Engage Software">
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
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
    using DotNetNuke.UI.Utilities;
    using Globals = DotNetNuke.Common.Globals;

    /// <summary>
    /// Code behind for a page that loads the <see cref="Respond"/> control.
    /// </summary>
    public partial class RespondPage : PageBase
    {
        /// <summary>
        /// Gets the tab ID of the module calling this page.
        /// </summary>
        /// <value>The tab ID of the calling module.</value>
        private int TabId
        {
            get
            {
                int tabId;
                if (int.TryParse(this.Request.QueryString["TabId"], NumberStyles.Integer, CultureInfo.InvariantCulture, out tabId))
                {
                    return tabId;
                }

                return new ModuleController().GetModuleByDefinition(this.PortalSettings.PortalId, Utility.DesktopModuleName).TabID;
            }
        }

        /// <summary>
        /// Gets the module ID of the module calling this page.
        /// </summary>
        /// <value>The module ID of the calling module.</value>
        private int ModuleId
        {
            get
            {
                int moduleId;
                if (int.TryParse(this.Request.QueryString["ModuleId"], NumberStyles.Integer, CultureInfo.InvariantCulture, out moduleId))
                {
                    return moduleId;
                }

                return new ModuleController().GetModuleByDefinition(this.PortalSettings.PortalId, Utility.DesktopModuleName).ModuleID;
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            AJAX.AddScriptManager(this.Page);

            base.OnInit(e);
            this.Load += this.Page_Load;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.RespondControl.ModuleConfiguration = new ModuleController().GetModule(this.ModuleId, this.TabId);
            this.LoadStylesheets();
        }

        /// <summary>
        /// Loads the CSS stylesheets for this page
        /// </summary>
        /// <remarks>
        /// Based on the method of the same name in <see cref="DotNetNuke.Framework.CDefault"/> (version 4.8.2),
        /// combined with similar code in InjectSkin,
        /// translated through http://www.codechanger.com/.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.LastIndexOf(System.String)", Justification = "Mimicking the behavior of DNN core"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.EndsWith(System.String)", Justification = "Mimicking the behavior of DNN core"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower", Justification = "Mimicking the behavior of DNN core")]
        private void LoadStylesheets()
        {
            string id;
            Hashtable stylesheetCache = DataCache.GetCache("CSS") as Hashtable ?? new Hashtable();

            // module stylesheet
            bool saveCache = false;
            ModuleInfo moduleConfiguration = this.RespondControl.ModuleConfiguration;
            if (moduleConfiguration.ControlSrc.ToLower().EndsWith(".ascx"))
            {
                // Get module.css from Path to control
                id = Globals.CreateValidID(Globals.ApplicationPath + "/" + moduleConfiguration.ControlSrc.Substring(0, moduleConfiguration.ControlSrc.LastIndexOf("/")));
                if (!stylesheetCache.ContainsKey(id))
                {
                    string moduleControlStylesheetPath = Globals.ApplicationPath + "/" + moduleConfiguration.ControlSrc.Substring(0, moduleConfiguration.ControlSrc.LastIndexOf("/") + 1);
                    if (File.Exists(HostingEnvironment.MapPath(moduleControlStylesheetPath) + "module.css"))
                    {
                        stylesheetCache[id] = moduleControlStylesheetPath + "module.css";
                    }
                    else
                    {
                        stylesheetCache[id] = string.Empty;
                    }

                    saveCache = true;
                }
            }
            else
            {
                // Get module.css from Folder
                id = Globals.CreateValidID(Globals.ApplicationPath + "/" + moduleConfiguration.FolderName);
                if (!stylesheetCache.ContainsKey(id))
                {
                    string moduleStylesheetPath = Globals.ApplicationPath + "/" + moduleConfiguration.FolderName + "/module.css";
                    if (File.Exists(HostingEnvironment.MapPath(moduleStylesheetPath)))
                    {
                        stylesheetCache[id] = moduleStylesheetPath;
                    }
                    else
                    {
                        stylesheetCache[id] = string.Empty;
                    }

                    saveCache = true;
                }
            }

            if (saveCache && Globals.PerformanceSetting != Globals.PerformanceSettings.NoCaching)
            {
                DataCache.SetCache("CSS", stylesheetCache);
            }

            if (!string.IsNullOrEmpty(stylesheetCache[id].ToString()))
            {
                // Add it to beginning of style list
                this.AddStylesheet(id, stylesheetCache[id].ToString());
            }
            
            // default style sheet ( required )
            id = Globals.CreateValidID(Globals.HostPath);
            this.AddStylesheet(id, Globals.HostPath + "default.css");

            // skin package style sheet
            id = Globals.CreateValidID(this.PortalSettings.ActiveTab.SkinPath);
            if (!stylesheetCache.ContainsKey(id))
            {
                if (File.Exists(HostingEnvironment.MapPath(this.PortalSettings.ActiveTab.SkinPath) + "skin.css"))
                {
                    stylesheetCache[id] = this.PortalSettings.ActiveTab.SkinPath + "skin.css";
                }
                else
                {
                    stylesheetCache[id] = string.Empty;
                }

                if (Globals.PerformanceSetting != Globals.PerformanceSettings.NoCaching)
                {
                    DataCache.SetCache("CSS", stylesheetCache);
                }
            }

            if (!string.IsNullOrEmpty(stylesheetCache[id].ToString()))
            {
                this.AddStylesheet(id, stylesheetCache[id].ToString());
            }

            // skin file style sheet
            string skinFileStylesheetPath = this.PortalSettings.ActiveTab.SkinSrc.Replace(".ascx", ".css");
            id = Globals.CreateValidID(skinFileStylesheetPath);
            if (!stylesheetCache.ContainsKey(id))
            {
                if (File.Exists(HostingEnvironment.MapPath(skinFileStylesheetPath)))
                {
                    stylesheetCache[id] = skinFileStylesheetPath;
                }
                else
                {
                    stylesheetCache[id] = string.Empty;
                }

                if (Globals.PerformanceSetting != Globals.PerformanceSettings.NoCaching)
                {
                    DataCache.SetCache("CSS", stylesheetCache);
                }
            }

            if (!string.IsNullOrEmpty(stylesheetCache[id].ToString()))
            {
                this.AddStylesheet(id, stylesheetCache[id].ToString());
            }

            // portal style sheet
            id = Globals.CreateValidID(this.PortalSettings.HomeDirectory);
            this.AddStylesheet(id, this.PortalSettings.HomeDirectory + "portal.css");
        }

        /// <summary>
        /// Adds the stylesheet to the page header.
        /// </summary>
        /// <remarks>
        /// Based on the method of the same name in <see cref="DotNetNuke.Framework.CDefault"/> (version 4.8.2),
        /// translated through http://www.codechanger.com/.
        /// </remarks>
        /// <param name="id">The id to use for the stylesheet link.</param>
        /// <param name="path">The path to the stylesheet.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "stylesheetLink is passed to the Page Controls collection and does not need to be manually disposed")]
        private void AddStylesheet(string id, string path)
        {
            // First see if we have already added the <LINK> control
            if (this.Page.Header.FindControl(id) == null)
            {
                HtmlLink stylesheetLink = new HtmlLink();
                stylesheetLink.ID = id;
                stylesheetLink.Attributes["rel"] = "stylesheet";
                stylesheetLink.Attributes["type"] = "text/css";
                stylesheetLink.Href = path;

                this.Page.Header.Controls.Add(stylesheetLink);
            }
        }
    }
}