using Sistema_de_Gestion_de_Rentas.Controls;
using System.Runtime.InteropServices;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class PanelHuespedForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private Button btnCerrarSesion;
        private TableLayoutPanel provinciasTable;
        private Label lblBienvenida;
        private Label lblMensaje;
        private PictureBox banner;

        public PanelHuespedForm()
        {
            ConfigurarFormulario();
            InicializarControles();
            CargarProvincias();
        }

        private void ConfigurarFormulario()
        {
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
        }

        private void InicializarControles()
        {
            // Botón "Cerrar Sesión"
            btnCerrarSesion = new Button();
            EstilosPanelHuesped.EstiloBotonCerrarSesion(btnCerrarSesion);
            btnCerrarSesion.Location = new Point(50, 50);
            btnCerrarSesion.Click += BtnCerrarSesion_Click;
            Controls.Add(btnCerrarSesion);

            // Etiqueta "Bienvenido"
            lblBienvenida = new Label();
            EstilosPanelHuesped.EstiloLabelBienvenida(lblBienvenida);
            Controls.Add(lblBienvenida);

            // Banner (Imagen)
            banner = new PictureBox();
            EstilosPanelHuesped.EstiloBanner(banner, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "banner.jpg"), 200, this.ClientSize.Width - 100);
            banner.Location = new Point(50, 180);
            Controls.Add(banner);

            // Etiqueta de mensaje
            lblMensaje = new Label();
            EstilosPanelHuesped.EstiloLabelMensaje(lblMensaje);
            lblMensaje.Location = new Point(50, banner.Bottom + 10);
            Controls.Add(lblMensaje);

            // TableLayoutPanel para Provincias
            provinciasTable = new TableLayoutPanel();
            EstilosPanelHuesped.EstiloTableLayoutPanelProvincias(provinciasTable);
            provinciasTable.Location = new Point(50, lblMensaje.Bottom + 20);
            Controls.Add(provinciasTable);

            // Ajustar controles al redimensionar ventana
            this.Resize += (s, e) =>
            {
                banner.Width = this.ClientSize.Width - 100;
                lblMensaje.Size = new Size(this.ClientSize.Width - 100, 30);
                provinciasTable.Size = new Size(this.ClientSize.Width - 100, 520);
            };
        }

        private void CargarProvincias()
        {
            string[] nombres = {
                "Alajuela", "Cartago", "San Jose", "Heredia",
                "Puntarenas", "Guanacaste", "Limon", "Costa Rica"
            };

            provinciasTable.Controls.Clear();

            // Llenar el grid 4x2
            int totalCeldas = provinciasTable.RowCount * provinciasTable.ColumnCount;
            for (int i = 0; i < totalCeldas; i++)
            {
                ProvinciaCard card;

                if (i < nombres.Length)
                {
                    string nombre = nombres[i];
                    card = new ProvinciaCard
                    {
                        Titulo = nombre,
                        Imagen = CargarImagenProvincia(nombre.ToLower().Replace(" ", ""))
                    };
                    card.CardClick += (s, e) => AbrirFormularioProvincia(nombre);
                }
                else
                {
                    card = new ProvinciaCard
                    {
                        Titulo = "",
                        Imagen = null
                    };
                }

                // Establecer Dock para que la tarjeta llene la celda
                card.Dock = DockStyle.Fill;

                provinciasTable.Controls.Add(card, i % provinciasTable.ColumnCount, i / provinciasTable.ColumnCount);
            }
        }

        private Image CargarImagenProvincia(string nombreArchivo)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", $"{nombreArchivo}.jpg");
            if (File.Exists(ruta))
                return Image.FromFile(ruta);
            return null; // imagen por defecto o nula
        }

        private void AbrirFormularioProvincia(string provincia)
        {
            MessageBox.Show($"Abrir hospedajes de {provincia}...");
            // Aquí podrías hacer: new HospedajesProvinciaForm(provincia).Show();
        }

        private void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            Hide();
            var inicioForm = new InicioForm();
            inicioForm.Show();
            Close();
        }
    }
}
