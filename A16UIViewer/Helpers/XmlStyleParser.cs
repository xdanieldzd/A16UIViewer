using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace A16UIViewer.Helpers
{
    public class XmlStyleParser
    {
        public static Dictionary<string, object> Parse(string path)
        {
            var dictionary = new Dictionary<string, object>();

            var document = new XmlDocument();
            document.Load(path);

            ParseImageListNode(dictionary, document["root"]);

            return dictionary;
        }

        private static void ParseImageNode(Dictionary<string, object> dictionary, XmlNode baseNode)
        {
            var attributes = new Dictionary<string, string>();
            foreach (var attrib in baseNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name != "name"))
                attributes.Add(attrib.Name, attrib.Value);
            dictionary.Add(baseNode.Attributes["name"].Value, attributes);
        }

        private static void ParseImageListNode(Dictionary<string, object> dictionary, XmlNode baseNode)
        {
            var tree = new Dictionary<string, object>();
            foreach (var childNode in baseNode.ChildNodes.Cast<XmlNode>())
            {
                switch (childNode.Name)
                {
                    case "image":
                        ParseImageNode(tree, childNode);
                        break;

                    case "image_list":
                        ParseImageListNode(tree, childNode);
                        break;
                }
            }

            if (baseNode.Attributes.Count > 0)
                dictionary.Add(baseNode.Attributes["name"].Value, tree);
            else
                dictionary.Add(baseNode.Name, tree);
        }
    }
}
