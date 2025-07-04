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
            // Configuración de la ventana
            this.FormBorderStyle = FormBorderStyle.None;  // Eliminar la barra de título
            this.BackColor = Color.FromArgb(50, 50, 50);  // Fondo gris oscuro
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar el formulario
            this.Size = new Size(450, 250); // Establecer tamaño rectangular (ancho más grande que alto)

            // Aplicar bordes redondeados a la ventana
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 30, 30)); // Esquinas redondeadas

            // Etiqueta Usuario
            Label lblUsuario = new Label
            {
                Text = "Usuario:",
                Location = new Point(40, 50),
                Size = new Size(100, 30),
                ForeColor = Color.White // Cambiar color de texto a blanco
            };
            EstilosUI.AplicarEstiloLabel(lblUsuario);
            Controls.Add(lblUsuario);

            // Campo de texto Usuario
            txtUsuario = new TextBox
            {
                Location = new Point(150, 50),
                Size = new Size(230, 35)
            };
            EstilosUI.AplicarEstiloTextBox(txtUsuario);
            Controls.Add(txtUsuario);

            // Etiqueta Contraseña
            Label lblContrasena = new Label
            {
                Text = "Contraseña:",
                Location = new Point(40, 100),
                Size = new Size(100, 30),
                ForeColor = Color.White // Cambiar color de texto a blanco
            };
            EstilosUI.AplicarEstiloLabel(lblContrasena);
            Controls.Add(lblContrasena);

            // Campo de texto Contraseña
            txtContrasena = new TextBox
            {
                Location = new Point(150, 100),
                Size = new Size(230, 35),
                UseSystemPasswordChar = true
            };
            EstilosUI.AplicarEstiloTextBox(txtContrasena);
            Controls.Add(txtContrasena);

            // Botón Ingresar
            btnIngresar = new Button
            {
                Text = "Ingresar",
                Size = new Size(200, 50),  // Hacer el botón más grande
                Location = new Point((Width - 420) / 2, 170)  // Centrar el botón
            };
            EstilosUI.AplicarEstiloBoton(btnIngresar);
            btnIngresar.Click += BtnIngresar_Click;
            Controls.Add(btnIngresar);

            // Botón Cerrar
            btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = new Size(200, 50),  // Hacer el botón más grande
                Location = new Point(btnIngresar.Right + 10, 170)  // Colocar al lado del botón Ingresar
            };
            EstilosUI.AplicarEstiloBoton(btnCerrar);
            btnCerrar.Click += BtnCerrar_Click;  // Evento para cerrar el LoginForm
            Controls.Add(btnCerrar);
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            try
            {
                // Verificar las credenciales y obtener el rol en una sola operación
                string rol = HuespedService.VerificarCredencialesYObtenerRol(usuario, contrasena);

                if (rol != null)
                {
                    CustomMessageBoxForm.MostrarOpciones("Inicio de sesión exitoso.", "Aceptar", "");

                    if (rol.ToLower() == "admin")
                    {
                        // Obtener el nombre del admin desde la base de datos
                        Huesped admin = HuespedDAO.ObtenerHuespedPorUsuario(usuario);
                        string nombreAdmin = admin != null ? admin.Nombre : "Admin"; // En caso de error, se muestra "Admin"

                        // Abre el formulario de opciones para el Admin
                        var opcionAdminForm = new OpcionAdminForm(nombreAdmin);
                        opcionAdminForm.Show();
                        this.Close(); // Cierra el formulario de login
                    }
                    else
                    {
                        var panelUsuario = new PanelHuespedForm();
                        panelUsuario.Show();
                        this.Close(); // Cierra el formulario de login
                    }
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

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            // Cerrar solo el LoginForm, pero mantener abierto el InicioForm
            this.Close();
        }
    }
}
