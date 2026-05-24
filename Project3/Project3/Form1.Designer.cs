namespace Project3
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnLoad1 = new Button();
            btnLoad2 = new Button();
            btnShowSignal = new Button();
            btnShowDTW = new Button();
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            btnRecord = new Button();
            buttonNorm = new Button();
            buttonTrim = new Button();
            SuspendLayout();
            // 
            // btnLoad1
            // 
            btnLoad1.Location = new Point(21, 24);
            btnLoad1.Margin = new Padding(5, 6, 5, 6);
            btnLoad1.Name = "btnLoad1";
            btnLoad1.Size = new Size(202, 60);
            btnLoad1.TabIndex = 0;
            btnLoad1.Text = "Load Wav 1";
            btnLoad1.UseVisualStyleBackColor = true;
            btnLoad1.Click += btnLoad1_Click;
            // 
            // btnLoad2
            // 
            btnLoad2.Location = new Point(21, 96);
            btnLoad2.Margin = new Padding(5, 6, 5, 6);
            btnLoad2.Name = "btnLoad2";
            btnLoad2.Size = new Size(202, 60);
            btnLoad2.TabIndex = 2;
            btnLoad2.Text = "Load Wav 2";
            btnLoad2.UseVisualStyleBackColor = true;
            btnLoad2.Click += btnLoad2_Click;
            // 
            // btnShowSignal
            // 
            btnShowSignal.Location = new Point(21, 200);
            btnShowSignal.Margin = new Padding(5, 6, 5, 6);
            btnShowSignal.Name = "btnShowSignal";
            btnShowSignal.Size = new Size(202, 60);
            btnShowSignal.TabIndex = 3;
            btnShowSignal.Text = "Show Signals";
            btnShowSignal.UseVisualStyleBackColor = true;
            btnShowSignal.Click += btnShowSignal_Click;
            // 
            // btnShowDTW
            // 
            btnShowDTW.Location = new Point(21, 272);
            btnShowDTW.Margin = new Padding(5, 6, 5, 6);
            btnShowDTW.Name = "btnShowDTW";
            btnShowDTW.Size = new Size(202, 60);
            btnShowDTW.TabIndex = 4;
            btnShowDTW.Text = "Show DTW";
            btnShowDTW.UseVisualStyleBackColor = true;
            btnShowDTW.Click += btnShowDTW_Click;
            // 
            // formsPlot1
            // 
            formsPlot1.Location = new Point(233, 24);
            formsPlot1.Margin = new Padding(5, 6, 5, 6);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(1118, 852);
            formsPlot1.TabIndex = 1;
            // 
            // btnRecord
            // 
            btnRecord.Location = new Point(21, 344);
            btnRecord.Margin = new Padding(5, 6, 5, 6);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new Size(202, 60);
            btnRecord.TabIndex = 5;
            btnRecord.Text = "🎙️ Record Mic";
            btnRecord.UseVisualStyleBackColor = true;
            btnRecord.Click += btnRecord_Click;
            // 
            // buttonNorm
            // 
            buttonNorm.Location = new Point(21, 416);
            buttonNorm.Margin = new Padding(5, 6, 5, 6);
            buttonNorm.Name = "buttonNorm";
            buttonNorm.Size = new Size(202, 60);
            buttonNorm.TabIndex = 6;
            buttonNorm.Text = "Normalize Audio";
            buttonNorm.UseVisualStyleBackColor = true;
            buttonNorm.Click += buttonNorm_Click;
            // 
            // buttonTrim
            // 
            buttonTrim.Location = new Point(21, 488);
            buttonTrim.Margin = new Padding(5, 6, 5, 6);
            buttonTrim.Name = "buttonTrim";
            buttonTrim.Size = new Size(202, 60);
            buttonTrim.TabIndex = 7;
            buttonTrim.Text = "Trim Silence";
            buttonTrim.UseVisualStyleBackColor = true;
            buttonTrim.Click += buttonTrim_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1371, 900);
            Controls.Add(buttonTrim);
            Controls.Add(buttonNorm);
            Controls.Add(btnShowDTW);
            Controls.Add(btnShowSignal);
            Controls.Add(btnLoad2);
            Controls.Add(formsPlot1);
            Controls.Add(btnLoad1);
            Controls.Add(btnRecord);
            Margin = new Padding(5, 6, 5, 6);
            Name = "Form1";
            Text = "DTW Analyzer";
            ResumeLayout(false);
        }

        #endregion

        private Button btnLoad1;
        private Button btnLoad2;
        private Button btnShowSignal;
        private Button btnShowDTW;
        private Button btnRecord;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Button buttonNorm;
        private Button buttonTrim;
    }
}
