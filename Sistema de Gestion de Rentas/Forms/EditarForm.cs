using Sistema_de_Gestion_de_Rentas.Controls;
using Sistema_de_Gestion_de_Rentas.Services;
using Sistema_de_Gestion_de_Rentas.Data;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public partial class EditarForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);

        private Label lblTitulo;
        private Button btnCerrar;
        private Button btnGuardar;

        // Campos del formulario
        private TextBox txtIdentificacion;
        private TextBox txtUsuario;
        private TextBox txtContrasena;
        private TextBox txtNombre;
        private TextBox txtPrimerApellido;
        private TextBox txtSegundoApellido;
        private TextBox txtCorreo;
        private TextBox txtTelefono;
        private TextBox txtPaisOrigen;

        private Huesped usuario;

        public EditarForm()
        {
            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(600, 600);
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void InicializarControles()
        {
            lblTitulo = new Label
            {
                Text = "Editar Perfil",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };
            EstilosUI.AplicarEstiloLabel(lblTitulo);
            Controls.Add(lblTitulo);

            usuario = ObtenerUsuarioActual();

            int top = 80;
            int spacing = 40;

            txtIdentificacion = CrearTextBox("Identificación:", usuario.Identificacion, top, false); top += spacing;
            txtUsuario = CrearTextBox("Usuario:", usuario.Usuario, top); top += spacing;
            txtContrasena = CrearTextBox("Contraseña:", usuario.Contrasena, top); top += spacing;
            txtNombre = CrearTextBox("Nombre:", usuario.Nombre, top); top += spacing;
            txtPrimerApellido = CrearTextBox("Primer Apellido:", usuario.PrimerApellido, top); top += spacing;
            txtSegundoApellido = CrearTextBox("Segundo Apellido:", usuario.SegundoApellido, top); top += spacing;
            txtCorreo = CrearTextBox("Correo:", usuario.Correo, top); top += spacing;
            txtTelefono = CrearTextBox("Teléfono:", usuario.Telefono, top); top += spacing;
            txtPaisOrigen = CrearTextBox("País de Origen:", usuario.PaisOrigen, top); top += spacing;

            btnGuardar = new Button
            {
                Text = "Guardar",
                Width = 120,
                Height = 40,
                Location = new Point((this.ClientSize.Width - 250) / 2, this.ClientSize.Height - 70)
            };
            EstilosUI.AplicarEstiloBoton(btnGuardar);
            btnGuardar.Click += BtnGuardar_Click;
            Controls.Add(btnGuardar);

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Width = 120,
                Height = 40,
                Location = new Point((this.ClientSize.Width + 10) / 2, this.ClientSize.Height - 70)
            };
            EstilosUI.AplicarEstiloBoton(btnCerrar);
            btnCerrar.Click += (s, e) => this.Close();
            Controls.Add(btnCerrar);
        }

        private TextBox CrearTextBox(string labelText, string valor, int top, bool editable = true)
        {
            Label lbl = new Label
            {
                Text = labelText,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                Location = new Point(40, top),
                Size = new Size(200, 25)
            };
            Controls.Add(lbl);

            TextBox txt = new TextBox
            {
                Text = valor,
                Location = new Point(240, top),
                Size = new Size(300, 25),
                Enabled = editable
            };
            Controls.Add(txt);

            return txt;
        }

        private Huesped ObtenerUsuarioActual()
        {
            var usuarioLogueado = SesionUsuario.Usuario;

            if (string.IsNullOrEmpty(usuarioLogueado))
                throw new Exception("No hay usuario logueado.");

            var huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuarioLogueado);

            if (huesped == null)
                throw new Exception("No se encontró información del usuario.");

            return huesped;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                CustomMessageBoxForm.MostrarOpciones("Nombre y correo son campos obligatorios.", "Aceptar", "");
                return;
            }

            usuario.Usuario = txtUsuario.Text;
            usuario.Contrasena = txtContrasena.Text;
            usuario.Nombre = txtNombre.Text;
            usuario.PrimerApellido = txtPrimerApellido.Text;
            usuario.SegundoApellido = txtSegundoApellido.Text;
            usuario.Correo = txtCorreo.Text;
            usuario.Telefono = txtTelefono.Text;
            usuario.PaisOrigen = txtPaisOrigen.Text;

            bool exito = HuespedDAO.ActualizarHuesped(usuario);

            if (exito)
            {
                CustomMessageBoxForm.MostrarOpciones("Datos actualizados correctamente.", "Aceptar", "");
                this.Close(); 
            }
            else
            {
                CustomMessageBoxForm.MostrarOpciones("Ocurrió un error al actualizar los datos.", "Aceptar", "");
            }
        }

    }
}
