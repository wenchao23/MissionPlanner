//using System.Drawing;

namespace MissionPlanner.Swarm
{
    partial class FormationControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

                #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CMB_mavs = new System.Windows.Forms.ComboBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.grid1 = new MissionPlanner.Swarm.Grid();
            this.BUT_Start = new MissionPlanner.Controls.MyButton();
            this.BUT_leader = new MissionPlanner.Controls.MyButton();
            this.BUT_Land = new MissionPlanner.Controls.MyButton();
            this.BUT_Takeoff = new MissionPlanner.Controls.MyButton();
            this.BUT_Disarm = new MissionPlanner.Controls.MyButton();
            this.BUT_Arm = new MissionPlanner.Controls.MyButton();
            this.BUT_Form1 = new MissionPlanner.Controls.MyButton(); //add form1
            this.BUT_Form2 = new MissionPlanner.Controls.MyButton(); //add form2
            this.BUT_Form3 = new MissionPlanner.Controls.MyButton(); //add form3
            this.BUT_Form4 = new MissionPlanner.Controls.MyButton(); //add form4
            this.BUT_Leader_alt = new MissionPlanner.Controls.MyButton(); //add alt
            this.BUT_altmin = new MissionPlanner.Controls.MyButton(); // add alt_minus
            this.BUT_altlvl = new MissionPlanner.Controls.MyButton(); // add alt_minus
            this.BUT_altadd = new MissionPlanner.Controls.MyButton(); // add alt_minus
            this.BUT_demospoof = new MissionPlanner.Controls.MyButton(); // add alt_minus
            this.BUT_demonoise = new MissionPlanner.Controls.MyButton(); // add alt_minus
            this.BUT_addnoise = new MissionPlanner.Controls.MyButton(); // add alt_minus
            this.BUT_flyto_distangle = new MissionPlanner.Controls.MyButton(); // add alt_minus
            this.BUT_guided_leader = new MissionPlanner.Controls.MyButton();
            this.BUT_poshold_leader = new MissionPlanner.Controls.MyButton();
            this.BUT_RTL = new MissionPlanner.Controls.MyButton();
            this.BUT_follow_me = new MissionPlanner.Controls.MyButton();
            this.Text_dis_gate = new System.Windows.Forms.TextBox();
            this.Text_angle_gate = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis1 = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis2 = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis3 = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis4 = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis5 = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis1_text = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis2_text = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis3_text = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis4_text = new System.Windows.Forms.TextBox();
            this.Text_UWB_dis5_text = new System.Windows.Forms.TextBox();

            this.Radio_fields_1 = new System.Windows.Forms.RadioButton();
            this.Radio_fields_2 = new System.Windows.Forms.RadioButton();
            //this.checkbox = new System.Windows.Forms.CheckBox();

            this.BUT_allone = new MissionPlanner.Controls.MyButton(); // add all 1
            this.Text_lat = new System.Windows.Forms.TextBox();
            this.Text_lat_text = new System.Windows.Forms.TextBox();
            this.Text_lng = new System.Windows.Forms.TextBox();
            this.Text_lng_text = new System.Windows.Forms.TextBox();
            this.Text_alt = new System.Windows.Forms.TextBox();
            this.Text_alt_text = new System.Windows.Forms.TextBox();
            this.Text_yaw = new System.Windows.Forms.TextBox();
            this.Text_yaw_text = new System.Windows.Forms.TextBox();
            this.BUT_yaw = new MissionPlanner.Controls.MyButton(); //add but yaw
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.BUT_Updatepos = new MissionPlanner.Controls.MyButton();
            this.PNL_status = new System.Windows.Forms.FlowLayoutPanel();
            this.timer_status = new System.Windows.Forms.Timer(this.components);
            this.but_guided = new MissionPlanner.Controls.MyButton();
            this.CMB_Choose_method = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();

            //add form1
            this.BUT_Form1.Enabled = false;
            this.BUT_Form1.Location = new System.Drawing.Point(12+81, 42);
            this.BUT_Form1.Name = "BUT_Form1";
            this.BUT_Form1.Size = new System.Drawing.Size(75, 23);
            this.BUT_Form1.TabIndex = 12;
            this.BUT_Form1.Text = "Form I";
            this.BUT_Form1.UseVisualStyleBackColor = true;
            this.BUT_Form1.Click += new System.EventHandler(this.BUT_Form1_Click);

