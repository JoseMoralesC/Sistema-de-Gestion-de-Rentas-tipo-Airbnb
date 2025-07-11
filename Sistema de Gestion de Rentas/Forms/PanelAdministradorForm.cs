using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class PanelAdministradorForm : Form
    {
        private Button btnCerrarSesion;
        private Button btnGestionarProvincias;
        private Button btnGestionarHospedajes;
        private Button btnGestionarReservas;

        public PanelAdministradorForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            string rutaFondo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Inicio_Fondo.jpg");
            if (File.Exists(rutaFondo))
            {
                this.BackgroundImage = Image.FromFile(rutaFondo);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            Size tamañoBoton = new Size(300, 60);
            int borderRadius = 15;
            int spacing = 80;
            int inicioY = 150;
            int centroX = (Screen.PrimaryScreen.Bounds.Width - tamañoBoton.Width) / 2;

            btnCerrarSesion = CrearBoton("Cerrar Sesión", centroX, inicioY);
            btnCerrarSesion.BackColor = Color.FromArgb(220, 20, 60);
            btnCerrarSesion.Click += BtnCerrarSesion_Click;
            Controls.Add(btnCerrarSesion);
            AplicarBordesRedondeados(btnCerrarSesion, borderRadius);

            btnGestionarProvincias = CrearBoton("Gestionar Provincias", centroX, inicioY + spacing);
            btnGestionarProvincias.Click += BtnGestionarProvincias_Click;
            Controls.Add(btnGestionarProvincias);
            AplicarBordesRedondeados(btnGestionarProvincias, borderRadius);

            btnGestionarHospedajes = CrearBoton("Gestionar Hospedajes", centroX, inicioY + spacing * 2);
            btnGestionarHospedajes.Click += BtnGestionarHospedajes_Click;
            Controls.Add(btnGestionarHospedajes);
            AplicarBordesRedondeados(btnGestionarHospedajes, borderRadius);

            btnGestionarReservas = CrearBoton("Gestionar Reservas", centroX, inicioY + spacing * 3);
            btnGestionarReservas.Click += BtnGestionarReservas_Click;
            Controls.Add(btnGestionarReservas);
            AplicarBordesRedondeados(btnGestionarReservas, borderRadius);
        }

        private Button CrearBoton(string texto, int x, int y)
        {
            return new Button
            {
                Text = texto,
                Size = new Size(300, 60),
                Font = new Font("Arial", 14, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                Location = new Point(x, y),
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private void AplicarBordesRedondeados(Button boton, int borderRadius)
        {
            boton.Paint += (s, e) =>
            {
                var path = new GraphicsPath();
                path.AddArc(0, 0, borderRadius * 2, borderRadius * 2, 180, 90);
                path.AddArc(boton.Width - borderRadius * 2, 0, borderRadius * 2, borderRadius * 2, 270, 90);
                path.AddArc(boton.Width - borderRadius * 2, boton.Height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 0, 90);
                path.AddArc(0, boton.Height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 90, 90);
                path.CloseAllFigures();
                boton.Region = new Region(path);
            };
            boton.Invalidate();
        }

        private void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Hide();
            var inicio = new InicioForm();
            inicio.Show();
        }

        private void BtnGestionarProvincias_Click(object sender, EventArgs e)
        {
            MostrarFormularioModal(new ProvinciasForm());
        }

        private void BtnGestionarHospedajes_Click(object sender, EventArgs e)
        {
            MostrarFormularioModal(new HospedajesForm());
        }

        private void BtnGestionarReservas_Click(object sender, EventArgs e)
        {
            MostrarFormularioModal(new ReservasForm());
        }

        private void MostrarFormularioModal(Form formulario)
        {
            this.Enabled = false;

            formulario.StartPosition = FormStartPosition.CenterParent;
            formulario.TopMost = true;

            formulario.FormClosed += (s, e) =>
            {
                this.Enabled = true;
                this.BringToFront();
                this.Focus();
            };

            formulario.ShowDialog(this);
        }
    }
}