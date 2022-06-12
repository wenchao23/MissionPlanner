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

namespace MissionPlanner.Swarm
{
    public partial class LinkQuality : Form
    {
        private Thread LQTread;
        byte[] mav_tag1 = { };
        public LinkQuality(byte[] mav_tag)
        {

            mav_tag1 = mav_tag;
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            LQTread = new Thread(new ThreadStart(this.start));
            LQTread.IsBackground = true;
            LQTread.Start();
            button1.Enabled = false;
        }
        private void start()
        {
            
            while (true) {
                if (panel2.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate { UpdateChart(); });
                }

                Thread.Sleep(2000);
            }
        }

        private void UpdateChart() {
            ProgressBar[] LQ = { LQ1, LQ2, LQ3, LQ4, LQ5 };



            TextBox[] LQ_text = { LQ1_text, LQ2_text, LQ3_text, LQ4_text, LQ5_text };

            foreach (var port in MainV2.Comports.ToArray())
            {

                foreach (var mav in port.MAVlist)
                {


                    int index = Array.IndexOf(mav_tag1, mav.sysid);
                    //mav_tag[mav.sysid] =  mav.sysid;
                    if (mav.cs.linkqualitygcs > 100)
                    {
                        
                        LQ[index].Value = 100;
                        LQ_text[index].Text = mav.cs.linkqualitygcs.ToString("0");
                    }
                    else
                    {
                        LQ[index].Value = mav.cs.linkqualitygcs;
                        LQ_text[index].Text = mav.cs.linkqualitygcs.ToString("0");
                    }

                    if (mav.cs.linkqualitygcs < 0)
                    {
                        LQ[index].Value = 0;
                        LQ_text[index].Text = mav.cs.linkqualitygcs.ToString("0");
                    }
                    else
                    {
                        LQ[index].Value = mav.cs.linkqualitygcs;
                        LQ_text[index].Text = mav.cs.linkqualitygcs.ToString("0");
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }



}
