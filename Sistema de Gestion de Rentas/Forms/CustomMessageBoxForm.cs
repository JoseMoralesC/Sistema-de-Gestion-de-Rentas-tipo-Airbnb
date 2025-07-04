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
        private Button btnSi, btnNo;

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

            Load += CustomMessageBoxForm_Load; // Manejamos la carga de los botones aquí
        }

        private void CustomMessageBoxForm_Load(object sender, EventArgs e)
        {
            // Botón Sí (Panel de Administrador)
            btnSi = new Button
            {
                Text = "Panel de Administrador",
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };
            btnSi.FlatAppearance.BorderSize = 0;
            btnSi.DialogResult = DialogResult.Yes;
            Controls.Add(btnSi);

            // Botón No (Panel de Usuario)
            btnNo = new Button
            {
                Text = "Panel de Usuario",
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };
            btnNo.FlatAppearance.BorderSize = 0;
            btnNo.DialogResult = DialogResult.No;
            Controls.Add(btnNo);

            // Establecer posiciones de los botones
            int padding = 10;
            int buttonWidth = btnSi.Width;
            int totalWidth = buttonWidth * 2 + padding;
            int startX = (ClientSize.Width - totalWidth) / 2;

            btnSi.Location = new Point(startX, ClientSize.Height - 70);
            btnNo.Location = new Point(btnSi.Right + padding, ClientSize.Height - 70);
        }

        // Método para mostrar opciones (con títulos personalizados)
        public static DialogResult MostrarOpciones(string mensaje, string opcion1, string opcion2)
        {
            using (var box = new CustomMessageBoxForm(mensaje))
            {
                return box.ShowDialog();
            }
        }

        // Sobrecarga del método Mostrar para aceptar solo el mensaje
        public static void Mostrar(string mensaje)
        {
            using (var box = new CustomMessageBoxForm(mensaje))
            {
                box.ShowDialog();
            }
        }
    }
}