            //add form2
            this.BUT_Form2.Enabled = false;
            this.BUT_Form2.Location = new System.Drawing.Point(12 + 81 * 2, 42);
            this.BUT_Form2.Name = "BUT_Form2";
            this.BUT_Form2.Size = new System.Drawing.Size(75, 23);
            this.BUT_Form2.TabIndex = 13;
            this.BUT_Form2.Text = "Form V";
            this.BUT_Form2.UseVisualStyleBackColor = true;
            this.BUT_Form2.Click += new System.EventHandler(this.BUT_Form2_Click);
            //
            //add form3
            this.BUT_Form3.Enabled = false;
            this.BUT_Form3.Location = new System.Drawing.Point(12 + 81*3, 42);
            this.BUT_Form3.Name = "BUT_Form3";
            this.BUT_Form3.Size = new System.Drawing.Size(75, 23);
            this.BUT_Form3.TabIndex = 15;
            this.BUT_Form3.Text = "Leader goto";
            this.BUT_Form3.UseVisualStyleBackColor = true;
            this.BUT_Form3.Click += new System.EventHandler(this.BUT_Form3_Click);
            //add form4 leader go to origin
            this.BUT_Form4.Enabled = false;
            this.BUT_Form4.Location = new System.Drawing.Point(12+81, 72);
            this.BUT_Form4.Name = "BUT_Form4";
            this.BUT_Form4.Size = new System.Drawing.Size(75, 23);
            this.BUT_Form4.TabIndex = 25;
            this.BUT_Form4.Text = "Form Sq";
            this.BUT_Form4.UseVisualStyleBackColor = true;
            this.BUT_Form4.Click += new System.EventHandler(this.BUT_Form4_Click);
            //add leader alt
            
            this.BUT_Leader_alt.Enabled = false;
            this.BUT_Leader_alt.Location = new System.Drawing.Point(12 + 81 * 3, 72);
            this.BUT_Leader_alt.Name = "BUT_Leader_alt";
            this.BUT_Leader_alt.Size = new System.Drawing.Size(75, 23);
            this.BUT_Leader_alt.TabIndex = 25;
            this.BUT_Leader_alt.Text = "Leader alt";
            this.BUT_Leader_alt.UseVisualStyleBackColor = true;
            this.BUT_Leader_alt.Click += new System.EventHandler(this.BUT_Leader_alt_Click);


            this.BUT_altmin.Enabled = false;
            this.BUT_altmin.Location = new System.Drawing.Point(463, 72);
            this.BUT_altmin.Name = "BUT_altmin";
            this.BUT_altmin.Size = new System.Drawing.Size(25, 23);
            this.BUT_altmin.TabIndex = 26;
            this.BUT_altmin.Text = "-";
            this.BUT_altmin.UseVisualStyleBackColor = true;
            this.BUT_altmin.Click += new System.EventHandler(this.BUT_altmin_click);
            //
            this.BUT_altlvl.Enabled = false;
            this.BUT_altlvl.Location = new System.Drawing.Point(463 + 25, 72);
            this.BUT_altlvl.Name = "BUT_altlvl";
            this.BUT_altlvl.Size = new System.Drawing.Size(25, 23);
            this.BUT_altlvl.TabIndex = 27;
            this.BUT_altlvl.Text = "0";
            this.BUT_altlvl.UseVisualStyleBackColor = true;
            this.BUT_altlvl.Click += new System.EventHandler(this.BUT_altlvl_click);
            //
            this.BUT_altadd.Enabled = false;
            this.BUT_altadd.Location = new System.Drawing.Point(463 + 50, 72);
            this.BUT_altadd.Name = "BUT_altadd";
            this.BUT_altadd.Size = new System.Drawing.Size(25, 23);
            this.BUT_altadd.TabIndex = 28;
            this.BUT_altadd.Text = "+";
            this.BUT_altadd.UseVisualStyleBackColor = true;
            this.BUT_altadd.Click += new System.EventHandler(this.BUT_altadd_click);
            //
            this.BUT_demonoise.Enabled = false;
            this.BUT_demonoise.Location = new System.Drawing.Point(544-83, 42);
            this.BUT_demonoise.Name = "BUT_demonoise";
            this.BUT_demonoise.Size = new System.Drawing.Size(75, 23);
            this.BUT_demonoise.TabIndex = 30;
            this.BUT_demonoise.Text = "demo Noise";
            this.BUT_demonoise.UseVisualStyleBackColor = true;
            this.BUT_demonoise.Click += new System.EventHandler(this.BUT_demonoise_click);
            //
            this.BUT_demospoof.Enabled = false;
            this.BUT_demospoof.Location = new System.Drawing.Point(544+83, 42);
            this.BUT_demospoof.Name = "BUT_demospoof";
            this.BUT_demospoof.Size = new System.Drawing.Size(75, 23);
            this.BUT_demospoof.TabIndex = 29;
            this.BUT_demospoof.Text = "demo Spoof";
            this.BUT_demospoof.UseVisualStyleBackColor = true;
            this.BUT_demospoof.Click += new System.EventHandler(this.BUT_demospoof_click);
            //
            
