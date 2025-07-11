using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public partial class ProvinciaInputForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int l, int t, int r, int b, int w, int h);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wp, int lp);

        public int CodigoProvincia { get; private set; }
        public string ProvinciaNombre { get; private set; }

        private TextBox txtCodigo;
        private TextBox txtNombre;
        private Button btnAceptar, btnCancelar;

        public ProvinciaInputForm(string titulo, int codigoInicial = 0, string nombreInicial = "")
        {
            InitializeComponent();
            Text = "";
            Size = new Size(800, 300); // más ancho y alto
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(25, 25, 35);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;

            // Posición superior centrada
            Load += (s, e) =>
            {
                var screen = Screen.FromControl(this.Owner ?? this);
                Top = screen.Bounds.Top + 20;
                Left = (screen.Bounds.Width - Width) / 2;
            };

            MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    SendMessage(Handle, 0x112, 0xf012, 0);
            };

            // Código
            var lblCod = new Label
            {
                Text = "Código:",
                AutoSize = true,
                Location = new Point(20, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12)
            };
            EstilosUI.AplicarEstiloLabel(lblCod);
            Controls.Add(lblCod);

            txtCodigo = new TextBox
            {
                Text = codigoInicial > 0 ? codigoInicial.ToString() : "",
                Location = new Point(20, 50),
                Size = new Size(200, 35),
                Font = new Font("Segoe UI", 14)
            };
            Controls.Add(txtCodigo);

            // Nombre
            var lblNom = new Label
            {
                Text = "Nombre de la provincia:",
                AutoSize = true,
                Location = new Point(20, 100),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12)
            };
            EstilosUI.AplicarEstiloLabel(lblNom);
            Controls.Add(lblNom);

            txtNombre = new TextBox
            {
                Text = nombreInicial,
                Location = new Point(20, 130),
                Size = new Size(740, 35),
                Font = new Font("Segoe UI", 14)
            };
            Controls.Add(txtNombre);

            // Estilos al mostrarse
            Shown += (s, e) =>
            {
                EstilosUI.AplicarEstiloTextBox(txtCodigo);
                EstilosUI.AplicarEstiloTextBox(txtNombre);
            };

            // Botones
            // Botones
            int espacioEntreBotones = 30;
            int botonAncho = 120;
            int botonAlto = 45;
            int totalAnchoBotones = (botonAncho * 2) + espacioEntreBotones;
            int leftBase = (this.ClientSize.Width - totalAnchoBotones) / 2;
            int topBotones = this.ClientSize.Height - 80;

            btnAceptar = new Button
            {
                Text = "Aceptar",
                Size = new Size(botonAncho, botonAlto),
                Location = new Point(leftBase, topBotones),
                DialogResult = DialogResult.OK
            };
            EstilosUI.AplicarEstiloBoton(btnAceptar);
            Controls.Add(btnAceptar);

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(botonAncho, botonAlto),
                Location = new Point(leftBase + botonAncho + espacioEntreBotones, topBotones),
                DialogResult = DialogResult.Cancel
            };
            EstilosUI.AplicarEstiloBoton(btnCancelar);
            Controls.Add(btnCancelar);


            // Validación
            btnAceptar.Click += (s, e) =>
            {
                if (!int.TryParse(txtCodigo.Text.Trim(), out int cod) || cod <= 0)
                {
                    MessageBox.Show(this, "El código debe ser un número positivo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show(this, "El nombre no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                CodigoProvincia = cod;
                ProvinciaNombre = txtNombre.Text.Trim();
            };
        }
    }
}
