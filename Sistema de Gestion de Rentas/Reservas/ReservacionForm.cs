using System;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Reservas;

namespace Sistema_de_Gestion_de_Rentas.Reservas
{
    public class ReservacionForm : Form
    {
        private Label lblTituloHospedaje;
        private Label lblPrecio;
        private Label lblUbicacion;
        private Label lblCapacidad;
        private Label lblHabitaciones;
        private Label lblDescripcion;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtCorreo;
        private Button btnConfirmar;
        private Button btnCancelar;

        private Hospedaje hospedajeSeleccionado;

        // Constructor que recibe el ID del hospedaje
        public ReservacionForm(int idHospedaje)
        {
            // Obtenemos el hospedaje usando el ID
            this.hospedajeSeleccionado = HospedajeLogic.ObtenerHospedajePorID(idHospedaje);

            if (hospedajeSeleccionado == null)
            {
                MessageBox.Show("Hospedaje no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Formulario de Reservación";
            this.Size = new System.Drawing.Size(450, 500); // Ajustamos el tamaño
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar el formulario
        }

        private void InicializarControles()
        {
            // Título del hospedaje
            lblTituloHospedaje = new Label
            {
                Text = $"Hospedaje: {hospedajeSeleccionado.Nombre}",
                Location = new System.Drawing.Point(50, 30),
                AutoSize = true
            };
            Controls.Add(lblTituloHospedaje);

            // Precio del hospedaje
            lblPrecio = new Label
            {
                Text = $"Precio: {hospedajeSeleccionado.PrecioPorNoche:C}",
                Location = new System.Drawing.Point(50, 70),
                AutoSize = true
            };
            Controls.Add(lblPrecio);

            // Ubicación del hospedaje
            lblUbicacion = new Label
            {
                Text = $"Ubicación: {hospedajeSeleccionado.Ubicacion}",
                Location = new System.Drawing.Point(50, 110),
                AutoSize = true
            };
            Controls.Add(lblUbicacion);

            // Capacidad de personas
            lblCapacidad = new Label
            {
                Text = $"Capacidad: {hospedajeSeleccionado.CapacidadPersonas} personas",
                Location = new System.Drawing.Point(50, 150),
                AutoSize = true
            };
            Controls.Add(lblCapacidad);

            // Habitaciones
            lblHabitaciones = new Label
            {
                Text = $"Habitaciones: {hospedajeSeleccionado.Habitaciones}",
                Location = new System.Drawing.Point(50, 190),
                AutoSize = true
            };
            Controls.Add(lblHabitaciones);

            // Descripción del hospedaje
            lblDescripcion = new Label
            {
                Text = $"Descripción: {hospedajeSeleccionado.Descripcion}",
                Location = new System.Drawing.Point(50, 230),
                AutoSize = true
            };
            Controls.Add(lblDescripcion);

            // Datos del usuario (Nombre, Apellido, Correo)
            txtNombre = new TextBox
            {
                PlaceholderText = "Nombre",
                Location = new System.Drawing.Point(50, 270),
                Width = 300
            };
            Controls.Add(txtNombre);

            txtApellido = new TextBox
            {
                PlaceholderText = "Apellido",
                Location = new System.Drawing.Point(50, 310),
                Width = 300
            };
            Controls.Add(txtApellido);

            txtCorreo = new TextBox
            {
                PlaceholderText = "Correo Electrónico",
                Location = new System.Drawing.Point(50, 350),
                Width = 300
            };
            Controls.Add(txtCorreo);

            // Botón de confirmación
            btnConfirmar = new Button
            {
                Text = "Confirmar",
                Location = new System.Drawing.Point(50, 390)
            };
            btnConfirmar.Click += BtnConfirmar_Click;
            Controls.Add(btnConfirmar);

            // Botón de cancelación
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(150, 390)
            };
            btnCancelar.Click += BtnCancelar_Click;
            Controls.Add(btnCancelar);
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"¡Reservación Confirmada para {hospedajeSeleccionado.Nombre}!", "Confirmación", MessageBoxButtons.OK);
            this.Close();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