            this.BUT_addnoise.Enabled = false;
            this.BUT_addnoise.Location = new System.Drawing.Point(544, 42);
            this.BUT_addnoise.Name = "BUT_addnoise";
            this.BUT_addnoise.Size = new System.Drawing.Size(75, 23);
            this.BUT_addnoise.TabIndex = 31;
            this.BUT_addnoise.Text = "+ noise";
            this.BUT_addnoise.UseVisualStyleBackColor = true;
            this.BUT_addnoise.Click += new System.EventHandler(this.BUT_addnoise_click);
            //
            this.BUT_flyto_distangle.Enabled = false;
            this.BUT_flyto_distangle.Location = new System.Drawing.Point(544, 72);
            this.BUT_flyto_distangle.Name = "BUT_flyto_distangle";
            this.BUT_flyto_distangle.Size = new System.Drawing.Size(75, 23);
            this.BUT_flyto_distangle.TabIndex = 32;
            this.BUT_flyto_distangle.Text = "fly to D/A";
            this.BUT_flyto_distangle.UseVisualStyleBackColor = true;
            this.BUT_flyto_distangle.Click += new System.EventHandler(this.BUT_flyto_distangle_click);
            //
            this.BUT_RTL.Enabled = false;
            this.BUT_RTL.Location = new System.Drawing.Point(12 + 81 * 2, 72);
            this.BUT_RTL.Name = "BUT_RTL";
            this.BUT_RTL.Size = new System.Drawing.Size(75, 23);
            this.BUT_RTL.TabIndex = 32;
            this.BUT_RTL.Text = "RTL";
            this.BUT_RTL.UseVisualStyleBackColor = true;
            this.BUT_RTL.Click += new System.EventHandler(this.BUT_RTL_click);
            //
            this.Text_dis_gate.Location = new System.Drawing.Point(544 + 83, 72);
            this.Text_dis_gate.Name = "dis_gate";
            this.Text_dis_gate.Size = new System.Drawing.Size(30, 13);
            this.Text_dis_gate.TabIndex = 35;
            this.Text_dis_gate.Text = "50";
            this.Text_dis_gate.ReadOnly = false;
            //
            this.Text_angle_gate.Location = new System.Drawing.Point(544 + 83 + 40, 72);
            this.Text_angle_gate.Name = "ang_gate";
            this.Text_angle_gate.Size = new System.Drawing.Size(30, 13);
            this.Text_angle_gate.TabIndex = 36;
            this.Text_angle_gate.Text = "90";
            this.Text_angle_gate.ReadOnly = false;
            //
            this.BUT_follow_me.Enabled = false;
            this.BUT_follow_me.Location = new System.Drawing.Point(544 + 83 + 40+40, 72);
            this.BUT_follow_me.Name = "BUT_follow_me";
            this.BUT_follow_me.Size = new System.Drawing.Size(75, 23);
            this.BUT_follow_me.TabIndex = 37;
            this.BUT_follow_me.Text = "AV(off)";
            this.BUT_follow_me.UseVisualStyleBackColor = true;
            this.BUT_follow_me.Click += new System.EventHandler(this.BUT_follow_me_click);
            //
            this.BUT_allone.Enabled = false;
            this.BUT_allone.Location = new System.Drawing.Point(12, 72);
            this.BUT_allone.Name = "BUT_allone";
            this.BUT_allone.Size = new System.Drawing.Size(75, 23);
            this.BUT_allone.TabIndex = 33;
            this.BUT_allone.Text = "all 1";
            this.BUT_allone.UseVisualStyleBackColor = true;
            this.BUT_allone.Click += new System.EventHandler(this.BUT_allone_click);
            //
            this.BUT_guided_leader.Enabled = false;
            this.BUT_guided_leader.Location = new System.Drawing.Point(12, 102);
            this.BUT_guided_leader.Name = "BUT_guided_leader";
            this.BUT_guided_leader.Size = new System.Drawing.Size(75, 23);
            this.BUT_guided_leader.TabIndex = 34;
            this.BUT_guided_leader.Text = "Leader Guided";
            this.BUT_guided_leader.UseVisualStyleBackColor = true;
            this.BUT_guided_leader.Click += new System.EventHandler(this.BUT_guided_leader_click);
            //
            this.BUT_poshold_leader.Enabled = false;
            this.BUT_poshold_leader.Location = new System.Drawing.Point(90, 102);
            this.BUT_poshold_leader.Name = "BUT_poshold_leader";
            this.BUT_poshold_leader.Size = new System.Drawing.Size(75, 23);
            this.BUT_poshold_leader.TabIndex = 35;
            this.BUT_poshold_leader.Text = "Leader PosHold";
            this.BUT_poshold_leader.UseVisualStyleBackColor = true;
            this.BUT_poshold_leader.Click += new System.EventHandler(this.BUT_poshold_leader_click);
            //
            this.Radio_fields_1.Enabled = false;
            this.Radio_fields_1.Location = new System.Drawing.Point(173, 102);
            this.Radio_fields_1.Name = "Radio_fields_1";
            this.Radio_fields_1.Size = new System.Drawing.Size(75, 23);
            this.Radio_fields_1.TabIndex = 36;
            this.Radio_fields_1.Text = "fields 1";
            this.Radio_fields_1.Checked = true;
            this.Radio_fields_1.UseVisualStyleBackColor = true;
            this.Radio_fields_1.BackColor = System.Drawing.Color.Green;
            this.Radio_fields_1.Click += new System.EventHandler(this.Radio_fields_1_click);


