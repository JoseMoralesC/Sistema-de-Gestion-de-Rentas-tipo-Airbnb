using Sistema_de_Gestion_de_Rentas.Controls;
using Sistema_de_Gestion_de_Rentas.Forms;
using Sistema_de_Gestion_de_Rentas.Reservas;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Sistema_de_Gestion_de_Rentas.ProvinciaForms
{
    public partial class SanJoseForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private Button btnRegresar;
        private TableLayoutPanel provinciasTable;
        private Label lblBienvenida;
        private Label lblMensaje;
        private PictureBox banner;
        private Panel contentPanel;

        private int provinciaId;

        public SanJoseForm(int provinciaId)
        {
            this.provinciaId = provinciaId;
            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
        }

        private void InicializarControles()
        {
            contentPanel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill
            };
            Controls.Add(contentPanel);

            btnRegresar = new Button();
            EstilosPanelHuesped.EstiloBotonCerrarSesion(btnRegresar);
            btnRegresar.Text = "Regresar";
            btnRegresar.Size = new Size(180, 60);
            btnRegresar.Location = new Point(50, 50);
            btnRegresar.Click += BtnRegresar_Click;
            contentPanel.Controls.Add(btnRegresar);

            lblBienvenida = new Label();
            EstilosPanelHuesped.EstiloLabelBienvenida(lblBienvenida);
            lblBienvenida.Text = "Bienvenido a San Jose";
            lblBienvenida.Location = new Point((this.ClientSize.Width - lblBienvenida.Width) / 2, 50);
            contentPanel.Controls.Add(lblBienvenida);

            banner = new PictureBox();
            EstilosPanelHuesped.EstiloBanner(banner, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "banner.jpg"), 200, this.ClientSize.Width - 100);
            banner.Location = new Point(50, lblBienvenida.Bottom + 10);
            contentPanel.Controls.Add(banner);

            lblMensaje = new Label();
            EstilosPanelHuesped.EstiloLabelMensaje(lblMensaje);
            lblMensaje.Text = "¡Descubre todo lo que San Jose tiene para ofrecer! Elige tu alojamiento ideal";
            lblMensaje.Location = new Point(50, banner.Bottom + 10);
            contentPanel.Controls.Add(lblMensaje);

            provinciasTable = new TableLayoutPanel();
            EstilosPanelHuesped.EstiloTableLayoutPanelProvincias(provinciasTable);
            provinciasTable.Location = new Point(50, lblMensaje.Bottom + 20);
            provinciasTable.Size = new Size(this.ClientSize.Width - 100, this.ClientSize.Height - 200);
            provinciasTable.RowCount = 2;
            provinciasTable.ColumnCount = 2;

            provinciasTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            provinciasTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            provinciasTable.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            provinciasTable.RowStyles.Add(new RowStyle(SizeType.Percent, 60));

            provinciasTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            provinciasTable.Margin = new Padding(10);
            contentPanel.Controls.Add(provinciasTable);

            CargarHospedajes();

            this.Resize += (s, e) =>
            {
                banner.Width = this.ClientSize.Width - 100;
                lblMensaje.Size = new Size(this.ClientSize.Width - 100, 30);
                provinciasTable.Size = new Size(this.ClientSize.Width - 100, this.ClientSize.Height - 200);
                lblBienvenida.Location = new Point((this.ClientSize.Width - lblBienvenida.Width) / 2, 50);
            };
        }

        private void CargarHospedajes()
        {
            var idsHospedajes = new List<int> { 9, 10, 11, 12 };
            var hospedajes = HospedajeLogic.ObtenerHospedajesParaContenedores("San Jose", idsHospedajes);

            provinciasTable.Controls.Clear();

            int totalCeldas = provinciasTable.RowCount * provinciasTable.ColumnCount;

            for (int i = 0; i < totalCeldas; i++)
            {
                ProvinciaCard card;

                if (i < hospedajes.Count)
                {
                    var hospedaje = hospedajes[i];
                    card = new ProvinciaCard
                    {
                        Titulo = hospedaje.Nombre,
                        Imagen = CargarImagenHospedaje(hospedaje.Nombre.ToLower().Replace(" ", ""))
                    };

                    card.EstablecerIdHospedaje(hospedaje.Id);
                    Console.WriteLine($"Contenedor {i + 1} - ID: {hospedaje.Id}, Nombre: {hospedaje.Nombre}");
                    card.CardClick += (s, e) => AbrirFormularioReservas(hospedaje);
                }
                else
                {
                    card = new ProvinciaCard
                    {
                        Titulo = "No disponible",
                        Imagen = null
                    };
                }

                card.Dock = DockStyle.Fill;
                provinciasTable.Controls.Add(card, i % provinciasTable.ColumnCount, i / provinciasTable.RowCount);
            }
        }

        private Image CargarImagenHospedaje(string nombreHospedaje)
        {
            string carpetaHospedajes = Path.Combine(Application.StartupPath, "Resources", "San Jose");
            string rutaImagen = Path.Combine(carpetaHospedajes, $"{nombreHospedaje}.jpg");

            if (File.Exists(rutaImagen))
            {
                return Image.FromFile(rutaImagen);
            }
            else
            {
                string imagenNoDisponible = Path.Combine(Application.StartupPath, "Resources", "NoDisponible.jpg");
                if (File.Exists(imagenNoDisponible))
                {
                    return Image.FromFile(imagenNoDisponible);
                }
                else
                {
                    Console.WriteLine("Imagen de 'No Disponible' no encontrada");
                    return null;
                }
            }
        }

        private void AbrirFormularioReservas(Hospedaje hospedaje)
        {
            CustomMessageBoxForm.Mostrar(this, $"¿Desea hospedarse en: {hospedaje.Nombre}?");
            ReservacionForm reservacionForm = new ReservacionForm(hospedaje.Id, provinciaId);
            reservacionForm.ShowDialog();
        }

        private void BtnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
