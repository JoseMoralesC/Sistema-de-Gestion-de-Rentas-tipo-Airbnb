using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Controls
{
    public class ProvinciaCard : UserControl
    {
        private PictureBox pictureBox;
        private Label lblTitulo;

        //  almacenar el ID del hospedaje
        public int IdHospedaje { get; private set; }

        public string Titulo
        {
            get => lblTitulo?.Text;  
            set
            {
                if (lblTitulo != null)
                {
                    lblTitulo.Text = value;
                }
            }
        }

        public Image Imagen
        {
            get => pictureBox?.Image;  
            set
            {
                if (pictureBox != null)
                {
                    pictureBox.Image = value;
                }
            }
        }

        public event EventHandler CardClick;

        public ProvinciaCard()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
           
            Size = new Size(300, 250);
            BorderStyle = BorderStyle.FixedSingle;

            // Configuraci�n del PictureBox para que se ajuste al tama�o del control sin distorsionarse
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 180, 
                SizeMode = PictureBoxSizeMode.Zoom 
            };
            pictureBox.Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
            Controls.Add(pictureBox);

            // Configuraci�n de la etiqueta para el t�tulo
            lblTitulo = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Black,
                Padding = new Padding(5, 10, 5, 5),
                BackColor = Color.WhiteSmoke
            };
            lblTitulo.Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
            Controls.Add(lblTitulo);

            
            Cursor = Cursors.Hand;

            
            this.Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
        }

        // metodo para establecer el ID del hospedaje
        public void EstablecerIdHospedaje(int id)
        {
            IdHospedaje = id;
        }

        // Metodo para manejar el cambio de tama�o del control
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Asegurarse de que los controles est�n inicializados antes de usarlos
            if (pictureBox != null && lblTitulo != null)
            {
                
                pictureBox.Height = (int)(this.ClientSize.Height * 0.7);  
                lblTitulo.Height = this.ClientSize.Height - pictureBox.Height;  
            }
        }
    }
}
