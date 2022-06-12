namespace MissionPlanner.Swarm
{
    partial class displayData
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.latChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button1 = new System.Windows.Forms.Button();
            this.lonChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.altChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button2 = new System.Windows.Forms.Button();
            this.verticalProgressBar1 = new MissionPlanner.Controls.VerticalProgressBar();
            this.verticalProgressBar2 = new MissionPlanner.Controls.VerticalProgressBar();
            this.verticalProgressBar3 = new MissionPlanner.Controls.VerticalProgressBar();
            this.verticalProgressBar4 = new MissionPlanner.Controls.VerticalProgressBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.lineSeparator4 = new MissionPlanner.Controls.LineSeparator();
            this.lineSeparator3 = new MissionPlanner.Controls.LineSeparator();
            this.lineSeparator2 = new MissionPlanner.Controls.LineSeparator();
            this.lineSeparator1 = new MissionPlanner.Controls.LineSeparator();
            this.circularProgressBar1 = new CircularProgressBar.CircularProgressBar();
            this.verticalProgressBar5 = new MissionPlanner.Controls.VerticalProgressBar();
            this.LQ2_text = new System.Windows.Forms.TextBox();
            this.LQ1_text = new System.Windows.Forms.TextBox();
            this.LQ4_text = new System.Windows.Forms.TextBox();
            this.LQ3 = new MissionPlanner.Controls.VerticalProgressBar();
            this.LQ3_text = new System.Windows.Forms.TextBox();
            this.LQ2 = new MissionPlanner.Controls.VerticalProgressBar();
            this.LQ1 = new MissionPlanner.Controls.VerticalProgressBar();
            this.LQ4 = new MissionPlanner.Controls.VerticalProgressBar();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.LQ5_text = new System.Windows.Forms.TextBox();
            this.LQ5 = new MissionPlanner.Controls.VerticalProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.latChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lonChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.altChart)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // latChart
            // 
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.Name = "ChartArea1";
            this.latChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.latChart.Legends.Add(legend1);
            this.latChart.Location = new System.Drawing.Point(42, 34);
            this.latChart.Name = "latChart";
            this.latChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            this.latChart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.Black;
            series1.Name = "Latitude";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            this.latChart.Series.Add(series1);
            this.latChart.Size = new System.Drawing.Size(1889, 347);
            this.latChart.TabIndex = 0;
            this.latChart.Text = "chart1";
            this.latChart.Click += new System.EventHandler(this.cpuChart_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1636, 1488);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(194, 99);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // lonChart
            // 
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.Name = "ChartArea1";
            this.lonChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.lonChart.Legends.Add(legend2);
            this.lonChart.Location = new System.Drawing.Point(42, 416);
            this.lonChart.Name = "lonChart";
            this.lonChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            this.lonChart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.IsXValueIndexed = true;
            series2.Legend = "Legend1";
            series2.MarkerColor = System.Drawing.Color.Black;
            series2.Name = "Longitude";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            this.lonChart.Series.Add(series2);
            this.lonChart.Size = new System.Drawing.Size(1889, 343);
            this.lonChart.TabIndex = 2;
            this.lonChart.Text = "chart1";
            // 
            // altChart
            // 
            chartArea3.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea3.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea3.Name = "ChartArea1";
            this.altChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.altChart.Legends.Add(legend3);
            this.altChart.Location = new System.Drawing.Point(42, 794);
            this.altChart.Name = "altChart";
            this.altChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            this.altChart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.IsXValueIndexed = true;
            series3.Legend = "Legend1";
            series3.MarkerColor = System.Drawing.Color.Black;
            series3.Name = "Altitude";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            this.altChart.Series.Add(series3);
            this.altChart.Size = new System.Drawing.Size(1889, 342);
            this.altChart.TabIndex = 3;
            this.altChart.Text = "chart1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1867, 1488);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(192, 99);
            this.button2.TabIndex = 4;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // verticalProgressBar1
            // 
            this.verticalProgressBar1.DrawLabel = true;
            this.verticalProgressBar1.Label = "distance";
            this.verticalProgressBar1.Location = new System.Drawing.Point(28, 49);
            this.verticalProgressBar1.Maximum = 50;
            this.verticalProgressBar1.maxline = 0;
            this.verticalProgressBar1.minline = 0;
            this.verticalProgressBar1.Name = "verticalProgressBar1";
            this.verticalProgressBar1.Size = new System.Drawing.Size(46, 221);
            this.verticalProgressBar1.Step = 1;
            this.verticalProgressBar1.TabIndex = 7;
            this.verticalProgressBar1.Click += new System.EventHandler(this.verticalProgressBar1_Click);
            // 
            // verticalProgressBar2
            // 
            this.verticalProgressBar2.DrawLabel = true;
            this.verticalProgressBar2.Label = "distance";
            this.verticalProgressBar2.Location = new System.Drawing.Point(123, 49);
            this.verticalProgressBar2.Maximum = 50;
            this.verticalProgressBar2.maxline = 0;
            this.verticalProgressBar2.minline = 0;
            this.verticalProgressBar2.Name = "verticalProgressBar2";
            this.verticalProgressBar2.Size = new System.Drawing.Size(46, 221);
            this.verticalProgressBar2.Step = 1;
            this.verticalProgressBar2.TabIndex = 8;
            // 
            // verticalProgressBar3
            // 
            this.verticalProgressBar3.DrawLabel = true;
            this.verticalProgressBar3.Label = "distance";
            this.verticalProgressBar3.Location = new System.Drawing.Point(221, 49);
            this.verticalProgressBar3.Maximum = 50;
            this.verticalProgressBar3.maxline = 0;
            this.verticalProgressBar3.minline = 0;
            this.verticalProgressBar3.Name = "verticalProgressBar3";
            this.verticalProgressBar3.Size = new System.Drawing.Size(46, 221);
            this.verticalProgressBar3.Step = 1;
            this.verticalProgressBar3.TabIndex = 10;
            // 
            // verticalProgressBar4
            // 
            this.verticalProgressBar4.DrawLabel = true;
            this.verticalProgressBar4.Label = "distance";
            this.verticalProgressBar4.Location = new System.Drawing.Point(319, 49);
            this.verticalProgressBar4.Maximum = 50;
            this.verticalProgressBar4.maxline = 0;
            this.verticalProgressBar4.minline = 0;
            this.verticalProgressBar4.Name = "verticalProgressBar4";
            this.verticalProgressBar4.Size = new System.Drawing.Size(46, 221);
            this.verticalProgressBar4.Step = 1;
            this.verticalProgressBar4.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox1.Location = new System.Drawing.Point(17, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(64, 31);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "0";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(114, 10);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(64, 31);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "0";
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(211, 10);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(64, 31);
            this.textBox3.TabIndex = 14;
            this.textBox3.Text = "0";
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(307, 10);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(64, 31);
            this.textBox4.TabIndex = 13;
            this.textBox4.Text = "0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox5);
            this.panel1.Controls.Add(this.lineSeparator4);
            this.panel1.Controls.Add(this.lineSeparator3);
            this.panel1.Controls.Add(this.lineSeparator2);
            this.panel1.Controls.Add(this.lineSeparator1);
            this.panel1.Controls.Add(this.verticalProgressBar4);
            this.panel1.Controls.Add(this.verticalProgressBar1);
            this.panel1.Controls.Add(this.verticalProgressBar2);
            this.panel1.Controls.Add(this.textBox3);
            this.panel1.Controls.Add(this.verticalProgressBar3);
            this.panel1.Controls.Add(this.textBox4);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Location = new System.Drawing.Point(42, 1153);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(392, 312);
            this.panel1.TabIndex = 17;
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox5.Location = new System.Drawing.Point(100, 278);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(186, 31);
            this.textBox5.TabIndex = 22;
            this.textBox5.Text = "Inter-distance";
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // lineSeparator4
            // 
            this.lineSeparator4.Location = new System.Drawing.Point(0, 93);
            this.lineSeparator4.MaximumSize = new System.Drawing.Size(2000, 2);
            this.lineSeparator4.MinimumSize = new System.Drawing.Size(0, 2);
            this.lineSeparator4.Name = "lineSeparator4";
            this.lineSeparator4.Size = new System.Drawing.Size(392, 2);
            this.lineSeparator4.TabIndex = 21;
            // 
            // lineSeparator3
            // 
            this.lineSeparator3.Location = new System.Drawing.Point(0, 138);
            this.lineSeparator3.MaximumSize = new System.Drawing.Size(2000, 2);
            this.lineSeparator3.MinimumSize = new System.Drawing.Size(0, 2);
            this.lineSeparator3.Name = "lineSeparator3";
            this.lineSeparator3.Size = new System.Drawing.Size(392, 2);
            this.lineSeparator3.TabIndex = 20;
            // 
            // lineSeparator2
            // 
            this.lineSeparator2.Location = new System.Drawing.Point(0, 182);
            this.lineSeparator2.MaximumSize = new System.Drawing.Size(2000, 2);
            this.lineSeparator2.MinimumSize = new System.Drawing.Size(0, 2);
            this.lineSeparator2.Name = "lineSeparator2";
            this.lineSeparator2.Size = new System.Drawing.Size(392, 2);
            this.lineSeparator2.TabIndex = 19;
            // 
            // lineSeparator1
            // 
            this.lineSeparator1.Location = new System.Drawing.Point(-3, 227);
            this.lineSeparator1.MaximumSize = new System.Drawing.Size(2000, 2);
            this.lineSeparator1.MinimumSize = new System.Drawing.Size(0, 2);
            this.lineSeparator1.Name = "lineSeparator1";
            this.lineSeparator1.Size = new System.Drawing.Size(392, 2);
            this.lineSeparator1.TabIndex = 18;
            // 
            // circularProgressBar1
            // 
            this.circularProgressBar1.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.circularProgressBar1.AnimationSpeed = 500;
            this.circularProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.circularProgressBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.circularProgressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.circularProgressBar1.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.circularProgressBar1.InnerMargin = 2;
            this.circularProgressBar1.InnerWidth = -1;
            this.circularProgressBar1.Location = new System.Drawing.Point(547, 1133);
            this.circularProgressBar1.MarqueeAnimationSpeed = 2000;
            this.circularProgressBar1.Maximum = 365;
            this.circularProgressBar1.Name = "circularProgressBar1";
            this.circularProgressBar1.OuterColor = System.Drawing.Color.Gray;
            this.circularProgressBar1.OuterMargin = -25;
            this.circularProgressBar1.OuterWidth = 26;
            this.circularProgressBar1.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.circularProgressBar1.ProgressWidth = 25;
            this.circularProgressBar1.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.circularProgressBar1.Size = new System.Drawing.Size(357, 340);
            this.circularProgressBar1.StartAngle = 270;
            this.circularProgressBar1.Step = 1;
            this.circularProgressBar1.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar1.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.circularProgressBar1.SubscriptText = "";
            this.circularProgressBar1.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar1.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.circularProgressBar1.SuperscriptText = "";
            this.circularProgressBar1.TabIndex = 18;
            this.circularProgressBar1.Text = "10";
            this.circularProgressBar1.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.circularProgressBar1.Value = 68;
            // 
            // verticalProgressBar5
            // 
            this.verticalProgressBar5.DrawLabel = true;
            this.verticalProgressBar5.Label = "Dist2Home";
            this.verticalProgressBar5.Location = new System.Drawing.Point(978, 1153);
            this.verticalProgressBar5.maxline = 0;
            this.verticalProgressBar5.minline = 0;
            this.verticalProgressBar5.Name = "verticalProgressBar5";
            this.verticalProgressBar5.Size = new System.Drawing.Size(55, 270);
            this.verticalProgressBar5.Step = 1;
            this.verticalProgressBar5.TabIndex = 23;
            // 
            // LQ2_text
            // 
            this.LQ2_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LQ2_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LQ2_text.Location = new System.Drawing.Point(114, 10);
            this.LQ2_text.Name = "LQ2_text";
            this.LQ2_text.ReadOnly = true;
            this.LQ2_text.Size = new System.Drawing.Size(64, 31);
            this.LQ2_text.TabIndex = 12;
            this.LQ2_text.Text = "0";
            // 
            // LQ1_text
            // 
            this.LQ1_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LQ1_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LQ1_text.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.LQ1_text.Location = new System.Drawing.Point(17, 10);
            this.LQ1_text.Name = "LQ1_text";
            this.LQ1_text.ReadOnly = true;
            this.LQ1_text.Size = new System.Drawing.Size(64, 31);
            this.LQ1_text.TabIndex = 11;
            this.LQ1_text.Text = "0";
            // 
            // LQ4_text
            // 
            this.LQ4_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LQ4_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LQ4_text.Location = new System.Drawing.Point(307, 10);
            this.LQ4_text.Name = "LQ4_text";
            this.LQ4_text.ReadOnly = true;
            this.LQ4_text.Size = new System.Drawing.Size(64, 31);
            this.LQ4_text.TabIndex = 13;
            this.LQ4_text.Text = "0";
            // 
            // LQ3
            // 
            this.LQ3.DrawLabel = true;
            this.LQ3.Label = "distance";
            this.LQ3.Location = new System.Drawing.Point(221, 49);
            this.LQ3.maxline = 0;
            this.LQ3.minline = 0;
            this.LQ3.Name = "LQ3";
            this.LQ3.Size = new System.Drawing.Size(46, 221);
            this.LQ3.Step = 1;
            this.LQ3.TabIndex = 10;
            // 
            // LQ3_text
            // 
            this.LQ3_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LQ3_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LQ3_text.Location = new System.Drawing.Point(211, 10);
            this.LQ3_text.Name = "LQ3_text";
            this.LQ3_text.ReadOnly = true;
            this.LQ3_text.Size = new System.Drawing.Size(64, 31);
            this.LQ3_text.TabIndex = 14;
            this.LQ3_text.Text = "0";
            // 
            // LQ2
            // 
            this.LQ2.DrawLabel = true;
            this.LQ2.Label = "distance";
            this.LQ2.Location = new System.Drawing.Point(123, 49);
            this.LQ2.maxline = 0;
            this.LQ2.minline = 0;
            this.LQ2.Name = "LQ2";
            this.LQ2.Size = new System.Drawing.Size(46, 221);
            this.LQ2.Step = 1;
            this.LQ2.TabIndex = 8;
            // 
            // LQ1
            // 
            this.LQ1.DrawLabel = true;
            this.LQ1.Label = "distance";
            this.LQ1.Location = new System.Drawing.Point(28, 49);
            this.LQ1.maxline = 0;
            this.LQ1.minline = 0;
            this.LQ1.Name = "LQ1";
            this.LQ1.Size = new System.Drawing.Size(46, 221);
            this.LQ1.Step = 1;
            this.LQ1.TabIndex = 7;
            // 
            // LQ4
            // 
            this.LQ4.DrawLabel = true;
            this.LQ4.Label = "distance";
            this.LQ4.Location = new System.Drawing.Point(319, 49);
            this.LQ4.maxline = 0;
            this.LQ4.minline = 0;
            this.LQ4.Name = "LQ4";
            this.LQ4.Size = new System.Drawing.Size(46, 221);
            this.LQ4.Step = 1;
            this.LQ4.TabIndex = 9;
            // 
            // textBox6
            // 
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox6.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox6.Location = new System.Drawing.Point(166, 316);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(186, 31);
            this.textBox6.TabIndex = 22;
            this.textBox6.Text = "Link quality";
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBox16);
            this.panel2.Controls.Add(this.textBox15);
            this.panel2.Controls.Add(this.textBox14);
            this.panel2.Controls.Add(this.textBox13);
            this.panel2.Controls.Add(this.textBox12);
            this.panel2.Controls.Add(this.textBox6);
            this.panel2.Controls.Add(this.LQ5_text);
            this.panel2.Controls.Add(this.LQ5);
            this.panel2.Controls.Add(this.LQ4);
            this.panel2.Controls.Add(this.LQ1);
            this.panel2.Controls.Add(this.LQ2);
            this.panel2.Controls.Add(this.LQ3_text);
            this.panel2.Controls.Add(this.LQ3);
            this.panel2.Controls.Add(this.LQ4_text);
            this.panel2.Controls.Add(this.LQ1_text);
            this.panel2.Controls.Add(this.LQ2_text);
            this.panel2.Location = new System.Drawing.Point(1115, 1172);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(518, 359);
            this.panel2.TabIndex = 24;
            // 
            // textBox16
            // 
            this.textBox16.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox16.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox16.Location = new System.Drawing.Point(437, 276);
            this.textBox16.Name = "textBox16";
            this.textBox16.ReadOnly = true;
            this.textBox16.Size = new System.Drawing.Size(22, 31);
            this.textBox16.TabIndex = 26;
            this.textBox16.Text = "5";
            // 
            // textBox15
            // 
            this.textBox15.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox15.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox15.Location = new System.Drawing.Point(330, 276);
            this.textBox15.Name = "textBox15";
            this.textBox15.ReadOnly = true;
            this.textBox15.Size = new System.Drawing.Size(22, 31);
            this.textBox15.TabIndex = 26;
            this.textBox15.Text = "4";
            // 
            // textBox14
            // 
            this.textBox14.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox14.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox14.Location = new System.Drawing.Point(234, 276);
            this.textBox14.Name = "textBox14";
            this.textBox14.ReadOnly = true;
            this.textBox14.Size = new System.Drawing.Size(22, 31);
            this.textBox14.TabIndex = 26;
            this.textBox14.Text = "3";
            // 
            // textBox13
            // 
            this.textBox13.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox13.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox13.Location = new System.Drawing.Point(135, 276);
            this.textBox13.Name = "textBox13";
            this.textBox13.ReadOnly = true;
            this.textBox13.Size = new System.Drawing.Size(22, 31);
            this.textBox13.TabIndex = 25;
            this.textBox13.Text = "2";
            this.textBox13.TextChanged += new System.EventHandler(this.textBox13_TextChanged);
            // 
            // textBox12
            // 
            this.textBox12.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox12.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox12.Location = new System.Drawing.Point(39, 276);
            this.textBox12.Name = "textBox12";
            this.textBox12.ReadOnly = true;
            this.textBox12.Size = new System.Drawing.Size(31, 31);
            this.textBox12.TabIndex = 23;
            this.textBox12.Text = "1";
            // 
            // LQ5_text
            // 
            this.LQ5_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LQ5_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LQ5_text.Location = new System.Drawing.Point(407, 10);
            this.LQ5_text.Name = "LQ5_text";
            this.LQ5_text.ReadOnly = true;
            this.LQ5_text.Size = new System.Drawing.Size(64, 31);
            this.LQ5_text.TabIndex = 16;
            this.LQ5_text.Text = "0";
            // 
            // LQ5
            // 
            this.LQ5.DrawLabel = true;
            this.LQ5.Label = "distance";
            this.LQ5.Location = new System.Drawing.Point(424, 49);
            this.LQ5.maxline = 0;
            this.LQ5.minline = 0;
            this.LQ5.Name = "LQ5";
            this.LQ5.Size = new System.Drawing.Size(46, 221);
            this.LQ5.Step = 1;
            this.LQ5.TabIndex = 15;
            // 
            // displayData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(2099, 1620);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.verticalProgressBar5);
            this.Controls.Add(this.circularProgressBar1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.altChart);
            this.Controls.Add(this.lonChart);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.latChart);
            this.Name = "displayData";
            this.Text = "displayData";
            this.Load += new System.EventHandler(this.displayData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.latChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lonChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.altChart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart latChart;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataVisualization.Charting.Chart altChart;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataVisualization.Charting.Chart lonChart;
        private Controls.VerticalProgressBar verticalProgressBar1;
        private Controls.VerticalProgressBar verticalProgressBar2;
        private Controls.VerticalProgressBar verticalProgressBar3;
        private Controls.VerticalProgressBar verticalProgressBar4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Panel panel1;
        private Controls.LineSeparator lineSeparator3;
        private Controls.LineSeparator lineSeparator2;
        private Controls.LineSeparator lineSeparator1;
        private Controls.LineSeparator lineSeparator4;
        private System.Windows.Forms.TextBox textBox5;
        private CircularProgressBar.CircularProgressBar circularProgressBar1;
        private Controls.VerticalProgressBar verticalProgressBar5;
        private System.Windows.Forms.TextBox LQ2_text;
        private System.Windows.Forms.TextBox LQ1_text;
        private System.Windows.Forms.TextBox LQ4_text;
        private Controls.VerticalProgressBar LQ3;
        private System.Windows.Forms.TextBox LQ3_text;
        private Controls.VerticalProgressBar LQ2;
        private Controls.VerticalProgressBar LQ1;
        private Controls.VerticalProgressBar LQ4;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox LQ5_text;
        private Controls.VerticalProgressBar LQ5;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.TextBox textBox14;
    }
}