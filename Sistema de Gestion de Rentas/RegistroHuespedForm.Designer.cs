namespace Sistema_de_Gestion_de_Rentas
{
    partial class RegistroHuespedForm
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
            SuspendLayout();
            // 
            // RegistroHuespedForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            Name = "RegistroHuespedForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Registro de Huéspedes";
            WindowState = FormWindowState.Maximized;
            Load += RegistroHuespedForm_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}