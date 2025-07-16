using Sistema_de_Gestion_de_Rentas.Controls;  // Asegúrate de que EstilosUI.cs esté en esta carpeta
using Sistema_de_Gestion_de_Rentas.Forms;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.ProvinciaForms
{
    public partial class CostaRicaForm : Form
    {
        public CostaRicaForm()
        {
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Configuración de la ventana
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;

            // Fondo de imagen
            EstablecerFondo();

            // Crear el botón "Volver"
            CrearBotonVolver();
        }

        private void EstablecerFondo()
        {
            string fondoRuta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "provincias.jpg");
            if (File.Exists(fondoRuta))
            {
                this.BackgroundImage = Image.FromFile(fondoRuta);
                this.BackgroundImageLayout = ImageLayout.Stretch;  // Ajustar imagen al tamaño de la ventana
            }
            else
            {
                MessageBox.Show("La imagen de fondo no se encontró.");
            }
        }

        private void CrearBotonVolver()
        {
            Button btnVolver = new Button();
            EstilosUI.AplicarEstiloBoton(btnVolver);  // Aplicamos el estilo desde EstilosUI
            btnVolver.Text = "Volver";
            btnVolver.Font = new Font("Arial", 48, FontStyle.Bold); // Tamaño de fuente grande, pero no tan masivo
            btnVolver.BackColor = Color.FromArgb(0, 102, 204);
            btnVolver.ForeColor = Color.White;
            btnVolver.Size = new Size(500, 120); // Un tamaño grande pero dentro de la pantalla

            // Centrar el botón en la parte inferior, asegurándonos que esté visible
            btnVolver.Location = new Point((this.ClientSize.Width - btnVolver.Width) / 2, this.ClientSize.Height - 170);
            btnVolver.Click += BtnVolver_Click;

            this.Controls.Add(btnVolver);
        }

        // Evento para regresar al PanelHuespedForm
        private void BtnVolver_Click(object sender, EventArgs e)
        {
            PanelHuespedForm panelHuespedForm = new PanelHuespedForm();
            panelHuespedForm.Show();
            this.Close();
        }
    }
}
