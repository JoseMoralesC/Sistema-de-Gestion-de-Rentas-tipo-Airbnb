using Sistema_de_Gestion_de_Rentas.Controls;
using Sistema_de_Gestion_de_Rentas.Forms;
using Sistema_de_Gestion_de_Rentas.Reservas;  // Importa el espacio de nombres de Reservas
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.ProvinciaForms
{
    public class CartagoForm : Form
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
        private Panel contentPanel;  // Panel para agregar barra de desplazamiento

        public CartagoForm()
        {
            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            WindowState = FormWindowState.Maximized;  // Pantalla completa
            FormBorderStyle = FormBorderStyle.None;  // Eliminar la barra superior
            BackColor = Color.White;  // Color de fondo
        }

        private void InicializarControles()
        {
            // Panel contenedor con desplazamiento
            contentPanel = new Panel
            {
                AutoScroll = true,  // Habilita el desplazamiento
                Dock = DockStyle.Fill  // Se ajusta a toda la ventana
            };
            Controls.Add(contentPanel);

            // Botón "Regresar" (en la esquina superior izquierda)
            btnRegresar = new Button();
            EstilosPanelHuesped.EstiloBotonCerrarSesion(btnRegresar);  // Usando estilo de otro formulario
            btnRegresar.Text = "Regresar";
            btnRegresar.Size = new Size(180, 60);  // Ajustamos el tamaño para que no sea muy grande
            btnRegresar.Location = new Point(50, 50);  // Lo ubicamos en la esquina superior izquierda
            btnRegresar.Click += BtnRegresar_Click;
            contentPanel.Controls.Add(btnRegresar);

            // Etiqueta "Bienvenido"
            lblBienvenida = new Label();
            EstilosPanelHuesped.EstiloLabelBienvenida(lblBienvenida);
            lblBienvenida.Text = "Bienvenido a Cartago";
            lblBienvenida.Location = new Point((this.ClientSize.Width - lblBienvenida.Width) / 2, 50);  // Centramos el título
            contentPanel.Controls.Add(lblBienvenida);

            // Banner (Imagen)
            banner = new PictureBox();
            EstilosPanelHuesped.EstiloBanner(banner, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "banner.jpg"), 200, this.ClientSize.Width - 100);
            banner.Location = new Point(50, lblBienvenida.Bottom + 10);
            contentPanel.Controls.Add(banner);

            // Etiqueta de mensaje
            lblMensaje = new Label();
            EstilosPanelHuesped.EstiloLabelMensaje(lblMensaje);
            lblMensaje.Text = "¡Descubre todo lo que Cartago tiene para ofrecer! Elige tu alojamiento ideal";
            lblMensaje.Location = new Point(50, banner.Bottom + 10);
            contentPanel.Controls.Add(lblMensaje);

            // TableLayoutPanel para Provincias
            provinciasTable = new TableLayoutPanel();
            EstilosPanelHuesped.EstiloTableLayoutPanelProvincias(provinciasTable);
            provinciasTable.Location = new Point(50, lblMensaje.Bottom + 20);
            provinciasTable.Size = new Size(this.ClientSize.Width - 100, this.ClientSize.Height - 200);  // Ajustamos el tamaño para ocupar el espacio restante

            provinciasTable.RowCount = 2;  // Dos filas
            provinciasTable.ColumnCount = 2;  // Dos columnas

            // Configuración de filas y columnas para que sean proporcionales
            provinciasTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));  // 50% para cada columna
            provinciasTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));  // 50% para cada columna
            provinciasTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));  // 50% para cada fila
            provinciasTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));  // 50% para cada fila

            // Ajustamos la separación entre celdas
            provinciasTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            provinciasTable.Margin = new Padding(10);  // Aumentamos el espacio entre las celdas
            contentPanel.Controls.Add(provinciasTable);

            // Cargar los hospedajes desde la lógica
            CargarHospedajes();

            // Ajustar controles al redimensionar ventana
            this.Resize += (s, e) =>
            {
                // Mantener las proporciones al redimensionar
                banner.Width = this.ClientSize.Width - 100;
                lblMensaje.Size = new Size(this.ClientSize.Width - 100, 30);
                provinciasTable.Size = new Size(this.ClientSize.Width - 100, this.ClientSize.Height - 200);
                lblBienvenida.Location = new Point((this.ClientSize.Width - lblBienvenida.Width) / 2, 50);  // Centramos el título
            };
        }

        private void CargarHospedajes()
        {
            // Obtener los hospedajes de Cartago desde la lógica
            var hospedajes = HospedajeLogic.ObtenerHospedajePorProvincia("Cartago");

            provinciasTable.Controls.Clear();  // Limpiar la tabla de la UI

            // Llenar el grid 2x2 con los hospedajes
            int totalCeldas = provinciasTable.RowCount * provinciasTable.ColumnCount;
            for (int i = 0; i < totalCeldas; i++)
            {
                ProvinciaCard card;

                if (i < hospedajes.Count)
                {
                    var hospedaje = hospedajes.Values.ElementAt(i);
                    card = new ProvinciaCard
                    {
                        Titulo = hospedaje.Nombre,
                        Imagen = CargarImagenHospedaje(hospedaje.Nombre.ToLower().Replace(" ", "")) // Cambié el método para reflejar hospedaje
                    };

                    // Asignar el evento de clic para abrir el formulario de reservación cuando se selecciona el hospedaje
                    card.CardClick += (s, e) => AbrirFormularioReservas(hospedaje);
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
                provinciasTable.Controls.Add(card, i % provinciasTable.ColumnCount, i / provinciasTable.RowCount);
            }
        }

        private Image CargarImagenHospedaje(string nombreHospedaje)
        {
            // Ruta de la carpeta Cartago dentro de Resources
            string carpetaHospedajes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Cartago");

            // Nombre del archivo de la imagen que debe coincidir con el nombre del hospedaje
            string rutaImagen = Path.Combine(carpetaHospedajes, $"{nombreHospedaje}.jpg");

            // Verificamos si el archivo de imagen existe en esa ruta
            if (File.Exists(rutaImagen))
            {
                return Image.FromFile(rutaImagen);  // Si existe, lo cargamos
            }
            else
            {
                return null;  // Si no existe, retornamos null o podríamos usar una imagen por defecto
            }
        }

        private void AbrirFormularioReservas(Hospedaje hospedaje)
        {
            // Usamos el CustomMessageBoxForm para mostrar la opción de confirmación con solo un OK
            CustomMessageBoxForm.Mostrar(this, $"¿Desea hospedarse en: {hospedaje.Nombre}?");

            // Procedemos con la reservación solo si el usuario hizo clic en "OK"
            ReservacionForm reservacionForm = new ReservacionForm(hospedaje.Nombre, hospedaje.PrecioPorNoche);
            reservacionForm.ShowDialog();  // Usamos ShowDialog() para hacerlo modal
        }

        private void BtnRegresar_Click(object sender, EventArgs e)
        {
            // Volver al formulario anterior
            this.Close();  // Cierra el formulario actual
        }
    }
}
