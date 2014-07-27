using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace XMLUpdateMerge
{
    public class XMLMerge
    {
        private Dictionary<string, List<KeyValuePair<string, string>>> oldXMLInfo;
        List<KeyValuePair<string, string>> xmlContent;
        List<KeyValuePair<string, string>> tempXmlContent;
        private StringBuilder sbRecord;

        public XMLMerge()
        {
            oldXMLInfo = new Dictionary<string, List<KeyValuePair<string, string>>>();
            tempXmlContent = new List<KeyValuePair<string, string>>();
            sbRecord = new StringBuilder();
        }

        /// <summary>
        /// save old xml file content into new
        /// </summary>
        /// <param name="oldXMLPath"></param>
        /// <param name="newXMLPath"></param>
        public void UpdateXMLFile(string oldXMLPath, string newXMLPath)
        {
            try
            {
                XmlDocument xOldDoc = new XmlDocument();
                xOldDoc.Load(oldXMLPath);
                XmlDocument xNewDoc = new XmlDocument();
                xNewDoc.Load(newXMLPath);
                XmlElement root = xOldDoc.DocumentElement;
                ScanAllNode(root);
                //take the old node information into new file
                foreach (var key in oldXMLInfo.Keys)
                {
                    XmlElement node = (XmlElement)xNewDoc.SelectSingleNode(key);
                    foreach (var item in oldXMLInfo[key])
                    {
                        node.SetAttribute(item.Key, item.Value);
                    }
                }
                xNewDoc.Save(newXMLPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ScanAllNode(XmlElement tempRoot)
        {
            XmlNodeList childNodes = tempRoot.ChildNodes;
            if (childNodes != null)
            {
                foreach (XmlNode node in childNodes)
                {
                    //check whether the node has attribute, if true, record the attribute information
                    if (node.Attributes.Count != 0)
                    {
                        sbRecord.Clear();
                        tempXmlContent.Clear();
                        string nodeLocation = GetNodeSorce(node) + "/" + node.Name;
                        for (int i = 0; i < node.Attributes.Count; i++)
                        {
                            string tempName = node.Attributes[i].Name;
                            string tempValue = node.Attributes[i].Value;
                            tempXmlContent.Add(new KeyValuePair<string, string>(tempName, tempValue));
                        }
                        xmlContent = new List<KeyValuePair<string, string>>();
                        for (int j = 0; j < tempXmlContent.Count; j++)
                        {
                            xmlContent.Add(tempXmlContent[j]);
                        }
                        oldXMLInfo.Add(nodeLocation, xmlContent);
                    }
                    //Recursive until there's no child node
                    if (node.ChildNodes.Count > 0)
                    {
                        ScanAllNode((XmlElement)node);
                    }
                }
            }
        }

        //record the node path
        private string GetNodeSorce(XmlNode node)
        {
            if (node.ParentNode.Name != "#document")
            {
                sbRecord.Insert(0, "/" + node.ParentNode.Name);
                GetNodeSorce(node.ParentNode);
            }
            return sbRecord.ToString();
        }
    }
}
