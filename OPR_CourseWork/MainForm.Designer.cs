namespace OPR_CourseWork
{
    partial class MainForm
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonReadMatrix = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.richTextBoxInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // buttonReadMatrix
            // 
            this.buttonReadMatrix.Location = new System.Drawing.Point(14, 558);
            this.buttonReadMatrix.Margin = new System.Windows.Forms.Padding(5);
            this.buttonReadMatrix.Name = "buttonReadMatrix";
            this.buttonReadMatrix.Size = new System.Drawing.Size(687, 38);
            this.buttonReadMatrix.TabIndex = 0;
            this.buttonReadMatrix.Text = "Прочитать матрицу из файла и найти контур";
            this.buttonReadMatrix.UseVisualStyleBackColor = true;
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(14, 324);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.Size = new System.Drawing.Size(687, 226);
            this.richTextBox.TabIndex = 1;
            this.richTextBox.Text = "";
            // 
            // richTextBoxInfo
            // 
            this.richTextBoxInfo.Location = new System.Drawing.Point(14, 12);
            this.richTextBoxInfo.Name = "richTextBoxInfo";
            this.richTextBoxInfo.ReadOnly = true;
            this.richTextBoxInfo.Size = new System.Drawing.Size(687, 306);
            this.richTextBoxInfo.TabIndex = 2;
            this.richTextBoxInfo.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(715, 610);
            this.Controls.Add(this.richTextBoxInfo);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.buttonReadMatrix);
            this.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Решение задачи коммивояжера методов ветвей и границ";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenFileDialog openFileDialog;
        private Button buttonReadMatrix;
        private RichTextBox richTextBox;
        private RichTextBox richTextBoxInfo;
    }
}