using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems;
using MissionPlanner.Utilities;
using System.Linq;
using System.Device.Location;
using System.Diagnostics;
namespace MissionPlanner.Swarm
{

    public partial class FormationControl : Form
    {
        private PointLatLngAlt leader_homeloc = new PointLatLngAlt();
        Formation SwarmInterface = null;
        bool threadrun = false;

        //public int[] sysid_enter_start = new int[] { 0, 0, 0, 0, 0, 0, 0 };

        public FormationControl()
        {
            InitializeComponent();

            SwarmInterface = new Formation();

            TopMost = true;

            Dictionary<String, MAVState> mavStates = new Dictionary<string, MAVState>();

            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    mavStates.Add(port.BaseStream.PortName + " " + mav.sysid + " " + mav.compid, mav);
                }
            }

            if (mavStates.Count == 0)
                return;

            bindingSource1.DataSource = mavStates;

            CMB_mavs.DataSource = bindingSource1;
            CMB_mavs.ValueMember = "Value";
            CMB_mavs.DisplayMember = "Key";

            updateicons();

            this.MouseWheel += new MouseEventHandler(FollowLeaderControl_MouseWheel);

            MessageBox.Show("this is beta, use at own risk");
            //var  d = SwarmInterface.Leader.cs.HomeLocation;
            MissionPlanner.Utilities.Tracking.AddPage(this.GetType().ToString(), this.Text);
        }

        void FollowLeaderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                grid1.setScale(grid1.getScale() + 4);
            }
            else
            {
                grid1.setScale(grid1.getScale() - 4);
            }
        }

        void updateicons()
        {
            bindingSource1.ResetBindings(false);

            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    if (mav == SwarmInterface.getLeader())
                    {
                        ((Formation)SwarmInterface).setOffsets(mav, 0, 0, 0);
                        var vector = SwarmInterface.getOffsets(mav);
                        grid1.UpdateIcon(mav, (float)vector.x, (float)vector.y, (float)vector.z, false);
                    }
                    else
                    {
                        var vector = SwarmInterface.getOffsets(mav);
                        grid1.UpdateIcon(mav, (float)vector.x, (float)vector.y, (float)vector.z, true);
                    }
                }
            }
            grid1.Invalidate();
        }

        private void CMB_mavs_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    if (mav == CMB_mavs.SelectedValue)
                    {
                        MainV2.comPort = port;
                        port.sysidcurrent = mav.sysid;
                        port.compidcurrent = mav.compid;
                    }
                }
            }
        }


        private void CMB_Choose_method_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
                SwarmInterface.CMB_status = CMB_Choose_method.SelectedIndex;


        }
        private void BUT_Start_Click(object sender, EventArgs e)
        {

            if (threadrun == true)
            {
                threadrun = false;

                BUT_Start.Text = Strings.Start;
                CMB_Choose_method.Enabled = true;
                return;
            }

            if (SwarmInterface != null)
            {
                new System.Threading.Thread(mainloop) { IsBackground = true }.Start();
                BUT_Start.Text = Strings.Stop;
                SwarmInterface.ResetStatus();
                CMB_Choose_method.Enabled = false;


            }
            BUT_altmin.Enabled = true;
            BUT_altlvl.Enabled = true;
            BUT_altadd.Enabled = true;
        }

        //add form1
        private void BUT_Form1_Click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav == SwarmInterface.Leader)
                        continue;


                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    // {
                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    offset.y = 0;
                    //offset.x = -mav.sysid * 4
                    offset.x = -((index) * 10);
                    //offset.z = -mav.sysid *1.5;
                    offset.z = -((index) * 5);

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    // }
                }
            }
        }

        //add form2
        //add form1

        // The coordinate watcher.
        private GeoCoordinateWatcher Watcher = null;

        //// Create and start the watcher.

        private void BUT_Form2_Click(object sender, EventArgs e)
        {

            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav == SwarmInterface.Leader)
                        continue;

                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    // {

                    //offset.y = mav.sysid * 2 * Math.Pow(-1, i);
                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    offset.y = (index * 5) * Math.Pow(-1, index);
                    //offset.x = -mav.sysid * 2;
                    offset.x = -(index * 10);
                    offset.z = -(index * 5);

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);

                    // }
                }
            }
        }
        private void BUT_Form3_Click(object sender, EventArgs e)//leader goto button
        {
            //foreach (var port in MainV2.Comports)
            //{
            //    foreach (var mav in port.MAVlist)
            //    {
            //        //mav.cs.UpdateCurrentSettings(null, true, port, mav);

            //        if (mav == SwarmInterface.Leader)
            //            continue;


            //        Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

            //        //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
            //        // {
            //        int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
            //        offset.y = 0;
            //        //offset.x = -mav.sysid * 4
            //        offset.x = -((index) * 10);
            //        //offset.z = -mav.sysid *1.5;
            //        offset.z = -((index) * 5);

            //        grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
            //        ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
            //        // }
            //    }
            //}
            float alt = float.Parse(Text_alt.Text);
            float lat = float.Parse(Text_lat.Text);
            float lng = float.Parse(Text_lng.Text);

            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            float lng_ori = (float)SwarmInterface.Leaderpos_lng;

            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            var pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0 };//149.16559338569641F, -35.362407919859933F, 50 
            System.Threading.Thread Form3_thread = new System.Threading.Thread(flyto);
            Form3_thread.IsBackground = true;
            Form3_thread.Start(pos);

            //flyto(pos);
        }
        private void BUT_Form4_Click(object sender, EventArgs e)
        {


            if (SwarmInterface.mav_tag.Length <= 5)
            {
                double[] pos_y = new double[] { 0, 10, 5, 0, 10 };
                double[] pos_x = new double[] { 0, 0, -5, -10, -10 };
                foreach (var port in MainV2.Comports)
                {
                    foreach (var mav in port.MAVlist)
                    {
                        //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                        if (mav == SwarmInterface.Leader)
                            continue;

                        Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                        //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                        // {

                        //offset.y = mav.sysid * 2 * Math.Pow(-1, i);
                        //if (mav.sysid != 5)
                        //{
                            int index = Math.DivRem(Array.IndexOf(SwarmInterface.mav_tag, mav.sysid), 5, out int remainder);
                            offset.y = pos_y[remainder];
                            //offset.x = -mav.sysid * 2;
                            offset.x = pos_x[remainder];
                            offset.z = -(remainder + index) * 5;
                        //}
                        //else
                        //{
                         //   offset.y = pos_y[4];
                            //offset.x = -mav.sysid * 2;
                        //    offset.x = pos_x[4];
                        //    offset.z = -(5) * 5;
                       // }
                        grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                        ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    }


                    // }
                }
            }
            else
            {
                MessageBox.Show("# of UAVs is greater than 5, the command will not be operated");
            }
        }


        private void flyto(object pos1)
        {

            float[] pos = (float[])pos1;
            byte id = (byte)pos[6];
            byte compid = (byte)pos[7];
            byte after_yaw_flag = 0;
            float yaw_target = 0F;
            if (pos.Length == 10)
            {
                after_yaw_flag = 1;
                yaw_target = (float)pos[9];
            }

            //lat -35.362407919859933
            //lng 149.16559338569641
            //alt 50
            //double x = math.cos(pos[1] * mathhelper.deg2rad) * math.sin((pos[0] - swarminterface.leader.cs.lng) * mathhelper.deg2rad);
            //double y = math.cos(swarminterface.leader.cs.lat * mathhelper.deg2rad) * math.sin(pos[1] * mathhelper.deg2rad) - math.sin(swarminterface.leader.cs.lat * mathhelper.deg2rad) * math.cos(pos[1] * mathhelper.deg2rad) * math.cos((pos[0] - swarminterface.leader.cs.lng) * mathhelper.deg2rad);
            //double yaw = math.round(math.atan2(x, y) * mathhelper.rad2deg+720);
            //int i = math.divrem((int)yaw, 360, out int yaw2);
            //float yaw1 = (float)yaw2;
            double dist = SwarmInterface.cmp_distance(new double[] { SwarmInterface.Leader.cs.lat, SwarmInterface.Leader.cs.lng, pos[5] }, new double[] { pos[1], pos[0], pos[5] });
            if (dist > 3 && pos[8] == 0)
            {
                //float yaw = cmp_yaw(new double[] { pos[3], pos[4] }, new double[] { pos[0], pos[1] });
                float yaw = cmp_yaw(new double[] { SwarmInterface.Leader.cs.lng, SwarmInterface.Leader.cs.lat }, new double[] { pos[0], pos[1] });
                MainV2.comPort.doCommand(id, compid, MAVLink.MAV_CMD.CONDITION_YAW, yaw, 100.0f, 0, 0, 0, 0, 0, false);
                int n = 1;

                while (Math.Abs(SwarmInterface.Leader.cs.yaw - yaw) > 5 && n < 1000)
                {
                    n = n + 1;
                    System.Threading.Thread.Sleep(10);

                    continue;
                }
            }
            if (pos[8] == 0)
            {
                MainV2.comPort.setGuidedModeWP(id, compid, new Locationwp
                {
                    alt = pos[2],
                    lat = pos[1],
                    lng = pos[0]
                });
            }
            else
            {
                //SwarmInterface.Leader.cs.mode = "POSHOLD";
                MainV2.comPort.setMode(SwarmInterface.leader_tag, SwarmInterface.leader_compid, "POSHOLD");
                System.Threading.Thread.Sleep(10);
                MainV2.comPort.setMode(SwarmInterface.leader_tag, SwarmInterface.leader_compid, "GUIDED");
                //MainV2.comPort.setGuidedModeWP(id, compid, new Locationwp
                //{
                //    alt = SwarmInterface.Leader.cs.alt,
                //    lat = SwarmInterface.Leader.cs.lat,
                //    lng = SwarmInterface.Leader.cs.lng
                //});
            }
            System.Threading.Thread.Sleep(200);
            if (after_yaw_flag == 1)
            {
                dist = SwarmInterface.cmp_distance(new double[] { SwarmInterface.Leader.cs.lat, SwarmInterface.Leader.cs.lng, SwarmInterface.Leader.cs.alt }, new double[] { pos[1], pos[0], pos[2] });
                int n = 1;
                while (dist > 3 && n < 100000)
                {

                    dist = SwarmInterface.cmp_distance(new double[] { SwarmInterface.Leader.cs.lat, SwarmInterface.Leader.cs.lng, SwarmInterface.Leader.cs.alt }, new double[] { pos[1], pos[0], pos[2] });
                    n = n + 1;
                    System.Threading.Thread.Sleep(10);
                }
                //float yaw = yaw_target;
                MainV2.comPort.doCommand(SwarmInterface.leader_tag, SwarmInterface.leader_compid, MAVLink.MAV_CMD.CONDITION_YAW, yaw_target, 100.0f, 0, 0, 0, 0, 0, false);
                //System.Threading.Thread.Sleep(5000);
                //int n = 0;
                //while (Math.Abs(SwarmInterface.Leader.cs.yaw- yaw_target) > 5&& n<1000 )
                //{


                //    System.Threading.Thread.Sleep(10);
                //    n = n + 1;
                //    continue;
                //}
            }
        }

        public float cmp_yaw(double[] ori, double[] tar)
        {
            double X = Math.Cos(tar[1] * MathHelper.deg2rad) * Math.Sin((tar[0] - ori[0]) * MathHelper.deg2rad);
            double Y = Math.Cos(ori[1] * MathHelper.deg2rad) * Math.Sin(tar[1] * MathHelper.deg2rad) - Math.Sin(ori[1] * MathHelper.deg2rad) * Math.Cos(tar[1] * MathHelper.deg2rad) * Math.Cos((tar[0] - ori[0]) * MathHelper.deg2rad);
            double yaw = Math.Round(Math.Atan2(X, Y) * MathHelper.rad2deg + 720);
            int i = Math.DivRem((int)yaw, 360, out int yaw2);
            float yaw1 = (float)yaw2;
            return yaw1;
        }


        public void BUT_demospoof_click(object sender, EventArgs e)
        {
            if (SwarmInterface.mav_tag.Length > 1)
            {
                System.Threading.Thread demo_spoof_thread = new System.Threading.Thread(demospoof);
                demo_spoof_thread.IsBackground = true;
                demo_spoof_thread.Start();
            }
            else { MessageBox.Show("Only one UVA!!"); }


        }
        public void demospoof()
        {
            ProcessThreadCollection currentThreads = Process.GetCurrentProcess().Threads;
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav.sysid != SwarmInterface.mav_tag[SwarmInterface.mav_tag.Length - 1])
                        continue;


                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    // {
                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    //offset.y = 0;
                    //offset.x = -mav.sysid * 25;
                    //offset.y = ((index) * 20);
                    offset.y = (index * 25) * Math.Pow(-1, index);
                    
                    offset.x = -(index * 20);

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    // }
                }
            }


            //float alt = (float)SwarmInterface.Leaderpos_alt;
            //float lat = (float)SwarmInterface.Leaderpos_lat;
            //float lng = (float)SwarmInterface.Leaderpos_lng;

            //float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            //float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            //float lng_ori = (float)SwarmInterface.Leaderpos_lng;
            //float id = (float)SwarmInterface.leader_tag;
            //float compid = (float)SwarmInterface.leader_compid;
            //float stay = 1;
            double temp_x = 0;
            double temp_y = 0;
            //var pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, //stay };//149.16559338569641F, -35.362407919859933F, 50 
            //System.Threading.Thread.Sleep(2000);
            //flyto(pos);
            MainV2.comPort.setMode(SwarmInterface.leader_tag, SwarmInterface.leader_compid, "POSHOLD");
            System.Threading.Thread.Sleep(10);
            MainV2.comPort.setMode(SwarmInterface.leader_tag, SwarmInterface.leader_compid, "GUIDED");
            System.Threading.Thread.Sleep(15000);
            
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav.sysid != SwarmInterface.mav_tag[SwarmInterface.mav_tag.Length - 1])
                        continue;


                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    // {
                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    //offset.y = 0;


                    offset.y = (index * 5) * Math.Pow(-1, index);
                    temp_y = (index * 5) * Math.Pow(-1, index);
                    //offset.x = -mav.sysid * 2;
                    offset.x = -(index * 10);
                    temp_x = -(index * 10);
                    //offset.z = -mav.sysid * 1.5;
                    //offset.z = -((index) * 5);

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    // }
                }
            }
            int flag = 0;
            
            while (true)
            {
                foreach (var port in MainV2.Comports)
                {
                    foreach (var mav in port.MAVlist)
                    {
                        if (mav.sysid != SwarmInterface.mav_tag[SwarmInterface.mav_tag.Length - 1])
                            continue;
                        Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);


                        if ((Math.Abs(offset.y - temp_y) + Math.Abs(offset.x - temp_x)) < 3)
                        {
                            flag = 1;
                            
                            break;
                        }

                        // }
                    }
                    if (flag == 1)
                        break;
                }
                if (flag == 1)
                    break;
            }
            float alt = float.Parse(Text_alt.Text);
            float lat = float.Parse(Text_lat.Text);
            float lng = float.Parse(Text_lng.Text);

            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            float lng_ori = (float)SwarmInterface.Leaderpos_lng;

            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            var pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0 };//149.16559338569641F, -35.362407919859933F, 50 
            //System.Threading.Thread demospoof_thread = new System.Threading.Thread(flyto);
            //demospoof_thread.IsBackground = true;
            //demospoof_thread.Start(pos);
            flyto(pos);


        }
        public void BUT_demonoise_click(object sender, EventArgs e)
        {
            BUT_Form4_Click(sender, e);
            float alt = float.Parse(Text_alt.Text);
            float lat = float.Parse(Text_lat.Text);
            float lng = float.Parse(Text_lng.Text);

            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            float lng_ori = (float)SwarmInterface.Leaderpos_lng;

            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            var pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0, 0 };//149.16559338569641F, -35.362407919859933F, 50 
            System.Threading.Thread demo_noise_thread_inside = new System.Threading.Thread(flyto);
            demo_noise_thread_inside.IsBackground = true;
            demo_noise_thread_inside.Start(pos);
            //System.Threading.Thread demo_noise_thread = new System.Threading.Thread(demonoise);
            //demo_noise_thread.IsBackground = true;
            //demo_noise_thread.Start();
            //BUT_altlvl_click(sender, e);
        }
        public void demonoise()
        {
            float alt = float.Parse(Text_alt.Text);
            float lat = float.Parse(Text_lat.Text);
            float lng = float.Parse(Text_lng.Text);

            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            float lng_ori = (float)SwarmInterface.Leaderpos_lng;

            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            var pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0 };//149.16559338569641F, -35.362407919859933F, 50 
            System.Threading.Thread demo_noise_thread_inside = new System.Threading.Thread(flyto);
            demo_noise_thread_inside.IsBackground = true;
            demo_noise_thread_inside.Start(pos);
            //double dist = SwarmInterface.cmp_distance(new double[] { SwarmInterface.Leader.cs.lat, SwarmInterface.Leader.cs.lng, SwarmInterface.Leader.cs.alt }, new double[] { lat, lng, alt });
            //while (dist > 3) {
            //    dist = SwarmInterface.cmp_distance(new double[] { SwarmInterface.Leader.cs.lat, SwarmInterface.Leader.cs.lng, SwarmInterface.Leader.cs.alt }, new double[] { lat, lng, alt });
            //}
            //float yaw = 0F;
            //MainV2.comPort.doCommand(SwarmInterface.leader_tag, SwarmInterface.leader_compid, MAVLink.MAV_CMD.CONDITION_YAW, yaw,
            //                            100.0f, 0, 0, 0, 0, 0, false);
            ////System.Threading.Thread.Sleep(5000);
            //while (Math.Abs(SwarmInterface.Leader.cs.yaw) > 5) {
            //    continue;
            //}
            //foreach (var port in MainV2.Comports)
            //{
            //    foreach (var mav in port.MAVlist)
            //    {
            //        //mav.cs.UpdateCurrentSettings(null, true, port, mav);

            //        if (mav == SwarmInterface.Leader)
            //            continue;


            //        Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

            //        //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
            //        // {
            //        int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
            //        //offset.y = 0;
            //        //offset.x = -mav.sysid * 4
            //        //offset.x = -((index) * 10);
            //        //offset.z = -mav.sysid *1.5;
            //        offset.z = 0;

            //        grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
            //        ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
            //        // }
            //    }
            //}


        }
        private void BUT_addnoise_click(object sender, EventArgs e)
        {
            if (BUT_addnoise.Text == "+ noise")
            {
                BUT_addnoise.Text = "- noise";
                SwarmInterface.std = .000001;
            }
            else
            {
                BUT_addnoise.Text = "+ noise";
                SwarmInterface.std = 0;
            }
        }

        private void BUT_flyto_distangle_click(object sender, EventArgs e)
        {
            float alt = float.Parse(Text_alt.Text);
            float lat = float.Parse(Text_lat.Text);
            float lng = float.Parse(Text_lng.Text);

            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            float lng_ori = (float)SwarmInterface.Leaderpos_lng;

            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            //var pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0 };//149.16559338569641F, -35.362407919859933F, 50 
            double d = double.Parse(Text_dis_gate.Text) / 1000.0;
            float angle = float.Parse(Text_angle_gate.Text);
            float[] startlatlng = find_lat_lon(d, lat, lng, angle);
            float angle1 = 0;
            if (angle < 180)
            {
                angle1 = 180 + angle;
            }
            else { angle1 = angle - 180; }
            //var pos = new float[] { startlatlng[0], startlatlng[1],alt };
            var pos = new float[] { startlatlng[0], startlatlng[1], alt, lng_ori, lat_ori, alt_ori, id, compid, 0, angle1 };
            System.Threading.Thread demo_gate_thread = new System.Threading.Thread(flyto);
            demo_gate_thread.IsBackground = true;
            demo_gate_thread.Start(pos);

        }

        public float[] find_lat_lon(double d, float lat, float lng, float angle)
        {
            double lat0 = (double)lat;
            double lng0 = (double)lng;
            double R = 6378.1;
            double brng = (double)angle * MathHelper.deg2rad;
            double lat1 = lat0 * MathHelper.deg2rad;
            double lng1 = lng0 * MathHelper.deg2rad;

            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(d / R) + Math.Cos(lat1) * Math.Sin(d / R) * Math.Cos(brng));
            double X = Math.Cos(lat1) * Math.Sin(d / R) * Math.Sin(brng);
            double Y = Math.Cos(d / R) - Math.Sin(lat1) * Math.Sin(lat2);

            double lng2 = lng1 + Math.Atan2(X, Y);
            lat2 = lat2 * MathHelper.rad2deg;
            lng2 = lng2 * MathHelper.rad2deg;
            float[] result = { (float)lng2, (float)lat2 };
            return (result);
        }
        private void BUT_altmin_click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav == SwarmInterface.Leader)
                        continue;


                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    // {
                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    //offset.y = 0;
                    //offset.x = -mav.sysid * 4
                    //offset.x = -((index) * 10);
                    //offset.z = -mav.sysid *1.5;
                    offset.z = -((index) * 5);

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    // }
                }
            }
        }
        private void BUT_altlvl_click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav == SwarmInterface.Leader)
                        continue;


                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    // {
                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    //offset.y = 0;
                    //offset.x = -mav.sysid * 4
                    //offset.x = -((index) * 10);
                    //offset.z = -mav.sysid *1.5;
                    offset.z = 0;

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    // }
                }
            }
        }
        private void BUT_altadd_click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav == SwarmInterface.Leader)
                        continue;


                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    // {
                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    //offset.y = 0;
                    //offset.x = -mav.sysid * 4
                    //offset.x = -((index) * 10);
                    //offset.z = -mav.sysid *1.5;
                    offset.z = ((index) * 5);

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    // }
                }
            }
        }



        private void BUT_follow_me_click(object sender, EventArgs e)
        {

            if (Watcher == null)
            {
                BUT_follow_me.Text = "F.M.(on)";
                Watcher = new GeoCoordinateWatcher();
                // Catch the StatusChanged event.

                //Watcher.StatusChanged += Watcher_StatusChanged;
                // Start the watcher.

                //Watcher.Start();
                Watcher.MovementThreshold = 1;
                Watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(Watcher_PositionChanged);
                bool started = Watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
            }
            else
            {
                BUT_follow_me.Text = "F.M.(off)";
                Watcher.Stop();
                Watcher = null;
            }
        }

        private void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            sendPosition(e.Position.Location.Latitude, e.Position.Location.Longitude);
        }

        private void sendPosition(double Latitude, double Longitude)
        {
            float alt = 50F;
            float lat = (float)Latitude;
            float lng = (float)Longitude;

            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            float lng_ori = (float)SwarmInterface.Leaderpos_lng;

            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            //var pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0 };//149.16559338569641F, -35.362407919859933F, 50 
            double d = 30.0 / 1000.0;

            float angle = 270F;
            float[] startlatlng = find_lat_lon(d, lat, lng, angle);
            float angle1 = 0F;
            if (angle < 180)
            {
                angle1 = 180 + angle;
            }
            else { angle1 = angle - 180; }
            //var pos = new float[] { startlatlng[0], startlatlng[1],alt };
            var pos = new float[] { startlatlng[0], startlatlng[1], alt, lng_ori, lat_ori, alt_ori, id, compid, 0, angle1 };
            System.Threading.Thread demo_gate_thread = new System.Threading.Thread(flyto);
            demo_gate_thread.IsBackground = true;
            demo_gate_thread.Start(pos);
        }



        // The watcher's status has change. See if it is ready.
        //private void Watcher_StatusChanged(object sender,
        //    GeoPositionStatusChangedEventArgs e)
        //{
        //    if (e.Status == GeoPositionStatus.Ready)
        //    {
        //        // Display the latitude and longitude.
        //        if (Watcher.Position.Location.IsUnknown)
        //        {
        //            string txtLat = "Cannot find location data";
        //        }
        //        else
        //        {
        //            GeoCoordinate location =
        //                Watcher.Position.Location;
        //            float alt = 50F;
        //            float lat = (float)location.Latitude;
        //            float lng = (float)location.Longitude;

        //            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
        //            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
        //            float lng_ori = (float)SwarmInterface.Leaderpos_lng;

        //            float id = (float)SwarmInterface.leader_tag;
        //            float compid = (float)SwarmInterface.leader_compid;
        //            //var pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0 };//149.16559338569641F, -35.362407919859933F, 50 
        //            double d = 50.0 / 1000.0;

        //            float angle = 0F;
        //            float[] startlatlng = find_lat_lon(d, lat, lng, angle);
        //            float angle1 = 0;
        //            if (angle < 180)
        //            {
        //                angle1 = 180 + angle;
        //            }
        //            else { angle1 = angle - 180; }
        //            //var pos = new float[] { startlatlng[0], startlatlng[1],alt };
        //            var pos = new float[] { startlatlng[0], startlatlng[1], alt, lng_ori, lat_ori, alt_ori, id, compid, 0, angle1 };
        //            System.Threading.Thread demo_gate_thread = new System.Threading.Thread(flyto);
        //            demo_gate_thread.IsBackground = true;
        //            demo_gate_thread.Start(pos);
        //        }
        //    }
        //}

        private void BUT_yaw_Click(object sender, EventArgs e)
        {
            float yaw = float.Parse(Text_yaw.Text);
            MainV2.comPort.doCommand(SwarmInterface.leader_tag, SwarmInterface.leader_compid, MAVLink.MAV_CMD.CONDITION_YAW, yaw,
                                        100.0f, 0, 0, 0, 0, 0, false);
        }
        private void Form3_stage_1(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav == SwarmInterface.Leader)

                        continue;


                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    // {
                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    offset.y = 0;
                    //offset.x = -mav.sysid * 4
                    offset.x = -((index) * 10);
                    //offset.z = -mav.sysid *1.5;
                    offset.z = -((index) * 5);

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    // }
                }
            }
        }

        private void BUT_allone_click(object sender, EventArgs e)
        {
            //Array.(SwarmInterface.sysid_enter_start, 1, SwarmInterface.sysid_enter_start.Length);
            SwarmInterface.sysid_enter_start = Enumerable.Repeat(1, SwarmInterface.sysid_enter_start.Length).ToArray();
        }
        void mainloop()
        {
            threadrun = true;

            // make sure leader is high freq updates
            SwarmInterface.Leader.parent.requestDatastream(MAVLink.MAV_DATA_STREAM.POSITION, 10, SwarmInterface.Leader.sysid, SwarmInterface.Leader.compid);
            SwarmInterface.Leader.cs.rateposition = 10;
            SwarmInterface.Leader.cs.rateattitude = 10;

            while (threadrun && !this.IsDisposed)
            {
                // update leader pos
                SwarmInterface.Update();

                // update other mavs
                SwarmInterface.SendCommand();
                //MainV2.comPort.setGuidedModeWP(new Locationwp
                //{
                //    alt = MainV2.comPort.MAV.GuidedMode.z,
                //    lat = MainV2.comPort.MAV.GuidedMode.x,
                //    lng = MainV2.comPort.MAV.GuidedMode.y
                //}); 
                // 10 hz
                System.Threading.Thread.Sleep(100);
            }
        }

        private void BUT_Arm_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.Arm();
            }
        }

        private void BUT_Disarm_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.Disarm();
            }
        }

        private void BUT_Takeoff_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.Takeoff();
            }
        }

        private void BUT_Land_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.Land();
            }
        }

        private void BUT_leader_Click(object sender, EventArgs e)
        {

            if (SwarmInterface != null)
            {
                var vectorlead = SwarmInterface.getOffsets(MainV2.comPort.MAV);

                foreach (var port in MainV2.Comports)
                {
                    foreach (var mav in port.MAVlist)
                    {
                        var vector = SwarmInterface.getOffsets(mav);

                        SwarmInterface.setOffsets(mav, (float)(vector.x - vectorlead.x),
                            (float)(vector.y - vectorlead.y),
                            (float)(vector.z - vectorlead.z));


                    }
                }

                SwarmInterface.setLeader(MainV2.comPort.MAV);

                //added by wenchao
                var mav1 = SwarmInterface.getLeader();
                SwarmInterface.leader_tag = mav1.sysid;
                SwarmInterface.leader_compid = mav1.compid;
                int index_temp = SwarmInterface.mav_tag_temp.IndexOf(SwarmInterface.leader_tag);
                //int index_temp = Array.IndexOf(SwarmInterface.mav_tag_temp, SwarmInterface.leader_tag);
                //byte temp = SwarmInterface.mav_tag_temp[0];
                List<byte> mav_tag_temp_temp = new List<byte>();
                SwarmInterface.mav_tag_temp.ForEach(i => mav_tag_temp_temp.Add(i));

                SwarmInterface.mav_tag_temp[index_temp] = 0;
                SwarmInterface.mav_tag = SwarmInterface.mav_tag_temp.ToArray();
                Array.Sort(SwarmInterface.mav_tag);
                SwarmInterface.mav_tag[0] = SwarmInterface.leader_tag;
                //SwarmInterface.mav_tag_temp[index_temp] = temp;
                //SwarmInterface.mav_tag_temp.Sort();
                //SwarmInterface.mav_tag_temp.FindIndex(SwarmInterface.leader_tag);
                SwarmInterface.mav_tag_temp = mav_tag_temp_temp;
                SwarmInterface.Leaderpos_lat = mav1.cs.lat;
                SwarmInterface.Leaderpos_lng = mav1.cs.lng;
                SwarmInterface.Leaderpos_alt = mav1.cs.alt; SwarmInterface.Leaderpos_lat = mav1.cs.lat;
                SwarmInterface.Leaderpos_lng = mav1.cs.lng;
                SwarmInterface.Leaderpos_alt = mav1.cs.alt;
                //end
                updateicons();
                BUT_Start.Enabled = true;
                BUT_Updatepos.Enabled = true;
                BUT_Form1.Enabled = true;
                BUT_Form2.Enabled = true;
                BUT_Form3.Enabled = true;
                BUT_Form4.Enabled = true;
                BUT_yaw.Enabled = true;
                BUT_demonoise.Enabled = true;
                BUT_demospoof.Enabled = true;
                BUT_addnoise.Enabled = true;
                BUT_flyto_distangle.Enabled = true;
                BUT_allone.Enabled = true;
                BUT_follow_me.Enabled = true;
                leader_homeloc = new PointLatLngAlt(SwarmInterface.Leader.cs.HomeLocation);
                Text_lat.Text = leader_homeloc.Lat.ToString();
                Text_lng.Text = leader_homeloc.Lng.ToString();
                Text_lat.Refresh();
                Text_lng.Refresh();
                //leader_tag = mav.sysid;
                //mav_tag[mav.sysid] = mav.sysid;

            }


        }

        private void BUT_connect_Click(object sender, EventArgs e)
        {
            Comms.CommsSerialScan.Scan(true);

            DateTime deadline = DateTime.Now.AddSeconds(50);

            while (Comms.CommsSerialScan.foundport == false)
            {
                System.Threading.Thread.Sleep(100);

                if (DateTime.Now > deadline)
                {
                    CustomMessageBox.Show("Timeout waiting for autoscan/no mavlink device connected");
                    return;
                }
            }

            bindingSource1.ResetBindings(false);
        }

        public Vector3 getOffsetFromLeader(MAVState leader, MAVState mav)
        {
            //convert Wgs84ConversionInfo to utm
            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

            IGeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;

            int utmzone = (int)((leader.cs.lng - -186.0) / 6.0);

            IProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(utmzone,
                leader.cs.lat < 0 ? false : true);

            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84, utm);

            double[] masterpll = { leader.cs.lng, leader.cs.lat };

            // get leader utm coords
            double[] masterutm = trans.MathTransform.Transform(masterpll);

            double[] mavpll = { mav.cs.lng, mav.cs.lat };

            //getLeader follower utm coords
            double[] mavutm = trans.MathTransform.Transform(mavpll);

            var heading = -leader.cs.yaw;

            var norotation = new Vector3(masterutm[1] - mavutm[1], masterutm[0] - mavutm[0], 0);

            norotation.x *= -1;
            norotation.y *= -1;

            return new Vector3(norotation.x * Math.Cos(heading * MathHelper.deg2rad) - norotation.y * Math.Sin(heading * MathHelper.deg2rad), norotation.x * Math.Sin(heading * MathHelper.deg2rad) + norotation.y * Math.Cos(heading * MathHelper.deg2rad), 0);
        }

        private void grid1_UpdateOffsets(MAVState mav, float x, float y, float z, Grid.icon ico)
        {
            if (mav == SwarmInterface.Leader)
            {
                CustomMessageBox.Show("Can not move Leader");
                ico.z = 0;
            }
            else
            {
                ((Formation)SwarmInterface).setOffsets(mav, x, y, z);
            }
        }

        private void Control_FormClosing(object sender, FormClosingEventArgs e)
        {
            threadrun = false;
        }

        private void BUT_Updatepos_Click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav == SwarmInterface.Leader)
                        continue;

                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    {
                        grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                        ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    }
                }
            }
        }

        private void timer_status_Tick(object sender, EventArgs e)
        {
            // clean up old
            foreach (Control ctl in PNL_status.Controls)
            {
                bool match = false;
                foreach (var port in MainV2.Comports)
                {
                    foreach (var mav in port.MAVlist)
                    {
                        if (mav == (MAVState)ctl.Tag)
                        {
                            match = true;

                        }
                    }
                }

                if (match == false)
                    ctl.Dispose();
            }

            // setup new
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    bool exists = false;
                    foreach (Control ctl in PNL_status.Controls)
                    {
                        if (ctl is Status && ctl.Tag == mav)
                        {
                            exists = true;
                            ((Status)ctl).GPS.Text = mav.cs.gpsstatus >= 3 ? "OK" : "Bad";
                            ((Status)ctl).Armed.Text = mav.cs.armed.ToString();
                            ((Status)ctl).Mode.Text = mav.cs.mode;
                            ((Status)ctl).MAV.Text = mav.ToString();
                            ((Status)ctl).Guided.Text = mav.GuidedMode.x + "," + mav.GuidedMode.y + "," +
                                                         mav.GuidedMode.z;
                            ((Status)ctl).Location1.Text = mav.cs.lat + "," + mav.cs.lng + "," +
                                                            mav.cs.alt;

                            if (mav == SwarmInterface.Leader)
                            {
                                ((Status)ctl).ForeColor = Color.Red;
                            }
                            else
                            {
                                ((Status)ctl).ForeColor = Color.Black;
                            }
                        }
                    }

                    if (!exists)
                    {
                        Status newstatus = new Status();
                        newstatus.Tag = mav;
                        PNL_status.Controls.Add(newstatus);
                    }
                }
            }
        }

        private void but_guided_Click(object sender, EventArgs e)
        {
            if (SwarmInterface != null)
            {
                SwarmInterface.GuidedMode();
            }
        }
    }
}