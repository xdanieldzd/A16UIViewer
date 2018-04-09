using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Numerics;

using A16UIViewer.FileHandlers;

namespace A16UIViewer.Controls
{
    public partial class LayoutRenderControl : UserControl
    {
        Layout layout;
        Animation animation;
        Styles styles;

        Timer animTimer;
        float animValue;

        Stack<string> currentAnimType;
        Dictionary<string, int> currentNodeImageIndex;
        Dictionary<string, float> currentNodeImageTimer;

        public LayoutRenderControl()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            animTimer = new Timer();
            animTimer.Interval = (int)(1000 / 60.0);
            animTimer.Tick += (s, e) =>
            {
                animValue += 0.001f;
                this.Invalidate();
            };

            currentAnimType = new Stack<string>();
            currentNodeImageIndex = new Dictionary<string, int>();
            currentNodeImageTimer = new Dictionary<string, float>();
        }

        public void InitializeLayout(Layout layoutFile, Animation animationFile, Styles stylesFile)
        {
            layout = layoutFile;
            animation = animationFile;
            styles = stylesFile;

            animValue = 0.0f;
            currentAnimType.Clear();
            animTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (layout == null || animation == null || styles == null) return;

            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            e.Graphics.Clip = new Region(new Rectangle(0, 0, 1280, 720));

            foreach (var node in layout.LayoutNodes)
                DrawNode(e.Graphics, node);
        }

        private void DrawNode(Graphics g, Layout.BaseNode node)
        {
            bool animWasSet = false;
            GraphicsContainer container = g.BeginContainer();

            g.TranslateTransform(node.Position.X, node.Position.Y);

            if (layout.AnimationBindings.ContainsKey(node.Name))
            {
                currentAnimType.Push(layout.AnimationBindings[node.Name]);
                animWasSet = true;
            }

            if (node.Type == FileHandlers.Layout.NodeTypes.Image)
            {
                DrawImage(g, node as Layout.ImageNode);
            }

            foreach (var childNode in node.Children)
                DrawNode(g, childNode);

            if (animWasSet)
                currentAnimType.Pop();

            g.EndContainer(container);
        }

        private void DrawImage(Graphics g, Layout.ImageNode node)
        {
            if (!node.Visible) return;

            Vector4 offset = Vector4.Zero;
            Color color = Color.White;

            if (currentAnimType.Count > 0)
            {
                var animNode = GetAnimationNode(animation.LayoutNodes, currentAnimType.Peek());
                if (animNode != null)
                {
                    var curveNode = animNode.Children.FirstOrDefault(x => x is Animation.CurveNode);
                    var keyNodes = curveNode.Children.Where(x => x is Animation.KeyNode).Cast<Animation.KeyNode>();

                    float maxTime = keyNodes.Max(x => x.Time);
                    float localTime = (animValue % maxTime);

                    Animation.KeyNode last = keyNodes.FirstOrDefault(x => x.Time <= localTime);
                    Animation.KeyNode curr = keyNodes.FirstOrDefault(x => x.Time >= localTime);

                    if ((curveNode as Animation.CurveNode).Attribute == "pos")
                        offset = Vector4.Lerp(last.Value, curr.Value, localTime);
                    else if ((curveNode as Animation.CurveNode).Attribute == "color")
                    {
                        Vector4 tempResult = Vector4.Lerp(last.Value, curr.Value, localTime);
                        color = Color.FromArgb((int)tempResult.W, (int)tempResult.X, (int)tempResult.Y, (int)tempResult.Z);
                    }
                }
            }

            string textureName;
            if (styles.ImageLists.ContainsKey(node.Style))
            {
                var imageList = styles.ImageLists[node.Style];

                if (!currentNodeImageIndex.ContainsKey(node.Style)) currentNodeImageIndex.Add(node.Style, (node.ImageNo + 1));
                if (!currentNodeImageTimer.ContainsKey(node.Style)) currentNodeImageTimer.Add(node.Style, 0.0f);

                if ((currentNodeImageTimer[node.Style] += animValue) >= node.FlickTime)
                {
                    currentNodeImageTimer[node.Style] = 0.0f;

                    currentNodeImageIndex[node.Style]++;
                    if (currentNodeImageIndex[node.Style] > imageList.Count)
                        currentNodeImageIndex[node.Style] = (node.ImageNo + 1);
                }

                textureName = $"{node.Style}__xLIST{currentNodeImageIndex[node.Style]:D2}";
            }
            else
            {
                textureName = node.Style;
            }

            var texture = styles.Images.FirstOrDefault(x => x.Key == textureName);
            if (texture.Value != null)
            {
                g.TranslateTransform(offset.X, offset.Y);
                g.DrawImage(texture.Value, 0, 0);
            }
        }

        private Animation.AnimNode GetAnimationNode(List<Animation.BaseNode> nodes, string name)
        {
            Animation.AnimNode result = null;

            foreach (var node in nodes)
            {
                if (node is Animation.AnimNode && (node as Animation.AnimNode).Name == name)
                    return (node as Animation.AnimNode);
                else
                    result = GetAnimationNode(node.Children, name);
            }

            return result;
        }
    }
}
