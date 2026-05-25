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
            button1 = new Button();
            RecognizeButton = new Button();
            SuspendLayout();
            // 
            // btnLoad1
            // 
            btnLoad1.Location = new Point(12, 12);
            btnLoad1.Name = "btnLoad1";
            btnLoad1.Size = new Size(118, 30);
            btnLoad1.TabIndex = 0;
            btnLoad1.Text = "Load Wav 1";
            btnLoad1.UseVisualStyleBackColor = true;
            btnLoad1.Click += btnLoad1_Click;
            // 
            // btnLoad2
            // 
            btnLoad2.Location = new Point(12, 48);
            btnLoad2.Name = "btnLoad2";
            btnLoad2.Size = new Size(118, 30);
            btnLoad2.TabIndex = 2;
            btnLoad2.Text = "Load Wav 2";
            btnLoad2.UseVisualStyleBackColor = true;
            btnLoad2.Click += btnLoad2_Click;
            // 
            // btnShowSignal
            // 
            btnShowSignal.Location = new Point(12, 100);
            btnShowSignal.Name = "btnShowSignal";
            btnShowSignal.Size = new Size(118, 30);
            btnShowSignal.TabIndex = 3;
            btnShowSignal.Text = "Show Signals";
            btnShowSignal.UseVisualStyleBackColor = true;
            btnShowSignal.Click += btnShowSignal_Click;
            // 
            // btnShowDTW
            // 
            btnShowDTW.Location = new Point(12, 136);
            btnShowDTW.Name = "btnShowDTW";
            btnShowDTW.Size = new Size(118, 30);
            btnShowDTW.TabIndex = 4;
            btnShowDTW.Text = "Show DTW";
            btnShowDTW.UseVisualStyleBackColor = true;
            btnShowDTW.Click += btnShowDTW_Click;
            // 
            // formsPlot1
            // 
            formsPlot1.Location = new Point(136, 12);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(652, 426);
            formsPlot1.TabIndex = 1;
            // 
            // btnRecord
            // 
            btnRecord.Location = new Point(12, 172);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new Size(118, 30);
            btnRecord.TabIndex = 5;
            btnRecord.Text = "🎙️ Record Mic";
            btnRecord.UseVisualStyleBackColor = true;
            btnRecord.Click += btnRecord_Click;
            // 
            // buttonNorm
            // 
            buttonNorm.Location = new Point(12, 208);
            buttonNorm.Name = "buttonNorm";
            buttonNorm.Size = new Size(118, 30);
            buttonNorm.TabIndex = 6;
            buttonNorm.Text = "Normalize Audio";
            buttonNorm.UseVisualStyleBackColor = true;
            buttonNorm.Click += buttonNorm_Click;
            // 
            // buttonTrim
            // 
            buttonTrim.Location = new Point(12, 244);
            buttonTrim.Name = "buttonTrim";
            buttonTrim.Size = new Size(118, 30);
            buttonTrim.TabIndex = 7;
            buttonTrim.Text = "Trim Silence";
            buttonTrim.UseVisualStyleBackColor = true;
            buttonTrim.Click += buttonTrim_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 334);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 8;
            button1.Text = "temp";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // RecognizeButton
            // 
            RecognizeButton.Location = new Point(12, 376);
            RecognizeButton.Name = "RecognizeButton";
            RecognizeButton.Size = new Size(75, 23);
            RecognizeButton.TabIndex = 9;
            RecognizeButton.Text = "Recognize";
            RecognizeButton.UseVisualStyleBackColor = true;
            RecognizeButton.Click += RecognizeButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(RecognizeButton);
            Controls.Add(button1);
            Controls.Add(buttonTrim);
            Controls.Add(buttonNorm);
            Controls.Add(btnShowDTW);
            Controls.Add(btnShowSignal);
            Controls.Add(btnLoad2);
            Controls.Add(formsPlot1);
            Controls.Add(btnLoad1);
            Controls.Add(btnRecord);
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
        private Button button1;
        private Button RecognizeButton;
    }
}
