using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public partial class ReservaInputForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int l, int t, int r, int b, int w, int h);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wp, int lp);

        public int HuespedID { get; private set; }
        public int HospedajeID { get; private set; }
        public DateTime FechaEntrada { get; private set; }
        public DateTime FechaSalida { get; private set; }
        public int CantidadPersonas { get; private set; }
        public int CantidadNoches { get; private set; }
        public bool Estado { get; private set; }

        private TextBox txtHuespedID, txtHospedajeID, txtCantidadPersonas, txtCantidadNoches;
        private DateTimePicker dtpFechaEntrada, dtpFechaSalida;
        private Button btnAceptar, btnCancelar;

        public ReservaInputForm(string titulo, int huespedID = 0, int hospedajeID = 0, DateTime? fechaEntrada = null, DateTime? fechaSalida = null, int cantidadPersonas = 0, int cantidadNoches = 0, bool estado = false)
        {
            InitializeComponent();
            Text = "";
            Size = new Size(500, 650); 
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(25, 25, 35);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;

            
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

            
            int posY = 20; 
            int margenHorizontal = 20; 
            int anchoCampos = this.ClientSize.Width - (margenHorizontal * 2); 

            
            var lblHuesped = new Label
            {
                Text = "ID Huesped:",
                AutoSize = true,
                Location = new Point(20, posY),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12)
            };
            EstilosUI.AplicarEstiloLabel(lblHuesped);
            Controls.Add(lblHuesped);

            txtHuespedID = new TextBox
            {
                Text = huespedID > 0 ? huespedID.ToString() : "",
                Location = new Point(margenHorizontal, posY + 30),
                Size = new Size(anchoCampos, 35),
                Font = new Font("Segoe UI", 14)
            };
            Controls.Add(txtHuespedID);

            posY += 70; 

            var lblHospedaje = new Label
            {
                Text = "ID Hospedaje:",
                AutoSize = true,
                Location = new Point(20, posY),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12)
            };
            EstilosUI.AplicarEstiloLabel(lblHospedaje);
            Controls.Add(lblHospedaje);

            txtHospedajeID = new TextBox
            {
                Text = hospedajeID > 0 ? hospedajeID.ToString() : "",
                Location = new Point(margenHorizontal, posY + 30),
                Size = new Size(anchoCampos, 35),
                Font = new Font("Segoe UI", 14)
            };
            Controls.Add(txtHospedajeID);

            posY += 70; 

            var lblFechaEntrada = new Label
            {
                Text = "Fecha Entrada:",
                AutoSize = true,
                Location = new Point(20, posY),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12)
            };
            EstilosUI.AplicarEstiloLabel(lblFechaEntrada);
            Controls.Add(lblFechaEntrada);

            dtpFechaEntrada = new DateTimePicker
            {
                Value = fechaEntrada ?? DateTime.Now,
                Location = new Point(margenHorizontal, posY + 30),
                Size = new Size(anchoCampos, 35),
                Font = new Font("Segoe UI", 14)
            };
            Controls.Add(dtpFechaEntrada);

            posY += 70;

            
            var lblFechaSalida = new Label
            {
                Text = "Fecha Salida:",
                AutoSize = true,
                Location = new Point(20, posY),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12)
            };
            EstilosUI.AplicarEstiloLabel(lblFechaSalida);
            Controls.Add(lblFechaSalida);

            dtpFechaSalida = new DateTimePicker
            {
                Value = fechaSalida ?? DateTime.Now,
                Location = new Point(margenHorizontal, posY + 30),
                Size = new Size(anchoCampos, 35),
                Font = new Font("Segoe UI", 14)
            };
            Controls.Add(dtpFechaSalida);

            posY += 70;

            
            var lblPersonas = new Label
            {
                Text = "Cantidad de Personas:",
                AutoSize = true,
                Location = new Point(20, posY),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12)
            };
            EstilosUI.AplicarEstiloLabel(lblPersonas);
            Controls.Add(lblPersonas);

            txtCantidadPersonas = new TextBox
            {
                Text = cantidadPersonas.ToString(),
                Location = new Point(margenHorizontal, posY + 30),
                Size = new Size(anchoCampos, 35),
                Font = new Font("Segoe UI", 14)
            };
            Controls.Add(txtCantidadPersonas);

            posY += 70;

            
            var lblNoches = new Label
            {
                Text = "Cantidad de Noches:",
                AutoSize = true,
                Location = new Point(20, posY),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12)
            };
            EstilosUI.AplicarEstiloLabel(lblNoches);
            Controls.Add(lblNoches);

            txtCantidadNoches = new TextBox
            {
                Text = cantidadNoches.ToString(),
                Location = new Point(margenHorizontal, posY + 30),
                Size = new Size(anchoCampos, 35),
                Font = new Font("Segoe UI", 14)
            };
            Controls.Add(txtCantidadNoches);

            posY += 70;

            
            var lblEstado = new Label
            {
                Text = "Estado:",
                AutoSize = true,
                Location = new Point(20, posY),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12)
            };
            EstilosUI.AplicarEstiloLabel(lblEstado);
            Controls.Add(lblEstado);

            
            var lblEstadoValor = new Label
            {
                Text = estado ? "Activo" : "Cancelado", 
                Location = new Point(margenHorizontal, posY + 30),
                Size = new Size(anchoCampos, 35),
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.White, 
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblEstadoValor);

            posY += 70;

            
            int espacioEntreBotones = 30;
            int botonAncho = 120;
            int botonAlto = 45;

            
            int leftBase = (this.ClientSize.Width - (botonAncho * 2 + espacioEntreBotones)) / 2;
            int topBotones = posY + 30;

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

            
            btnAceptar.Click += (s, e) =>
            {
                if (!int.TryParse(txtHuespedID.Text.Trim(), out int huespedID) || huespedID <= 0)
                {
                    MessageBox.Show(this, "El ID de huésped debe ser un número positivo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (!int.TryParse(txtHospedajeID.Text.Trim(), out int hospedajeID) || hospedajeID <= 0)
                {
                    MessageBox.Show(this, "El ID de hospedaje debe ser un número positivo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCantidadPersonas.Text) || !int.TryParse(txtCantidadPersonas.Text, out int cantidadPersonas) || cantidadPersonas <= 0)
                {
                    MessageBox.Show(this, "La cantidad de personas debe ser un número positivo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCantidadNoches.Text) || !int.TryParse(txtCantidadNoches.Text, out int cantidadNoches) || cantidadNoches <= 0)
                {
                    MessageBox.Show(this, "La cantidad de noches debe ser un número positivo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                HuespedID = huespedID;
                HospedajeID = hospedajeID;
                FechaEntrada = dtpFechaEntrada.Value;
                FechaSalida = dtpFechaSalida.Value;
                CantidadPersonas = cantidadPersonas;
                CantidadNoches = cantidadNoches;
                Estado = estado;
            };
        }
    }
}
