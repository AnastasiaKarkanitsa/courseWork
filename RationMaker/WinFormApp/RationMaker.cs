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
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Compression;
using Syncfusion.PdfViewer.Base;



namespace WinFormApp
{
    public partial class RationMaker : Form
    {
        static Service service = new Service();
        UserProfile user = new UserProfile();
        int Kcalories = 0;
        Product product;

        public RationMaker()
        {
            InitializeComponent();
            FillTreeViewProducts();
            FillTreeViewRation();
            FillAntropoMetryBox();
            SetActivityBox();
            FillDailyNormaBox();
            progressBar2.Value = user.GetDailyCalories();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.treeCategories.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeMealTime.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeCategories.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            this.treeMealTime.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            this.treeCategories.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeMealTime.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
        }

        private void FillTreeViewProducts()
        {
           treeCategories.BeginUpdate();
           treeCategories.Nodes.Clear();
           // Add a root TreeNode for each Category
            int i = 0;
            foreach (Category c in service.GetCategories())
            {
                treeCategories.Nodes.Add(new TreeNode(c.CategoryName));
                treeCategories.Nodes[i].ImageIndex = i;
                treeCategories.Nodes[i].Name = c.CategoryName;

                // Add a child treenode for each Order object in the current Customer object.
                foreach (Product p in service.GetProductsByCategory(c.CategoryName))
                {
                    TreeNode node = new TreeNode(p.Name);
                    node.Name = p.Name;
                    treeCategories.Nodes[i].Nodes.Add(node);
                }
                i++;
            }

            // Reset the cursor to the default for all controls.
           // Cursor.Current = Cursors.Default;

            // Begin repainting the TreeView.
            treeCategories.EndUpdate();
        }

        private void FillTreeViewRation()
        {
            treeMealTime.BeginUpdate();
            treeMealTime.Nodes.Clear();
            // Add a root TreeNode for each MealTime per Day
            int i = 0;
            foreach (string s in service.GetRation().MealTimes.Keys)
            {
                treeMealTime.Nodes.Add(new TreeNode(s));
                treeMealTime.Nodes[i].ImageIndex = i;
                treeMealTime.Nodes[i].Name = s;
                i++;
            }
            treeMealTime.EndUpdate();
        }

        private void FillAntropoMetryBox()
        {
            this.textBox2.Text = user.Weight.ToString();
            this.textBox3.Text = user.Height.ToString();
            this.textBox4.Text = user.Age.ToString();
            this.textBox5.Text = user.GetBMR().ToString();
            
        }

        private void SetActivityBox()
        {
            switch (user.Activity)
            {
                case ActivityType.Low: radioButton1.Checked = true; break;
                case ActivityType.Normal: radioButton2.Checked = true; break;
                case ActivityType.Average: radioButton2.Checked = true; break;
                case ActivityType.High: radioButton2.Checked = true; break;
            }
            textBox6.Text = user.GetARM().ToString();
        }

        private void FillDailyNormaBox()
        {
            textBox8.Text = user.GetDailyCalories().ToString();
            label17.Text+= user.GetDailyCalories().ToString();
            Kcalories = (int)service.GetRation().GetCalories();
            label24.Text = Kcalories.ToString();
        }

