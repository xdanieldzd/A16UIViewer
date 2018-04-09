using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Drawing;
using System.IO;

using Scarlet.IO;
using Scarlet.IO.ImageFormats;

namespace A16UIViewer.FileHandlers
{
    public class Styles
    {
        public enum NodeTypes { Root, Image, ImageList }

        static Dictionary<string, NodeTypes> nodeTypeDict = new Dictionary<string, NodeTypes>()
        {
            { "root", NodeTypes.Root },
            { "image", NodeTypes.Image },
            { "image_list", NodeTypes.ImageList }
        };

        static Dictionary<string, Func<XmlNode, BaseNode>> nodeTypeHandlers = new Dictionary<string, Func<XmlNode, BaseNode>>()
        {
            { "root", (n) => { return new RootNode(n); } },
            { "image", (n) => { return new ImageNode(n); } },
            { "image_list", (n) => { return new ImageListNode(n); } },
        };

        public List<BaseNode> LayoutNodes { get; private set; }

        Dictionary<string, List<Bitmap>> sourceBitmapCache;
        Stack<string> currentImageList;

        public Dictionary<string, Bitmap> Images { get; private set; }
        public Dictionary<string, List<string>> ImageLists { get; private set; }

        public Styles(string path)
        {
            var document = new XmlDocument();
            document.Load(path);

            LayoutNodes = new List<BaseNode>();
            LayoutNodes.AddRange(CreateLayoutNodes(document.ChildNodes));

            sourceBitmapCache = new Dictionary<string, List<Bitmap>>();
            currentImageList = new Stack<string>();

            Images = new Dictionary<string, Bitmap>();
            ImageLists = new Dictionary<string, List<string>>();
            FindImages(LayoutNodes);
        }

        private void FindImages(List<BaseNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is ImageListNode)
                {
                    currentImageList.Push(node.Name);
                    ImageLists.Add(node.Name, new List<string>());
                }
                else if (node is ImageNode)
                {
                    var imageNode = (node as ImageNode);

                    string texturePath = Path.Combine(Program.DataPath, imageNode.Texture.Replace('/', '\\'));
                    if (File.Exists(texturePath))
                    {
                        List<Bitmap> bitmaps;
                        if (!sourceBitmapCache.ContainsKey(texturePath))
                        {
                            var texture = FileFormat.FromFile<ImageFormat>(texturePath);
                            bitmaps = texture.GetBitmaps(0).ToList();
                            sourceBitmapCache.Add(texturePath, bitmaps);
                        }
                        else
                            bitmaps = sourceBitmapCache[texturePath];

                        Bitmap image = new Bitmap(imageNode.UVWH.Width, imageNode.UVWH.Height);
                        using (Graphics g = Graphics.FromImage(image))
                        {
                            g.DrawImage(bitmaps[imageNode.TextureIndex], 0, 0, imageNode.UVWH, GraphicsUnit.Pixel);
                        }

                        Images.Add(imageNode.Name, image);

                        if (currentImageList.Count > 0)
                            ImageLists[currentImageList.Peek()].Add(imageNode.Name);
                    }
                }

                FindImages(node.Children);

                if (node is ImageListNode)
                    currentImageList.Pop();
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

        public class ImageNode : BaseNode
        {
            public string Texture { get; private set; }
            public int TextureIndex { get; private set; }
            public Rectangle UVWH { get; private set; }
            public string AlphaBlend { get; private set; }
            public bool TexMagLinear { get; private set; }
            public bool TexMinLinear { get; private set; }
            public bool UWrap { get; private set; }
            public bool VWrap { get; private set; }

            public ImageNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                if (baseNode.Attributes["texture"] != null)
                    Texture = baseNode.Attributes["texture"].Value;

                if (baseNode.Attributes["texture_index"] != null)
                    TextureIndex = int.Parse(baseNode.Attributes["texture_index"].Value);

                if (baseNode.Attributes["uvwh"] != null)
                {
                    int[] valueRaw = baseNode.Attributes["uvwh"].Value.Split(',').Select(int.Parse).ToArray();
                    UVWH = new Rectangle(valueRaw[0], valueRaw[1], valueRaw[2], valueRaw[3]);
                }

                if (baseNode.Attributes["alpha_blend"] != null)
                    AlphaBlend = baseNode.Attributes["alpha_blend"].Value;

                if (baseNode.Attributes["tex_mag_linear"] != null)
                    TexMagLinear = bool.Parse(baseNode.Attributes["tex_mag_linear"].Value);

                if (baseNode.Attributes["tex_min_linear"] != null)
                    TexMinLinear = bool.Parse(baseNode.Attributes["tex_min_linear"].Value);

                if (baseNode.Attributes["u_wrap"] != null)
                    UWrap = bool.Parse(baseNode.Attributes["u_wrap"].Value);

                if (baseNode.Attributes["v_wrap"] != null)
                    VWrap = bool.Parse(baseNode.Attributes["v_wrap"].Value);
            }
        }

        public class ImageListNode : BaseNode
        {
            public ImageListNode(XmlNode baseNode) : base(baseNode) { }

            public override void ParseNode(XmlNode baseNode)
            {
                // do nothing
            }
        }
    }
}
