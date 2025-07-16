using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.ProvinciaForms
{
    public partial class CostaRicaForm : Form
    {
        private Button btnSalir;

        public CostaRicaForm()
        {
            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            // Configuración del formulario
            this.WindowState = FormWindowState.Maximized;  // Ventana maximizada
            this.FormBorderStyle = FormBorderStyle.None;   // Sin bordes
            this.BackColor = Color.White;                  // Fondo blanco

            // Establecer imagen de fondo
            EstablecerFondo();
        }

        private void EstablecerFondo()
        {
            // Ruta de la imagen de fondo
            string fondoRuta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "provincias.jpg");

            if (File.Exists(fondoRuta))
            {
                // Si existe la imagen, se configura como fondo
                this.BackgroundImage = Image.FromFile(fondoRuta);
                this.BackgroundImageLayout = ImageLayout.Stretch;  // Ajuste de la imagen al tamaño de la ventana
            }
            else
            {
                // Si no se encuentra la imagen, se muestra un mensaje
                MessageBox.Show("La imagen de fondo no se encontró.");
            }
        }

        private void InicializarControles()
        {
            // Crear el botón de salida
            btnSalir = new Button();
            btnSalir.Text = "Salir";
            btnSalir.Size = new Size(180, 60);
            btnSalir.Location = new Point((this.ClientSize.Width - btnSalir.Width) / 2, this.ClientSize.Height - btnSalir.Height - 20);
            btnSalir.BackColor = Color.FromArgb(0, 102, 204);
            btnSalir.ForeColor = Color.White;
            btnSalir.Font = new Font("Arial", 16, FontStyle.Bold);
            btnSalir.Click += BtnSalir_Click; // Evento click del botón
            this.Controls.Add(btnSalir);
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            // Cierra el formulario
            this.Close();
        }

        // Evento para ajustar la posición del botón al redimensionar la ventana
        private void CostaRicaForm_Resize(object sender, EventArgs e)
        {
            btnSalir.Location = new Point((this.ClientSize.Width - btnSalir.Width) / 2, this.ClientSize.Height - btnSalir.Height - 20);
        }
    }
}
