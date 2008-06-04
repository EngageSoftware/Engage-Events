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
using System.Data;
using System.Web.Caching;
using System.Reflection;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.Framework.Providers;
using Engage.Dnn.Events.Util;

namespace Engage.Dnn.Events.Data
{
	public abstract class DataProvider
	{
		#region Shared/Static Methods
       
        private static DataProvider provider;

		// return the provider
		public static DataProvider Instance()
		{
            if (provider == null)
            {
                string assembly = "Engage.Dnn.Events.Data.SqlDataprovider,EngageEvents";
                Type objectType = Type.GetType(assembly, true, true);

                provider = (DataProvider)Activator.CreateInstance(objectType);
                DataCache.SetCache(objectType.FullName, provider);
           }

			return provider;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not returning class state information")]
        public static IDbConnection GetConnection()
		{
			const string providerType = "data";
			ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

			Provider objProvider = ((Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);
			string _connectionString;
			if (!String.IsNullOrEmpty(objProvider.Attributes["connectionStringName"]) && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]])) 
			{
				_connectionString = System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]];
			} 
			else 
			{
				_connectionString = objProvider.Attributes["connectionString"];
			}

			IDbConnection newConnection = new System.Data.SqlClient.SqlConnection();
			newConnection.ConnectionString = _connectionString.ToString();
			newConnection.Open();
			return newConnection;
		}

		#endregion

        //public abstract DataSet GetEvents(int portalId, string sortColumn, int index, int pageSize);
             
    }
}

