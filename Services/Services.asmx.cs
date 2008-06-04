using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;

namespace Engage.Dnn.Events.Services
{
    /// <summary>
    /// Summary description for PublishServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class Services : System.Web.Services.WebService
    {
        [WebMethod][ScriptMethod]
        public string[] GetEvents()
        {
            return null;
        }



    }
}
