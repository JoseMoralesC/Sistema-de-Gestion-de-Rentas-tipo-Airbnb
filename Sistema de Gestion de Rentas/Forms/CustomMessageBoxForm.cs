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
        private string _opcion1 = "S�";
        private string _opcion2 = "No";

        public CustomMessageBoxForm(string mensaje, string titulo = "Mensaje")
        {
            Text = titulo;
            Size = new Size(400, 200);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;

            // Bordes redondeados para el formulario
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            // Label para el mensaje
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

            // Botones: Si y No
            btnSi = CrearBoton("S�", DialogResult.Yes);
            btnNo = CrearBoton("No", DialogResult.No);

            // Configuraci�n del layout de los botones
            OrganizarBotones();

            // Establecer el bot�n Aceptar (Enter) y Cancelar (Esc)
            AcceptButton = btnSi;
            CancelButton = btnNo;

            // Aseguramos que el formulario est� al frente
            this.TopMost = true;
            this.Activate();
            this.KeyPreview = true;
            this.KeyDown += CustomMessageBoxForm_KeyDown;
        }

        // Crear bot�n con estilo
        private Button CrearBoton(string texto, DialogResult resultado)
        {
            var boton = new Button
            {
                Text = texto,
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                DialogResult = resultado,
            };
            boton.FlatAppearance.BorderSize = 0;
            return boton;
        }

        // Organizar botones en el formulario
        private void OrganizarBotones()
        {
            int padding = 10;
            int totalWidth = btnSi.Width * 2 + padding;
            int startX = (ClientSize.Width - totalWidth) / 2;

            btnSi.Location = new Point(startX, ClientSize.Height - 70);
            btnNo.Location = new Point(btnSi.Right + padding, ClientSize.Height - 70);

            // Si no se deben mostrar los botones de opci�n
            if (!_showTwoButtons)
            {
                btnNo.Visible = false;
                btnSi.Text = "OK";
                btnSi.DialogResult = DialogResult.OK;
            }
            else
            {
                btnSi.Text = _opcion1;
                btnNo.Text = _opcion2;
            }

            Controls.Add(btnSi);
            Controls.Add(btnNo);
        }

        // Manejo de teclas presionadas (Enter)
        private void CustomMessageBoxForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
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

        // M�todos est�ticos para mostrar el cuadro de mensaje

        // M�todo sin IWin32Window, solo con mensaje
        public static DialogResult Mostrar(string mensaje)
        {
            using var box = new CustomMessageBoxForm(mensaje);
            box._showTwoButtons = false;
            return box.ShowDialog();  // No se pasa IWin32Window
        }

        // M�todo con IWin32Window y mensaje
        public static DialogResult Mostrar(IWin32Window owner, string mensaje)
        {
            using var box = new CustomMessageBoxForm(mensaje);
            box._showTwoButtons = false;
            return box.ShowDialog(owner);  // Aqu� s� pasamos IWin32Window
        }

        // M�todo con opciones
        public static DialogResult MostrarOpciones(string mensaje, string opcion1 = "S�", string opcion2 = "No")
        {
            using var box = new CustomMessageBoxForm(mensaje);
            box._showTwoButtons = true;
            box._opcion1 = opcion1;
            box._opcion2 = opcion2;
            return box.ShowDialog();  // No se pasa IWin32Window
        }

        // M�todo con opciones y IWin32Window
        public static DialogResult MostrarOpciones(IWin32Window owner, string mensaje, string opcion1 = "S�", string opcion2 = "No")
        {
            using var box = new CustomMessageBoxForm(mensaje);
            box._showTwoButtons = true;
            box._opcion1 = opcion1;
            box._opcion2 = opcion2;
            return box.ShowDialog(owner);  // Aqu� pasamos IWin32Window
        }
    }
}
