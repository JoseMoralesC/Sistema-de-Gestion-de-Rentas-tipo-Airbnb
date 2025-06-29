using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class CustomMessageBoxForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private Label lblMensaje;
        private Button btnAceptar;

        public CustomMessageBoxForm(string mensaje, string titulo = "Mensaje")
        {
            Text = titulo;
            Size = new Size(400, 200);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;

            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            lblMensaje = new Label
            {
                Text = mensaje,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12),
                Padding = new Padding(20),
            };
            Controls.Add(lblMensaje);

            btnAceptar = new Button
            {
                Text = "Aceptar",
                Size = new Size(100, 40),
                Location = new Point((Width - 100) / 2, Height - 70),
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };
            btnAceptar.FlatAppearance.BorderSize = 0;
            btnAceptar.Click += (s, e) => this.Close();
            Controls.Add(btnAceptar);

            btnAceptar.BringToFront();
        }

        public static void Mostrar(string mensaje, string titulo = "Mensaje")
        {
            using (var box = new CustomMessageBoxForm(mensaje, titulo))
            {
                box.ShowDialog();
            }
        }
    }
}
