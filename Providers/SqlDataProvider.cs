//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace Engage.Dnn.Events.Data
{
    public class SqlDataProvider : DataProvider
    {
        private const string providerType = "data";

        #region Private Members

        private readonly ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
        private readonly string connectionString;
        private readonly string providerPath;
        private readonly string objectQualifier;
        private readonly string databaseOwner;
        private const string moduleQualifier = "Engage_";

        #endregion

        #region Constructors
        public SqlDataProvider()
        {
            Provider provider = ((Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider]);

            this.connectionString = Config.GetConnectionString();

            if (String.IsNullOrEmpty(this.connectionString))
            {
                this.connectionString = provider.Attributes["connectionString"];
            }

            //if (provider.Attributes["connectionStringName"] != "" && ConfigurationManager.AppSettings[provider.Attributes["connectionStringName"]] != "") 
            //{
            //    this.connectionString = ConfigurationManager.AppSettings[provider.Attributes["connectionStringName"]];
            //} 
            //else 
            //{
            //    this.connectionString = provider.Attributes["connectionString"];
            //}

            this.providerPath = provider.Attributes["providerPath"];

            this.objectQualifier = provider.Attributes["objectQualifier"];
            if (!String.IsNullOrEmpty(this.objectQualifier) & this.objectQualifier.EndsWith("_") == false)
            {
                this.objectQualifier += "_";
            }

            //this.objectQualifier = "Publish_";

            this.databaseOwner = provider.Attributes["databaseOwner"];
            if (!String.IsNullOrEmpty(this.databaseOwner) & this.databaseOwner.EndsWith(".") == false)
            {
                this.databaseOwner += ".";
            }
        }

        #endregion

        #region Properties
        public string ConnectionString
        {
            get { return this.connectionString; }
        }

        public string ProviderPath
        {
            get { return this.providerPath; }
        }

        public string ObjectQualifier
        {
            get { return this.objectQualifier; }
        }


        public string DatabaseOwner
        {
            get { return this.databaseOwner; }
        }

        public string NamePrefix
        {
            get { return this.databaseOwner + this.objectQualifier + moduleQualifier; }
        }


        #endregion

        //public override DataSet GetEvents(int portalId, string sortColumn, int index, int pageSize)
        //  {
        //      return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, NamePrefix + "spGetEvents",
        //         Engage.Utility.CreateIntegerParam("@portalId", portalId),
        //         Engage.Utility.CreateVarcharParam("@sortColumn", sortColumn, 200),
        //         Engage.Utility.CreateIntegerParam("@index", index),
        //         Engage.Utility.CreateIntegerParam("@pageSize", pageSize)
        //         );
        //  }
    }
}


