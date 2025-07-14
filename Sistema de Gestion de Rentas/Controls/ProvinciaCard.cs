using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Controls
{
    public class ProvinciaCard : UserControl
    {
        private PictureBox pictureBox;
        private Label lblTitulo;

        // Nueva propiedad para almacenar el ID del hospedaje
        public int IdHospedaje { get; private set; }

        public string Titulo
        {
            get => lblTitulo.Text;
            set => lblTitulo.Text = value;
        }

        public Image Imagen
        {
            get => pictureBox.Image;
            set => pictureBox.Image = value;
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

            pictureBox = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 180,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pictureBox.Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
            Controls.Add(pictureBox);

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

        // Nuevo método para establecer el ID del hospedaje
        public void EstablecerIdHospedaje(int id)
        {
            IdHospedaje = id;
        }
    }
}
