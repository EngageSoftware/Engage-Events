//using System;
//using System.Data;
//using System.Configuration;
//using System.IO;
//using System.Text;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
//using System.Xml;
//using System.Xml.XPath;

//using DotNetNuke.Services.Exceptions;
//using DotNetNuke.Entities.Modules;
//using DotNetNuke.Entities.Users;
//using DotNetNuke.Common.Utilities;

//using Engage.Dnn.Events;
//using Engage.Dnn.Events.Portability;

//namespace Engage.Dnn.Events.Components
//{

//    /// <summary>
//    /// Features Controller Class supports IPortable currently.
//    /// </summary>
//    public class FeaturesController : IPortable
//    {

//        #region IPortable Members

//        /// <summary>
//        /// Method is invoked when portal template is imported or user selects Import Content from menu.
//        /// </summary>
//        /// <param name="ModuleID"></param>
//        /// <param name="Content"></param>
//        /// <param name="Version"></param>
//        /// <param name="UserID"></param>
//        public void ImportModule(int moduleId, string content, string version, int userId)
//        {
     
//            TransportableXmlValidator validator = new TransportableXmlValidator();
//            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

//            if (validator.Validate(stream) == false)
//            {
//                Exceptions.LogException(new Exception("Unable to import publish content due to incompatible XML file. Error: " + validator.Errors[0].ToString()));
//                return;
//            }

//            //The DNN ValidatorBase closes the stream? Must re-create. hk
//            stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
//            XPathDocument doc = new XPathDocument(stream);
//            XmlDirector director = new XmlDirector();
//            XmlTransporter builder = new XmlTransporter(moduleId);
//            director.Deconstruct(builder, doc);
//        }


//        /// <summary>
//        /// Method is invoked when portal template is created or user selects Export Content from menu.
//        /// </summary>
//        /// <param name="ModuleID"></param>
//        /// <returns></returns>
//        public string ExportModule(int moduleId)
//        {

//            bool exportAll = false;

//            //check query string for a "All" param to signal all rows, not just for a moduleId
//            if (HttpContext.Current.Request.QueryString["all"] != null) exportAll = true;

//            XmlDirector director = new XmlDirector();
//            XmlTransporter builder = new XmlTransporter(moduleId);

//            director.Construct(builder, exportAll);
//            XmlDocument doc = builder.GetDocument();

//            return doc.OuterXml;
//        }

//        #endregion

     
//    }
//}
