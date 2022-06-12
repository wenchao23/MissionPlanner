using System;
using System.Collections.Generic;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems;
using MissionPlanner.Utilities;
using MissionPlanner.ArduPilot;
using Vector3 = MissionPlanner.Utilities.Vector3;

//add by wenchao
using System.Collections;
//end
namespace MissionPlanner.Swarm
{

    /// <summary>
    /// Follow the leader
    /// </summary>
    /// 
    class Formation : Swarm
    {


        Dictionary<MAVState, Vector3> offsets = new Dictionary<MAVState, Vector3>();

        private Dictionary<MAVState, Tuple<PID, PID, PID, PID>> pids =
            new Dictionary<MAVState, Tuple<PID, PID, PID, PID>>();

        private PointLatLngAlt masterpos = new PointLatLngAlt();


        //declare var//added by wenchao
        private int desried_dis = 30;
        private int desried_dis_takeoff = 20;
        public int[] sysid_enter_start = new int[10];
        public int[] sysid_thread_takoff_start = new int[10];
        public int[] sysid_thread_takoff_end = new int[10];
        public int CMB_status = 0;
        public bool collision_avoidence_start_button = false;
        public bool UWB_start_button = false;
        

        public byte leader_tag = 0;
        public byte leader_compid = 0;
        //public byte[] mav_tag = new byte[10];
        public List<byte> mav_tag_temp = new List<byte>();

        public double[,] dis_matrix = new double[10, 10];
        int[,] collision_detect = new int[10, 10];
        public double Leaderpos_lat = 0;
        public double Leaderpos_lng = 0;
        public double Leaderpos_alt = 0;
        public double[,] pos = new double[10, 3];
        public double[,] posxyz = new double[10, 3];
        public double[,] posxyz_abs = new double[10, 3];
        public double d;
        Random rnd = new Random();
        public double std = 0;//.000001;
        public byte[] mav_tag = new byte[] { };
        public int Numberofmav = 0;
        public double[] target_store = { 0, 0, 0 };
        public double[] target_store_xyz = { 0, 0, 0 };
        public double[,] points_circle = new double[12, 3];
        
        public int collision_avoid_start = 0;
        public int UWB_start_start = 0;
        public int UWB_threadrun = 0;

        public double cost = int.MaxValue;
        public double[] next_wp = { 0, 0, 0 };

        //public KalmanFilter.Parameters parameters = new KalmanFilter.Parameters(); //Create an instance of the class Parameters.
        public double[,] zGPS = new double[3 * 1, 1] { { 0 }, { 0 }, { 0 } };
        public int KF_start = 0;
        public KalmanFilter.KalmanFilterMath.KalmanFilterMatrices kalmanFilterMatrices = KalmanFilter.KalmanFilterInitialize.InitializeKalmanFilter(new double[,] { { 0 }, { 0 }, { 0 } });
        public Formation()
        {

            foreach (var port in MainV2.Comports.ToArray())
            {

                foreach (var mav in port.MAVlist)
                {
                    
                    mav_tag_temp.Add(mav.sysid);
                    
                }
            }

            //var myForm = new displayData();
            //myForm.Show();


            //mav_tag_temp.Sort();
            //mav_tag = mav_tag_temp.ToArray();
            Numberofmav = mav_tag_temp.Count;

            
            
            

    }

        public void setOffsets(MAVState mav, double x, double y, double z)
        {
            offsets[mav] = new Vector3(x, y, z);
            log.Info(mav.ToString() + " " + offsets[mav].ToString());
            
        }

        public Vector3 getOffsets(MAVState mav)
        {
            if (offsets.ContainsKey(mav))
            {
                return offsets[mav];
            }

            return new Vector3(offsets.Count, 0, 0);
        }

        public override void Update()
        {
            if (MainV2.comPort.MAV.cs.lat == 0 || MainV2.comPort.MAV.cs.lng == 0)
                return;

            if (Leader == null)
                Leader = MainV2.comPort.MAV;

            masterpos = new PointLatLngAlt(Leader.cs.lat, Leader.cs.lng, Leader.cs.alt, "");
        }

        double wrap_180(double input)
        {
            if (input > 180)
                return input - 360;
            if (input < -180)
                return input + 360;
            return input;
        }

        //convert Wgs84ConversionInfo to utm
        CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

        IGeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;

        public void ResetStatus()
        {
            sysid_enter_start =new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //sysid_thread_takoff_start = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var port in MainV2.Comports.ToArray())
            {

                foreach (var mav in port.MAVlist)
                {
                    if (mav.cs.armed)
                    {
                        sysid_thread_takoff_start[mav.sysid] = 1;
                        sysid_thread_takoff_end[mav.sysid] = 1;
                    }
                    else
                    {
                        sysid_thread_takoff_start[mav.sysid] = 0;
                        sysid_thread_takoff_end[mav.sysid] = 0;
                    }
                }
            }
        }

        public double cmp_distance(double[] position_follower,double[] leaderpos)
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


