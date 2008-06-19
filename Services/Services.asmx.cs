//TODO: Do we need this class?
namespace Engage.Dnn.Events.Services
{
    using System.Web.Script.Services;
    using System.Web.Services;

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