            //
            this.Radio_fields_2.Enabled = false;
            this.Radio_fields_2.Location = new System.Drawing.Point(173+83, 102);
            this.Radio_fields_2.Name = "Radio_fields_2";
            this.Radio_fields_2.Size = new System.Drawing.Size(75, 23);
            this.Radio_fields_2.TabIndex = 37;
            this.Radio_fields_2.Text = "fields 2";
            this.Radio_fields_2.Checked = false;
            this.Radio_fields_2.UseVisualStyleBackColor = true;
            this.Radio_fields_2.BackColor = System.Drawing.Color.Green;
            this.Radio_fields_2.Click += new System.EventHandler(this.Radio_fields_2_click);
            //add lat input
            this.Text_lat_text.Location = new System.Drawing.Point(12 + 81 * 4, 38);
            this.Text_lat_text.Name = "lat";
            this.Text_lat_text.Size = new System.Drawing.Size(43, 23);
            this.Text_lat_text.TabIndex = 19;
            this.Text_lat_text.Text = "lat:";
            this.Text_lat_text.ReadOnly = true;

            this.Text_lat.Location = new System.Drawing.Point(40 + 81 * 4, 38);
            this.Text_lat.Name = "lat";
            this.Text_lat.Size = new System.Drawing.Size(95, 23);
            this.Text_lat.TabIndex = 16;
            
            this.Text_lat.Text = "-35.362407919859933";

            //add lng input
            this.Text_lng_text.Location = new System.Drawing.Point(12 + 81 * 4, 55);
            this.Text_lng_text.Name = "lng";
            this.Text_lng_text.Size = new System.Drawing.Size(43, 23);
            this.Text_lng_text.TabIndex = 20;
            this.Text_lng_text.Text = "lng:";
            this.Text_lng_text.ReadOnly = true;
            this.Text_lng.Location = new System.Drawing.Point(40 + 81 * 4, 55);
            this.Text_lng.Name = "lng";
            this.Text_lng.Size = new System.Drawing.Size(95, 23);
            this.Text_lng.TabIndex = 17;
            this.Text_lng.Text = "149.16559338569641";
            //add lng input
            this.Text_alt_text.Location = new System.Drawing.Point(12 + 81 * 4, 72);
            this.Text_alt_text.Name = "lng";
            this.Text_alt_text.Size = new System.Drawing.Size(23, 23);
            this.Text_alt_text.TabIndex = 21;
            this.Text_alt_text.Text = "alt:";
            this.Text_alt_text.ReadOnly = true;
            this.Text_alt.Location = new System.Drawing.Point(35 + 81 * 4, 72);
            this.Text_alt.Name = "alt";
            this.Text_alt.Size = new System.Drawing.Size(23, 23);
            this.Text_alt.TabIndex = 18;
            this.Text_alt.Text = "50";
            //input ywa
            this.Text_yaw_text.Location = new System.Drawing.Point(17 + 81 * 4 + 43, 72);
            this.Text_yaw_text.Name = "yaw";
            this.Text_yaw_text.Size = new System.Drawing.Size(23, 23);
            this.Text_yaw_text.TabIndex = 22;
            this.Text_yaw_text.Text = "yaw:";
            this.Text_yaw_text.ReadOnly = true;
            this.Text_yaw.Location = new System.Drawing.Point(40 + 81 * 4 + 43 , 72);
            this.Text_yaw.Name = "yaw";
            this.Text_yaw.Size = new System.Drawing.Size(23, 23);
            this.Text_yaw.TabIndex = 23;
            this.Text_yaw.Text = "0";
            //add but for yaw
            this.BUT_yaw.Enabled = false;
            this.BUT_yaw.Location = new System.Drawing.Point(40 + 81 * 4 + 66, 72);
            this.BUT_yaw.Name = "BUT_yaw";
            this.BUT_yaw.Size = new System.Drawing.Size(23, 23);
            this.BUT_yaw.TabIndex = 24;
            this.BUT_yaw.Text = "set";
            this.BUT_yaw.UseVisualStyleBackColor = true;
            this.BUT_yaw.Click += new System.EventHandler(this.BUT_yaw_Click);

