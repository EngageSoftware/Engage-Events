//Engage: Events - http://www.engagemodules.com
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
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Events.Util;

namespace Engage.Dnn.Events.Controls
{
    public partial class RecurrenceEditor : ModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RecurrenceSelector1.Editor = this;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            

        }

        public void Clear()
        {
            this.phRecurrencePattern.Controls.Clear();
        }

        public void Add(System.Web.UI.Control control)
        {
            this.phRecurrencePattern.Controls.Add(control);
        }
    }
}

