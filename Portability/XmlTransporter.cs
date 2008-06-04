////Engage: Events - http://www.engagemodules.com
////Copyright (c) 2004-2008
////by Engage Software ( http://www.engagesoftware.com )

////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
////TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
////THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
////CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
////DEALINGS IN THE SOFTWARE.

//using System;
//using System.Configuration;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
//using System.Xml;
//using System.Xml.Serialization;
//using System.Xml.XPath;

//using DotNetNuke.Entities.Modules;
//using DotNetNuke.Common.Utilities;

//using Engage.Dnn.Events.Util;

//namespace Engage.Dnn.Events.Portability
//{
//    public class XmlTransporter
//    {
//        private XmlDocument _doc;
//        private int _moduleId = -1;
//        private int _portalId = -1;
//        private string _version = string.Empty;

//        public XmlTransporter(int moduleId)
//        {
//            this._moduleId = moduleId;

//            using (IDataReader dr = Data.DataProvider.Instance().GetModuleInfo(moduleId))
//            {
//                if (dr.Read())
//                {
//                    //This might/should be CurrentPortalId????
//                    _portalId = (int)dr["PortalID"];
//                    _version = dr["Version"].ToString();
//                }
//            }
//        }

//        #region Construct Methods

//        public void BuildRootNode()
//        {
//            _doc = new XmlDocument();

//            string xsiNS = "http://www.w3.org/2001/XMLSchema-instance";
//            XmlElement element = _doc.CreateElement("publish");
//            element.SetAttribute("xmlns:xsi", xsiNS);
//            element.SetAttribute("noNamespaceSchemaLocation", xsiNS, "Content.Publish.xsd");
//            //element.SetAttribute("version", _version);
//            _doc.AppendChild(element);
//            XmlNode publishNode = _doc.AppendChild(element);

//        }

//        public void BuildItemTypes()
//        {

//        }

//        public void BuildRelationshipTypes()
//        {

//        }

//        public void BuildApprovalStatusTypes()
//        {

//        }

//        public void BuildCategories(bool exportAll)
//        {
//            XmlNode publishNode = _doc.SelectSingleNode("publish");
//            XmlNode categoriesNode = _doc.CreateElement("categories");
//            DataTable dt = null;

//            if (exportAll)
//            {
//                dt = Category.GetCategoriesByPortalId(_portalId); 
//            }
//            else
//            {
//                dt = Category.GetCategoriesByModuleId(_moduleId);
//            }

//            try
//            {
//                foreach (DataRow row in dt.Rows)
//                {
//                    int itemVersionId = (int)row["itemVersionId"];
//                    int portalId = Convert.ToInt32(row["PortalId"]);
//                    Category c = Category.GetCategoryVersion(itemVersionId, portalId);

//                    string xml = c.SerializeObject();
//                    XmlDocument categoryDoc = new XmlDocument();
//                    categoryDoc.LoadXml(xml);

//                    //strip off namespace and schema
//                    XmlNode node = categoryDoc.SelectSingleNode("category");
//                    node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
//                    node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

//                    categoriesNode.AppendChild(_doc.ImportNode(node, true));
//                }

//                publishNode.AppendChild(_doc.ImportNode(categoriesNode, true));

//            }
//            catch (Exception e)
//            {
//                System.Diagnostics.Debug.Write(e);
//            }

//        }

//        public void BuildArticles(bool exportAll)
//        {
//            XmlNode publishNode = _doc.SelectSingleNode("publish");
//            XmlNode articlesNode = _doc.CreateElement("articles");
//            DataTable dt = null;

//            if (exportAll)
//            {
//                dt = Article.GetArticlesByPortalId(_portalId);
//            }
//            else
//            {
//                dt = Article.GetArticlesByModuleId(_moduleId);
//            }


//            foreach (DataRow row in dt.Rows)
//            {
//                int itemVersionId = (int)row["itemVersionId"];
//                int portalId = Convert.ToInt32(row["PortalId"]);

//                Article a = Article.GetArticleVersion(itemVersionId, portalId);

//                string xml = a.SerializeObject();
//                XmlDocument articleDoc = new XmlDocument();
//                articleDoc.LoadXml(xml);

//                //strip off namespace and schema
//                XmlNode node = articleDoc.SelectSingleNode("article");
//                node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
//                node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

//                articlesNode.AppendChild(_doc.ImportNode(node, true));
//            }

//            publishNode.AppendChild(_doc.ImportNode(articlesNode, true));
//        }

//        public void BuildRelationships(bool exportAll)
//        {
//            XmlNode publishNode = _doc.SelectSingleNode("publish");
//            XmlNode relationshipsNode = _doc.CreateElement("relationships");
//            List<ItemRelationship> relationships = null;

//            if (exportAll)
//            {
//                relationships = ItemRelationship.GetAllRelationshipsByPortalId(_portalId);
//            }
//            else
//            {
//                relationships = ItemRelationship.GetAllRelationships(_moduleId);
//            }

