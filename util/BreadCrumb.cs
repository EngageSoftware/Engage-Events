//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Web.SessionState;
using DotNetNuke.Services.Localization;


namespace Engage.Dnn.Events.Util
{
	/// <summary>
	/// Summary description for BreadCrumb.
	/// </summary>
	public static class BreadCrumb
	{
		private const int MaxBreadCrumbs = 4;

		public static void Add(string pageName, string pageUrl)
		{
			ArrayList q = GetBreadCrumbs();
			BreadCrumbItem bci = new BreadCrumbItem();
			bci.PageName = pageName;
			bci.PageUrl= pageUrl;

			q.Remove(bci);
			q.Add(bci);

			if (q.Count > MaxBreadCrumbs) q.RemoveAt(0);

			//if (q.Contains(bci) == false) q.Enqueue(bci);
			//if (q.Count >= MaxBreadCrumbs) q.Dequeue();	
		}

		private static ArrayList GetBreadCrumbs()
		{
			HttpSessionState session = HttpContext.Current.Session;
			ArrayList q = (ArrayList) session["breadCrumbs"];
			if (q == null)
			{
				q = new ArrayList(MaxBreadCrumbs);
				session.Add("breadCrumbs", q);
			}

			return q;
		}

		public static string ToString()
		{
			ArrayList q = GetBreadCrumbs();
			StringBuilder sb = new StringBuilder(q.Count * 100);
        
            //TODO: breadcrumb text is hardcoded!


			//sb.Append("Recent&nbsp;Pages&nbsp;");
            sb.Append(Localization.GetString("RecentPages", Utility.DesktopModuleFolderName + "util/app_localresources/BreadCrumb.cs.resx"));
			
			IEnumerator ie = q.GetEnumerator();
			while (ie.MoveNext())
			{
				BreadCrumbItem bci = (BreadCrumbItem) ie.Current;
				sb.Append("&gt;&gt; <a href=\"");
				sb.Append(bci.PageUrl);
				sb.Append("\" class=\"RecentPagesLink\">");
                sb.Append(bci.PageName);
				sb.Append("</a> ");
			}

			return sb.ToString();
		}
	}

	public class BreadCrumbItem
	{
		private string pageName;
		private string pageUrl;

		public string PageName
		{
			get {return (this.pageName == null ? string.Empty : this.pageName);}
			set {this.pageName = value;}
		}

		public string PageUrl
		{
			get {return (this.pageUrl == null ? string.Empty : this.pageUrl);}
			set {this.pageUrl = value;}
		}

		public override bool Equals(object obj)
		{
			BreadCrumbItem bci = obj as BreadCrumbItem;
			if (bci == null)
			{
				return false;
			}
			else
			{
				return (bci.pageName == this.pageName && bci.pageUrl == this.pageUrl);
			}
		}

		public override int GetHashCode()
		{
			return (this.pageName.GetHashCode() + this.pageUrl.GetHashCode());
		}
	}
}

