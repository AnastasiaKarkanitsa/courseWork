using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceAction;
using BusinessObjects;

namespace RacionMaker
{
    public partial class Form1 : Form
    {
        static Service service = new Service();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //treeView1.BeginUpdate();
            //TreeNode root = new TreeNode("Root Node");
            //  int i = 0;
            //    foreach (Category c in service.GetCategories())
            //    {
            //        treeView1.Nodes.Add(new TreeNode(c.CategoryName));
            //        foreach (Product p in service.GetProductsByCategory(c.CategoryName))
            //        {
            //            treeView1.Nodes[i].Nodes.Add(new TreeNode(p.Name));
            //        }
            //    }
            //treeView1.EndUpdate();
        }
            
    }
}
