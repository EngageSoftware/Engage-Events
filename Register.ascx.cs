// <copyright file="Register.ascx.cs" company="Engage Software">
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
    using System.Web;
    using DotNetNuke.Common;

    /// <summary>
    /// The code-behind for the <c>Register.ascx</c> control, which allows the user to specify whether they are or are not attending an event.
    /// </summary>
    public partial class Register : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
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
            if (Engage.Utility.IsLoggedIn)
            {
                this.Response.Redirect(this.ResponseUrl, true);
            }

            this.SetupLinks();
        }

        /// <summary>
        /// Sets up the login and register links destinations.
        /// </summary>
        private void SetupLinks()
        {
            this.RegisterLink.NavigateUrl = Globals.NavigateURL("Register", "returnUrl=" + HttpUtility.UrlEncode(this.Request.RawUrl));
            this.LogOnLink.NavigateUrl = Dnn.Utility.GetLoginUrl(this.PortalSettings, this.Request);
        }
    }
}