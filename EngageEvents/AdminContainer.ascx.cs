//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Data;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Events.Admin;
using Engage.Dnn.Events.Util;

namespace Engage.Dnn.Events
{
	public partial class AdminContainer : ModuleBase
	{
    	private static StringDictionary _adminControlKeys;
        private string controlToLoad;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a property")]
        public static StringDictionary GetAdminControlKeys()
		{
			if (_adminControlKeys == null)
			{
                FillAdminControlKeys();
			}
			return _adminControlKeys;
		}

		private static void FillAdminControlKeys()
		{	
			StringDictionary adminControlKeys = new StringDictionary();

            adminControlKeys.Add("RsvpSummary", "RsvpSummary.ascx");
            adminControlKeys.Add("RsvpDetail", "RsvpDetail.ascx");
	
			_adminControlKeys = adminControlKeys;			
		}

		#region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            ReadQueryString();
            LoadControlType();

            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            //this.Load += new System.EventHandler(this.Page_Load);
            ModuleConfiguration.ModuleTitle = Localization.GetString("Title", LocalResourceFile);

        }

        #endregion
	

		#region " Private Methods "

		private void ReadQueryString()
		{
			StringDictionary returnDict = GetAdminControlKeys();
			string adminTypeParam = Request.Params["adminKey"];

			if(Engage.Utility.HasValue(adminTypeParam))
			{
				controlToLoad = returnDict[adminTypeParam.ToLower(CultureInfo.InvariantCulture)];
			}
			else
			{
                controlToLoad = "RsvpSummary.ascx";
			}
						
            //if (!IsSetup)
            //{
            //    controlToLoad = "Admin/AdminSettings.ascx";
            //}
		}

		private void LoadControlType()
		{

            //AdminMainBase mb = (AdminMainBase)LoadControl("Admin/AdminMain.ascx");
            //mb.ModuleConfiguration = ModuleConfiguration;
            
            //mb.ID = System.IO.Path.GetFileNameWithoutExtension("Admin/AdminMain.ascx");
            //phAdminControls.Controls.Add(mb);

            ModuleBase amb;
            amb = (ModuleBase)LoadControl(controlToLoad);
            amb.ModuleConfiguration = ModuleConfiguration;
            amb.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
            phControls.Controls.Add(amb);
		}
		#endregion
	}
}

