namespace FreiPost.DemoGui
{
    partial class Form1
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.massLabel = new System.Windows.Forms.Label();
            this.tare_button = new System.Windows.Forms.Button();
            this.tareReset_button = new System.Windows.Forms.Button();
            this.calibrateOffset_button = new System.Windows.Forms.Button();
            this.calibrateMult_button = new System.Windows.Forms.Button();
            this.read_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(90, 37);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(528, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(90, 120);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(528, 23);
            this.progressBar2.TabIndex = 1;
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(90, 198);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(528, 23);
            this.progressBar3.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(624, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(624, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "label5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(624, 208);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "label6";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(46, 250);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 31);
            this.label7.TabIndex = 9;
            this.label7.Text = "Mass:";
            // 
            // massLabel
            // 
            this.massLabel.AutoSize = true;
            this.massLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.massLabel.Location = new System.Drawing.Point(169, 250);
            this.massLabel.Name = "massLabel";
            this.massLabel.Size = new System.Drawing.Size(65, 31);
            this.massLabel.TabIndex = 10;
            this.massLabel.Text = "0 kg";
            // 
            // tare_button
            // 
            this.tare_button.Location = new System.Drawing.Point(360, 276);
            this.tare_button.Name = "tare_button";
            this.tare_button.Size = new System.Drawing.Size(75, 23);
            this.tare_button.TabIndex = 11;
            this.tare_button.Text = "Tare";
            this.tare_button.UseVisualStyleBackColor = true;
            this.tare_button.Click += new System.EventHandler(this.tare_button_Click);
            // 
            // tareReset_button
            // 
            this.tareReset_button.Location = new System.Drawing.Point(360, 305);
            this.tareReset_button.Name = "tareReset_button";
            this.tareReset_button.Size = new System.Drawing.Size(75, 23);
            this.tareReset_button.TabIndex = 12;
            this.tareReset_button.Text = "Reset Tare";
            this.tareReset_button.UseVisualStyleBackColor = true;
            this.tareReset_button.Click += new System.EventHandler(this.tareReset_button_Click);
            // 
            // calibrateOffset_button
            // 
            this.calibrateOffset_button.Location = new System.Drawing.Point(441, 276);
            this.calibrateOffset_button.Name = "calibrateOffset_button";
            this.calibrateOffset_button.Size = new System.Drawing.Size(147, 23);
            this.calibrateOffset_button.TabIndex = 13;
            this.calibrateOffset_button.Text = "Calibrate Offset";
            this.calibrateOffset_button.UseVisualStyleBackColor = true;
            this.calibrateOffset_button.Click += new System.EventHandler(this.calibrateOffset_button_Click);
            // 
            // calibrateMult_button
            // 
            this.calibrateMult_button.Location = new System.Drawing.Point(441, 305);
            this.calibrateMult_button.Name = "calibrateMult_button";
            this.calibrateMult_button.Size = new System.Drawing.Size(147, 23);
            this.calibrateMult_button.TabIndex = 14;
            this.calibrateMult_button.Text = "Calibrate Multiplier";
            this.calibrateMult_button.UseVisualStyleBackColor = true;
            this.calibrateMult_button.Click += new System.EventHandler(this.calibrateMult_button_Click);
            // 
            // read_button
            // 
            this.read_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.read_button.Location = new System.Drawing.Point(52, 284);
            this.read_button.Name = "read_button";
            this.read_button.Size = new System.Drawing.Size(167, 44);
            this.read_button.TabIndex = 15;
            this.read_button.Text = "READ";
            this.read_button.UseVisualStyleBackColor = true;
            this.read_button.Click += new System.EventHandler(this.read_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 356);
            this.Controls.Add(this.read_button);
            this.Controls.Add(this.calibrateMult_button);
            this.Controls.Add(this.calibrateOffset_button);
            this.Controls.Add(this.tareReset_button);
            this.Controls.Add(this.tare_button);
            this.Controls.Add(this.massLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label massLabel;
        private System.Windows.Forms.Button tare_button;
        private System.Windows.Forms.Button tareReset_button;
        private System.Windows.Forms.Button calibrateOffset_button;
        private System.Windows.Forms.Button calibrateMult_button;
        private System.Windows.Forms.Button read_button;
    }
}