            //this.Text_lat.Click += new System.EventHandler(this.BUT_Form3_Click);
            //
            //add combobox of choose
            this.CMB_Choose_method.Items.Add("Normal");
            this.CMB_Choose_method.Items.Add("New");
            this.CMB_Choose_method.FormattingEnabled = true;
            this.CMB_Choose_method.Location = new System.Drawing.Point(12, 42);
            this.CMB_Choose_method.Name = "Choose_method";
            this.CMB_Choose_method.Size = new System.Drawing.Size(75, 23);
            this.CMB_Choose_method.TabIndex = 14;
            this.CMB_Choose_method.SelectedItem = 1;
            this.CMB_Choose_method.SelectedIndexChanged += new System.EventHandler(this.CMB_Choose_method_SelectedIndexChanged);
            this.CMB_Choose_method.SelectedIndex = 0;
            // 
            // CMB_mavs
            // 
            this.CMB_mavs.DataSource = this.bindingSource1;
            this.CMB_mavs.FormattingEnabled = true;
            this.CMB_mavs.Location = new System.Drawing.Point(336, 12);
            this.CMB_mavs.Name = "CMB_mavs";
            this.CMB_mavs.Size = new System.Drawing.Size(121, 21);
            this.CMB_mavs.TabIndex = 4;
            this.CMB_mavs.SelectedIndexChanged += new System.EventHandler(this.CMB_mavs_SelectedIndexChanged);
            // 
            // grid1
            // 
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.Location = new System.Drawing.Point(3, 3);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(755, 388);
            this.grid1.TabIndex = 8;
            this.grid1.Vertical = false;
            this.grid1.UpdateOffsets += new MissionPlanner.Swarm.Grid.UpdateOffsetsEvent(this.grid1_UpdateOffsets);
            // 
            // BUT_Start
            // 
            this.BUT_Start.Enabled = false;
            this.BUT_Start.Location = new System.Drawing.Point(706, 12);
            this.BUT_Start.Name = "BUT_Start";
            this.BUT_Start.Size = new System.Drawing.Size(75, 23);
            this.BUT_Start.TabIndex = 6;
            this.BUT_Start.Text = global::MissionPlanner.Strings.Start;
            this.BUT_Start.UseVisualStyleBackColor = true;
            this.BUT_Start.Click += new System.EventHandler(this.BUT_Start_Click);
            // 

