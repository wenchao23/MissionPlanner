using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MissionPlanner.Utilities;

using System.Diagnostics;
using System.Threading;

namespace MissionPlanner.Swarm
{
    public partial class displayData : Form
    {
        private Thread cpuThread;
        private double[] latArray = new double[60];
        private double[] lonArray = new double[60];
        private double[] altArray = new double[60];
        private double[] Time = new double[60];
        private double[,] pos = new double[10, 3];
        public MissionPlanner.MAVState mav = null;
        private List<DateTime> TimeList = new List<DateTime>();
        public displayData(MissionPlanner.MAVState mav1)
        {
            InitializeComponent();
            //MainV2.comPort.MAV.cs.lea
           mav = mav1;
            //this.ControlBox = false;
            this.latChart.ChartAreas[0].AxisX.LabelStyle.Format = "mm:ss";
            this.lonChart.ChartAreas[0].AxisX.LabelStyle.Format = "mm:ss";
           this.altChart.ChartAreas[0].AxisX.LabelStyle.Format = "mm:ss";
            for (int i = 0; i < 60 - 1; i++)
                TimeList.Add(DateTime.Now);
            //MissionPlanner.Swarm.FormationControl.
        }
        System.Timers.Timer chartTimer = new System.Timers.Timer();
        private void getPerformanceCounters()
        {
            //var cpuPerfCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            
            while (true)
            {
                //cpuArray[cpuArray.Length - 1] = Math.Round(cpuPerfCounter.NextValue(), 0);
                latArray[latArray.Length - 1] = mav.cs.lat;

                Array.Copy(latArray, 1, latArray, 0, latArray.Length - 1);
                //
                lonArray[lonArray.Length - 1] = mav.cs.lng;

                Array.Copy(lonArray, 1, lonArray, 0, lonArray.Length - 1);
                //
                if(mav.cs.alt<0)
                    altArray[altArray.Length - 1] = 0;
                else
                    altArray[altArray.Length - 1] = mav.cs.alt;


                Array.Copy(altArray, 1, altArray, 0, altArray.Length - 1);


                Time[latArray.Length - 1] = mav.cs.timeInAirMinSec;

                Array.Copy(Time, 1, Time, 0, Time.Length - 1);

                if (TimeList.Count == 60)
                {
                    TimeList.RemoveAt(0);
                    TimeList.Add(DateTime.Now);
                }
                else
                    TimeList.Add(DateTime.Now);
            
                if (latChart.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate { UpdateChart(); });
                }
                else
                {
                    //......
                }
                

                
                Thread.Sleep(2000);
            }
        }

