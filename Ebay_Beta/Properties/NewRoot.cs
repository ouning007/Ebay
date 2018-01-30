using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ebay_Beta.Properties
{
    public partial class NewRoot : Form
    {
        private string inipath;
        public NewRoot()
        {
            InitializeComponent();
            this.CenterToParent();
            this.InitComponent();
            this.AcceptButton = button3;
        }


        public NewRoot(string name)
        {
            InitializeComponent();
            this.CenterToParent();
            this.InitComponent();
            this.AcceptButton = button3;


            this.textBox1.Enabled = false;
            inipath = @".\" + name + ".ini";
            this.Load_ini();
        }

        private void Load_ini()
        {
            using(Ini ini=new Ini(inipath))
            {
               this.textBox1.Text=ini.GetIni("Information", "store name");
               this.comboBox1.Text= ini.GetIni("Information", "connect time");
               this.comboBox2.Text= ini.GetIni("Information", "net delay");
               this.textBox2.Text= ini.GetIni("Information", "url");
               this.comboBox3.Text = ini.GetIni("Information", "cmd visible");
               string[] s = { "Find Good", "Good Value", "Next Page", "Next Value", "Next Text", "Sold", "Sold Value", "Sold Endwith" };
               for(int i = 0; i < s.Length; i++)
                {
                    dataGridView1.Rows[0].Cells[i].Value = ini.GetIni("Rules",s[i]);
                }
            }
            
        }

        private void InitComponent()
        {
            this.dataGridView1.EndEdit();
            for(int i = 1; i <= 20; i++)
            {
                comboBox1.Items.Add(i);
                comboBox2.Items.Add(i);
            }
            comboBox3.Items.Add("True");
            comboBox3.Items.Add("False");

            comboBox1.SelectedIndex = 9;
            comboBox2.SelectedIndex = 9;
            comboBox3.SelectedIndex = 0;
            for(int i=0;i<4;i++)
            dataGridView1.Rows.Add();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count - dataGridView1.SelectedColumns.Count <= 1)
                return;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            dataGridView1.Rows.Remove(row);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == ""||dataGridView1.Rows.Count<1)
            {
                MessageBox.Show("Must input store name or rules", "Error Message",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Save_ini();
            
        }

        private void Save_ini()
        {
            using (Ini ini = new Ini(@".\" + textBox1.Text.Trim() + ".ini"))
            {
                if (ini.ExistINIFile())
                {

                    if (MessageBox.Show("File has existed. Override?", "File Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    FileStream file= File.Create(textBox1.Text.Trim()+".txt");
                    file.Close();
                    int num = int.Parse(ini.GetIni("Number", "num", @".\sys.ini")) + 1;
                    ini.WriteIni("Number", "num", num.ToString(), @".\sys.ini");
                    ini.WriteIni("AllStore", num.ToString(), textBox1.Text.Trim(), @".\sys.ini");
                }

                ini.WriteIni("Information", "store name", textBox1.Text.Trim());
                ini.WriteIni("Information", "connect time", comboBox1.Text.Trim());
                ini.WriteIni("Information", "net delay", comboBox2.Text.Trim());
                ini.WriteIni("Information", "url", textBox2.Text.Trim());
                ini.WriteIni("Information", "cmd visible", comboBox3.Text.Trim());

                string[] s = { "Find Good", "Good Value", "Next Page", "Next Value", "Next Text","Sold", "Sold Value", "Sold Endwith" };
                for(int i = 0; i < s.Length; i++)
                {
                    if(dataGridView1.Rows[0].Cells[i].Value!=null)
                        ini.WriteIni("Rules", s[i], dataGridView1.Rows[0].Cells[i].Value.ToString().Trim());
                    else
                        ini.WriteIni("Rules", s[i], "");
                }

                MessageBox.Show("Write Success", "File Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void NewRoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.button3_Click(sender,e);
            }
        }
    }
}
