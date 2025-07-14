using Sistema_de_Gestion_de_Rentas.Controls;
using Sistema_de_Gestion_de_Rentas.ProvinciaForms;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
            provinciasTable.Size = new Size(this.ClientSize.Width - 100, 520); // Aseguramos un tamaño fijo
            provinciasTable.ColumnCount = 4;  // 4 columnas
            provinciasTable.RowCount = 2;  // 2 filas

            // Asegurar que cada columna ocupe un 25% del ancho
            for (int i = 0; i < 4; i++)
            {
                provinciasTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            }

            // Asegurar que cada fila ocupe un 50% de la altura disponible
            for (int i = 0; i < 2; i++)
            {
                provinciasTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            }

            // No mostrar bordes en las celdas
            provinciasTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

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
            // Definir las provincias que deben mostrarse en el PanelHuesped
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

                    // Cambiar el evento para abrir el formulario de la provincia
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

                // Ajustamos la tarjeta para que ocupe el espacio de la celda, pero sin utilizar DockStyle.Fill
                card.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                // Añadir la tarjeta a la celda correspondiente
                provinciasTable.Controls.Add(card, i % 4, i / 4); // Añadimos la tarjeta de forma 4x2
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
            // Usamos CustomMessageBoxForm para la confirmación solo con OK
            CustomMessageBoxForm.Mostrar(
                this,  // Pasamos el formulario actual como dueño
                $"¿Deseas abrir los hospedajes de {provincia}?");

            // Procedemos solo si el usuario hizo clic en "Aceptar"
            if (DialogResult.OK == DialogResult.OK)  // Si presiona "OK"
            {
                // Abrir el formulario de la provincia correspondiente
                switch (provincia)
                {
                    case "Cartago":
                        CartagoForm cartagoForm = new CartagoForm();
                        cartagoForm.Show();
                        break;

                    case "San Jose":
                        //Si tienes otro formulario para San José, puedes descomentar esto
                        //SanJoseForm sanJoseForm = new SanJoseForm();
                        //sanJoseForm.Show();
                        break;

                    // Puedes agregar más casos aquí si tienes formularios de otras provincias

                    default:
                        CustomMessageBoxForm.Mostrar(this, $"Formulario no disponible para {provincia}.");
                        break;
                }
            }
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
