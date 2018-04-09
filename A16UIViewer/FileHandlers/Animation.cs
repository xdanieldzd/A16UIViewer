using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Numerics;

namespace A16UIViewer.FileHandlers
{
    public class Animation
    {
        public enum NodeTypes { Root, Anim, Curve, Key }

        static Dictionary<string, NodeTypes> nodeTypeDict = new Dictionary<string, NodeTypes>()
        {
            { "root", NodeTypes.Root },
            { "anim", NodeTypes.Anim },
            { "curve", NodeTypes.Curve },
            { "key", NodeTypes.Key }
        };

        static Dictionary<string, Func<XmlNode, BaseNode>> nodeTypeHandlers = new Dictionary<string, Func<XmlNode, BaseNode>>()
        {
            { "root", (n) => { return new RootNode(n); } },
            { "anim", (n) => { return new AnimNode(n); } },
            { "curve", (n) => { return new CurveNode(n); } },
            { "key", (n) => { return new KeyNode(n); } }
        };

        public List<BaseNode> LayoutNodes { get; private set; }

        public Animation(string path)
        {
            var document = new XmlDocument();
            document.Load(path);

            LayoutNodes = new List<BaseNode>();
            LayoutNodes.AddRange(CreateLayoutNodes(document.ChildNodes));
        }

        private static List<BaseNode> CreateLayoutNodes(XmlNodeList xmlNodes)
        {
            List<BaseNode> layoutNodes = new List<BaseNode>();
            foreach (var childNode in xmlNodes.Cast<XmlNode>().Where(x => x.NodeType == XmlNodeType.Element))
            {
                if (nodeTypeHandlers.ContainsKey(childNode.Name))
                    layoutNodes.Add(nodeTypeHandlers[childNode.Name](childNode));
            }
            return layoutNodes;
        }

        public abstract class BaseNode
        {
            public string Name { get; private set; }
            public NodeTypes Type { get; private set; }

            public List<BaseNode> Children { get; private set; }

            public BaseNode(XmlNode baseNode)
            {
                if (baseNode.Attributes != null && baseNode.Attributes["name"] != null)
                    Name = baseNode.Attributes["name"].Value;
                else
                    Name = string.Empty;

                Type = nodeTypeDict[baseNode.Name];

                if (baseNode.Attributes != null)
                    ParseNode(baseNode);

                ParseChildren(baseNode);
            }

            public abstract void ParseNode(XmlNode baseNode);

            private void ParseChildren(XmlNode baseNode)
            {
                Children = new List<BaseNode>();
                Children.AddRange(CreateLayoutNodes(baseNode.ChildNodes));
            }
        }

        public class RootNode : BaseNode
        {
            public RootNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                // do nothing
            }
        }

        public class AnimNode : BaseNode
        {
            public AnimNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                // do nothing
            }
        }

        public class CurveNode : BaseNode
        {
            public string Attribute { get; private set; }

            public CurveNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                if (baseNode.Attributes["attr"] != null)
                    Attribute = baseNode.Attributes["attr"].Value;
            }
        }

        public class KeyNode : BaseNode
        {
            public float Time { get; private set; }
            public Vector4 Value { get; private set; }

            public KeyNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                if (baseNode.Attributes["time"] != null)
                    Time = float.Parse(baseNode.Attributes["time"].Value);

                if (baseNode.Attributes["value"] != null)
                {
                    float[] valueRaw = baseNode.Attributes["value"].Value.Split(',').Select(float.Parse).ToArray();

                    if (valueRaw.Length == 1)
                        Value = new Vector4(valueRaw[0], 0.0f, 0.0f, 0.0f);
                    else if (valueRaw.Length == 2)
                        Value = new Vector4(valueRaw[0], valueRaw[1], 0.0f, 0.0f);
                    else if (valueRaw.Length == 3)
                        Value = new Vector4(valueRaw[0], valueRaw[1], valueRaw[2], 0.0f);
                    else
                        Value = new Vector4(valueRaw[0], valueRaw[1], valueRaw[2], valueRaw[3]);
                }
            }
        }
    }
}