        public float cmp_yaw(double[] ori, double[] tar)
        {
            double X = Math.Cos(tar[1] * MathHelper.deg2rad) * Math.Sin((tar[0] - ori[0]) * MathHelper.deg2rad);
            double Y = Math.Cos(ori[1] * MathHelper.deg2rad) * Math.Sin(tar[1] * MathHelper.deg2rad) - Math.Sin(ori[1] * MathHelper.deg2rad) * Math.Cos(tar[1] * MathHelper.deg2rad) * Math.Cos((tar[0] - ori[0]) * MathHelper.deg2rad);
            double yaw = Math.Round(Math.Atan2(X, Y) * MathHelper.rad2deg + 720);
            int i = Math.DivRem((int)yaw, 360, out int yaw2);
            float yaw1 = (float)yaw2;
            return yaw1;
        }

        public double[] geo2ned(double[] ori, double[] tar)
        {
            double e2 = Math.Pow(0.0818,2);

            double s1 = Math.Sin(ori[0] * MathHelper.deg2rad);
            double c1 = Math.Cos(ori[0] * MathHelper.deg2rad);

            double s2 = Math.Sin(tar[0] * MathHelper.deg2rad);
            double c2 = Math.Cos(tar[0] * MathHelper.deg2rad);


            double p1 = c1* Math.Cos(ori[1] * MathHelper.deg2rad);
            double p2 = c2* Math.Cos(tar[1] * MathHelper.deg2rad);

            double q1 = c1* Math.Sin(ori[1] * MathHelper.deg2rad);
            double q2 = c2* Math.Sin(tar[1] * MathHelper.deg2rad);

            double w1 = 1/ Math.Sqrt(1 - e2 * Math.Pow(s1, 2));
            double w2 = 1/ Math.Sqrt(1 - e2 * Math.Pow(s2, 2));

            double deltaX = 6378137 * (p2* w2 - p1* w1) + (tar[2] * p2 - ori[2] * p1);
            double deltaY = 6378137 * (q2* w2 - q1* w1) + (tar[2] * q2 - ori[2] * q1);
            double deltaZ = (1 - e2) * 6378137 * (s2* w2 - s1* w1) + (tar[2] * s2 - ori[2] * s1);

            double cosPhi = Math.Cos(ori[0] * MathHelper.deg2rad);
            double sinPhi = Math.Sin(ori[0] * MathHelper.deg2rad);
            double cosLambda = Math.Cos(ori[1] * MathHelper.deg2rad);
            double sinLambda = Math.Sin(ori[1] * MathHelper.deg2rad);

            double t = cosLambda * deltaX + sinLambda * deltaY;
            double uEast = -sinLambda * deltaX + cosLambda * deltaY;

            double wUp = cosPhi* t + sinPhi* deltaZ;
            double vNorth = -sinPhi* t + cosPhi* deltaZ;
            double[] xyz = { uEast, vNorth, wUp };
            return xyz;

        }


        public double[] ned2geo(double[] tar )
        {
            double[] target = { 0, 0, 0 };
            int utmzone = (int)((Leader.cs.lng - -186.0) / 6.0);

            IProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(utmzone,
                Leader.cs.alt < 0 ? false : true);

            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84, utm);

            double[] pll1 = { Leader.cs.lng, Leader.cs.lat };

            double[] p1 = trans.MathTransform.Transform(pll1);

            double heading = -Leader.cs.yaw;


            var x = tar[0];

            var y = tar[1];

            // add offsets to utm
            p1[0] += x * Math.Cos(heading * MathHelper.deg2rad) - y * Math.Sin(heading * MathHelper.deg2rad);
            p1[1] += x * Math.Sin(heading * MathHelper.deg2rad) + y * Math.Cos(heading * MathHelper.deg2rad);

            // convert back to wgs84
            IMathTransform inversedTransform = trans.MathTransform.Inverse();
            double[] point = inversedTransform.Transform(p1);

            target[0] = point[1] ;
            target[1] = point[0] ;
            target[2] = tar[2];

            return target;



            //double f = 1/298.2572;
            //double e2 = f*(2-f);

            //double sinphi = Math.Sin(ori[0] * MathHelper.deg2rad);
            //double cosphi = Math.Cos(ori[0] * MathHelper.deg2rad);

            
            //double N = 6378137 / (Math.Sqrt(1 - e2 * sinphi * sinphi));
            //double rho = (N + ori[2])*cosphi;
            //double z0 = (N * (1 - e2) + ori[2]) * sinphi;

            //double  x0 = rho* Math.Cos(ori[1] * MathHelper.deg2rad);
            //double  y0 = rho* Math.Sin(ori[1] * MathHelper.deg2rad);


            //double cosPhi = Math.Cos(ori[0] * MathHelper.deg2rad);
            //double sinPhi = Math.Sin(ori[0] * MathHelper.deg2rad);

            //double cosLambda = Math.Cos(ori[1] * MathHelper.deg2rad);
            //double sinLambda = Math.Sin(ori[1] * MathHelper.deg2rad);

            //double t = cosPhi* (-tar[2]) - sinPhi* (tar[0]);
            //double w = sinPhi* (-tar[2]) + cosPhi* (tar[0]);

            //double u = cosLambda* t - sinLambda* (tar[1]);
            //double v = sinLambda* t + cosLambda* (tar[1]);

            //double x = x0 + u;
            //double y = y0 + w;
            //double z = z0 + v;