            // BUT_leader
            // 
            this.BUT_leader.Location = new System.Drawing.Point(463, 12);
            this.BUT_leader.Name = "BUT_leader";
            this.BUT_leader.Size = new System.Drawing.Size(75, 23);
            this.BUT_leader.TabIndex = 5;
            this.BUT_leader.Text = "Set Leader";
            this.BUT_leader.UseVisualStyleBackColor = true;
            this.BUT_leader.Click += new System.EventHandler(this.BUT_leader_Click);
            // 
            // BUT_Land
            // 
            this.BUT_Land.Location = new System.Drawing.Point(255, 12);
            this.BUT_Land.Name = "BUT_Land";
            this.BUT_Land.Size = new System.Drawing.Size(75, 23);
            this.BUT_Land.TabIndex = 3;
            this.BUT_Land.Text = "Land (all)";
            this.BUT_Land.UseVisualStyleBackColor = true;
            this.BUT_Land.Click += new System.EventHandler(this.BUT_Land_Click);
            // 
            // BUT_Takeoff
            // 
            this.BUT_Takeoff.Location = new System.Drawing.Point(174, 12);
            this.BUT_Takeoff.Name = "BUT_Takeoff";
            this.BUT_Takeoff.Size = new System.Drawing.Size(75, 23);
            this.BUT_Takeoff.TabIndex = 2;
            this.BUT_Takeoff.Text = "Takeoff";
            this.BUT_Takeoff.UseVisualStyleBackColor = true;
            this.BUT_Takeoff.Click += new System.EventHandler(this.BUT_Takeoff_Click);
            // 
            // BUT_Disarm
            // 
            this.BUT_Disarm.Location = new System.Drawing.Point(93, 12);
            this.BUT_Disarm.Name = "BUT_Disarm";
            this.BUT_Disarm.Size = new System.Drawing.Size(75, 23);
            this.BUT_Disarm.TabIndex = 1;
            this.BUT_Disarm.Text = "Disarm (exl leader)";
            this.BUT_Disarm.UseVisualStyleBackColor = true;
            this.BUT_Disarm.Click += new System.EventHandler(this.BUT_Disarm_Click);
            // 
            // BUT_Arm
            // 
            this.BUT_Arm.Location = new System.Drawing.Point(12, 12);
            this.BUT_Arm.Name = "BUT_Arm";
            this.BUT_Arm.Size = new System.Drawing.Size(75, 23);
            this.BUT_Arm.TabIndex = 0;
            this.BUT_Arm.Text = "Arm (exl leader)";
            this.BUT_Arm.UseVisualStyleBackColor = true;
            this.BUT_Arm.Click += new System.EventHandler(this.BUT_Arm_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 129);//default 12,39
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(769, 330);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(761, 394);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Stage 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // BUT_Updatepos
            // 
            this.BUT_Updatepos.Enabled = false;
            this.BUT_Updatepos.Location = new System.Drawing.Point(625, 12);
            this.BUT_Updatepos.Name = "BUT_Updatepos";
            this.BUT_Updatepos.Size = new System.Drawing.Size(75, 23);
            this.BUT_Updatepos.TabIndex = 10;
            this.BUT_Updatepos.Text = "Update Pos";
            this.BUT_Updatepos.UseVisualStyleBackColor = true;
            this.BUT_Updatepos.Click += new System.EventHandler(this.BUT_Updatepos_Click);
            // 
            // PNL_status
            // 
            this.PNL_status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PNL_status.AutoScroll = true;
            this.PNL_status.Location = new System.Drawing.Point(783, 61);
            this.PNL_status.Name = "PNL_status";
            this.PNL_status.Size = new System.Drawing.Size(147, 398);
            this.PNL_status.TabIndex = 11;
            // 
            // timer_status
            // 
            this.timer_status.Enabled = true;
            this.timer_status.Interval = 200;
            this.timer_status.Tick += new System.EventHandler(this.timer_status_Tick);
            // 
            // but_guided
            // 
            this.but_guided.Location = new System.Drawing.Point(544, 12);
            this.but_guided.Name = "but_guided";
            this.but_guided.Size = new System.Drawing.Size(75, 23);
            this.but_guided.TabIndex = 12;
            this.but_guided.Text = "Guided Mode";
            this.but_guided.UseVisualStyleBackColor = true;
            this.but_guided.Click += new System.EventHandler(this.but_guided_Click);
            // 
            // FormationControl
            // 
            this.Text_UWB_dis1.Multiline = true;
            this.Text_UWB_dis1.Location = new System.Drawing.Point(846, 12);
            this.Text_UWB_dis1.Size = new System.Drawing.Size(245, 23);
            this.Text_UWB_dis1.Name = "UWB_dist";
            this.Text_UWB_dis1.Text = "0.000, 0.000, 0.000, 0.000, 0.000";
            this.Text_UWB_dis1.Font = new System.Drawing.Font("Times New Roman", 12.0f); 
            this.Text_UWB_dis1.ReadOnly = true;
            //this.Text_UWB_dis.WordWrap = true;
            //this.Text_UWB_dis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            ///
            this.Text_UWB_dis2.Multiline = true;
            this.Text_UWB_dis2.Location = new System.Drawing.Point(846, 40);
            this.Text_UWB_dis2.Size = new System.Drawing.Size(245, 23);
            this.Text_UWB_dis2.Name = "UWB_dist";
            this.Text_UWB_dis2.Text = "0.000, 0.000, 0.000, 0.000, 0.000";
            this.Text_UWB_dis2.Font = new System.Drawing.Font("Times New Roman", 12.0f);
            this.Text_UWB_dis2.ReadOnly = true;
            ///
            this.Text_UWB_dis3.Multiline = true;
            this.Text_UWB_dis3.Location = new System.Drawing.Point(846, 68);
            this.Text_UWB_dis3.Size = new System.Drawing.Size(245, 23);
            this.Text_UWB_dis3.Name = "UWB_dist";
            this.Text_UWB_dis3.Text = "0.000, 0.000, 0.000, 0.000, 0.000";
            this.Text_UWB_dis3.Font = new System.Drawing.Font("Times New Roman", 12.0f);
            this.Text_UWB_dis3.ReadOnly = true;
            ///
            this.Text_UWB_dis4.Multiline = true;
            this.Text_UWB_dis4.Location = new System.Drawing.Point(846, 96);
            this.Text_UWB_dis4.Size = new System.Drawing.Size(245, 23);
            this.Text_UWB_dis4.Name = "UWB_dist";
            this.Text_UWB_dis4.Text = "0.000, 0.000, 0.000, 0.000, 0.000";
            this.Text_UWB_dis4.Font = new System.Drawing.Font("Times New Roman", 12.0f);
            this.Text_UWB_dis4.ReadOnly = true;

