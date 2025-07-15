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
            get => lblTitulo?.Text;  // Protegemos contra null si lblTitulo es null.
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
            get => pictureBox?.Image;  // Protegemos contra null si pictureBox es null.
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
            // Establecer el tama�o del control base
            Size = new Size(300, 250);
            BorderStyle = BorderStyle.FixedSingle;

            // Configuraci�n del PictureBox para que se ajuste al tama�o del control sin distorsionarse
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 180, // Mantener una altura inicial, ajustable seg�n el tama�o del contenedor
                SizeMode = PictureBoxSizeMode.Zoom // Ajustar la imagen para que se vea correctamente sin distorsi�n
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

            // Cambiar el cursor al pasar por encima
            Cursor = Cursors.Hand;

            // El control responde al clic (tambi�n se activa cuando se hace clic en el PictureBox o en el t�tulo)
            this.Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
        }

        // Nuevo m�todo para establecer el ID del hospedaje
        public void EstablecerIdHospedaje(int id)
        {
            IdHospedaje = id;
        }

        // M�todo para manejar el cambio de tama�o del control
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Asegurarse de que los controles est�n inicializados antes de usarlos
            if (pictureBox != null && lblTitulo != null)
            {
                // Aseguramos que el PictureBox se ajuste din�micamente a la nueva altura del control
                pictureBox.Height = (int)(this.ClientSize.Height * 0.7);  // El PictureBox ocupar� el 70% de la altura total
                lblTitulo.Height = this.ClientSize.Height - pictureBox.Height;  // El t�tulo ocupa el resto
            }
        }
    }
}
