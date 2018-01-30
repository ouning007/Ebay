using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Ebay_Beta.Properties;

namespace Ebay_Beta
{
    public partial class Form1 : Form
    {
        private BackgroundWorker worker = new BackgroundWorker();
        public Form1()
        {
            InitializeComponent();
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(Config_Begin);
            worker.ProgressChanged += new ProgressChangedEventHandler(Config_Read);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Config_Completed);
            worker.RunWorkerAsync(this);
        }

        private void Config_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Hide();
            Main m = new Main(this);
            m.Show();
        }

        private void Config_Begin(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            while (i <= 100)
            {
                worker.ReportProgress(i++);
                Thread.Sleep(50);
            }
        }

        private void Config_Read(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }
    }
}
