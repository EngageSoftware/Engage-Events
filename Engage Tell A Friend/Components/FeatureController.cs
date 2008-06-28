/*
' Copyright (c) 2004-2008 Engage Software
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System.Collections.Generic;
//using System.Xml;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace Engage.Dnn.TellAFriend
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for TellAFriend
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class FeatureController : IPortable, ISearchable, IUpgradeable
    {

        #region Public Methods



        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        public string ExportModule(int ModuleID)
        {
            //string strXML = "";

            //List<TellAFriendInfo> colTellAFriends = GetTellAFriends(ModuleID);
            //if (colTellAFriends.Count != 0)
            //{
            //    strXML += "<TellAFriends>";

            //    foreach (TellAFriendInfo objTellAFriend in colTellAFriends)
            //    {
            //        strXML += "<TellAFriend>";
            //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objTellAFriend.Content) + "</content>";
            //        strXML += "</TellAFriend>";
            //    }
            //    strXML += "</TellAFriends>";
            //}

            //return strXML;

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            //XmlNode xmlTellAFriends = DotNetNuke.Common.Globals.GetContent(Content, "TellAFriends");
            //foreach (XmlNode xmlTellAFriend in xmlTellAFriends.SelectNodes("TellAFriend"))
            //{
            //    TellAFriendInfo objTellAFriend = new TellAFriendInfo();
            //    objTellAFriend.ModuleId = ModuleID;
            //    objTellAFriend.Content = xmlTellAFriend.SelectSingleNode("content").InnerText;
            //    objTellAFriend.CreatedByUser = UserID;
            //    AddTellAFriend(objTellAFriend);
            //}

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        {
            //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

            //List<TellAFriendInfo> colTellAFriends = GetTellAFriends(ModInfo.ModuleID);

            //foreach (TellAFriendInfo objTellAFriend in colTellAFriends)
            //{
            //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objTellAFriend.Content, objTellAFriend.CreatedByUser, objTellAFriend.CreatedDate, ModInfo.ModuleID, objTellAFriend.ItemId.ToString(), objTellAFriend.Content, "ItemId=" + objTellAFriend.ItemId.ToString());
            //    SearchItemCollection.Add(SearchItem);
            //}

            //return SearchItemCollection;

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        public string UpgradeModule(string Version)
        {
            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        #endregion

    }

}
