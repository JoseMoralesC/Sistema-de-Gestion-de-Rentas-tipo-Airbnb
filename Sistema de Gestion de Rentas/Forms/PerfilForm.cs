using Sistema_de_Gestion_de_Rentas.Controls;

using Sistema_de_Gestion_de_Rentas.Services;
using Sistema_de_Gestion_de_Rentas.Data;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public partial class PerfilForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);

        private Label lblTitulo;
        private Label lblNombre;
        private Label lblCorreo;
        private Label lblTelefono;
        private Button btnCerrar;

        public PerfilForm()
        {
            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(30, 30, 40); // Fondo oscuro
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(600, 400);
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void InicializarControles()
        {
            lblTitulo = new Label
            {
                Text = "Mi Perfil",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };
            EstilosUI.AplicarEstiloLabel(lblTitulo);
            Controls.Add(lblTitulo);

            // Obtener datos del usuario actual desde SesionUsuario
            Huesped usuario = ObtenerUsuarioActual();

            lblNombre = CrearLabel($"Nombre: {usuario.Nombre} {usuario.PrimerApellido} {usuario.SegundoApellido}", 100);
            lblCorreo = CrearLabel($"Correo: {usuario.Correo ?? "No proporcionado"}", 150);
            lblTelefono = CrearLabel($"Teléfono: {usuario.Telefono ?? "No proporcionado"}", 200);

            Controls.Add(lblNombre);
            Controls.Add(lblCorreo);
            Controls.Add(lblTelefono);

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Width = 120,
                Height = 40,
                Location = new Point((this.ClientSize.Width - 120) / 2, this.ClientSize.Height - 70)
            };
            EstilosUI.AplicarEstiloBoton(btnCerrar);
            btnCerrar.Click += (s, e) => this.Close();
            Controls.Add(btnCerrar);
        }

        private Label CrearLabel(string texto, int top)
        {
            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = this.ClientSize.Width - 80,
                Height = 30,
                Location = new Point(40, top)
            };
            EstilosUI.AplicarEstiloLabel(lbl);
            return lbl;
        }

        // Método auxiliar para obtener el usuario actual completo desde SesionUsuario
        private Huesped ObtenerUsuarioActual()
        {
            // Suponiendo que SesionUsuario.Usuario contiene el nombre de usuario
            var usuario = SesionUsuario.Usuario;

            if (string.IsNullOrEmpty(usuario))
                throw new Exception("No hay usuario logueado.");

            // Obtener el usuario completo desde DAO
            Huesped huesped = HuespedDAO.ObtenerHuespedPorUsuario(usuario);

            if (huesped == null)
                throw new Exception("No se encontró información del usuario.");

            return huesped;
        }
    }
}
