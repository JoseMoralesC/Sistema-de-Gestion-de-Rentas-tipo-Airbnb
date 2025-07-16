using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class ModalBackgroundForm : Form
    {
        
        public Color FondoColor { get; set; } = Color.Black;
        public double Opacidad { get; set; } = 0.5;

        public ModalBackgroundForm()
        {
            InitializeForm();
        }

        public ModalBackgroundForm(Color fondoColor, double opacidad = 0.5)
        {
            FondoColor = fondoColor;
            Opacidad = opacidad;
            InitializeForm();
        }

        private void InitializeForm()
        {
            
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
            ShowInTaskbar = false;

            BackColor = FondoColor;
            Opacity = Opacidad;

            
            this.Click += (s, e) => { /* No hacer nada */ };
        }


        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                return true; 
            }
            return base.ProcessDialogKey(keyData);
        }

        
        public void CerrarFormulario()
        {
            this.Close();
        }
    }
}
