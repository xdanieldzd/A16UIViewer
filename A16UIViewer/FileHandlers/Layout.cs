using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
using System.Numerics;

namespace A16UIViewer.FileHandlers
{
    public class Layout
    {
        public enum NodeTypes { Root, Command, Image, Node, AnimCtrl, BindNode }

        static Dictionary<string, NodeTypes> nodeTypeDict = new Dictionary<string, NodeTypes>()
        {
            { "root", NodeTypes.Root },
            { "command", NodeTypes.Command },
            { "image", NodeTypes.Image },
            { "node", NodeTypes.Node },
            { "anim_ctrl", NodeTypes.AnimCtrl },
            { "bind_node", NodeTypes.BindNode },
        };

        static Dictionary<string, Func<XmlNode, BaseNode>> nodeTypeHandlers = new Dictionary<string, Func<XmlNode, BaseNode>>()
        {
            { "root", (n) => { return new RootNode(n); } },
            // command
            { "image", (n) => { return new ImageNode(n); } },
            { "node", (n) => { return new LayoutNode(n); } },
            { "anim_ctrl", (n) => { return new AnimCtrlNode(n); } },
            { "bind_node", (n) => { return new BindNodeNode(n); } },
        };

        public List<BaseNode> LayoutNodes { get; private set; }

        public Dictionary<string, string> AnimationBindings { get; private set; }

        public Layout(string path)
        {
            var document = new XmlDocument();
            document.Load(path);

            LayoutNodes = new List<BaseNode>();
            LayoutNodes.AddRange(CreateLayoutNodes(document.ChildNodes));

            AnimationBindings = new Dictionary<string, string>();
            FindAnimationBindings(LayoutNodes);
        }

        private void FindAnimationBindings(List<BaseNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is BindNodeNode)
                {
                    var bindNodeNode = (node as BindNodeNode);
                    AnimationBindings.Add(bindNodeNode.Node, bindNodeNode.Anim);
                }
                FindAnimationBindings(node.Children);
            }
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

            public Vector3 Position { get; private set; }
            public bool Visible { get; private set; }

            public List<BaseNode> Children { get; private set; }

            public BaseNode(XmlNode baseNode)
            {
                if (baseNode.Attributes != null && baseNode.Attributes["name"] != null)
                    Name = baseNode.Attributes["name"].Value;
                else
                    Name = string.Empty;

                Type = nodeTypeDict[baseNode.Name];

                if (baseNode.Attributes != null)
                {
                    if (baseNode.Attributes["pos"] != null)
                    {
                        float[] posRaw = baseNode.Attributes["pos"].Value.Split(',').Select(float.Parse).ToArray();
                        if (posRaw.Length == 2)
                            Position = new Vector3(posRaw[0], posRaw[1], 0.0f);
                        else
                            Position = new Vector3(posRaw[0], posRaw[1], posRaw[2]);
                    }

                    if (baseNode.Attributes["visible"] != null)
                        Visible = bool.Parse(baseNode.Attributes["visible"].Value);
                    else
                        Visible = true;

                    ParseNode(baseNode);
                }

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

        public class ImageNode : BaseNode
        {
            public string Style { get; private set; }
            public int ImageNo { get; private set; }
            public float FlickTime { get; private set; }

            public ImageNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                if (baseNode.Attributes["style"] != null)
                    Style = baseNode.Attributes["style"].Value;

                if (baseNode.Attributes["image_no"] != null)
                    ImageNo = int.Parse(baseNode.Attributes["image_no"].Value);

                if (baseNode.Attributes["flick_time"] != null)
                    FlickTime = float.Parse(baseNode.Attributes["flick_time"].Value);
            }
        }

        public class LayoutNode : BaseNode
        {
            public LayoutNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                // do nothing
            }
        }

        public class AnimCtrlNode : BaseNode
        {
            public bool Loop { get; private set; }
            public bool Play { get; private set; }
            public float Speed { get; private set; }

            public AnimCtrlNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                if (baseNode.Attributes["loop"] != null)
                    Loop = bool.Parse(baseNode.Attributes["loop"].Value);

                if (baseNode.Attributes["play"] != null)
                    Play = bool.Parse(baseNode.Attributes["play"].Value);

                if (baseNode.Attributes["speed"] != null)
                    Speed = float.Parse(baseNode.Attributes["speed"].Value);
            }
        }

        public class BindNodeNode : BaseNode
        {
            public string Node { get; private set; }
            public string Anim { get; private set; }
            public bool StartVisible { get; private set; }

            public BindNodeNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                if (baseNode.Attributes["node"] != null)
                    Node = baseNode.Attributes["node"].Value;

                if (baseNode.Attributes["anim"] != null)
                    Anim = baseNode.Attributes["anim"].Value;

                if (baseNode.Attributes["start_visible"] != null)
                    StartVisible = bool.Parse(baseNode.Attributes["start_visible"].Value);
                else
                    StartVisible = false;
            }
        }
    }
}
