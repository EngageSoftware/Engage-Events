//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Events.Data;
using Engage.Dnn.Events.Util;

namespace Engage.Dnn.Events
{
    public partial class MainContainer : ModuleBase
    {
        private static StringDictionary _controlKeys;
        private string controlToLoad;

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
          
            base.OnInit(e);

            ReadItemType();
            LoadControlType();

            //add on a query string param so that we can grab ALL content, not just for this moduleId. hk
            string title = Localization.GetString(ModuleActionType.ExportModule, Localization.GlobalResourceFile);
            foreach (ModuleAction action in Actions)
            {
                if (action.Title == title)
                {
                    action.Url = action.Url + "?all=1";
                    break;
                }

            }
        }

        #endregion

        #region Private Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a property")]
        public static StringDictionary GetAdminControlKeys()
        {
            if (_controlKeys == null)
            {
                FillControlKeys();
            }
            return _controlKeys;
        }

        private static void FillControlKeys()
        {
            StringDictionary controlKeys = new StringDictionary();

            controlKeys.Add("EmailEdit", "EmailEdit.ascx");
            controlKeys.Add("EventEdit", "EventEdit.ascx");
            controlKeys.Add("EventListing", "EventListing.ascx");
            controlKeys.Add("EventListingCustom", "EventListingCustom.ascx");
            controlKeys.Add("EventListingGridStyle", "EventListingGridStyle.ascx");
            controlKeys.Add("EventListingAdmin", "EventListingAdmin.ascx");
            controlKeys.Add("RsvpSummary", "RsvpSummary.ascx");
            controlKeys.Add("RsvpDetail", "RsvpDetail.ascx");
            controlKeys.Add("Rsvp", "Rsvp.ascx");
            controlKeys.Add("EmailAFriend", "EmailAFriend.ascx");
            
            _controlKeys = controlKeys;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Code paths are easy to understand, test, and maintain")]
        private void ReadItemType()
        {

            StringDictionary returnDict = GetAdminControlKeys();
            string keyParam = Request.Params["key"];

            if (Engage.Utility.HasValue(keyParam))
            {
                controlToLoad = returnDict[keyParam.ToLower(CultureInfo.InvariantCulture)];
            }
            else
            {
                if (IsAdmin)
                {
                    //display the admin version
                    controlToLoad = "EventListingAdmin.ascx";

                }
                else
                {
                    //display unathenticated version to user based on setting by administrator.
                    object o = Settings["DisplayType"];
                    if (o != null && !String.IsNullOrEmpty(o.ToString()))
                    {
                        controlToLoad = o.ToString() + ".ascx";
                    }
                    else
                    {
                        controlToLoad = "EventListingCustom.ascx";
                    }
                }
            }

            //if (!IsSetup)
            //{
            //    controlToLoad = "Admin/AdminSettings.ascx";
            //}
        }

        private void LoadControlType()
        {
            try
            {

                if (controlToLoad == null) return;

                ModuleBase mb = (ModuleBase)LoadControl(controlToLoad);
                mb.ModuleConfiguration = ModuleConfiguration;
                mb.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
                phControls.Controls.Add(mb);

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        //public ModuleActionCollection ModuleActions
        //{
        //    get
        //    {
        //        ModuleActionCollection Actions = new ModuleActionCollection();
        //        Actions.Add(GetNextActionID(), Localization.GetString("RsvpMenu", this.LocalResourceFile), "", "", "", EditUrl(Engage.Dnn.Events.Util.Utility.AdminContainer), false, SecurityAccessLevel.Edit, true, false);
        //        return Actions;
        //    }
        //}
    }
}

