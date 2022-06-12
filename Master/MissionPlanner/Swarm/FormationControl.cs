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
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net;
using System.Text;


namespace MissionPlanner.Swarm
{

    public partial class FormationControl : Form
    {
        delegate void SetTextCallback(string text);
        private PointLatLngAlt leader_homeloc = new PointLatLngAlt();
        Formation SwarmInterface = null;
        bool threadrun = false;
        private GeoCoordinateWatcher Watcher = null;
        //public int[] sysid_enter_start = new int[] { 0, 0, 0, 0, 0, 0, 0 };
        public int UWB_id = 0;
        public MqttClient client;
        public string[] strValue = { "102", "103", "104", "108", "205" };
        public string[] topics = { "102message", "103message", "104message", "108message", "205message" };
        private System.Threading.AutoResetEvent  _messageReceived;
        public double[,] darray = new double[5, 5];
        private double randNormal_lng = 0;
        private double randNormal_lat = 0;
        private Random rnd = new Random();

        public string[] connected_UWB = new string[4];
        public int endFlag = 0;
        public int connected_UWB_count = 0;
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


            //MQTT
            IPAddress a = IPAddress.Parse("192.168.0.125");
            client = new MqttClient(a);


            //string[] topics = new string[2] { "104message", "205message" };




            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE };
            //client.Subscribe(new string[] { "103message" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            //client.Subscribe(topic, qosLevels);
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            
            Console.WriteLine(Guid.NewGuid().ToString());
            client.Connect(Guid.NewGuid().ToString());

            //MQTT
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

            //float rssi = SwarmInterface.Leader.cs.linkqualitygcs;
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
                    offset.x = -((index) * 5);
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


        //// Create and start the watcher.

        private void BUT_Form2_Click(object sender, EventArgs e)
        {
            //int odd = 0;
            //int group = Math.DivRem(SwarmInterface.mav_tag.Length, 2, out int remainder);
            //if (remainder != 0) { odd = 1; }
            //int group_num = (SwarmInterface.mav_tag.Length) / 2;

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
                    int index = Math.DivRem(Array.IndexOf(SwarmInterface.mav_tag, mav.sysid), 3, out int remainder);
                    int index1 = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    //offset.y = (index * 3.5) * Math.Pow(-1, index);
                    //offset.y = Math.Cos(Math.PI/6)* (index + 1) * 5 * Math.Pow(-1, index1);
                    offset.y = Math.Sqrt(Math.Pow(((index + 1) * 5), 2) - Math.Pow(2.5 * (index + 1), 2)) * Math.Pow(-1, index1);
                    offset.x = -((index + 1) * 5);
                    offset.z = -(index1 * 5);




                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);

