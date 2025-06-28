using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class InicioForm : Form
    {
        private Button btnIniciarSesion;
        private Button btnRegistrarse;

        public InicioForm()
        {
            Text = "Bienvenido al Sistema de Gestión de Rentas";
            Size = new Size(500, 300);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblTitulo = new Label
            {
                Text = "¡Bienvenido!",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(140, 30),
                AutoSize = true
            };
            Controls.Add(lblTitulo);

            btnIniciarSesion = new Button
            {
                Text = "Iniciar Sesión",
                Location = new Point(170, 100),
                Size = new Size(150, 40)
            };
            btnIniciarSesion.Click += BtnIniciarSesion_Click;
            Controls.Add(btnIniciarSesion);

            btnRegistrarse = new Button
            {
                Text = "Registrarse",
                Location = new Point(170, 160),
                Size = new Size(150, 40)
            };
            btnRegistrarse.Click += BtnRegistrarse_Click;
            Controls.Add(btnRegistrarse);
        }

        private void BtnIniciarSesion_Click(object sender, EventArgs e)
        {
            // Aquí cargaremos el login (lo haremos en el siguiente paso)
            var loginForm = new LoginForm();
            loginForm.Show();
            this.Hide(); // Ocultamos esta ventana
        }

        private void BtnRegistrarse_Click(object sender, EventArgs e)
        {
            var registroForm = new RegistroHuespedForm();
            registroForm.Show();
            this.Hide(); // Ocultamos esta ventana
        }
    }
}