            //
            //
            this.Text_UWB_dis5.Multiline = true;
            this.Text_UWB_dis5.Location = new System.Drawing.Point(846, 124);
            this.Text_UWB_dis5.Size = new System.Drawing.Size(245, 23);
            this.Text_UWB_dis5.Name = "UWB_dist";
            this.Text_UWB_dis5.Text = "0.000, 0.000, 0.000, 0.000, 0.000";
            this.Text_UWB_dis5.Font = new System.Drawing.Font("Times New Roman", 12.0f);
            this.Text_UWB_dis5.ReadOnly = true;


            this.Text_UWB_dis1_text.Multiline = true;
            this.Text_UWB_dis1_text.Location = new System.Drawing.Point(786, 12);
            this.Text_UWB_dis1_text.Size = new System.Drawing.Size(55, 23);
            this.Text_UWB_dis1_text.Name = "UWB_dist";
            this.Text_UWB_dis1_text.Text = "UAV1";
            this.Text_UWB_dis1_text.Font = new System.Drawing.Font("Times New Roman", 12.0f);
            this.Text_UWB_dis1_text.ReadOnly = true;

            this.Text_UWB_dis2_text.Multiline = true;
            this.Text_UWB_dis2_text.Location = new System.Drawing.Point(786, 40);
            this.Text_UWB_dis2_text.Size = new System.Drawing.Size(55, 23);
            this.Text_UWB_dis2_text.Name = "UWB_dist";
            this.Text_UWB_dis2_text.Text = "UAV2";
            this.Text_UWB_dis2_text.Font = new System.Drawing.Font("Times New Roman", 12.0f);
            this.Text_UWB_dis2_text.ReadOnly = true;

            this.Text_UWB_dis3_text.Multiline = true;
            this.Text_UWB_dis3_text.Location = new System.Drawing.Point(786, 68);
            this.Text_UWB_dis3_text.Size = new System.Drawing.Size(55, 23);
            this.Text_UWB_dis3_text.Name = "UWB_dist";
            this.Text_UWB_dis3_text.Text = "UAV3";
            this.Text_UWB_dis3_text.Font = new System.Drawing.Font("Times New Roman", 12.0f);
            this.Text_UWB_dis3_text.ReadOnly = true;

            this.Text_UWB_dis4_text.Multiline = true;
            this.Text_UWB_dis4_text.Location = new System.Drawing.Point(786, 96);
            this.Text_UWB_dis4_text.Size = new System.Drawing.Size(55, 23);
            this.Text_UWB_dis4_text.Name = "UWB_dist";
            this.Text_UWB_dis4_text.Text = "UAV4";
            this.Text_UWB_dis4_text.Font = new System.Drawing.Font("Times New Roman", 12.0f);
            this.Text_UWB_dis4_text.ReadOnly = true;

            this.Text_UWB_dis5_text.Multiline = true;
            this.Text_UWB_dis5_text.Location = new System.Drawing.Point(786, 124);
            this.Text_UWB_dis5_text.Size = new System.Drawing.Size(55, 23);
            this.Text_UWB_dis5_text.Name = "UWB_dist";
            this.Text_UWB_dis5_text.Text = "UAV1";
            this.Text_UWB_dis5_text.Font = new System.Drawing.Font("Times New Roman", 12.0f);
            this.Text_UWB_dis5_text.ReadOnly = true;
            //

