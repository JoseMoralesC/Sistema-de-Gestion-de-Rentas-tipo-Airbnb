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
       
            this.WindowState = FormWindowState.Maximized;  
            this.FormBorderStyle = FormBorderStyle.None;   
            this.BackColor = Color.White;                  

            
            EstablecerFondo();
        }

        private void EstablecerFondo()
        {
           
            string fondoRuta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "provincias.jpg");

            if (File.Exists(fondoRuta))
            {
                
                this.BackgroundImage = Image.FromFile(fondoRuta);
                this.BackgroundImageLayout = ImageLayout.Stretch; 
            }
            else
            {
                
                MessageBox.Show("La imagen de fondo no se encontró.");
            }
        }

        private void InicializarControles()
        {
            
            btnSalir = new Button();
            btnSalir.Text = "Salir";
            btnSalir.Size = new Size(180, 60);
            btnSalir.Location = new Point((this.ClientSize.Width - btnSalir.Width) / 2, this.ClientSize.Height - btnSalir.Height - 20);
            btnSalir.BackColor = Color.FromArgb(0, 102, 204);
            btnSalir.ForeColor = Color.White;
            btnSalir.Font = new Font("Arial", 16, FontStyle.Bold);
            btnSalir.Click += BtnSalir_Click; 
            this.Controls.Add(btnSalir);
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        
        private void CostaRicaForm_Resize(object sender, EventArgs e)
        {
            btnSalir.Location = new Point((this.ClientSize.Width - btnSalir.Width) / 2, this.ClientSize.Height - btnSalir.Height - 20);
        }
    }
}
