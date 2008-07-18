// <copyright file="Utility.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Util
{
    using System.Text;
    using DotNetNuke.Common;

    /// <summary>
    /// All common, shared functionality for the Engage: Events module.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// The friendly name of this module.
        /// </summary>
        public const string DnnFriendlyModuleName = "Engage: Events";
        
        /// <summary>
        /// The host setting key base for whether this module have been configured
        /// </summary>
        public const string ModuleConfigured = "ModuleConfigured";
        
        ////public const string AdminContainer = "AdminContainer";
        ////public const string Container = "UserContainer";
        ////public const string AdminRole = "EventsAminRole";

        /// <summary>
        /// Gets the name of the desktop module folder.
        /// </summary>
        /// <value>The name of the desktop module folder.</value>
        public static string DesktopModuleFolderName
        {
            get
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append("/DesktopModules/");
                sb.Append(Globals.GetDesktopModuleByName(DnnFriendlyModuleName).FolderName);
                sb.Append("/");
                return sb.ToString();
            }
        }

        public static string TemplatesFolderName
        {
            get
            {
                return DesktopModuleFolderName + "Templates/";
            }
        }

        public static string PhysicialTemplatesFolderName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.MapPath("~" + TemplatesFolderName);
            }
        }

        /// <summary>
        /// Determines whether the specified email address is valid.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns>
        /// 	<c>true</c> if the specified email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            aspNetEmail.EmailMessage message = new aspNetEmail.EmailMessage();
            return message.IsValidEmail(emailAddress);
        }
    }
}