            //double d = Math.Sqrt(x * x + y * y);
            //double b = (1 - f) * 6378137;
            //double ep2 = ep2 = e2 / (1 - e2);
            //double beta = Math.Atan2(z, (1 - f) * d)*MathHelper.rad2deg;
            //double phi = Math.Atan2(z + b * ep2 * Math.Pow(Math.Sin(beta * MathHelper.deg2rad),3), rho - 6378137 * e2 * Math.Pow(Math.Cos(beta * MathHelper.deg2rad), 3)) * MathHelper.rad2deg;
            //double betaNew = Math.Atan2((1 - f) * Math.Sin(phi * MathHelper.deg2rad), Math.Cos(phi * MathHelper.deg2rad)) * MathHelper.rad2deg;
            //sinphi = Math.Sin(phi * MathHelper.deg2rad);
            //N = 6378137 / Math.Sqrt(1 - e2 * sinphi* sinphi);
            //double h = rho* Math.Cos(phi * MathHelper.deg2rad) + (z + e2 * N* sinphi)* sinphi - N;

            //double lambda = Math.Atan2(y, x) * MathHelper.rad2deg;
            //double[] gps_converted = { phi, lambda, h };
            //return gps_converted;

        }

        public void takeoff_func(object port1)
        {
            MAVLinkInterface port =  (MAVLinkInterface)port1;
            port.setMode((byte)port.sysidcurrent, (byte)port.compidcurrent, "GUIDED");
            System.Threading.Thread.Sleep(100);
            port.doARM((byte)port.sysidcurrent, (byte)port.compidcurrent, true);
            
            while (!port.MAV.cs.armed) { System.Threading.Thread.Sleep(10); }
            System.Threading.Thread.Sleep(2000);
            port.doCommand((byte)port.sysidcurrent, (byte)port.compidcurrent, MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 5);
            System.Threading.Thread.Sleep(3000);
            sysid_thread_takoff_end[port.sysidcurrent] = 1;
        }

        public double[] change_to_xyz(double[] follower_loc, bool change_coordinate) {

            double[] posxyz_temp = geo2ned(new double[] { Leader.cs.lat, Leader.cs.lng, Leader.cs.alt }, new double[] { follower_loc[0], follower_loc[1], follower_loc[2] });
            
            if (!change_coordinate)
                return posxyz_temp;
            else
            {
                double theta = Leader.cs.yaw;
                if (Leader.cs.yaw > 0 && Leader.cs.yaw < 180)
                {
                    theta = -(Leader.cs.yaw) * MathHelper.deg2rad;
                }
                else
                {
                    theta = (360 - Leader.cs.yaw) * MathHelper.deg2rad;
                }
                double[] posxyz = { 0, 0, 0 };
                posxyz[0] = posxyz_temp[0] * Math.Cos(theta) + posxyz_temp[1] * Math.Sin(theta);
                posxyz[1] = -posxyz_temp[0] * Math.Sin(theta) + posxyz_temp[1] * Math.Cos(theta);
            //}

                posxyz[2] = posxyz_temp[2];
                return posxyz;
            }
            

            
        }

