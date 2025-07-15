using Sistema_de_Gestion_de_Rentas.Controls;
using Sistema_de_Gestion_de_Rentas.Forms;
using Sistema_de_Gestion_de_Rentas.Reservas;  // Importa el espacio de nombres de Reservas
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;  // Asegúrate de que esta línea esté presente si usas listas

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

        // Provincia ID de Cartago (ahora lo tomamos como parámetro)
        private int provinciaId;

        // Constructor con parámetro provinciaId
        public CartagoForm(int provinciaId)
        {
            this.provinciaId = provinciaId;  // Guardamos el provinciaId
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
            provinciasTable.RowStyles.Add(new RowStyle(SizeType.Percent, 40));  // Reducimos la altura de las filas (40% cada una)
            provinciasTable.RowStyles.Add(new RowStyle(SizeType.Percent, 60));  // Para la segunda fila también reducimos un poco la altura

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
            // Definir los IDs de los hospedajes que se deben cargar (fijos, uno para cada contenedor)
            var idsHospedajes = new List<int> { 5, 6, 7, 8 };  // Ejemplo de IDs fijos para los 4 contenedores

            // Obtener los hospedajes desde la lógica, solo aquellos con los IDs especificados
            var hospedajes = HospedajeLogic.ObtenerHospedajesParaContenedores("Cartago", idsHospedajes);

            provinciasTable.Controls.Clear();  // Limpiar la tabla de la UI

            // Obtener el número total de celdas de la tabla
            int totalCeldas = provinciasTable.RowCount * provinciasTable.ColumnCount;

            for (int i = 0; i < totalCeldas; i++)
            {
                ProvinciaCard card;

                if (i < hospedajes.Count)
                {
                    var hospedaje = hospedajes[i];
                    card = new ProvinciaCard
                    {
                        // El título sigue sin mostrar el índice, solo el nombre
                        Titulo = hospedaje.Nombre,  // Solo el nombre del hospedaje
                        Imagen = CargarImagenHospedaje(hospedaje.Nombre.ToLower().Replace(" ", ""))  // Cambiar esto si el nombre no coincide con la imagen
                    };

                    // Asignar el ID del hospedaje a la tarjeta
                    card.EstablecerIdHospedaje(hospedaje.Id);  // Este ID es lo que se usará para la búsqueda

                    // Verificar si el nombre del hospedaje es correcto en consola
                    Console.WriteLine($"Contenedor {i + 1} - ID: {hospedaje.Id}, Nombre: {hospedaje.Nombre}");

                    // Asignar evento de clic para abrir formulario de reservación
                    card.CardClick += (s, e) => AbrirFormularioReservas(hospedaje);
                }
                else
                {
                    card = new ProvinciaCard
                    {
                        Titulo = "No disponible",  // Esto se muestra si no hay hospedajes
                        Imagen = null
                    };
                }

                // Ajustar el tamaño del contenedor
                card.Dock = DockStyle.Fill;

                // Agregar la tarjeta al contenedor correspondiente en la tabla
                provinciasTable.Controls.Add(card, i % provinciasTable.ColumnCount, i / provinciasTable.RowCount);
            }
        }

        private Image CargarImagenHospedaje(string nombreHospedaje)
        {
            // Ruta de la carpeta Cartago dentro de Resources
            string carpetaHospedajes = Path.Combine(Application.StartupPath, "Resources", "Cartago");

            // Nombre del archivo de la imagen que debe coincidir con el nombre del hospedaje
            string rutaImagen = Path.Combine(carpetaHospedajes, $"{nombreHospedaje}.jpg");

            // Verificamos si el archivo de imagen existe en esa ruta
            if (File.Exists(rutaImagen))
            {
                // Cargar la imagen
                Image imagen = Image.FromFile(rutaImagen);

                // Retornar la imagen cargada
                return imagen;
            }
            else
            {
                // Cargar la imagen "no disponible"
                string imagenNoDisponible = Path.Combine(Application.StartupPath, "Resources", "NoDisponible.jpg");

                if (File.Exists(imagenNoDisponible))
                {
                    // Cargar y retornar la imagen "no disponible"
                    return Image.FromFile(imagenNoDisponible);
                }
                else
                {
                    Console.WriteLine("Imagen de 'No Disponible' no encontrada");
                    return null;  // Si no se encuentra la imagen de "No disponible", retornamos null
                }
            }
        }

        private void AbrirFormularioReservas(Hospedaje hospedaje)
        {
            // Usamos el CustomMessageBoxForm para mostrar la opción de confirmación con solo un OK
            CustomMessageBoxForm.Mostrar(this, $"¿Desea hospedarse en: {hospedaje.Nombre}?");

            // Procedemos con la reservación solo si el usuario hizo clic en "OK"
            // Se pasa el ID del hospedaje y el provinciaId al constructor de ReservacionForm
            ReservacionForm reservacionForm = new ReservacionForm(hospedaje.Id, provinciaId);
            reservacionForm.ShowDialog();  // Usamos ShowDialog() para hacerlo modal
        }

        private void BtnRegresar_Click(object sender, EventArgs e)
        {
            // Volver al formulario anterior
            this.Close();  // Cierra el formulario actual
        }
    }
}
