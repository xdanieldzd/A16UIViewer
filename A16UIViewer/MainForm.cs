using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace A16UIViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            string uisFile = @"Saves\ui\gen_styles\uis_gen_title.xml";
            string uilFile = @"Saves\ui\title\uil_title.xml";
            string uiaFile = @"Saves\ui\title\uia_title.xml";

            string uisFullPath = Path.Combine(Program.BasePath, uisFile);
            string uilFullPath = Path.Combine(Program.BasePath, uilFile);
            string uiaFullPath = Path.Combine(Program.BasePath, uiaFile);

            Text = $"{Application.ProductName} [{Path.GetFileName(uisFullPath)}, {Path.GetFileName(uilFullPath)}, {Path.GetFileName(uiaFullPath)}]";

            FileHandlers.Layout uil = new FileHandlers.Layout(uilFullPath);
            FileHandlers.Styles uis = new FileHandlers.Styles(uisFullPath);
            FileHandlers.Animation uia = new FileHandlers.Animation(uiaFullPath);

            // list textures
            dgvImages.Columns.Add(new DataGridViewTextBoxColumn() { Width = 250 });
            dgvImages.Columns.Add(new DataGridViewImageColumn() { Width = 500, ImageLayout = DataGridViewImageCellLayout.Zoom });
            foreach (var texture in uis.Images)
            {
                dgvImages.Rows.Add(new object[] { texture.Key, texture.Value });
                DataGridViewRow row = dgvImages.Rows[dgvImages.Rows.GetLastRow(DataGridViewElementStates.None)];
                row.Height = Math.Max((int)(texture.Value.Height / 2.5), 24);
            }

            // layout tree
            PopulateTreeView(tvLayout, null, uil.LayoutNodes);
            tvLayout.ExpandAll();

            // draw layout
            lrcLayout.InitializeLayout(uil, uia, uis);
            lrcLayout.Invalidate();
        }

        private void PopulateTreeView(TreeView treeView, TreeNode parentNode, List<FileHandlers.Layout.BaseNode> nodes)
        {
            foreach (var node in nodes)
            {
                TreeNode treeNode = new TreeNode((node.Name == string.Empty ? $"{node.Type}" : $"{node.Type}: {node.Name}"));
                (parentNode?.Nodes ?? treeView.Nodes).Add(treeNode);
                PopulateTreeView(treeView, treeNode, node.Children);
            }
        }
    }
}
