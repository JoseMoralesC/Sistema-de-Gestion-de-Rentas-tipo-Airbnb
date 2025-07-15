using Sistema_de_Gestion_de_Rentas.Data;
using Sistema_de_Gestion_de_Rentas.Services;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class LoginForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private TextBox txtUsuario, txtContrasena;
        private Button btnIngresar, btnCerrar;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(50, 50, 50);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(450, 250);
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 30, 30));

            Label lblUsuario = new Label
            {
                Text = "Usuario:",
                Location = new Point(40, 50),
                Size = new Size(100, 30),
                ForeColor = Color.White
            };
            EstilosUI.AplicarEstiloLabel(lblUsuario);
            Controls.Add(lblUsuario);

            txtUsuario = new TextBox
            {
                Location = new Point(150, 50),
                Size = new Size(230, 35)
            };
            EstilosUI.AplicarEstiloTextBox(txtUsuario);
            Controls.Add(txtUsuario);

            Label lblContrasena = new Label
            {
                Text = "Contraseña:",
                Location = new Point(40, 100),
                Size = new Size(100, 30),
                ForeColor = Color.White
            };
            EstilosUI.AplicarEstiloLabel(lblContrasena);
            Controls.Add(lblContrasena);

            txtContrasena = new TextBox
            {
                Location = new Point(150, 100),
                Size = new Size(230, 35),
                UseSystemPasswordChar = true
            };
            EstilosUI.AplicarEstiloTextBox(txtContrasena);
            Controls.Add(txtContrasena);

            btnIngresar = new Button
            {
                Text = "Ingresar",
                Size = new Size(200, 50),
                Location = new Point((Width - 420) / 2, 170)
            };
            EstilosUI.AplicarEstiloBoton(btnIngresar);
            btnIngresar.Click += BtnIngresar_Click;
            Controls.Add(btnIngresar);

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = new Size(200, 50),
                Location = new Point(btnIngresar.Right + 10, 170)
            };
            EstilosUI.AplicarEstiloBoton(btnCerrar);
            btnCerrar.Click += (s, e) => this.Close();
            Controls.Add(btnCerrar);
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            try
            {
                string rol = HuespedService.VerificarCredencialesYObtenerRol(usuario, contrasena);

                if (rol != null)
                {
                    CustomMessageBoxForm.MostrarOpciones("Inicio de sesión exitoso.", "Aceptar", "");

                    Huesped huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuario);

                    if (huesped == null)
                    {
                        CustomMessageBoxForm.MostrarOpciones("No se pudo cargar el perfil del usuario.", "Aceptar", "");
                        return;
                    }

                    // Ajustar sesión con los datos del huésped
                    SesionUsuario.Id = huesped.Identificacion; // string identificacion
                    SesionUsuario.Usuario = huesped.Usuario;
                    SesionUsuario.Nombre = huesped.Nombre;
                    SesionUsuario.PrimerApellido = huesped.PrimerApellido;
                    SesionUsuario.SegundoApellido = huesped.SegundoApellido;
                    SesionUsuario.Correo = huesped.Correo;
                    SesionUsuario.Telefono = huesped.Telefono;
                    SesionUsuario.PaisOrigen = huesped.PaisOrigen;
                    SesionUsuario.Rol = huesped.Rol;

                    if (rol.ToLower() == "admin")
                    {
                        var opcionAdminForm = new OpcionAdminForm(huesped.Nombre);
                        opcionAdminForm.Show();
                    }
                    else
                    {
                        var panelUsuario = new PanelHuespedForm();
                        panelUsuario.Show();
                    }

                    this.Close();
                }
                else
                {
                    CustomMessageBoxForm.MostrarOpciones("Usuario o contraseña incorrectos.", "Aceptar", "");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.MostrarOpciones("Error al iniciar sesión:\n" + ex.Message, "Aceptar", "");
            }
        }
    }
}
