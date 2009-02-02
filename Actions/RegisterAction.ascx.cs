// <copyright file="RegisterAction.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Web;
    using DotNetNuke.Common;
    using DotNetNuke.UI.Skins;

    /// <summary>
    /// Displays the actions that users can perform on an event instance.
    /// </summary>
    /// <remarks>
    /// This control's behavior changed from using LinkButtons to standard buttons. Something to do with a postback
    /// not occurring on the container form. Not sure why? Anyhow, it stores the EventID in viewstate and uses it if needed. hk
    /// Note: the visibility of this control must be done outside by calling code.
    /// </remarks>
    public partial class RegisterAction : ActionControlBase
    {
        /// <summary>
        /// Gets or sets the text to display on this button.
        /// </summary>
        /// <value>The text to display on this button</value>
        public string Text
        {
            get { return this.RegisterButton.Text; }
            set { this.RegisterButton.Text = value; }
        }

        /// <summary>
        /// Performs all necessary operations to display the control's data correctly.
        /// </summary>
        protected override void BindData()
        {
            LocalizeControls();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.LocalResourceFile = TemplateResourceFile;
            this.Load += this.Page_Load;
        }

        /// <summary>
        /// Localizes this control's child controls.
        /// </summary>
        private static void LocalizeControls()
        {
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.SetupFancyBox();
        }

        /// <summary>
        /// Sets up the FancyBox plugin to allow registration within an on-page popup.
        /// </summary>
        private void SetupFancyBox()
        {
            const string FancyboxWireupScript =
                @"jQuery(function() {{
                     InitializeBehaviors();

                     var scriptManager = Sys.WebForms.PageRequestManager.getInstance();
                     if (scriptManager) {
                        scriptManager.add_endRequest(InitializeBehaviors);
                     }
                }});      

                function InitializeBehaviors() {{          
                    jQuery('a.PopupTriggerLink').fancybox(); 
                    jQuery('input.RegisterButton').click(function(event) {{
                        event.preventDefault();
                        jQuery(this).siblings('a.PopupTriggerLink').click();
                    }});
                }}";

            this.AddJQueryReference();
            this.Page.ClientScript.RegisterClientScriptResource(typeof(RegisterAction), "Engage.Dnn.Events.JavaScript.jquery.fancybox-1.0.0.js");
            this.Page.ClientScript.RegisterStartupScript(typeof(RegisterAction), "FancyBox wire-up", FancyboxWireupScript, true);

            // We're simulating DNN Print Mode to eliminate the other modules on the page while still loading the correct skin/container stuff
            string containerSrc = this.ModuleConfiguration.ContainerSrc;
            if (containerSrc.EndsWith(".ASCX", StringComparison.OrdinalIgnoreCase))
            {
                containerSrc = containerSrc.Substring(0, containerSrc.Length - 5);
            }

            // TODO: handle case when used is not logged in
            this.PopupTriggerLink.NavigateUrl = this.BuildLinkUrl(this.ModuleId, "Response", Utility.GetEventParameters(this.CurrentEvent.Id, this.CurrentEvent.EventStart, "mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture), "SkinSrc=" + HttpUtility.UrlEncode("[G]" + SkinInfo.RootSkin + "/" + Globals.glbHostSkinFolder + "/" + "No Skin"), "ContainerSrc=" + HttpUtility.UrlEncode(containerSrc), "dnnprintmode=true"));
        }
    }
}