        private void UpdateRationCaloriesView()
        {
            Kcalories = (int)service.GetRation().GetCalories();
            progressBar1.Value = Kcalories > progressBar1.Maximum ? progressBar1.Maximum : Kcalories;
            label24.ForeColor = Kcalories > user.GetDailyCalories() ? Color.Red : Color.Black;
            label24.Text = Kcalories.ToString();
        }
              
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty)
            {
                foreach (TreeNode node in treeCategories.Nodes)
                {
                    foreach (TreeNode childnode in node.Nodes)
                    {
                        if (childnode.Name.StartsWith(textBox1.Text, true, System.Globalization.CultureInfo.CurrentCulture))
                        {
                            treeCategories.SelectedNode = childnode;
                            treeCategories.EndUpdate();
                            return;
                        }

                    }
                }
            }
        }
                
        private void treeView_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Copy);
        }

        private void treeView_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void treeView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNode NewNode;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (DestinationNode.TreeView != NewNode.TreeView)
                {
                    DestinationNode.Nodes.Add((TreeNode)NewNode.Clone());
                    DestinationNode.Expand();
                    Product product = service.GetProduct(NewNode.Text);
                    service.InsertProduct(DestinationNode.Text, product);
                    UpdateRationCaloriesView();
                    //Remove Original Node
                    //NewNode.Remove();
                }
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Delete")
            {
              if (treeMealTime.SelectedNode.Parent==null)
                service.DeleteMealTime(treeMealTime.SelectedNode.Text);
              else
              service.DeleteMealTimeProduct(treeMealTime.SelectedNode.Parent.Text, treeMealTime.SelectedNode.Text);
              treeMealTime.Nodes.Remove(treeMealTime.SelectedNode);
              UpdateRationCaloriesView();
            }
            if (e.ClickedItem.Text == "Add")
            {
                string mealTimeName = "MealTime " + (service.GetRation().N + 1);
                service.InsertMealTime(mealTimeName);
                treeMealTime.Nodes.Add(new TreeNode(mealTimeName));
                UpdateRationCaloriesView();
            }
        }
        
        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
           if (e.Node.Parent != null)
            {
                product = service.GetMealTimeProduct(e.Node.Parent.Text, e.Node.Text);
                if (product != null)
                {
                    this.trackBar1.Value = (int)product.Weight;
                    this.label16.Text = product.Name;
                    this.label1.Text = "Белки: " + product.Protein.ToString();
                    this.label2.Text = "Жиры: " + product.Fats.ToString();
                    this.label3.Text = "Углеводы: " + product.Carbs.ToString();
                    this.label4.Text = "Калории: " + product.Calories.ToString();
                    this.label13.Text = "Вес(грамм): " + product.Weight.ToString();
                  }
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (product != null)
            {
                product.Weight = trackBar1.Value;
                label4.Text = "Калории: " + product.Calories.ToString();
                label13.Text = "Вес(грамм): " + product.Weight;
                UpdateRationCaloriesView();
             }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Tag.ToString() == "weight") user.Weight = tb.Text==String.Empty ? 0 : Convert.ToDouble(tb.Text);
            if (tb.Tag.ToString() == "height") user.Height = tb.Text == String.Empty ? 0 : Convert.ToDouble(tb.Text);
            if (tb.Tag.ToString() == "age") user.Age = tb.Text == String.Empty ? 0 : Convert.ToInt32(tb.Text);
            textBox5.Text = user.GetBMR().ToString();
            textBox8.Text = user.GetDailyCalories().ToString();
            label23.Text= user.GetDailyCalories().ToString();
            progressBar2.Value = user.GetDailyCalories();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in groupBox3.Controls)
            {
                if (c is RadioButton)
                {
                    if ((c as RadioButton).Checked)
                    {
                        RadioButton rb = (sender as RadioButton);
                        switch (rb.Tag.ToString())
                        {
                            case "low": user.Activity = ActivityType.Low; break;
                            case "normal": user.Activity = ActivityType.Normal; break;
                            case "average": user.Activity = ActivityType.Average; break;
                            case "high": user.Activity = ActivityType.High; break;
                        }
                    }
                }
            }
            textBox6.Text = user.GetARM().ToString();
            textBox8.Text = user.GetDailyCalories().ToString();
            progressBar2.Value = user.GetDailyCalories();
            label23.Text = user.GetDailyCalories().ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Create a new PDF document
            using (PdfDocument document = new PdfDocument())
            {
                //Add a page to the document
                Syncfusion.Pdf.PdfPage page = document.Pages.Add();

                //Create PDF graphics for a page
                PdfGraphics graphics = page.Graphics;

                string column1 = String.Format("Вес: {0}\nРост: {1}\nAge: {2}\nАктивность: {3}\n", user.Weight, user.Height, user.Age, user.Activity);
                string column2 = String.Format("Белки: {0}\nЖиры: {1}\nУглеводы: {2}\nККалории: {3}\n", 0, 0, 0, user.GetDailyCalories());
                string copyright = "Copyright © 2019 - A.Karkanitsa Inc. All Rights Reserved";
                
                //Set the standard font
                PdfFont font1 = new PdfStandardFont(PdfFontFamily.Courier, 28, PdfFontStyle.Bold);
                PdfFont font2 = new PdfTrueTypeFont(new Font("Bookman Old Style", 14), true);
                PdfFont font3 = new PdfTrueTypeFont(new Font("Georgia", 14, FontStyle.Bold),true);
                PdfFont font4 = new PdfStandardFont(PdfFontFamily.TimesRoman, 9, PdfFontStyle.Bold);
                PdfFont font5 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 10), true);
                PdfFont font6 = new PdfTrueTypeFont(new Font("Georgia", 14), true);


                graphics.DrawString("Daily ", font1, PdfBrushes.Brown, new PointF(10, 10));
                graphics.DrawString("   Food ", font1, PdfBrushes.Brown, new PointF(10, 40));
                graphics.DrawString("      Ration ", font1, PdfBrushes.Brown, new PointF(10, 70));
                
                //Draw the image
                PdfImage image = PdfImage.FromFile("header.jpg");
                graphics.DrawImage(image, 300, 5);
                graphics.DrawLine(new PdfPen(Color.DarkBlue, 1), new PointF(10, 105), new PointF(500, 105));

                graphics.DrawString("Данные пользователя", font6, PdfBrushes.Blue, new PointF(10, 110));
                graphics.DrawString(column1, font2, PdfBrushes.Black, new PointF(10, 125));

                graphics.DrawString("КБЖУ", font6, PdfBrushes.Blue, new PointF(300, 110));
                graphics.DrawString(column2, font2, PdfBrushes.Black, new PointF(300, 125));
                graphics.DrawLine(new PdfPen(Color.DarkBlue, 1), new PointF(10, 200), new PointF(500, 200));

                //Write Ration
                int posY = 215;
                int imageindex = 1;
                foreach (string s in service.GetRation().MealTimes.Keys)
                {
                    graphics.DrawString(s, font3, PdfBrushes.Brown, new PointF(55, posY));
                    posY += 15;
                    image = PdfImage.FromFile("mealtime"+imageindex+".png");
                    imageindex++;
                    graphics.DrawImage(image, 10, posY,40,40);
                    foreach (Product p in service.GetRation().MealTimes[s].Meal)
                    {
                        string menuitem = String.Format("{0}: {1} грамм",p.Name,p.Weight);
                        graphics.DrawString(menuitem, font5, PdfBrushes.Black, new PointF(55, posY));
                        posY += 15;
                    }
                }
                graphics.DrawLine(new PdfPen(Color.DarkBlue, 2), new PointF(10, posY+5), new PointF(500, posY+5));
                graphics.DrawString("Итого (ккалорий): ", font3, PdfBrushes.Brown, new PointF(10, posY+15));
                graphics.DrawString(service.GetRation().GetCalories().ToString(), font3, PdfBrushes.Green, new PointF(150, posY + 15));
                graphics.DrawString(copyright,font4, PdfBrushes.Black, new PointF(250, 750));

                document.Save("DailyRation.pdf");
                document.Close(true);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.Text = "Search...";
        }
             
        private void button3_Click(object sender, EventArgs e)
        {
            service.ClearRation();
            FillTreeViewRation();
        }

        private void button4_Click(object sender, EventArgs e)
        {
             
        }

        private void treeMealTime_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void treeCategories_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
