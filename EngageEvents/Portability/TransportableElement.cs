﻿////Engage: Events - http://www.engagemodules.com
////Copyright (c) 2004-2008
////by Engage Software ( http://www.engagesoftware.com )

////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
////TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
////THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
////CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
////DEALINGS IN THE SOFTWARE.

//using System;
//using System.Data;
//using System.Configuration;
//using System.IO;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml;
//using System.Xml.Serialization;

//namespace Engage.Dnn.Events.Portability
//{
//    public abstract class TransportableElement
//    {

//        /// <summary>
//        /// Method to convert a custom Object to XML string
//        /// </summary>
//        /// <param name="pObject">Object that is to be serialized to XML</param>
//        /// <returns>XML string</returns>
//        public string SerializeObject()
//        {
//            try
//            {
//                string xml = null;
//                MemoryStream memoryStream = new MemoryStream();
//                XmlSerializer xs = new XmlSerializer(this.GetType());
//                //XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.UTF8);
//                StringWriter writer = new StringWriter();

//                xs.Serialize(writer, this);

//                return writer.GetStringBuilder().ToString();
//            }
//            catch (Exception e)
//            {
//                DotNetNuke.Services.Exceptions.Exceptions.LogException(e);
//                return null;
//            }
//        }

//        public abstract void Import(int currentModuleId, int portalId);

//    }
//}
