using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class ModalBackgroundForm : Form
    {
        // Propiedades configurables
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
            // Configuración de la ventana
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
            ShowInTaskbar = false;

            BackColor = FondoColor;
            Opacity = Opacidad;

            // Evitar que se cierre al hacer clic
            this.Click += (s, e) => { /* No hacer nada */ };
        }

        // Evita que se cierre al presionar Esc
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                return true; // Ignora la tecla
            }
            return base.ProcessDialogKey(keyData);
        }

        // Método para cerrar el formulario manualmente
        public void CerrarFormulario()
        {
            this.Close();
        }
    }
}