        public override void SendCommand()
        {
            if (masterpos.Lat == 0 || masterpos.Lng == 0)
                return;            
            int a = 0;

            //////declare var and obtain realtime pos of leader //add by Wenchao
            double randNormal_lat = 0;
            double randNormal_lng = 0;
            double randNormal_lat_KF = 0;
            double randNormal_lng_KF = 0;

            if (CMB_status != 0)
            {
                                              
                foreach (var port in MainV2.Comports.ToArray())
                {

                    foreach (var mav in port.MAVlist)
                    {
                        if (mav == Leader)
                        {
                            Leaderpos_lat = mav.cs.lat;
                            Leaderpos_lng = mav.cs.lng;
                            Leaderpos_alt = mav.cs.alt;
                            
                        }
                        pos[mav.sysid, 0] = mav.cs.lat;
                        pos[mav.sysid, 1] = mav.cs.lng;
                        pos[mav.sysid, 2] = mav.cs.alt;
                        
                        double[] posxyz_temp = change_to_xyz(new double[] { mav.cs.lat, mav.cs.lng, mav.cs.alt}, true);//true == alwasy north//false = use leader's yaw as positive-x axis//must use true when changing it back to gps is needed
                        //double[] posxyz_temp_temp = ned2geo(posxyz_temp);
                        //double d = cmp_distance(new double[] { posxyz_temp_temp[0], posxyz_temp_temp[1], 0 }, new double[] { mav.cs.lat, mav.cs.lng, 0 });
                        posxyz[mav.sysid, 0] = posxyz_temp[0];
                        posxyz[mav.sysid, 1] = posxyz_temp[1];
                        posxyz[mav.sysid, 2] = posxyz_temp[2];

                         posxyz_temp = change_to_xyz(new double[] { mav.cs.lat, mav.cs.lng, mav.cs.alt }, false);//true == alwasy north//false = use leader's yaw as positive-x axis//must use true when changing it back to gps is needed
                        //double[] posxyz_temp_temp = ned2geo(posxyz_temp);
                        //double d = cmp_distance(new double[] { posxyz_temp_temp[0], posxyz_temp_temp[1], 0 }, new double[] { mav.cs.lat, mav.cs.lng, 0 });
                        posxyz_abs[mav.sysid, 0] = posxyz_temp[0];
                        posxyz_abs[mav.sysid, 1] = posxyz_temp[1];
                        posxyz_abs[mav.sysid, 2] = posxyz_temp[2];

                    }
                }

                
                Array.Clear(collision_detect, 0, collision_detect.Length);
                for (int i = 1; i < Numberofmav; i = i + 1) {
                    for (int j = 1; j < Numberofmav; j = j + 1) {
                        if (i == j)
                            continue;

                        dis_matrix[mav_tag[i], mav_tag[j]] = cmp_distance(new double[] { pos[mav_tag[i], 0], pos[mav_tag[i], 1], pos[mav_tag[i], 2] }, new double[] { pos[mav_tag[j], 0], pos[mav_tag[j], 1], pos[mav_tag[j], 2] });
                        if (dis_matrix[mav_tag[i], mav_tag[j]] < 30) {
                            collision_detect[mav_tag[i], mav_tag[j]] = 1;
                                }
                    }
                }
            }


            //Array.Sort(mav_tag);
            //end//add by Wenchao
            foreach (var port in MainV2.Comports.ToArray())
            {
                
                foreach (var mav in port.MAVlist)
                {
                    if (mav == Leader)
                    {
                        continue;
                    }
                    PointLatLngAlt target = new PointLatLngAlt(masterpos);
                    //compute realtime dist between leader and follower//add by Wenchao
                    if (CMB_status!= 0)
                    {
                        int index_mav_tag = Array.IndexOf(mav_tag, mav.sysid);
                        

                        if (mav.sysid == mav_tag[1])
                        {
                            d = cmp_distance(new double[] { mav.cs.lat, mav.cs.lng, mav.cs.alt }, new double[] { Leaderpos_lat, Leaderpos_lng, Leaderpos_alt });
                        }
                        else {
                            int index_id = mav_tag[index_mav_tag - 1];
                            d = cmp_distance(new double[] { mav.cs.lat, mav.cs.lng, mav.cs.alt }, new double[] { pos[index_id, 0], pos[index_id,1],pos[index_id, 2] });
                        }

                        if (mav.sysid == mav_tag[mav_tag.Length - 1] && std!= 0) 
                        {
                            double u1 = 1.0 - rnd.NextDouble(); //uniform(0,1] random doubles
                            double u2 = 1.0 - rnd.NextDouble();
                            randNormal_lat = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                            randNormal_lat = 0 + std * randNormal_lat;

                            u1 = 1.0 - rnd.NextDouble(); //uniform(0,1] random doubles
                            u2 = 1.0 - rnd.NextDouble();
                            randNormal_lng = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                            randNormal_lng = 0 + std * randNormal_lng;
                        }


                        if (d > desried_dis_takeoff && this.sysid_enter_start[mav.sysid] == 0 && !mav.cs.armed )
                        {   
                            
                                if (sysid_thread_takoff_start[mav.sysid] == 0)
                                {
                                sysid_thread_takoff_start[mav.sysid] = 1;
                                System.Threading.Thread takeoff_thread = new System.Threading.Thread(takeoff_func);
                                
                                takeoff_thread.IsBackground = true;
                                takeoff_thread.Start(port);
                                

                                }
                        }

                        
                        if ((d < desried_dis && this.sysid_enter_start[mav.sysid] == 0)|| sysid_thread_takoff_end[mav.sysid] !=1|| !mav.cs.armed)
                        {

                            //if (d > desried_dis / 1.5 && mav.cs.armed)
                            //{
                            //    port.setMode(mav.sysid, mav.compid, "GUIDED");

                            //    port.doCommand(mav.sysid, mav.compid, MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 5);
                            //}
                            
                            continue;
                        }
                    }
                    //end
                    try
                    {
                        if (mav.sysid == 3&&false)//KF
                        {

                            if (KF_start == 0)
                            { 

                                kalmanFilterMatrices = KalmanFilter.KalmanFilterInitialize.InitializeKalmanFilter(new double[,] { { mav.cs.lat }, { mav.cs.lng }, { mav.cs.alt } });
                                KF_start = 1;
                            }
                            else
                            { 
                            double u1 = 1.0 - rnd.NextDouble(); //uniform(0,1] random doubles
                            double u2 = 1.0 - rnd.NextDouble();
                            randNormal_lat_KF = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                            randNormal_lat_KF = 0 + 0.000002 * randNormal_lat_KF;

                            u1 = 1.0 - rnd.NextDouble(); //uniform(0,1] random doubles
                            u2 = 1.0 - rnd.NextDouble();
                            randNormal_lng_KF = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                            randNormal_lng_KF = 0 + 0.000002 * randNormal_lng_KF;

                            zGPS[0, 0] = mav.cs.lat+ randNormal_lat_KF;
                            zGPS[1, 0] = mav.cs.lng+ randNormal_lng_KF;
                            zGPS[2, 0] = mav.cs.alt;

                            string filePath = @"D:\test.csv";  
                            System.Text.StringBuilder output = new System.Text.StringBuilder();
                            

                            KalmanFilter.KalmanFilterMath.RunKalmanFilter(kalmanFilterMatrices, 0.1, zGPS);
                            output.AppendLine(string.Join(",", mav.cs.lat));
                            output.AppendLine(string.Join(",", mav.cs.lng));
                            output.AppendLine(string.Join(",", mav.cs.alt));
                            for (int i = 0; i <= 2; i++)
                                output.AppendLine(string.Join(",", zGPS[i, 0]));
                            output.AppendLine(string.Join(",", kalmanFilterMatrices.stateUpdate[0,0]));
                            output.AppendLine(string.Join(",", kalmanFilterMatrices.stateUpdate[2, 0]));
                            output.AppendLine(string.Join(",", kalmanFilterMatrices.stateUpdate[4, 0]));

                            //System.IO.File.WriteAllText(filePath, output.ToString());
                            System.IO.File.AppendAllText(filePath, output.ToString());


                            string filePath2 = @"D:\noise.csv";
                            System.Text.StringBuilder output2 = new System.Text.StringBuilder();


                            
                            output2.AppendLine(string.Join(",", randNormal_lat_KF));
                            output2.AppendLine(string.Join(",", randNormal_lng_KF));
                            
                            

                            //System.IO.File.WriteAllText(filePath, output.ToString());
                            System.IO.File.AppendAllText(filePath2, output2.ToString());
                            }
                        }
                        this.sysid_enter_start[mav.sysid] = 1;
                        
                        int utmzone = (int) ((masterpos.Lng - -186.0)/6.0);

                        IProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(utmzone,
                            masterpos.Lat < 0 ? false : true);
                        
                        ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84, utm);
                        
                        double[] pll1 = {target.Lng, target.Lat};

                        double[] p1 = trans.MathTransform.Transform(pll1);

                        double heading = -Leader.cs.yaw;

                        double length = offsets[mav].length();

                        var x = ((Vector3) offsets[mav]).x;

                        var y = ((Vector3) offsets[mav]).y;
                        
                        // add offsets to utm
                        p1[0] += x*Math.Cos(heading*MathHelper.deg2rad) - y*Math.Sin(heading*MathHelper.deg2rad);
                        p1[1] += x*Math.Sin(heading*MathHelper.deg2rad) + y*Math.Cos(heading*MathHelper.deg2rad);

                        // convert back to wgs84
                        IMathTransform inversedTransform = trans.MathTransform.Inverse();
                        double[] point = inversedTransform.Transform(p1);

                        target.Lat = point[1]+ randNormal_lat;
                        target.Lng = point[0]+ randNormal_lng;
                        target.Alt += ((Vector3) offsets[mav]).z;
                        //

                        //
                        //var ned = geo2ned(new double[] { pos[mav_tag[0], 0],pos[mav_tag[0], 1],0},new double[] { pos[mav_tag[1], 0],pos[mav_tag[1], 1],0 });

                        if (mav.cs.firmware == Firmwares.ArduPlane)
                        {
                            // get distance from target position
                            var dist = target.GetDistance(mav.cs.Location);

                            // get bearing to target
                            var targyaw = mav.cs.Location.GetBearing(target);

                            var targettrailer = target.newpos(Leader.cs.yaw, Math.Abs(dist) * -0.25);
                            var targetleader = target.newpos(Leader.cs.yaw, 10 + dist);

                            var yawerror = wrap_180(targyaw - mav.cs.yaw);
                            var mavleadererror = wrap_180(Leader.cs.yaw - mav.cs.yaw);

                            if (dist < 100)
                            {
                                targyaw = mav.cs.Location.GetBearing(targetleader);
                                yawerror = wrap_180(targyaw - mav.cs.yaw);

                                var targBearing = mav.cs.Location.GetBearing(target);

                                // check the bearing for the leader and target are within 45 degrees.
                                if (Math.Abs(wrap_180(targBearing - targyaw)) > 45)
                                    dist *= -1;
                            }
                            else
                            {
                                targyaw = mav.cs.Location.GetBearing(targettrailer);
                                yawerror = wrap_180(targyaw - mav.cs.yaw);
                            }

                            // display update
                            mav.GuidedMode.x = (float)target.Lat;
                            mav.GuidedMode.y = (float)target.Lng;
                            mav.GuidedMode.z = (float)target.Alt;

                            MAVLink.mavlink_set_attitude_target_t att_target = new MAVLink.mavlink_set_attitude_target_t();
                            att_target.target_system = mav.sysid;
                            att_target.target_component = mav.compid;
                            att_target.type_mask = 0xff;

                            Tuple<PID, PID, PID, PID> pid;

                            if (pids.ContainsKey(mav))
                            {
                                pid = pids[mav];
                            }
                            else
                            {
                                pid = new Tuple<PID, PID, PID, PID>(
                                    new PID(1f, .03f, 0.02f, 10, 20, 0.1f, 0),
                                    new PID(1f, .03f, 0.02f, 10, 20, 0.1f, 0),
                                    new PID(1, 0, 0.00f, 15, 20, 0.1f, 0),
                                    new PID(0.01f, 0.001f, 0, 0.5f, 20, 0.1f, 0));
                                pids.Add(mav, pid);
                            }

                            var rollp = pid.Item1;
                            var pitchp = pid.Item2;
                            var yawp = pid.Item3;
                            var thrustp = pid.Item4;

                            var newroll = 0d;
                            var newpitch = 0d;

                            if (true)
                            {
                                var altdelta = target.Alt - mav.cs.alt;
                                newpitch = altdelta;
                                att_target.type_mask -= 0b00000010;

                                pitchp.set_input_filter_all((float)altdelta);

                                newpitch = pitchp.get_pid();
                            }

                            if (true)
                            {
                                var leaderturnrad = Leader.cs.radius;
                                var mavturnradius = leaderturnrad - x;

                                {
                                    var distToTarget = mav.cs.Location.GetDistance(target);
                                    var bearingToTarget = mav.cs.Location.GetBearing(target);

                                    // bearing stability
                                    if (distToTarget < 30)
                                        bearingToTarget = mav.cs.Location.GetBearing(targetleader);
                                    // fly in from behind
                                    if (distToTarget > 100)
                                        bearingToTarget = mav.cs.Location.GetBearing(targettrailer);

                                    var bearingDelta = wrap_180(bearingToTarget - mav.cs.yaw);
                                    var tangent90 = bearingDelta > 0 ? 90 : -90;

                                    newroll = 0;

                                    // if the delta is > 90 then we are facing the wrong direction
                                    if (Math.Abs(bearingDelta) < 85)
                                    {
                                        var insideAngle = Math.Abs(tangent90 - bearingDelta);
                                        var angleCenter = 180 - insideAngle * 2;

                                        // sine rule
                                        var sine1 = Math.Max(distToTarget, 40) /
                                                    Math.Sin(angleCenter * MathHelper.deg2rad);
                                        var radius = sine1 * Math.Sin(insideAngle * MathHelper.deg2rad);

                                        // average calced + leader offset turnradius - acts as a FF
                                        radius = (Math.Abs(radius) + Math.Abs(mavturnradius)) / 2;

                                        var angleBank = ((mav.cs.groundspeed * mav.cs.groundspeed) / radius) / 9.8;

                                        angleBank *= MathHelper.rad2deg;

                                        if (bearingDelta > 0)
                                            newroll = Math.Abs(angleBank);
                                        else
                                            newroll = -Math.Abs(angleBank);
                                    }

                                    newroll += MathHelper.constrain(bearingDelta, -20, 20);
                                }

                                // tr = gs2 / (9.8 * x)
                                // (9.8 * x) * tr = gs2
                                // 9.8 * x = gs2 / tr
                                // (gs2/tr)/9.8 = x

                                var angle = ((mav.cs.groundspeed * mav.cs.groundspeed) / mavturnradius) / 9.8;

                                //newroll = angle * MathHelper.rad2deg;

                                // 1 degree of roll for ever 1 degree of yaw error
                                //newroll += MathHelper.constrain(yawerror, -20, 20);

                                //rollp.set_input_filter_all((float)yawdelta);
                            }

                            // do speed
                            if (true)
                            {
                                //att_target.thrust = (float) MathHelper.mapConstrained(dist, 0, 40, 0, 1);
                                att_target.type_mask -= 0b01000000;

                                // in m out 0-1
                                thrustp.set_input_filter_all((float) dist);

                                // prevent buildup prior to being close
                                if(dist>40)
                                    thrustp.reset_I();

                                // 0.1 demand + pid results
                                att_target.thrust = (float)MathHelper.constrain(thrustp.get_pid(), 0.1, 1);
                            }

                            Quaternion q = new Quaternion();
                            q.from_vector312(newroll * MathHelper.deg2rad, newpitch * MathHelper.deg2rad, yawerror * MathHelper.deg2rad);

                            att_target.q = new float[4];
                            att_target.q[0] = (float) q.q1;
                            att_target.q[1] = (float) q.q2;
                            att_target.q[2] = (float) q.q3;
                            att_target.q[3] = (float) q.q4;
                   
                             //0b0= rpy
                            att_target.type_mask -= 0b10000101;
                            //att_target.type_mask -= 0b10000100;

                            Console.WriteLine("sysid {0} - {1} dist {2} r {3} p {4} y {5}", mav.sysid,
                                att_target.thrust, dist, newroll, newpitch, (targyaw - mav.cs.yaw));

                          /*  Console.WriteLine("rpyt {0} {1} {2} {3} I {4} {5} {6} {7}",
                                rollp.get_pid(), pitchp.get_pid(), yawp.get_pid(), thrustp.get_pid(),
                                rollp.get_i(), pitchp.get_i(), yawp.get_i(), thrustp.get_i());
                                */
                            port.sendPacket(att_target, mav.sysid, mav.compid);
                        }
                        else
                        {
                            Vector3 vel = new Vector3(Leader.cs.vx, Leader.cs.vy, Leader.cs.vz);
                            if (collision_detect[mav_tag[2], mav_tag[1]] == 1 && mav.sysid== mav_tag[2] && collision_avoid_start ==0&& collision_avoidence_start_button)
                            {

                                target_store[0] = target.Lat;
                                target_store[1] = target.Lng;
                                target_store[2] = target.Alt;
                                target_store_xyz = change_to_xyz(new double[] { target_store[0], target_store[1],target_store[2] }, false);
                                
                                collision_avoid_start = 1;
                                //port.setPositionTargetGlobalInt(mav.sysid, mav.compid, false,
                               // true, false, false,
                               // MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT, 0, 0, target.Alt, 0,
                               // 0, vel.z, 0, 0);

                                //////////////////////////////////////////////////////////////
                                //for (int i =0; i <= 11; i++)
                                //{
                                //    points_circle[i, 0] = 5 * Math.Cos(2 * Math.PI / (i + 1)) + posxyz[mav_tag[2], 0];
                                //    points_circle[i, 1] = 5 * Math.Sin(2 * Math.PI / (i + 1)) + posxyz[mav_tag[2], 1];
                                //    points_circle[i, 2] = target_store[2];

                                //    double d_tar_wp = Math.Sqrt(Math.Pow(points_circle[i, 0] - target_store_xyz[0], 2) + Math.Pow(points_circle[i, 1] - target_store_xyz[1], 2));
                                //    double d_cur_wp = Math.Sqrt(Math.Pow(points_circle[i, 0] - posxyz[mav_tag[1], 0], 2) + Math.Pow(points_circle[i, 1] - posxyz[mav_tag[1], 1], 2));//dis_curr_obstacle
                                //    if (d_cur_wp > 5) {
                                //        if (d_tar_wp < cost)
                                //        {
                                //            cost = d_tar_wp;
                                //            i_nextwp = i;
                                //        }
                                //    }
                                    
                                //}

                                //next_wp[0] = points_circle[i_nextwp, 0];
                                //next_wp[1] = points_circle[i_nextwp, 1];
                                //next_wp[2] = points_circle[i_nextwp, 2];

                                //double[] gps_temp =  ned2geo(next_wp);
                                //target.Lat = gps_temp[0];
                                //target.Lng = gps_temp[1];
                                //target.Alt = gps_temp[2];

                                //////////////////////////////////////////////////////////////
                                ///

                                
                            }

                            //cost = int.MaxValue;
                            //i_nextwp = 0;

                            //if (collision_avoid_start == 1 && mav.sysid == mav_tag[2]&& collision_avoidence_start_button && false)
                            //{
                            //    double[] gps_temp = ned2geo(next_wp);
                                
                            //    //double d = cmp_distance(gps_temp, new double[] { mav.cs.lat, mav.cs.lng, mav.cs.alt });
                            //    if (true)
                            //    {
                            //        for (int i = 0; i <= 11; i++)
                            //        {
                            //            points_circle[i, 0] = 3 * Math.Cos(2 * Math.PI / (i + 1)) + posxyz[mav_tag[2], 0];
                            //            points_circle[i, 1] = 3 * Math.Sin(2 * Math.PI / (i + 1)) + posxyz[mav_tag[2], 1];
                            //            points_circle[i, 2] = target_store[2];

                            //            double d_tar_wp = Math.Sqrt(Math.Pow(points_circle[i, 0] - target_store_xyz[0], 2) + Math.Pow(points_circle[i, 1] - target_store_xyz[1], 2));
                            //            double d_cur_wp = Math.Sqrt(Math.Pow(points_circle[i, 0] - posxyz[mav_tag[1], 0], 2) + Math.Pow(points_circle[i, 1] - posxyz[mav_tag[1], 1], 2));
                            //            if (d_cur_wp > 5)
                            //            {
                            //                if (d_tar_wp < cost)
                            //                {
                            //                    cost = d_tar_wp;
                            //                    i_nextwp = i;
                            //                }
                            //            }

                            //        }
                            //        next_wp[0] = points_circle[i_nextwp, 0];
                            //        next_wp[1] = points_circle[i_nextwp, 1];
                            //        next_wp[2] = points_circle[i_nextwp, 2];
                            //        gps_temp = ned2geo(next_wp);
                            //        target.Lat = gps_temp[0];
                            //        target.Lng = gps_temp[1];
                            //        target.Alt = gps_temp[2];
                            //    }
                            //    else {
                            //        target.Lat = gps_temp[0];
                            //        target.Lng = gps_temp[1];
                            //        target.Alt = gps_temp[2];
                            //    }

                                

                            //}




                            // do pos/vel
                            if(collision_avoid_start == 1 && mav.sysid == mav_tag[2] && collision_avoidence_start_button)
                            {
                                double velocity_max = 1;

                                double angle = Math.Atan2(posxyz[mav_tag[2], 1] - posxyz[mav_tag[1], 1], posxyz[mav_tag[2], 0] - posxyz[mav_tag[1], 0])*MathHelper.rad2deg;

                                double dx = posxyz_abs[mav_tag[2], 0] - posxyz_abs[mav_tag[1], 0];
                                double dy = posxyz_abs[mav_tag[2],1] - posxyz_abs[mav_tag[1], 1];
                                double d_4 = Math.Pow(Math.Pow(dx, 2)+ Math.Pow(dy,2),2);
                                dx = dx / d_4;
                                dy = dy / d_4;

                                double eta = 1;
                                if (cmp_distance(new double[] { pos[mav_tag[2], 0], pos[mav_tag[2], 1], 0 }, new double[] { pos[mav_tag[1], 0], pos[mav_tag[1], 1], 0 })<30)
                                    eta =1000;
                                else eta = 0;
                                
                                double dx2 = target_store_xyz[0] - posxyz_abs[mav_tag[2], 0];
                                double dy2 = target_store_xyz[1] - posxyz_abs[mav_tag[2], 1];

                                double vx =(.5) * dx2 + eta * dx;
                                double vy = (.5) * dy2 + eta * dy;

                                if (Math.Abs(vx) >= Math.Abs(vy) && Math.Abs(vx)> velocity_max)
                                {
                                    vy = vy * velocity_max / Math.Abs(vx);
                                    vx = velocity_max*Math.Sign(vx);
                                }
                                else if (Math.Abs(vx) < Math.Abs(vy) && Math.Abs(vy) > velocity_max)
                                {
                                    vx = vx * velocity_max / Math.Abs(vy);
                                    vy = velocity_max * Math.Sign(vy);                                    
                                }
                                vel.x = vy;
                                vel.y = vx;
                                
                                port.setPositionTargetGlobalInt(mav.sysid, mav.compid, false,
                                true, false, false,
                                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT, 0, 0, target.Alt, vel.x,
                                vel.y, vel.z, 0, 0);
                            }
                            else
                            {
                            port.setPositionTargetGlobalInt(mav.sysid, mav.compid, true,
                                true, false, false,
                                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT, target.Lat, target.Lng, target.Alt, vel.x,
                                vel.y, vel.z, 0, 0);
                            }
                            // do yaw
                            if (!gimbal)
                            {
                                // within 3 degrees dont send
                                if (Math.Abs(mav.cs.yaw - Leader.cs.yaw) > 3)
                                    port.doCommand(mav.sysid, mav.compid, MAVLink.MAV_CMD.CONDITION_YAW, Leader.cs.yaw,
                                        100.0f, 0, 0, 0, 0, 0, false);
                            }
                            else
                            {
                                // gimbal direction
                                if (Math.Abs(mav.cs.yaw - Leader.cs.yaw) > 3)
                                    port.setMountControl(mav.sysid, mav.compid, 45, 0, Leader.cs.yaw, false);
                            }
                        }

                        //Console.WriteLine("{0} {1} {2} {3}", port.ToString(), target.Lat, target.Lng, target.Alt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to send command " + mav.ToString() + "\n" + ex.ToString());
                    }

                    a++;
                }
            }
        }

        public bool gimbal { get; set; }

        
    }


    public class PID
    {
        /*
                    previous_error = 0
                    integral = 0
                    loop:
                    error = setpoint - measured_value
                        integral = integral + error * dt
                    derivative = (error - previous_error) / dt
                        output = Kp * error + Ki * integral + Kd * derivative
                    previous_error = error
                        wait(dt)
                        goto loop*/
        private float _dt;
        private float M_2PI = (float)(Math.PI * 2);
        private float _input;
        private float _derivative;
        private float _kp;
        private float _ki;
        private float _integrator;
        private float _imax;
        private float _kd;
        private float _ff;
        private float _filt_hz = AC_PID_FILT_HZ_DEFAULT;

        const float AC_PID_FILT_HZ_DEFAULT = 20.0f; // default input filter frequency
        const float AC_PID_FILT_HZ_MIN = 0.01f; // minimum input filter frequency

        // Constructor
        public PID(float initial_p, float initial_i, float initial_d, float initial_imax, float initial_filt_hz, float dt, float initial_ff)
        {
            _dt = dt;
            _integrator = 0.0f;
            _input = 0.0f;
            _derivative = 0.0f;

            _kp = initial_p;
            _ki = initial_i;
            _kd = initial_d;
            _imax = Math.Abs(initial_imax);
            filt_hz(initial_filt_hz);
            _ff = initial_ff;

            // reset input filter to first value received
            _flags._reset_filter = true;
        }

        // set_dt - set time step in seconds
        public void set_dt(float dt)
        {
            // set dt and calculate the input filter alpha
            _dt = dt;
        }

        // filt_hz - set input filter hz
        public void filt_hz(float hz)
        {
            _filt_hz = hz;

            // sanity check _filt_hz
            _filt_hz = Math.Max(_filt_hz, AC_PID_FILT_HZ_MIN);
        }

        public void set_input_filter_all(float input)
        {
            // don't process inf or NaN
            if (!isfinite(input))
            {
                return;
            }

            // reset input filter to value received
            if (_flags._reset_filter)
            {
                _flags._reset_filter = false;
                _input = input;
                _derivative = 0.0f;
            }

            // update filter and calculate derivative
            float input_filt_change = get_filt_alpha() * (input - _input);
            _input = _input + input_filt_change;
            if (_dt > 0.0f)
            {
                _derivative = input_filt_change / _dt;
            }
        }

        private bool isfinite(float input)
        {
            return !float.IsInfinity(input);
        }

        public float get_p()
        {
            _pid_info.P = (_input * _kp);
            return _pid_info.P;
        }

        public float get_i()
        {
            if (!is_zero(_ki) && !is_zero(_dt))
            {
                _integrator += ((float)_input * _ki) * _dt;
                if (_integrator < -_imax)
                {
                    _integrator = -_imax;
                }
                else if (_integrator > _imax)
                {
                    _integrator = _imax;
                }

                _pid_info.I = _integrator;
                return _integrator;
            }

            return 0;
        }

        public float get_d()
        {
            // derivative component
            _pid_info.D = (_kd * _derivative);
            return _pid_info.D;
        }

        public float get_ff(float requested_rate)
        {
            _pid_info.FF = (float)requested_rate * _ff;
            return _pid_info.FF;
        }

        public float get_pi()
        {
            return get_p() + get_i();
        }

        public float get_pid()
        {
            return get_p() + get_i() + get_d();
        }

        public void reset_I()
        {
            _integrator = 0;
        }

        public float get_filt_alpha()
        {
            if (is_zero(_filt_hz))
            {
                return 1.0f;
            }

            // calculate alpha
            float rc = 1 / (M_2PI * _filt_hz);
            return _dt / (_dt + rc);
        }

        private bool is_zero(float filt_hz)
        {
            return filt_hz == 0;
        }

        internal class flags
        {
            internal bool _reset_filter;
        }

        flags _flags = new flags();

        pid_info _pid_info = new pid_info();

        internal class pid_info
        {
            internal float P;
            internal float I;
            internal float D;
            internal float FF;
        }
    }


}