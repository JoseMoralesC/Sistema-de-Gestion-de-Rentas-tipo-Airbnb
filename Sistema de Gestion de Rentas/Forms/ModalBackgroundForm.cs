using System.Drawing;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class ModalBackgroundForm : Form
    {
        public ModalBackgroundForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
            ShowInTaskbar = false;

            BackColor = Color.Black;
            Opacity = 0.5;

            // IMPORTANTE: evitar cerrar al hacer clic
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
    }
}