//            foreach (ItemRelationship relationship  in relationships)
//            {
//                string xml = relationship.SerializeObject();
//                XmlDocument doc = new XmlDocument();
//                doc.LoadXml(xml);

//                //strip off namespace and schema
//                XmlNode node = doc.SelectSingleNode("relationship");
//                node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
//                node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

//                relationshipsNode.AppendChild(_doc.ImportNode(node, true));
//            }

//            publishNode.AppendChild(_doc.ImportNode(relationshipsNode, true));
//        }

//        public void BuildRatings()
//        {

//        }

//        public void BuildTags()
//        {

//        }

//        public void BuildComments()
//        {

//        }

//        public void BuildItemVersionSettings(bool exportAll)
//        {
//            XmlNode publishNode = _doc.SelectSingleNode("publish");
//            XmlNode settingsNode = _doc.CreateElement("itemversionsettings");

//            List<ItemVersionSetting> settings = null;
//            if (exportAll)
//            {
//                settings = ItemVersionSetting.GetItemVersionSettingsByPortalId(_portalId);
//            }
//            else
//            {
//                settings = ItemVersionSetting.GetItemVersionSettingsByModuleId(_moduleId);
//            }

//            foreach (ItemVersionSetting setting in settings)
//            {
//                string xml = setting.SerializeObject();
//                XmlDocument doc = new XmlDocument();
//                doc.LoadXml(xml);

//                //strip off namespace and schema
//                XmlNode node = doc.SelectSingleNode("itemversionsetting");
//                node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
//                node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

//                settingsNode.AppendChild(_doc.ImportNode(node, true));
//            }

//            publishNode.AppendChild(_doc.ImportNode(settingsNode, true));
//        }
        
//        #endregion

//        #region Deconstruct Methods

//        internal void ImportCategories(XPathDocument doc)
//        {
//            // parse categories
//            XPathNavigator navigator = doc.CreateNavigator();
//            XPathNavigator categoriesNode = navigator.SelectSingleNode("//publish/categories");

//            foreach (XPathNavigator categoryNode in categoriesNode.Select("//category"))
//            {
//                // Create an instance of the XmlSerializer specifying type.
//                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Category));
//                System.IO.StringReader reader = new System.IO.StringReader(categoryNode.OuterXml);

//                // Use the Deserialize method to restore the object's state.
//                Category c = (Category)serializer.Deserialize(reader);
//                TransportableElement te = (TransportableElement)c;
//                te.Import(_moduleId, _portalId);
//            }
//        }

//        internal void ImportArticles(XPathDocument doc)
//        {
//            // parse categories
//            XPathNavigator navigator = doc.CreateNavigator();
//            XPathNavigator articlesNode = navigator.SelectSingleNode("//publish/articles");
//            foreach (XPathNavigator articleNode in articlesNode.Select("//article"))
//            {
//                // Create an instance of the XmlSerializer specifying type.
//                XmlSerializer serializer = new XmlSerializer(typeof(Article));
//                StringReader reader = new StringReader(articleNode.OuterXml);

//                // Use the Deserialize method to restore the object's state.
//                Article a = (Article)serializer.Deserialize(reader);
//                TransportableElement te = (TransportableElement)a;
//                te.Import(_moduleId, _portalId);
//            }
//        }

//        internal void ImportRelationships(XPathDocument doc)
//        {

//            // parse relationships
//            XPathNavigator navigator = doc.CreateNavigator();
//            XPathNavigator relationshipsNode = navigator.SelectSingleNode("//publish/relationships");
//            foreach (XPathNavigator relationshipNode in relationshipsNode.Select("//relationship"))
//            {
//                // Create an instance of the XmlSerializer specifying type.
//                XmlSerializer serializer = new XmlSerializer(typeof(ItemRelationship));
//                StringReader reader = new StringReader(relationshipNode.OuterXml);

//                // Use the Deserialize method to restore the object's state.
//                ItemRelationship r = (ItemRelationship) serializer.Deserialize(reader);
//                TransportableElement te = (TransportableElement)r;
//                te.Import(_moduleId, _portalId);
//            }
//        }

//        internal void ImportItemVersionSettings(XPathDocument doc)
//        {
//            // parse settings
//            XPathNavigator navigator = doc.CreateNavigator();
//            XPathNavigator settingsNode = navigator.SelectSingleNode("//publish/itemversionsettings");
//            foreach (XPathNavigator settingNode in settingsNode.Select("//itemversionsetting"))
//            {
//                // Create an instance of the XmlSerializer specifying type.
//                XmlSerializer serializer = new XmlSerializer(typeof(ItemVersionSetting));
//                StringReader reader = new StringReader(settingNode.OuterXml);

//                // Use the Deserialize method to restore the object's state.
//                ItemVersionSetting s = (ItemVersionSetting) serializer.Deserialize(reader);
//                TransportableElement te = (TransportableElement)s;
//                te.Import(_moduleId, _portalId);
//            }
//        }

//        #endregion

//        public XmlDocument GetDocument()
//        {
//            return this._doc;
//        }

    
//    }
//}
