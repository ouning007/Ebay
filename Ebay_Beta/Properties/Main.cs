using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ebay_Beta.Properties
{
    public partial class Main : Form
    {
        const int N = 12;
        const int startWidth = 712;
        const int runWidth = 845;
        private Form1 form;
        private BackgroundWorker worker ;
        private Process p;
        private string name;
        private string[] information = { "store name", "connect time", "net delay", "url" };
        private string[] rules = { "Find Good", "Good Value", "Next Page", "Next Value", "Next Text", "Sold", "Sold Value", "Sold Endwith" };
        private List<string> result=new List<string>();
        private string visible;
        private System.Windows.Forms.Timer timer;
        private int timer_index = 0;
        public Main(Form1 form)
        {
            InitializeComponent();
            this.form = form;
            this.Width = startWidth;
            InitTreeView();
            InitDataView("init");
            dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Do_Python);
            p = new Process();
            this.progressBar1.Step = 1;
            name = "";

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Enabled = false;
            timer.Tick += new EventHandler(Time_Update);
        }

        private void Time_Update(object sender, EventArgs e)
        {
            if (timer_index  == 5)
            {
                InitDataView("show");
                timer_index = 0;
            }           
            timer_index++;

            label5.Text = (dataGridView1.RowCount-1).ToString();
            label11.Text = DateTime.Now.ToString();
            DateTime d1 = Convert.ToDateTime(label9.Text);
            DateTime d2 = Convert.ToDateTime(label11.Text);
            TimeSpan d = d2.Subtract(d1);
            label13.Text = d.ToString();

        }

        private void Do_Python(object sender, DoWorkEventArgs e)
        {
                p.StartInfo.FileName = @".\test1.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;   //重定向标准输入   
                p.StartInfo.RedirectStandardOutput = false;  //重定向标准输出   
                p.StartInfo.RedirectStandardError = true;   //重定向错误输出   
                p.StartInfo.CreateNoWindow = (visible=="False")?true:false;
                p.Start();
            for (int i = 0; i < result.Count; i++)
                p.StandardInput.WriteLine(result[i]);
                p.WaitForExit();

            if(button1.Text=="Stop")
                Exist_run_prepare();
                name = "";
            
        }


        private void InitDataView(string s)
        {
            if (s == "init")
            {
               for(int i = 0; i < 15; i++)
              {
                dataGridView1.Rows.Add();
               }
            }else if (s == "show")
            {
              
                if (!File.Exists("./" + name + ".txt"))
                {
                    MessageBox.Show("Not Found!", "File information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                    
                this.dataGridView1.Rows.Clear();
                StreamReader sr = new StreamReader("./"+name+".txt");
                String line;
                int i = 0;
                int index = this.dataGridView1.RowCount;
                while ((line = sr.ReadLine()) != null)
                {

                    i %= 3;
                    if (i == 0)
                    {
                        index = this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[index].Cells[i++].Value = index + 1;

                    }


                    this.dataGridView1.Rows[index].Cells[i++].Value = line;



                }
                sr.Close();
            }
        }
            
       

        private void InitTreeView()
        {
            treeView1.Nodes.Clear();
            using (Ini ini = new Ini(@".\sys.ini"))
            {
                try
                {
                   int num = int.Parse(ini.GetIni("Number", "num"));
                
                
                for (int i = 1; i <= num; i++)
                {
                    TreeNode node = new TreeNode();
                    node.Text = ini.GetIni("AllStore", i.ToString());
                    treeView1.Nodes.Add(node);
                    TreeNode node_ = new TreeNode("run()");
                    node.Nodes.Add(node_);
                }
                }catch(Exception)
                {
                    return;
                }
                treeView1.ExpandAll();
            }
        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure to quit?", "Exit Message",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
                e.Cancel=true;
        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewRoot root = new NewRoot())
                root.ShowDialog();
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            InitTreeView();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;
            if (mouse.Button == MouseButtons.Right)
            {
                Point p = new Point(mouse.X, mouse.Y);
                TreeNode node = treeView1.GetNodeAt(p);
                if (node != null)
                {
                    treeView1.SelectedNode = node;
                    name=(treeView1.SelectedNode.Parent == null) ? (treeView1.SelectedNode.Text) : ( treeView1.SelectedNode.Parent.Text);
                    treeView1.ContextMenuStrip = contextMenuStrip1;
                    contextMenuStrip1.Show(this,p.X, p.Y+100);                
                }
            }
            else if(mouse.Button == MouseButtons.Left){
                Point p = new Point(mouse.X, mouse.Y);
                treeView1.SelectedNode = treeView1.GetNodeAt(p);
                name = (treeView1.SelectedNode.Parent == null) ? (treeView1.SelectedNode.Text) : (treeView1.SelectedNode.Parent.Text);
            }
        }

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if ( node == null)
                return;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            treeView1.Nodes.Remove(node);

            Ini ini = new Ini(@".\sys.ini");
            ini.RemoveKey("AllStore", (node.Index+1).ToString());
            int num =int.Parse(ini.GetIni("Number", "num"));
            num -= 1;
            ini.WriteIni("Number", "num", num.ToString());
            int i = 1;
            ini.RemoveSection("AllStore");
            foreach (TreeNode n in treeView1.Nodes)
            {            
                ini.WriteIni("AllStore", i.ToString(), n.Text);
                i++;
            }
            ini.DeleteFile(@".\" + node.Text+".ini");

        }

        private void 编辑EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            NewRoot root =new NewRoot(node.Text);
            root.ShowDialog();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs m = e as MouseEventArgs;
            if (m.Button == MouseButtons.Left)
            {
                Point p = new Point(m.X, m.Y);
                TreeNode node = treeView1.GetNodeAt(p);
                treeView1.SelectedNode = node;
                name = (treeView1.SelectedNode.Parent == null) ? (treeView1.SelectedNode.Text) : (treeView1.SelectedNode.Parent.Text);
                if (node.Parent == null)
                    编辑EToolStripMenuItem_Click(sender, e);
                else
                    Run();
            }
        }

        private void 运行RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void 运行RToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            if (name == "")
            {
                MessageBox.Show("Must select one store", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (button1.Text == "Run")
            {
                using (Ini ini = new Ini(@".\" + name + ".ini"))
                {
                    for (int i = 0; i < information.Length; i++)
                        result.Add(ini.GetIni("Information", information[i]));
                    for (int j = 0; j < rules.Length; j++)
                        result.Add(ini.GetIni("Rules", rules[j]));
                    visible = ini.GetIni("Information", "cmd visible");
                }
                Into_run_prepare();
                worker.RunWorkerAsync();
               
            }
            else if (button1.Text == "Stop")
            {
                Exist_run_prepare();
                p.CloseMainWindow();
                p.Kill();
              
            }
               

        }

        private void Into_run_prepare()
        {
            button1.Text = "Stop";
            textBox1.Text = "Don't worry The web crawler is Running";
            treeView1.Enabled = false;
            运行RToolStripMenuItem.Enabled = false;
            this.progressBar1.Show();
            this.Width = runWidth;
            label3.Text = name;
            label5.Text = "0";
            label7.Text = "0";
            label9.Text = DateTime.Now.ToString();
            label11.Text = DateTime.Now.ToString();
            label13.Text = "00:00:00";
            timer.Start();
        }

        private void Exist_run_prepare()
        {
            button1.Text = "Run";
            textBox1.Text = "The Data Is Saved .Show The Top 100 Of "+name;
            InitDataView("show");
            treeView1.Enabled = true;
            运行RToolStripMenuItem.Enabled = true;
            this.progressBar1.Hide();
            this.Width = startWidth;
            timer.Stop();
        }

    }
}
