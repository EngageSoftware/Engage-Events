using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security.Membership;
using DotNetNuke.Entities.Users;

namespace Engage.Dnn.Events
{
    /// <summary>
    /// Summary description for Register.
    /// </summary>
    public partial class Register : ModuleBase
    {
        private string parentTitle;

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        #region Events

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (UserInfo.UserID > 0) Response.Redirect(RsvpUrl, true);

            //Localize the linkbuttons
            //btnCart.Text = Localization.GetString("btnCart");
            //btnProfile.Text = Localization.GetString("btnProfile");
            //btnOrders.Text = Localization.GetString("btnOrders");

            try
            {
            }
            catch (Exception ex)
            {
                string ErrorSettings = Localization.GetString("ErrorSettings", this.LocalResourceFile);
                Exceptions.ProcessModuleLoadException(ErrorSettings, this, ex, true);
            }
        }

        #endregion

        protected void btnCreate_Click(object sender, ImageClickEventArgs e)
        {
            string href = DotNetNuke.Common.Globals.NavigateURL("", "ctl=register&returnurl=" + Server.UrlEncode(Request.Url.PathAndQuery));
            //returnurl = here, if we are then logged in we will be redirected to checkout wizard
            Response.Redirect(href, true);
        }

        protected bool UseCaptcha
        {
            get
            {
                object setting = UserModuleBase.GetSetting(PortalId, "Security_CaptchaLogin");
                return Convert.ToBoolean(setting);
            }
        }

        private string RsvpUrl
        {
            get
            {
                string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=Rsvp&eventid=" + EventId.ToString());
                return href;
            }
        }

        protected void cmdLogin_Click(object sender, ImageClickEventArgs e)
        {
            if ((UseCaptcha && ctlCaptcha.IsValid) || (!UseCaptcha))
            {
                UserLoginStatus status = UserLoginStatus.LOGIN_FAILURE;
                UserInfo user = UserController.ValidateUser(PortalId, txtUsername.Text, txtPassword.Text, "", PortalSettings.PortalName, Request.UserHostAddress, ref status);

                //'Check if the User has valid Password/Profile
                switch (status)
                {
                    case UserLoginStatus.LOGIN_SUCCESS:
                        //Complete Login
                        UserController.UserLogin(PortalId, user, PortalSettings.PortalName, Request.UserHostAddress, false);
                        Response.Redirect(RsvpUrl);
                        break;
                    case UserLoginStatus.LOGIN_SUPERUSER:
                        //Complete Login
                        UserController.UserLogin(PortalId, user, PortalSettings.PortalName, Request.UserHostAddress, false);
                        Response.Redirect(RsvpUrl);
                        break;
                    default:
                        Response.Redirect(RsvpUrl);
                        break;
                }
            }
        }
    }
}