                    // }
                }
            }
        }
        private void BUT_Form3_Click(object sender, EventArgs e)//leader goto button
        {
            float alt = float.Parse(Text_alt.Text);
            float lat = float.Parse(Text_lat.Text);
            float lng = float.Parse(Text_lng.Text);

            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            float lng_ori = (float)SwarmInterface.Leaderpos_lng;

            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            var pos = new float[] { lng, lat, alt_ori, lng_ori, lat_ori, alt_ori, id, compid, 0 };//149.16559338569641F, -35.362407919859933F, 50 
            System.Threading.Thread Form3_thread = new System.Threading.Thread(flyto);
            Form3_thread.IsBackground = true;
            Form3_thread.Start(pos);

            //flyto(pos);
        }
        private void BUT_Form4_Click(object sender, EventArgs e)//demo noise
        {


            if (SwarmInterface.mav_tag.Length <= 5)
            {
                double[] pos_y = new double[] { 0, 15, 7.5, 0, 15 };
                double[] pos_x = new double[] { 0, 0, -7.5, -15, -15 };

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
                        int index1 = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                        offset.y = pos_y[remainder];
                        //offset.x = -mav.sysid * 2;
                        offset.x = pos_x[remainder];
                        //if(SwarmInterface.Leaderpos_alt>30)
                        offset.z = -index1 * 5;
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

        private void BUT_Leader_alt_Click(object sender, EventArgs e)
        {
            float alt = float.Parse(Text_alt.Text);
            float lat = float.Parse(Text_lat.Text);
            float lng = float.Parse(Text_lng.Text);

            float alt_ori = (float)SwarmInterface.Leader.cs.alt;
            float lat_ori = (float)SwarmInterface.Leader.cs.lat;
            float lng_ori = (float)SwarmInterface.Leader.cs.lng;

            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            var pos = new float[] { lng_ori, lat_ori, alt, lng_ori, lat_ori, alt_ori, id, compid, 0 };//149.16559338569641F, -35.362407919859933F, 50 
            System.Threading.Thread Leader_alt_thread = new System.Threading.Thread(flyto);
            Leader_alt_thread.IsBackground = true;
            Leader_alt_thread.Start(pos);
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


            double dist = SwarmInterface.cmp_distance(new double[] { SwarmInterface.Leader.cs.lat, SwarmInterface.Leader.cs.lng, pos[5] }, new double[] { pos[1], pos[0], pos[5] });
            if (dist > 3 && pos[8] == 0)//change heading
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
            if (pos[8] == 0)//flyto destination
            {
                MainV2.comPort.setGuidedModeWP(id, compid, new Locationwp
                {
                    alt = pos[2],
                    lat = pos[1],
                    lng = pos[0]
                });
            }
            else//stay here
            {
                //SwarmInterface.Leader.cs.mode = "POSHOLD";
                MainV2.comPort.setMode(SwarmInterface.leader_tag, SwarmInterface.leader_compid, "POSHOLD");
                System.Threading.Thread.Sleep(100);
                MainV2.comPort.setMode(SwarmInterface.leader_tag, SwarmInterface.leader_compid, "GUIDED");
                //MainV2.comPort.setGuidedModeWP(id, compid, new Locationwp
                //{
                //    alt = SwarmInterface.Leader.cs.alt,
                //    lat = SwarmInterface.Leader.cs.lat,
                //    lng = SwarmInterface.Leader.cs.lng
                //});
            }
            System.Threading.Thread.Sleep(200);
            if (after_yaw_flag == 1)//change heading after arriving
            {
                dist = SwarmInterface.cmp_distance(new double[] { SwarmInterface.Leader.cs.lat, SwarmInterface.Leader.cs.lng, SwarmInterface.Leader.cs.alt }, new double[] { pos[1], pos[0], pos[2] });
                int n = 1;
                while (dist > 3 && n < 100000)
                {

                    dist = SwarmInterface.cmp_distance(new double[] { SwarmInterface.Leader.cs.lat, SwarmInterface.Leader.cs.lng, SwarmInterface.Leader.cs.alt }, new double[] { pos[1], pos[0], pos[2] });
                    n = n + 1;
                    System.Threading.Thread.Sleep(10);
                }
                MainV2.comPort.doCommand(SwarmInterface.leader_tag, SwarmInterface.leader_compid, MAVLink.MAV_CMD.CONDITION_YAW, yaw_target, 100.0f, 0, 0, 0, 0, 0, false);

            }
            Console.WriteLine("Fly to has finished");
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
            double offsetX_pre = 0;
            double offsetY_pre = 0;
            double offsetZ_pre = 0;
            float lng = 0;
            float alt = 0;
            float lat = 0;
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);


                    if (mav == SwarmInterface.Leader)
                    {

                        lng = (float)mav.GuidedMode.y;
                        lat = (float)mav.GuidedMode.x;
                        alt = (float)mav.GuidedMode.z;
                    }

                    if (mav.sysid != SwarmInterface.mav_tag[SwarmInterface.mav_tag.Length - 1])
                        //if (mav.sysid != SwarmInterface.mav_tag[1])
                        continue;

                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);

                    //if (Math.Abs(offset.x) < 200 && Math.Abs(offset.y) < 200)
                    // {
                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    //offset.y = 0;
                    //offset.x = -mav.sysid * 25;
                    //offset.y = ((index) * 20);

                    offsetX_pre = offset.x;
                    offsetY_pre = offset.y;
                    offsetZ_pre = (float)mav.cs.alt - SwarmInterface.Leader.cs.alt;
                    offset.y = (index * 25) * Math.Pow(-1, index);

                    offset.x = -(index * 20);
                    offset.z = offsetZ_pre;

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);
                    // }
                }
            }



            //double temp_x = 0;
            //double temp_y = 0;

            MainV2.comPort.setMode(SwarmInterface.leader_tag, SwarmInterface.leader_compid, "POSHOLD");
            System.Threading.Thread.Sleep(10);
            MainV2.comPort.setMode(SwarmInterface.leader_tag, SwarmInterface.leader_compid, "GUIDED");
            System.Threading.Thread.Sleep(10000);

            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    //mav.cs.UpdateCurrentSettings(null, true, port, mav);

                    if (mav.sysid != SwarmInterface.mav_tag[SwarmInterface.mav_tag.Length - 1])
                        //if (mav.sysid != SwarmInterface.mav_tag[1])
                        continue;


                    Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);


                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    offset.y = offsetY_pre;
                    //temp_y = offset.y;

                    offset.x = offsetX_pre;
                    offset.z = offsetZ_pre;
                    //temp_x = offset.x;
                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);

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
                            //if (mav.sysid != SwarmInterface.mav_tag[1])
                            continue;
                        Vector3 offset = getOffsetFromLeader(((Formation)SwarmInterface).getLeader(), mav);


                        if ((Math.Abs(offset.y - offsetY_pre) + Math.Abs(offset.x - offsetX_pre)) < 3)
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
            //float alt = float.Parse(Text_alt.Text);
            //float lat = float.Parse(Text_lat.Text);
            //float lng = float.Parse(Text_lng.Text);

            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            float lng_ori = (float)SwarmInterface.Leaderpos_lng;

            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            var pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0 };

            System.Threading.Thread demo_spoof_thread = new System.Threading.Thread(flyto);
            demo_spoof_thread.IsBackground = true;
            demo_spoof_thread.Start(pos);


        }

        public void BUT_guided_leader_click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    if (mav != SwarmInterface.Leader)
                        continue;

                    port.setMode(mav.sysid, mav.compid, "GUIDED");
                }
            }
        }

        public void BUT_poshold_leader_click(object sender, EventArgs e)
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    if (mav != SwarmInterface.Leader)
                        continue;

                    port.setMode(mav.sysid, mav.compid, "POSHOLD");
                }
            }
        }

        public void Radio_fields_1_click(object sender, EventArgs e)
        {
            Radio_fields_1.Checked = true;
            Radio_fields_2.Checked = false;
        }
        public void Radio_fields_2_click(object sender, EventArgs e)
        {
            Radio_fields_2.Checked = true;
            Radio_fields_1.Checked = false;
        }

        public void BUT_demonoise_click(object sender, EventArgs e)
        {
            if (SwarmInterface.Leader.cs.alt > 30)
                BUT_Form4_Click(sender, e);
            float alt = float.Parse(Text_alt.Text);
            float lng = float.Parse(Text_lng.Text);
            float lat = float.Parse(Text_lat.Text);
            if (Radio_fields_1.Checked == true)
            {

                lng = float.Parse(Text_lng.Text) + (float)randNormal_lng;
            }
            else
            {
                lat = float.Parse(Text_lat.Text) + (float)randNormal_lat;

            }


            float alt_ori = (float)SwarmInterface.Leaderpos_alt;
            float lat_ori = (float)SwarmInterface.Leaderpos_lat;
            float lng_ori = (float)SwarmInterface.Leaderpos_lng;
            var pos = new float[] { };
            float id = (float)SwarmInterface.leader_tag;
            float compid = (float)SwarmInterface.leader_compid;
            if (SwarmInterface.Leaderpos_alt > 30)
                if (Radio_fields_1.Checked == true)
                    pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0, 0 };
                else pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0, 90 };
            //149.16559338569641F, -35.362407919859933F, 50 
            else
                pos = new float[] { lng, lat, alt, lng_ori, lat_ori, alt_ori, id, compid, 0 };
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

        }
        private void BUT_addnoise_click(object sender, EventArgs e)
        {
            if (BUT_addnoise.Text == "+ noise")
            {
                BUT_addnoise.Text = "- noise";
                //SwarmInterface.std = .000001;
                double u1 = 1.0 - rnd.NextDouble(); //uniform(0,1] random doubles
                double u2 = 1.0 - rnd.NextDouble();
                randNormal_lat = Math.Sqrt(-2.0 * Math.Log(u1)) *
                             Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                randNormal_lat = 0 + .00005 * randNormal_lat;

                u1 = 1.0 - rnd.NextDouble(); //uniform(0,1] random doubles
                u2 = 1.0 - rnd.NextDouble();
                randNormal_lng = Math.Sqrt(-2.0 * Math.Log(u1)) *
                             Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                randNormal_lng = 0 + .00005 * randNormal_lng;
            }
            else
            {
                BUT_addnoise.Text = "+ noise";
                //SwarmInterface.std = 0;
                randNormal_lng = 0;
                randNormal_lat = 0;
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


                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);
                    offset.z = -((index) * 5);

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);

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


                    int index = Array.IndexOf(SwarmInterface.mav_tag, mav.sysid);

                    offset.z = ((index) * 5);

                    grid1.UpdateIcon(mav, (float)offset.y, (float)offset.x, (float)offset.z, true);
                    ((Formation)SwarmInterface).setOffsets(mav, offset.y, offset.x, offset.z);

                }
            }
        }



        private void BUT_follow_me_click(object sender, EventArgs e)
        {
            //Access to UWB data
            //if (Watcher == null)
            //{
            //    BUT_follow_me.Text = "F.M.(on)";
            //    Watcher = new GeoCoordinateWatcher();

            //    Watcher.MovementThreshold = 1;
            //    Watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(Watcher_PositionChanged);
            //    bool started = Watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
            //}
            //else
            //{
            //    BUT_follow_me.Text = "F.M.(off)";
            //    Watcher.Stop();
            //    Watcher = null;
            //}

            if (SwarmInterface.UWB_start_button)
            {
                //SwarmInterface.collision_avoidence_start_button = false;
                //SwarmInterface.collision_avoid_start = 0;
                //BUT_follow_me.Text = "AV(off)";


                for (int ii = 0; ii < topics.Length; ii++)
                {
                    client.Unsubscribe(new string[] { topics[ii] });

                }
                SwarmInterface.UWB_start_button = false;
                SwarmInterface.UWB_start_start = 0;
                SwarmInterface.UWB_threadrun = 0;
            }
            else
            {
                //SwarmInterface.collision_avoidence_start_button = true;
                //SwarmInterface.collision_avoid_start = 0;
                //BUT_follow_me.Text = "AV(on)";

                for (int ii = 0; ii < topics.Length; ii++)
                {
                    client.Subscribe(new string[] { topics[ii] }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                }
                SwarmInterface.UWB_start_button = true;
                SwarmInterface.UWB_start_start = 1;
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

        private void BUT_RTL_click(object sender, EventArgs e)
        {
            if (BUT_Start.Text == Strings.Stop)
                BUT_Start_Click(sender, e);
            System.Threading.Thread RTL_thread = new System.Threading.Thread(RTL);
            RTL_thread.IsBackground = true;

            RTL_thread.Start();
        }

        private void RTL()
        {
            int flag = SwarmInterface.mav_tag.Length;
            int[] flag_set = { 0, 0, 0, 0, 0, 0, 0, 0 };
            while (flag > 0)
            {
                foreach (var port in MainV2.Comports)
                {
                    foreach (var mav in port.MAVlist)
                    {

                        //port.setMode(mav.sysid, mav.compid, "POSHOLD");
                        if (flag > 0 && mav.sysid == SwarmInterface.mav_tag[flag - 1])
                        {
                            if (flag_set[flag - 1] != 1)
                            {
                                port.setMode(mav.sysid, mav.compid, "RTL");
                                flag_set[flag - 1] = 1;
                                flag = flag - 1;
                                System.Threading.Thread.Sleep(10000);
                            }

                        }


                    }
                }
            }
            flag = SwarmInterface.mav_tag.Length;
            //int index = SwarmInterface.mav_tag.Length;
            //while (true) {
            //    foreach (var port in MainV2.Comports)
            //    {
            //        foreach (var mav in port.MAVlist)
            //        {
            //            if (mav.sysid != SwarmInterface.mav_tag[index-1])
            //                continue;
            //            //port.setMode(mav.sysid, mav.compid, "POSHOLD");
            //            port.setMode(mav.sysid, mav.compid, "RTL");
            //            double d = SwarmInterface.cmp_distance(new double[] { mav.cs.HomeLocation.Lat, mav.cs.HomeLocation.Lng, mav.cs.alt }, new double[] { mav.cs.lat, mav.cs.lng, mav.cs.alt });
            //            while (d > 1)
            //            {
            //                System.Threading.Thread.Sleep(1000);
            //                d = SwarmInterface.cmp_distance(new double[] { mav.cs.HomeLocation.Lat, mav.cs.HomeLocation.Lng, mav.cs.alt }, new double[] { mav.cs.lat, mav.cs.lng, mav.cs.alt });

            //            }
            //            index = index - 1;
            //        }
            //    }

            //    if (index == 0)
            //        break;
            //}

            //int[] index = { 0, 0, 0, 0, 0, 0, 0, 0 };
            //int index1 = 1;
            //while (true)
            //{
            //    foreach (var port in MainV2.Comports)
            //    {
            //        foreach (var mav in port.MAVlist)
            //        {
            //            //mav.cs.UpdateCurrentSettings(null, true, port, mav);
            //            if (index[mav.sysid] != index1)
            //            {
            //                continue;
            //            }

            //            MainV2.comPort.setGuidedModeWP(new Locationwp
            //            {
            //                alt = (float)mav.cs.alt,
            //                lat = (float)mav.cs.HomeLocation.Lat,
            //                lng = (float)mav.cs.HomeLocation.Lng
            //            });
            //            double d = SwarmInterface.cmp_distance(new double[] { mav.cs.HomeLocation.Lat, mav.cs.HomeLocation.Lng, mav.cs.alt }, new double[] { mav.cs.lat, mav.cs.lng, mav.cs.alt });
            //            while (d < 1)
            //            {
            //                System.Threading.Thread.Sleep(1000);
            //                d = SwarmInterface.cmp_distance(new double[] { mav.cs.HomeLocation.Lat, mav.cs.HomeLocation.Lng, mav.cs.alt }, new double[] { mav.cs.lat, mav.cs.lng, mav.cs.alt });

            //            }
            //            index1 = index1 + 1;
            //        }
            //        ////    }

            //        //    if (index1 == SwarmInterface.mav_tag.Length)
            //        //        break;
            //        //    System.Threading.Thread.Sleep(100);

            //        //}

            //        //System.Threading.Thread.Sleep(3000);




            //    }
            //    if (index1 == SwarmInterface.mav_tag.Length)
            //        break;
            //}
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

                if (SwarmInterface.UWB_threadrun == 0 && SwarmInterface.UWB_start_start == 1)
                {
                    
                    SwarmInterface.UWB_threadrun = 1;
                    System.Threading.Thread UWB_get_data_thread = new System.Threading.Thread(UWB_get_data_func);
                    
                    UWB_get_data_thread.IsBackground = true;
                    UWB_get_data_thread.Start();



                }
                System.Threading.Thread.Sleep(100);
            }
        }

        public void UWB_get_data_func()
        {
            this._messageReceived = new System.Threading.AutoResetEvent(false);
            while (SwarmInterface.UWB_start_start == 1)
            {
                for (int UWB_id = 0; UWB_id < strValue.Length; UWB_id++)
                {
                    
                    client.Publish(strValue[UWB_id], Encoding.UTF8.GetBytes(strValue[UWB_id]), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                    this._messageReceived.WaitOne(1000);
                    System.Threading.Thread.Sleep(10);

                }
                
            }


        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            
            // access data bytes throug e.Message
            string str = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
            double[] value = { };
            string strtemp = "";
            if (str.Length < 5)
            {
                if (!(Array.Exists(connected_UWB, element => element == str)))
                {

                    connected_UWB[connected_UWB_count] = str;
                    connected_UWB_count = connected_UWB_count + 1;
                }
            }
            else
            {
                
                str = str.Substring(1);
                str = str.Substring(0, str.Length - 1);
                

                string[] multiArray = str.Split(new Char[] { ',' });
                
                for(int i=0; i<= strValue.Length-1; i++) {
                    if (e.Topic == topics[i])
                    {
                        int ii = 0;
                        foreach (string s in multiArray)
                        {
                            multiArray[ii] = multiArray[ii].Trim();
                            if ((Convert.ToDouble(s) == 0 | Convert.ToDouble(s)>500) && ii != i)
                            {
                                darray[i, ii] = darray[i, ii];
                                multiArray[ii] = darray[i, ii].ToString();
                                //ii = ii + 1;

                            }
                            else
                            {
                                darray[i, ii] = Convert.ToDouble(s);
                                //ii = ii + 1;
                                
                            }

                            if ( ii == i)
                            {
                                darray[i, ii] = 0;
                                multiArray[ii] = "0.000";
                                //ii = ii + 1;

                            }

                            if (multiArray[ii].Length < 5)
                            {
                                if (multiArray[ii] == "0")
                                {
                                    multiArray[ii] = "0.000";
                                    
                                }
                                else
                                { 
                                    for (int k=0;k<5- multiArray[ii].Length;k++)
                                    {
                                        multiArray[ii] = multiArray[ii] + '0';

                                    }
                                }
                                }
                            
                            ii = ii + 1;
                        }
                        strtemp = string.Join(",", multiArray);
                        strtemp = strtemp + strValue[i];
                        //Text_UWB_dis1.Text = str;
                        Set_Text_UWB(strtemp);
                        break;
                    }
                }
                
                

                //switch (e.Topic)
                //{
                    
                //    case "102message":
                        
                        
                //        foreach (string s in multiArray)
                //        {
                //            if (Convert.ToDouble(s) == 0 && ii == 0)
                //            {
                //                darray[0, ii] = darray[0, ii];
                //                multiArray[ii] = darray[0, ii].ToString();
                //                ii = ii + 1;

                //            }
                //            else {
                //                darray[0, ii] = Convert.ToDouble(s);
                //                ii = ii + 1;
                //            }
                            
                //        }
                //        strtemp = string.Join(",", multiArray);
                //        strtemp = strtemp + strValue[0];
                //        //Text_UWB_dis1.Text = str;
                //        Set_Text_UWB(strtemp);
                //        break;
                //    case "103message":
                //        //strtemp = str + "103";
                //        //Set_Text_UWB(strtemp);
                //        foreach (string s in multiArray)
                //        {
                //            if (Convert.ToDouble(s) == 0 && ii == 1)
                //            {
                //                darray[0, ii] = darray[0, ii];
                //                multiArray[ii] = darray[0, ii].ToString();
                //                ii = ii + 1;

                //            }
                //            else
                //            {
                //                darray[0, ii] = Convert.ToDouble(s);
                //                ii = ii + 1;
                //            }

                //        }
                //        strtemp = string.Join(",", multiArray);
                //        strtemp = strtemp + strValue[1];
                //        //Text_UWB_dis1.Text = str;
                //        Set_Text_UWB(strtemp);
                //        break;
                //    case "104message":
                //        //strtemp = str + "104";
                //        //Set_Text_UWB(strtemp);
                //        foreach (string s in multiArray)
                //        {
                //            if (Convert.ToDouble(s) == 0 && ii == 2)
                //            {
                //                darray[0, ii] = darray[0, ii];
                //                multiArray[ii] = darray[0, ii].ToString();
                //                ii = ii + 1;

                //            }
                //            else
                //            {
                //                darray[0, ii] = Convert.ToDouble(s);
                //                ii = ii + 1;
                //            }

                //        }
                //        strtemp = string.Join(",", multiArray);
                //        strtemp = strtemp + strValue[2];
                //        //Text_UWB_dis1.Text = str;
                //        Set_Text_UWB(strtemp);
                //        break;
                //    case "108message":
                //        strtemp = str + "108";
                //        Set_Text_UWB(strtemp);
                //        foreach (string s in multiArray)
                //        {
                //            darray[2, ii] = Convert.ToDouble(s);
                //            ii = ii + 1;
                //        }
                //        break;
                //    case "205message":
                //        strtemp = str + "205";
                //        Set_Text_UWB(strtemp);
                //        foreach (string s in multiArray)
                //        {
                //            darray[3,ii] = Convert.ToDouble(s);
                //            ii = ii + 1;
                //        }
                //        break;
                //    default:
                        
                //        break;

                //}
                
            }
            //Console.WriteLine(Encoding.UTF8.GetString(e.Message, 0, e.Message.Length));
            this._messageReceived.Set();
        }

        

        private void Set_Text_UWB(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            
            //string ID = text.Substring(text.Length - 3);
            //text = text.Remove(text.Length - 3);
            switch (text.Substring(text.Length - 3))
            {
                case "102":

                    //Text_UWB_dis1.Text = str;
                    if (Text_UWB_dis1.InvokeRequired)
                    {
                        //SetTextCallback d = new SetTextCallback(Set_Text_UWB);
                        this.Text_UWB_dis1.Invoke(new SetTextCallback(Set_Text_UWB), new object[] { text });
                        //Text_UWB_dis1.Invoke(d, new object[] { text });
                    }
                    else
                    {
                        Text_UWB_dis1.Text = text.Substring(0, text.Length - 3);
                    }
                    break;
                case "103":
                    if (Text_UWB_dis2.InvokeRequired)
                    {
                        this.Text_UWB_dis2.Invoke(new SetTextCallback(Set_Text_UWB), new object[] { text });
                    }
                    else
                    {
                        Text_UWB_dis2.Text = text.Substring(0, text.Length - 3);
                    }
                    break;
                case "104":
                    if (Text_UWB_dis3.InvokeRequired)
                    {
                        this.Text_UWB_dis3.Invoke(new SetTextCallback(Set_Text_UWB), new object[] { text });
                    }
                    else
                    {
                        Text_UWB_dis3.Text = text.Substring(0, text.Length - 3);
                    }
                    break;
                case "108":
                    if (Text_UWB_dis4.InvokeRequired)
                    {
                        this.Text_UWB_dis4.Invoke(new SetTextCallback(Set_Text_UWB), new object[] { text });
                    }
                    else
                    {
                        Text_UWB_dis4.Text = text.Substring(0, text.Length - 3);
                    }
                    break;
                case "205":
                    if (Text_UWB_dis5.InvokeRequired)
                    {
                        this.Text_UWB_dis5.Invoke(new SetTextCallback(Set_Text_UWB), new object[] { text });
                    }
                    else
                    {
                        Text_UWB_dis5.Text = text.Substring(0, text.Length - 3);
                    }
                    break;
                default:

                    break;

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

                if (SwarmInterface.mav_tag.Length >= 1)
                {
                    //var myForm = new displayData(mav1);
                    //myForm.Show();
                    var myForm = new LinkQuality(SwarmInterface.mav_tag);
                    myForm.Show();
                }
                SwarmInterface.mav_tag[0] = SwarmInterface.leader_tag;
                //SwarmInterface.mav_tag_temp[index_temp] = temp;
                //SwarmInterface.mav_tag_temp.Sort();
                //SwarmInterface.mav_tag_temp.FindIndex(SwarmInterface.leader_tag);
                SwarmInterface.mav_tag_temp = mav_tag_temp_temp;
                SwarmInterface.Leaderpos_lat = mav1.cs.lat;
                SwarmInterface.Leaderpos_lng = mav1.cs.lng;
                SwarmInterface.Leaderpos_alt = mav1.cs.alt;

                //SwarmInterface.Leaderpos_lat = mav1.cs.lat;
                //SwarmInterface.Leaderpos_lng = mav1.cs.lng;
                //SwarmInterface.Leaderpos_alt = mav1.cs.alt;
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
                BUT_guided_leader.Enabled = true;
                BUT_poshold_leader.Enabled = true;
                BUT_RTL.Enabled = true;
                BUT_Leader_alt.Enabled = true;
                Radio_fields_1.Enabled = true;
                Radio_fields_2.Enabled = true;
                leader_homeloc = new PointLatLngAlt(SwarmInterface.Leader.cs.HomeLocation);
                Text_lat.Text = mav1.cs.lat.ToString();
                Text_lng.Text = mav1.cs.lng.ToString();
                Text_lat.Refresh();
                Text_lng.Refresh();
                //leader_tag = mav.sysid;
                //mav_tag[mav.sysid] = mav.sysid;
                BUT_Form1_Click(sender, e);
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