            this.ClientSize = new System.Drawing.Size(931, 471);
            this.Controls.Add(this.Text_UWB_dis1);
            this.Controls.Add(this.Text_UWB_dis2);
            this.Controls.Add(this.Text_UWB_dis3);
            this.Controls.Add(this.Text_UWB_dis4);
            this.Controls.Add(this.Text_UWB_dis5);
            this.Controls.Add(this.Text_UWB_dis1_text);
            this.Controls.Add(this.Text_UWB_dis2_text);
            this.Controls.Add(this.Text_UWB_dis3_text);
            this.Controls.Add(this.Text_UWB_dis4_text);
            this.Controls.Add(this.Text_UWB_dis5_text);
            this.Controls.Add(this.but_guided);
            this.Controls.Add(this.PNL_status);
            this.Controls.Add(this.BUT_Updatepos);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.BUT_Start);
            this.Controls.Add(this.BUT_leader);
            this.Controls.Add(this.CMB_mavs);
            this.Controls.Add(this.CMB_Choose_method);
            this.Controls.Add(this.BUT_Land);
            this.Controls.Add(this.BUT_Takeoff);
            this.Controls.Add(this.BUT_Disarm);
            this.Controls.Add(this.BUT_Arm);
            this.Controls.Add(this.BUT_Form1);//add form1
            this.Controls.Add(this.BUT_Form2);//add form2
            this.Controls.Add(this.BUT_Form3);//add form3
            this.Controls.Add(this.BUT_Form4);//add form3
            this.Controls.Add(this.Text_lat);//add text_lat
            this.Controls.Add(this.Text_lat_text);
            this.Controls.Add(this.Text_lng);//add text_lat
            this.Controls.Add(this.Text_lng_text);//add text_lat
            this.Controls.Add(this.Text_alt);//add text_lat
            this.Controls.Add(this.Text_alt_text);//add text_lat
            this.Controls.Add(this.Text_yaw);//add text_lat
            this.Controls.Add(this.Text_yaw_text);//add text_lat
            this.Controls.Add(this.BUT_yaw);//add BUT_yaw
            this.Controls.Add(this.BUT_altmin);//add BUT_altmin
            this.Controls.Add(this.BUT_altlvl);//add BUT_altlvl
            this.Controls.Add(this.BUT_altadd);//add BUT_altadd
            this.Controls.Add(this.BUT_demospoof);//add BUT_demospoof
            this.Controls.Add(this.BUT_follow_me);//add BUT_follow
            this.Controls.Add(this.Text_dis_gate);//add text_lat
            this.Controls.Add(this.Text_angle_gate);//add text_lat
            this.Controls.Add(this.BUT_RTL);
            this.Controls.Add(this.BUT_demonoise);//add BUT_altadd
            this.Controls.Add(this.BUT_addnoise);//add BUT_altadd
            this.Controls.Add(this.BUT_flyto_distangle);//add BUT_flyto
            this.Controls.Add(this.BUT_allone);//add BUT_altadd
            this.Controls.Add(this.BUT_Leader_alt);//add BUT_alta
            this.Controls.Add(this.BUT_guided_leader);//
            this.Controls.Add(this.BUT_poshold_leader);//
            this.Controls.Add(this.Radio_fields_1);
            this.Controls.Add(this.Radio_fields_2);
            this.Name = "FormationControl";
            this.Text = "Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Control_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MyButton BUT_Arm;
        private Controls.MyButton BUT_Disarm;
        private Controls.MyButton BUT_Takeoff;
        private Controls.MyButton BUT_Land;
        private System.Windows.Forms.ComboBox CMB_mavs;
        private Controls.MyButton BUT_leader;
        private Controls.MyButton BUT_Start;
        private Grid grid1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Controls.MyButton BUT_Updatepos;
        private Controls.MyButton BUT_Form1;
        private Controls.MyButton BUT_Form2;
        private Controls.MyButton BUT_Form3;
        private Controls.MyButton BUT_Form4;
        private System.Windows.Forms.FlowLayoutPanel PNL_status;
        private System.Windows.Forms.Timer timer_status;
        private Controls.MyButton but_guided;
        private System.Windows.Forms.ComboBox CMB_Choose_method;
        private System.Windows.Forms.TextBox Text_lat;
        private System.Windows.Forms.TextBox Text_lat_text;
        private System.Windows.Forms.TextBox Text_lng;
        private System.Windows.Forms.TextBox Text_lng_text;
        private System.Windows.Forms.TextBox Text_alt;
        private System.Windows.Forms.TextBox Text_alt_text;
        private System.Windows.Forms.TextBox Text_yaw;
        private System.Windows.Forms.TextBox Text_yaw_text;
        
        private System.Windows.Forms.TextBox Text_UWB_dis1;
        private System.Windows.Forms.TextBox Text_UWB_dis2;
        private System.Windows.Forms.TextBox Text_UWB_dis3;
        private System.Windows.Forms.TextBox Text_UWB_dis4;
        private System.Windows.Forms.TextBox Text_UWB_dis5;
        private System.Windows.Forms.TextBox Text_UWB_dis1_text;
        private System.Windows.Forms.TextBox Text_UWB_dis2_text;
        private System.Windows.Forms.TextBox Text_UWB_dis3_text;
        private System.Windows.Forms.TextBox Text_UWB_dis4_text;
        private System.Windows.Forms.TextBox Text_UWB_dis5_text;
        private Controls.MyButton BUT_yaw;
        private Controls.MyButton BUT_altmin;
        private Controls.MyButton BUT_altlvl;
        private Controls.MyButton BUT_altadd;
        private Controls.MyButton BUT_demospoof;
        private Controls.MyButton BUT_demonoise;
        private Controls.MyButton BUT_addnoise;
        private Controls.MyButton BUT_flyto_distangle;
        private Controls.MyButton BUT_allone;
        private System.Windows.Forms.TextBox Text_dis_gate;
        private System.Windows.Forms.TextBox Text_angle_gate;
        private Controls.MyButton BUT_follow_me;
        private Controls.MyButton BUT_RTL;
        private Controls.MyButton BUT_Leader_alt;
        private Controls.MyButton BUT_guided_leader;
        private Controls.MyButton BUT_poshold_leader;
        private System.Windows.Forms.RadioButton Radio_fields_1;
        private System.Windows.Forms.RadioButton Radio_fields_2;
    }
}