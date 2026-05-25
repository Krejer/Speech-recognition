namespace Project3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.panelLeft = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRecord = new System.Windows.Forms.Button();
            this.btnLoad1 = new System.Windows.Forms.Button();
            this.btnLoad2 = new System.Windows.Forms.Button();
            this.gbPreproc = new System.Windows.Forms.GroupBox();
            this.chkTrimSilence = new System.Windows.Forms.CheckBox();
            this.chkNormalize = new System.Windows.Forms.CheckBox();
            this.chkPreemphasis = new System.Windows.Forms.CheckBox();
            this.gbFeatures = new System.Windows.Forms.GroupBox();
            this.rbFFT = new System.Windows.Forms.RadioButton();
            this.rbMFCC = new System.Windows.Forms.RadioButton();
            this.gbMetric = new System.Windows.Forms.GroupBox();
            this.rbEuclidean = new System.Windows.Forms.RadioButton();
            this.rbCosine = new System.Windows.Forms.RadioButton();
            this.btnShowSignal = new System.Windows.Forms.Button();
            this.btnShowDTW = new System.Windows.Forms.Button();
            this.btnGenerateDB = new System.Windows.Forms.Button();
            this.btnRecognize = new System.Windows.Forms.Button();
            this.panelPlot = new System.Windows.Forms.Panel();
            this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();

            this.panelLeft.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.gbPreproc.SuspendLayout();
            this.gbFeatures.SuspendLayout();
            this.gbMetric.SuspendLayout();
            this.panelPlot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelLeft.Controls.Add(this.flowLayoutPanel1);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(260, 711);
            this.panelLeft.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnRecord);
            this.flowLayoutPanel1.Controls.Add(this.btnLoad1);
            this.flowLayoutPanel1.Controls.Add(this.btnLoad2);
            this.flowLayoutPanel1.Controls.Add(this.gbPreproc);
            this.flowLayoutPanel1.Controls.Add(this.gbFeatures);
            this.flowLayoutPanel1.Controls.Add(this.gbMetric);
            this.flowLayoutPanel1.Controls.Add(this.btnShowSignal);
            this.flowLayoutPanel1.Controls.Add(this.btnShowDTW);
            this.flowLayoutPanel1.Controls.Add(this.btnGenerateDB);
            this.flowLayoutPanel1.Controls.Add(this.btnRecognize);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(260, 711);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // btnRecord
            // 
            this.btnRecord.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRecord.Location = new System.Drawing.Point(13, 13);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(234, 35);
            this.btnRecord.TabIndex = 0;
            this.btnRecord.Text = "🎙️ Record Mic";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // btnLoad1
            // 
            this.btnLoad1.Location = new System.Drawing.Point(13, 54);
            this.btnLoad1.Name = "btnLoad1";
            this.btnLoad1.Size = new System.Drawing.Size(234, 30);
            this.btnLoad1.TabIndex = 1;
            this.btnLoad1.Text = "Load Wav 1 (Test)";
            this.btnLoad1.UseVisualStyleBackColor = true;
            this.btnLoad1.Click += new System.EventHandler(this.btnLoad1_Click);
            // 
            // btnLoad2
            // 
            this.btnLoad2.Location = new System.Drawing.Point(13, 90);
            this.btnLoad2.Name = "btnLoad2";
            this.btnLoad2.Size = new System.Drawing.Size(234, 30);
            this.btnLoad2.TabIndex = 2;
            this.btnLoad2.Text = "Load Wav 2 (Reference)";
            this.btnLoad2.UseVisualStyleBackColor = true;
            this.btnLoad2.Click += new System.EventHandler(this.btnLoad2_Click);
            // 
            // gbPreproc
            // 
            this.gbPreproc.Controls.Add(this.chkTrimSilence);
            this.gbPreproc.Controls.Add(this.chkNormalize);
            this.gbPreproc.Controls.Add(this.chkPreemphasis);
            this.gbPreproc.Location = new System.Drawing.Point(13, 126);
            this.gbPreproc.Name = "gbPreproc";
            this.gbPreproc.Size = new System.Drawing.Size(234, 105);
            this.gbPreproc.TabIndex = 3;
            this.gbPreproc.TabStop = false;
            this.gbPreproc.Text = "Przetwarzanie (Preprocessing)";
            // 
            // chkTrimSilence
            // 
            this.chkTrimSilence.AutoSize = true;
            this.chkTrimSilence.Checked = true;
            this.chkTrimSilence.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrimSilence.Location = new System.Drawing.Point(16, 75);
            this.chkTrimSilence.Name = "chkTrimSilence";
            this.chkTrimSilence.Size = new System.Drawing.Size(124, 19);
            this.chkTrimSilence.TabIndex = 2;
            this.chkTrimSilence.Text = "Trim Silence (VAD)";
            this.chkTrimSilence.UseVisualStyleBackColor = true;
            // 
            // chkNormalize
            // 
            this.chkNormalize.AutoSize = true;
            this.chkNormalize.Checked = true;
            this.chkNormalize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNormalize.Location = new System.Drawing.Point(16, 50);
            this.chkNormalize.Name = "chkNormalize";
            this.chkNormalize.Size = new System.Drawing.Size(115, 19);
            this.chkNormalize.TabIndex = 1;
            this.chkNormalize.Text = "Normalize Audio";
            this.chkNormalize.UseVisualStyleBackColor = true;
            // 
            // chkPreemphasis
            // 
            this.chkPreemphasis.AutoSize = true;
            this.chkPreemphasis.Checked = true;
            this.chkPreemphasis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreemphasis.Location = new System.Drawing.Point(16, 25);
            this.chkPreemphasis.Name = "chkPreemphasis";
            this.chkPreemphasis.Size = new System.Drawing.Size(95, 19);
            this.chkPreemphasis.TabIndex = 0;
            this.chkPreemphasis.Text = "Preemphasis";
            this.chkPreemphasis.UseVisualStyleBackColor = true;
            // 
            // gbFeatures
            // 
            this.gbFeatures.Controls.Add(this.rbFFT);
            this.gbFeatures.Controls.Add(this.rbMFCC);
            this.gbFeatures.Location = new System.Drawing.Point(13, 237);
            this.gbFeatures.Name = "gbFeatures";
            this.gbFeatures.Size = new System.Drawing.Size(234, 80);
            this.gbFeatures.TabIndex = 4;
            this.gbFeatures.TabStop = false;
            this.gbFeatures.Text = "Ustawienia Ekstrakcji Cech";
            // 
            // rbFFT
            // 
            this.rbFFT.AutoSize = true;
            this.rbFFT.Location = new System.Drawing.Point(16, 49);
            this.rbFFT.Name = "rbFFT";
            this.rbFFT.Size = new System.Drawing.Size(117, 19);
            this.rbFFT.TabIndex = 1;
            this.rbFFT.Text = "Widmo FFT (Raw)";
            this.rbFFT.UseVisualStyleBackColor = true;
            // 
            // rbMFCC
            // 
            this.rbMFCC.AutoSize = true;
            this.rbMFCC.Checked = true;
            this.rbMFCC.Location = new System.Drawing.Point(16, 24);
            this.rbMFCC.Name = "rbMFCC";
            this.rbMFCC.Size = new System.Drawing.Size(176, 19);
            this.rbMFCC.TabIndex = 0;
            this.rbMFCC.TabStop = true;
            this.rbMFCC.Text = "Współczynniki Melowe MFCC";
            this.rbMFCC.UseVisualStyleBackColor = true;
            // 
            // gbMetric
            // 
            this.gbMetric.Controls.Add(this.rbEuclidean);
            this.gbMetric.Controls.Add(this.rbCosine);
            this.gbMetric.Location = new System.Drawing.Point(13, 323);
            this.gbMetric.Name = "gbMetric";
            this.gbMetric.Size = new System.Drawing.Size(234, 80);
            this.gbMetric.TabIndex = 5;
            this.gbMetric.TabStop = false;
            this.gbMetric.Text = "Metryka DTW";
            // 
            // rbEuclidean
            // 
            this.rbEuclidean.AutoSize = true;
            this.rbEuclidean.Location = new System.Drawing.Point(16, 49);
            this.rbEuclidean.Name = "rbEuclidean";
            this.rbEuclidean.Size = new System.Drawing.Size(133, 19);
            this.rbEuclidean.TabIndex = 1;
            this.rbEuclidean.Text = "Euklidesowa (Koszt)";
            this.rbEuclidean.UseVisualStyleBackColor = true;
            // 
            // rbCosine
            // 
            this.rbCosine.AutoSize = true;
            this.rbCosine.Checked = true;
            this.rbCosine.Location = new System.Drawing.Point(16, 24);
            this.rbCosine.Name = "rbCosine";
            this.rbCosine.Size = new System.Drawing.Size(161, 19);
            this.rbCosine.TabIndex = 0;
            this.rbCosine.TabStop = true;
            this.rbCosine.Text = "Kosinusowa (Kąt Wektora)";
            this.rbCosine.UseVisualStyleBackColor = true;
            // 
            // btnShowSignal
            // 
            this.btnShowSignal.Location = new System.Drawing.Point(13, 409);
            this.btnShowSignal.Name = "btnShowSignal";
            this.btnShowSignal.Size = new System.Drawing.Size(234, 35);
            this.btnShowSignal.TabIndex = 6;
            this.btnShowSignal.Text = "Show Time Signals";
            this.btnShowSignal.UseVisualStyleBackColor = true;
            this.btnShowSignal.Click += new System.EventHandler(this.btnShowSignal_Click);
            // 
            // btnShowDTWa
            // 
            this.btnShowDTW.Location = new System.Drawing.Point(13, 450);
            this.btnShowDTW.Name = "btnShowDTW";
            this.btnShowDTW.Size = new System.Drawing.Size(234, 35);
            this.btnShowDTW.TabIndex = 7;
            this.btnShowDTW.Text = "Compare Wav1 vs Wav2 (DTW)";
            this.btnShowDTW.UseVisualStyleBackColor = true;
            this.btnShowDTW.Click += new System.EventHandler(this.btnShowDTW_Click);
            // 
            // btnGenerateDB
            // 
            this.btnGenerateDB.Location = new System.Drawing.Point(13, 491);
            this.btnGenerateDB.Name = "btnGenerateDB";
            this.btnGenerateDB.Size = new System.Drawing.Size(234, 35);
            this.btnGenerateDB.TabIndex = 8;
            this.btnGenerateDB.Text = "Generate Database";
            this.btnGenerateDB.UseVisualStyleBackColor = true;
            this.btnGenerateDB.Click += new System.EventHandler(this.btnGenerateDB_Click);
            // 
            // btnRecognize
            // 
            this.btnRecognize.BackColor = System.Drawing.Color.LightGreen;
            this.btnRecognize.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRecognize.Location = new System.Drawing.Point(13, 532);
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.Size = new System.Drawing.Size(234, 45);
            this.btnRecognize.TabIndex = 9;
            this.btnRecognize.Text = "🔍 RECOGNIZE WORD";
            this.btnRecognize.UseVisualStyleBackColor = false;
            this.btnRecognize.Click += new System.EventHandler(this.btnRecognize_Click);
            // 
            // panelPlot
            // 
            this.panelPlot.BackColor = System.Drawing.Color.White;
            this.panelPlot.Controls.Add(this.formsPlot1);
            this.panelPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlot.Location = new System.Drawing.Point(260, 0);
            this.panelPlot.Name = "panelPlot";
            this.panelPlot.Size = new System.Drawing.Size(824, 711);
            this.panelPlot.TabIndex = 1;
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(0, 0);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(824, 711);
            this.formsPlot1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 711);
            this.Controls.Add(this.panelPlot);
            this.Controls.Add(this.panelLeft);
            this.Name = "Form1";
            this.Text = "Speech Recognition (DTW) - Advanced Toolkit";

            this.panelLeft.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.gbPreproc.ResumeLayout(false);
            this.gbPreproc.PerformLayout();
            this.gbFeatures.ResumeLayout(false);
            this.gbFeatures.PerformLayout();
            this.gbMetric.ResumeLayout(false);
            this.gbMetric.PerformLayout();
            this.panelPlot.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Button btnLoad1;
        private System.Windows.Forms.Button btnLoad2;
        private System.Windows.Forms.GroupBox gbPreproc;
        private System.Windows.Forms.CheckBox chkTrimSilence;
        private System.Windows.Forms.CheckBox chkNormalize;
        private System.Windows.Forms.CheckBox chkPreemphasis;
        private System.Windows.Forms.GroupBox gbFeatures;
        private System.Windows.Forms.RadioButton rbFFT;
        private System.Windows.Forms.RadioButton rbMFCC;
        private System.Windows.Forms.GroupBox gbMetric;
        private System.Windows.Forms.RadioButton rbEuclidean;
        private System.Windows.Forms.RadioButton rbCosine;
        private System.Windows.Forms.Button btnShowSignal;
        private System.Windows.Forms.Button btnShowDTW;
        private System.Windows.Forms.Button btnGenerateDB;
        private System.Windows.Forms.Button btnRecognize;
        private System.Windows.Forms.Panel panelPlot;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
    }
}