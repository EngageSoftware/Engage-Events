// <copyright file="RespondPage.aspx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
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
    using System.Collections;
    using System.IO;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using DotNetNuke.Framework;
    using DotNetNuke.UI.Utilities;
    using Globals = DotNetNuke.Common.Globals;

    /// <summary>
    /// Code behind for a page that loads the <see cref="Respond"/> control.
    /// </summary>
    public partial class RespondPage : PageBase
    {
        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
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
            this.LoadStylesheets();
        }

        /// <summary>
        /// Loads the CSS stylesheets for this page
        /// </summary>
        /// <remarks>
        /// Based on the method of the same name in <see cref="DotNetNuke.Framework.CDefault"/> (version 4.8.2),
        /// translated through http://www.codechanger.com/.
        /// </remarks>
        private void LoadStylesheets()
        {
            Hashtable stylesheetCache = DataCache.GetCache("CSS") as Hashtable ?? new Hashtable();

            // default style sheet ( required )
            string id = Globals.CreateValidID(Globals.HostPath);
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