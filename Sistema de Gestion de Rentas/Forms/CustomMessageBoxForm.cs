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

        private bool _showTwoButtons = false;
        private string _opcion1 = "Sí";
        private string _opcion2 = "No";

        public CustomMessageBoxForm(string mensaje, string titulo = "Mensaje")
        {
            Text = titulo;
            Size = new Size(400, 200);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
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

            btnSi = new Button
            {
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                DialogResult = DialogResult.Yes
            };
            btnSi.FlatAppearance.BorderSize = 0;
            Controls.Add(btnSi);

            btnNo = new Button
            {
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                DialogResult = DialogResult.No
            };
            btnNo.FlatAppearance.BorderSize = 0;
            Controls.Add(btnNo);

            int padding = 10;
            int totalWidth = btnSi.Width * 2 + padding;
            int startX = (ClientSize.Width - totalWidth) / 2;

            btnSi.Location = new Point(startX, ClientSize.Height - 70);
            btnNo.Location = new Point(btnSi.Right + padding, ClientSize.Height - 70);

            if (_showTwoButtons)
            {
                btnSi.Text = _opcion1;
                btnSi.DialogResult = DialogResult.Yes;

                btnNo.Text = _opcion2;
                btnNo.DialogResult = DialogResult.No;

                btnSi.Visible = true;
                btnNo.Visible = true;
            }
            else
            {
                btnSi.Text = "OK";
                btnSi.DialogResult = DialogResult.OK;
                btnSi.Visible = true;
                btnNo.Visible = false;
            }

            AcceptButton = btnSi;
            CancelButton = btnNo;

            // *** Estas líneas aseguran que el formulario esté siempre al frente ***
            this.TopMost = true;
            this.Activate();

            // Permitir capturar teclas en el formulario
            this.KeyPreview = true;
            this.KeyDown += CustomMessageBoxForm_KeyDown;
        }

        // Manejo del evento KeyDown para detectar cuando se presiona la tecla Enter
        private void CustomMessageBoxForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Simula el clic en el primer botón (Aceptar/Sí)
                btnSi.PerformClick();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.TopMost = true;
            this.BringToFront();
            this.Activate();
        }

        // Mostrar mensaje simple (sin dueño)
        public static DialogResult Mostrar(string mensaje)
        {
            using var box = new CustomMessageBoxForm(mensaje);
            box._showTwoButtons = false;
            return box.ShowDialog();
        }

        // Mostrar mensaje simple (con dueño)
        public static DialogResult Mostrar(IWin32Window owner, string mensaje)
        {
            using var box = new CustomMessageBoxForm(mensaje);
            box._showTwoButtons = false;
            return box.ShowDialog(owner);
        }

        // Mostrar mensaje con opciones Sí / No (sin dueño)
        public static DialogResult MostrarOpciones(string mensaje, string opcion1 = "Sí", string opcion2 = "No")
        {
            using var box = new CustomMessageBoxForm(mensaje);
            box._showTwoButtons = true;
            box._opcion1 = opcion1;
            box._opcion2 = opcion2;
            return box.ShowDialog();
        }

        // Mostrar mensaje con opciones Sí / No (con dueño)
        public static DialogResult MostrarOpciones(IWin32Window owner, string mensaje, string opcion1 = "Sí", string opcion2 = "No")
        {
            using var box = new CustomMessageBoxForm(mensaje);
            box._showTwoButtons = true;
            box._opcion1 = opcion1;
            box._opcion2 = opcion2;
            return box.ShowDialog(owner);
        }
    }
}
