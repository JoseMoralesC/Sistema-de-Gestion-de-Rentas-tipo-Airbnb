using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Services;
using Timer = System.Windows.Forms.Timer;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class LoginForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private TextBox txtUsuario, txtContrasena;
        private Button btnIngresar, btnRegistrar;
        private Timer fadeInTimer;

        public LoginForm()
        {
            ConfigurarFormulario();
            InicializarControles();
            AplicarEstilos();
            IniciarFadeIn();
        }

        private void ConfigurarFormulario()
        {
            Text = "";
            Size = new Size(450, 320);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(45, 45, 48);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void InicializarControles()
        {
            Label lblUsuario = new Label
            {
                Text = "Usuario:",
                Location = new Point(40, 50),
                Size = new Size(100, 30)
            };
            EstilosUI.AplicarEstiloLabel(lblUsuario);
            Controls.Add(lblUsuario);

            txtUsuario = new TextBox
            {
                Location = new Point(150, 50),
                Size = new Size(230, 35)
            };
            Controls.Add(txtUsuario);
            EstilosUI.AplicarEstiloTextBox(txtUsuario);

            Label lblContrasena = new Label
            {
                Text = "Contraseña:",
                Location = new Point(40, 100),
                Size = new Size(100, 30)
            };
            EstilosUI.AplicarEstiloLabel(lblContrasena);
            Controls.Add(lblContrasena);

            txtContrasena = new TextBox
            {
                Location = new Point(150, 100),
                Size = new Size(230, 35),
                UseSystemPasswordChar = true
            };
            Controls.Add(txtContrasena);
            EstilosUI.AplicarEstiloTextBox(txtContrasena);

            btnIngresar = new Button
            {
                Text = "Ingresar",
                Size = new Size(160, 45),
                Location = new Point((Width - 360) / 2 + 10, 170)
            };
            btnIngresar.Click += BtnIngresar_Click;
            Controls.Add(btnIngresar);

            btnRegistrar = new Button
            {
                Text = "Registrarse",
                Size = new Size(160, 45),
                Location = new Point(btnIngresar.Right + 10, 170)
            };
            btnRegistrar.Click += BtnRegistrar_Click;
            Controls.Add(btnRegistrar);
        }

        private void AplicarEstilos()
        {
            EstilosUI.AplicarEstiloBoton(btnIngresar);
            EstilosUI.AplicarEstiloBoton(btnRegistrar);
        }

        private void IniciarFadeIn()
        {
            Opacity = 0;
            fadeInTimer = new Timer { Interval = 10 };
            fadeInTimer.Tick += (s, e) =>
            {
                if (Opacity < 1)
                    Opacity += 0.05;
                else
                    fadeInTimer.Stop();
            };
            fadeInTimer.Start();
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            bool loginExitoso = HuespedService.VerificarCredenciales(usuario, contrasena);

            if (loginExitoso)
            {
                MessageBox.Show("Inicio de sesión exitoso.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            var background = new ModalBackgroundForm
            {
                StartPosition = FormStartPosition.Manual,
                Size = this.ClientSize,
                Location = this.PointToScreen(Point.Empty),
                Owner = this,
                TopMost = false
            };
            background.Show();

            var registroForm = new RegistroHuespedForm
            {
                StartPosition = FormStartPosition.CenterParent,
                Owner = this
            };

            registroForm.FormClosed += (s, args) =>
            {
                background.Close();
            };

            registroForm.ShowDialog(this);
        }
    }
}
