using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Controls;

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

            Label lblBienvenida = new Label
            {
                Text = "Bienvenido a su panel de usuario",
                Font = new Font("Arial", 26, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(50, 120)
            };
            Controls.Add(lblBienvenida);

            banner = new PictureBox
            {
                Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "banner.jpg")),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(50, 180),
                Height = 200,
                Width = this.ClientSize.Width - 100,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(banner);

            lblMensaje = new Label
            {
                Text = "Elige una de nuestras 7 provincias como tu próximo destino turístico",
                Font = new Font("Arial", 14, FontStyle.Italic),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, banner.Bottom + 10),
                Size = new Size(this.ClientSize.Width - 100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(lblMensaje);

            provinciasTable = new TableLayoutPanel
            {
                Location = new Point(50, lblMensaje.Bottom + 20),
                Size = new Size(this.ClientSize.Width - 100, 520), // Ajusta la altura que quieras para el grid
                ColumnCount = 4,
                RowCount = 2,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // Configurar filas y columnas para que sean proporcionales
            for (int i = 0; i < provinciasTable.ColumnCount; i++)
                provinciasTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            for (int i = 0; i < provinciasTable.RowCount; i++)
                provinciasTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

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
                "Alajuela", "Cartago", "San José", "Heredia",
                "Puntarenas", "Guanacaste", "Limón"
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
