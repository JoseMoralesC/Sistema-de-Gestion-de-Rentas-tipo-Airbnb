using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class PanelHuespedForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private Button btnCerrarSesion;

        public PanelHuespedForm()
        {
            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            // Opcional: pantalla completa, sin bordes
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
        }

        private void InicializarControles()
        {
            Label lblBienvenida = new Label
            {
                Text = "Bienvenido al Panel de Usuario",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 45, 48),
                AutoSize = true,
                Location = new Point(
                    (Screen.PrimaryScreen.Bounds.Width - 600) / 2, 100)
            };
            Controls.Add(lblBienvenida);

            btnCerrarSesion = new Button
            {
                Text = "Cerrar Sesión",
                Size = new Size(200, 50),
                Font = new Font("Arial", 12, FontStyle.Regular),
                Location = new Point(50, 50),
                BackColor = Color.FromArgb(220, 20, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;
            Controls.Add(btnCerrarSesion);
        }

        private void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Volver a InicioForm
            this.Hide();
            var inicioForm = new InicioForm();
            inicioForm.Show();
            this.Close();
        }
    }
}
