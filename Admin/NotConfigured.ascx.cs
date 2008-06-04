//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace Engage.Dnn.Events
{
    public partial class NotConfigured : ModuleBase
    {
        
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
          
            base.OnInit(e);

            lnkConfigure.Text = Localization.GetString("UnableToFindAction", LocalResourceFile);
            lnkConfigure.NavigateUrl = EditUrl("ModuleId", ModuleId.ToString(CultureInfo.InvariantCulture), "Module");
            lnkConfigure.Visible = true;

        }

        #endregion
    }
}

