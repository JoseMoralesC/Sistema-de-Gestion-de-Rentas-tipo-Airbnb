using System;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Reservas
{
    public class ReservacionForm : Form
    {
        private Label lblTituloHospedaje;
        private Label lblPrecio;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtCorreo;
        private Button btnConfirmar;
        private Button btnCancelar;

        private string nombreHospedaje;
        private decimal precioHospedaje;

        public ReservacionForm(string nombreHospedaje, decimal precioHospedaje)
        {
            this.nombreHospedaje = nombreHospedaje;
            this.precioHospedaje = precioHospedaje;

            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Formulario de Reservación";
            this.Size = new System.Drawing.Size(400, 300);
        }

        private void InicializarControles()
        {
            // Título del hospedaje
            lblTituloHospedaje = new Label
            {
                Text = $"Hospedaje: {nombreHospedaje}",
                Location = new System.Drawing.Point(50, 30),
                AutoSize = true
            };
            Controls.Add(lblTituloHospedaje);

            // Precio del hospedaje
            lblPrecio = new Label
            {
                Text = $"Precio: {precioHospedaje:C}",
                Location = new System.Drawing.Point(50, 70),
                AutoSize = true
            };
            Controls.Add(lblPrecio);

            // Datos del usuario (Nombre, Apellido, Correo)
            txtNombre = new TextBox
            {
                PlaceholderText = "Nombre",
                Location = new System.Drawing.Point(50, 110),
                Width = 300
            };
            Controls.Add(txtNombre);

            txtApellido = new TextBox
            {
                PlaceholderText = "Apellido",
                Location = new System.Drawing.Point(50, 150),
                Width = 300
            };
            Controls.Add(txtApellido);

            txtCorreo = new TextBox
            {
                PlaceholderText = "Correo Electrónico",
                Location = new System.Drawing.Point(50, 190),
                Width = 300
            };
            Controls.Add(txtCorreo);

            // Botón de confirmación
            btnConfirmar = new Button
            {
                Text = "Confirmar",
                Location = new System.Drawing.Point(50, 230)
            };
            btnConfirmar.Click += BtnConfirmar_Click;
            Controls.Add(btnConfirmar);

            // Botón de cancelación
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(150, 230)
            };
            btnCancelar.Click += BtnCancelar_Click;
            Controls.Add(btnCancelar);
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"¡Reservación Confirmada para {nombreHospedaje}!", "Confirmación", MessageBoxButtons.OK);
            this.Close();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