        private void UpdateChart()
        {
            //latChart.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(1).ToOADate();
            //latChart.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
            ProgressBar[] pB = { verticalProgressBar1, verticalProgressBar2, verticalProgressBar3,
            verticalProgressBar4};
            ProgressBar[] LQ = { LQ1, LQ2 , LQ3, LQ4,LQ5 };


            TextBox[] tB = { textBox1, textBox2, textBox3, textBox4 };
            TextBox[] LQ_text = { LQ1_text, LQ2_text, LQ3_text, LQ4_text, LQ5_text };
            latChart.Series["Latitude"].Points.Clear();
            lonChart.Series["Longitude"].Points.Clear();
            altChart.Series["Altitude"].Points.Clear();
            double[] d = { 0, 0, 0, 0, 0, 0, 0 };
            double[] yaw = { 0, 0, 0, 0, 0, 0, 0 };
            double[] distohome = { 0, 0, 0, 0, 0, 0, 0 };
            var index = new List<int>();
            
            foreach (var port in MainV2.Comports.ToArray())
            {

                foreach (var mav in port.MAVlist)
                {
                    

                    pos[mav.sysid, 0] = mav.cs.lat;
                    pos[mav.sysid, 1] = mav.cs.lng;
                    pos[mav.sysid, 2] = mav.cs.alt;
                    yaw[mav.sysid] = mav.cs.yaw;
                    distohome[mav.sysid] = mav.cs.DistToHome;
                    index.Add(mav.sysid);
                    //mav_tag[mav.sysid] =  mav.sysid;
                    if (mav.cs.linkqualitygcs > 100)
                    {
                        LQ[mav.sysid - 1].Value = 100;
                        LQ_text[mav.sysid - 1].Text = mav.cs.linkqualitygcs.ToString("0");
                    }
                    else {
                        LQ[mav.sysid - 1].Value = mav.cs.linkqualitygcs;
                        LQ_text[mav.sysid - 1].Text = mav.cs.linkqualitygcs.ToString("0");
                    }

                    if (mav.cs.linkqualitygcs <0)
                    {
                        LQ[mav.sysid - 1].Value = 0;
                        LQ_text[mav.sysid - 1].Text = mav.cs.linkqualitygcs.ToString("0");
                    }
                    else
                    {
                        LQ[mav.sysid - 1].Value = mav.cs.linkqualitygcs;
                        LQ_text[mav.sysid - 1].Text = mav.cs.linkqualitygcs.ToString("0");
                    }

                }
            }
            index.Sort();
            
            var index1 = index.ToArray();
            
            for (int i = 0; i < index1.Length-1; i++)
            {
                d[i] = cmp_distance(new double[] { pos[index1[i], 0], pos[index1[i], 1], pos[index1[i], 2] }, new double[] { pos[index1[i+1], 0], pos[index1[i + 1], 1], pos[index1[i + 1], 2] });
                if (d[i] > 50 || d[i] < 0)
                {
                    if (d[i] > 50)
                        pB[i].Value = 50;
                    if (d[i] < 0)
                        pB[i ].Value = 0;
                }
                else
                    pB[i ].Value = (int)d[i];

                tB[i ].Text = d[i].ToString("0.0");

                circularProgressBar1.Value = (int)yaw[index1[0]];
                circularProgressBar1.Text = yaw[index1[0]].ToString("0.0")+ "°";
                if (distohome[index1[0]] > 100 || distohome[index1[0]] < 0)
                {
                    if (distohome[index1[0]] > 100)
                        verticalProgressBar5.Value = 100;
                    if (distohome[index1[0]] < 0)
                        verticalProgressBar5.Value = 0;
                }
                else
                    verticalProgressBar5.Value = (int)distohome[index1[0]];
            }

            for (int i = 0; i < latArray.Length - 1; ++i)
            {

               // latChart.Series["Latitude"].Points.AddY(latArray[i]);
                lonChart.Series["Longitude"].Points.AddXY(TimeList[i], lonArray[i]);
                altChart.Series["Altitude"].Points.AddXY(TimeList[i], altArray[i]);
                latChart.Series["Latitude"].Points.AddXY(TimeList[i], latArray[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cpuThread = new Thread(new ThreadStart(this.getPerformanceCounters));
            cpuThread.IsBackground = true;
            cpuThread.Start();
            button1.Enabled = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            //latChart.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            latChart.ChartAreas[0].AxisX.ScaleView.Size = 5;
            latChart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            latChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;

            lonChart.ChartAreas[0].AxisX.ScaleView.Size = 5;
            lonChart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            lonChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;

            altChart.ChartAreas[0].AxisX.ScaleView.Size = 5;
            altChart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            altChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;

            latChart.DoubleClick += chartDemo_DoubleClick;

            latChart.ChartAreas[0].AxisY.Maximum = mav.cs.lat + .0015;
            latChart.ChartAreas[0].AxisY.Minimum = mav.cs.lat - .0015;
            lonChart.ChartAreas[0].AxisY.Maximum = mav.cs.lng + .0015;
            lonChart.ChartAreas[0].AxisY.Minimum = mav.cs.lng - .0015;
            altChart.ChartAreas[0].AxisY.Maximum = 100;
            altChart.ChartAreas[0].AxisY.Minimum = 0;
            
            

            //latChart.ChartAreas[0].AxisX.ScaleView.Position = latChart.Series[0].Points.Count - 5;
            cpuThread = new Thread(new ThreadStart(this.getPerformanceCounters));
            cpuThread.IsBackground = true;
            cpuThread.Start();

        }

        private void cpuChart_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        void chartDemo_DoubleClick(object sender, EventArgs e)//show the scrollBar
        {
            latChart.ChartAreas[0].AxisX.ScaleView.Size = 5;
            latChart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            latChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
        }

        private void displayData_Load(object sender, EventArgs e)
        {

        }

        public double cmp_distance(double[] position_follower, double[] leaderpos)
        {
            double R = 6371.0; // 6371 km
            double dLat = (position_follower[0] - leaderpos[0]) * MathHelper.deg2rad;
            double dLon = (position_follower[1] - leaderpos[1]) * MathHelper.deg2rad;
            double lat1 = leaderpos[0] * MathHelper.deg2rad;
            double lat2 = position_follower[0] * MathHelper.deg2rad;

            double a1 = Math.Sin(dLat / 2.0) * Math.Sin(dLat / 2.0) +
                   Math.Sin(dLon / 2.0) * Math.Sin(dLon / 2.0) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2.0 * Math.Atan2(Math.Sqrt(a1), Math.Sqrt(1.0 - a1));
            double d = R * c * 1000.0;
            double d1 = Math.Sqrt(Math.Pow(d, 2) + Math.Pow(Math.Abs(position_follower[2] - leaderpos[2]), 2));
            return d1;

        }

        private void verticalProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void circularProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }
    }
   
}
