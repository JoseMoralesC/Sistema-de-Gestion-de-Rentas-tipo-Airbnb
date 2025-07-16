using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Controls;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class NosotrosForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);

        private Label lblTitulo;
        private Label lblContenido;
        private Button btnCerrar;
        private Panel panelContenido;

        public NosotrosForm()
        {
            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(30, 30, 40); 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 600); 
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void InicializarControles()
        {
            lblTitulo = new Label()
            {
                Text = "Nosotros",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };
            EstilosUI.AplicarEstiloLabel(lblTitulo);
            Controls.Add(lblTitulo);


            panelContenido = new Panel()
            {
                Location = new Point(40, 80),
                Size = new Size(this.ClientSize.Width - 80, 400),
                AutoScroll = true,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(panelContenido);

            lblContenido = new Label()
            {
                Text = ObtenerTextoNosotros(),
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                MaximumSize = new Size(panelContenido.Width - 20, 0), 
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 0)
            };
            EstilosUI.AplicarEstiloLabel(lblContenido);
            panelContenido.Controls.Add(lblContenido);

            btnCerrar = new Button()
            {
                Text = "Cerrar",
                Width = 120,
                Height = 40,
                Location = new Point((this.ClientSize.Width - 120) / 2, this.ClientSize.Height - 70),
                Anchor = AnchorStyles.Bottom
            };
            EstilosUI.AplicarEstiloBoton(btnCerrar);
            btnCerrar.Click += (s, e) => this.Close();
            Controls.Add(btnCerrar);

            this.Resize += (s, e) =>
            {
                panelContenido.Size = new Size(this.ClientSize.Width - 80, this.ClientSize.Height - 170);
                btnCerrar.Location = new Point((this.ClientSize.Width - btnCerrar.Width) / 2, this.ClientSize.Height - 70);


                lblContenido.MaximumSize = new Size(panelContenido.Width - 20, 0);
            };
        }

        private string ObtenerTextoNosotros()
        {
            return
                "Somos una empresa costarricense dedicada a facilitar experiencias únicas de hospedaje para quienes desean explorar y disfrutar de Costa Rica. " +
                "Nuestra misión es conectar a los viajeros con alojamientos confiables, seguros y a precios accesibles, siempre procurando que cada experiencia sea inolvidable.\n\n" +

                "Promovemos el turismo local, respetamos nuestras zonas naturales y culturales, y trabajamos de la mano con anfitriones comprometidos con la hospitalidad y el buen servicio. " +
                "Nos aseguramos de que cada huésped reciba atención de calidad y se sienta bienvenido desde su llegada hasta su partida.\n\n" +

                "Si tienes dudas, comentarios o necesitas asistencia, no dudes en contactar con el desarrollador de esta plataforma:\n" +
                "Jose Rodolfo Morales Calderón  |   +506 6174-8562  |   305000192@cuc.cr\n\n" +

                "Gracias por confiar en nosotros. ¡Estamos aquí para hacer de tu estadía algo inolvidable!";
        }
    }
}
