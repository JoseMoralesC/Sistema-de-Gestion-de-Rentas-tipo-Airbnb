using System;
using System.Drawing;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Services;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class LoginForm : Form
    {
        private TextBox txtUsuario, txtContrasena;
        private Button btnIngresar, btnRegistrar;

        public LoginForm()
        {
            Text = "Iniciar Sesión";
            Size = new Size(400, 280);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblUsuario = new Label
            {
                Text = "Usuario:",
                Location = new Point(40, 40),
                Size = new Size(80, 30)
            };
            Controls.Add(lblUsuario);

            txtUsuario = new TextBox
            {
                Location = new Point(130, 40),
                Size = new Size(200, 30)
            };
            Controls.Add(txtUsuario);

            Label lblContrasena = new Label
            {
                Text = "Contraseña:",
                Location = new Point(40, 90),
                Size = new Size(80, 30)
            };
            Controls.Add(lblContrasena);

            txtContrasena = new TextBox
            {
                Location = new Point(130, 90),
                Size = new Size(200, 30),
                UseSystemPasswordChar = true
            };
            Controls.Add(txtContrasena);

            btnIngresar = new Button
            {
                Text = "Ingresar",
                Location = new Point(130, 140),
                Size = new Size(100, 35)
            };
            btnIngresar.Click += BtnIngresar_Click;
            Controls.Add(btnIngresar);

            btnRegistrar = new Button
            {
                Text = "Registrarse",
                Location = new Point(240, 140),
                Size = new Size(100, 35)
            };
            btnRegistrar.Click += BtnRegistrar_Click;
            Controls.Add(btnRegistrar);
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            bool loginExitoso = HuespedService.VerificarCredenciales(usuario, contrasena);

            if (loginExitoso)
            {
                MessageBox.Show("Inicio de sesión exitoso.");
                // Aquí puedes abrir el formulario principal o dashboard
                this.Close(); // Cierra el formulario de login
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            var registroForm = new RegistroHuespedForm();
            registroForm.ShowDialog();
        }
    }
}
