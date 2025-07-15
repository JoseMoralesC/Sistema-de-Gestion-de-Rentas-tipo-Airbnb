using Npgsql;
using Sistema_de_Gestion_de_Rentas.Data;
using Sistema_de_Gestion_de_Rentas.Forms;
using Sistema_de_Gestion_de_Rentas.Reservas;
using System;
using System.Windows.Forms;

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
        private Label lblHuespedId;
        private Label lblHospedajeId;
        private Label lblFechaEntrada;
        private Label lblFechaSalida;
        private Label lblCantidadPersonas;
        private Label lblCantidadNoches;
        private Label lblTotalCancelar;

        private DateTimePicker dtpFechaEntrada;
        private DateTimePicker dtpFechaSalida;
        private TextBox txtCantidadPersonas;
        private TextBox txtCantidadNoches;
        private TextBox txtTotalCancelar;
        private TextBox txtHuespedId;
        private Button btnConfirmar;
        private Button btnCancelar;

        private Hospedaje hospedajeSeleccionado;

        // Constructor que recibe solo el ID del hospedaje
        public ReservacionForm(int idHospedaje)
        {
            this.hospedajeSeleccionado = HospedajeLogic.ObtenerHospedajePorID(idHospedaje);

            if (hospedajeSeleccionado == null)
            {
                CustomMessageBoxForm.Mostrar(this, "Hospedaje no encontrado.");
                this.Close();
                return;
            }

            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Formulario de Reservación";
            this.Size = new System.Drawing.Size(800, 700);  // Reducimos el tamaño del formulario
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None; // Eliminar la barra de título
        }

        private void InicializarControles()
        {
            EstilosUI.AplicarEstiloFormulario(this);

            // Titulo del hospedaje
            lblTituloHospedaje = CrearLabel($"Hospedaje: {hospedajeSeleccionado.Nombre}", 50, 30);
            lblPrecio = CrearLabel($"Precio por noche: {hospedajeSeleccionado.PrecioPorNoche:C}", 50, 70);
            lblUbicacion = CrearLabel($"Ubicación: {hospedajeSeleccionado.Ubicacion}", 50, 110);
            lblCapacidad = CrearLabel($"Capacidad: {hospedajeSeleccionado.CapacidadPersonas} personas", 50, 150);
            lblHabitaciones = CrearLabel($"Habitaciones: {hospedajeSeleccionado.Habitaciones}", 50, 190);
            lblDescripcion = CrearLabel($"Descripción: {hospedajeSeleccionado.Descripcion}", 50, 230);

            // Etiquetas para los campos de ingreso de datos
            lblHospedajeId = CrearLabel($"Hospedaje ID: {hospedajeSeleccionado.Id}", 50, 270);
            lblHospedajeId.ForeColor = System.Drawing.Color.White; // Cambio de color de texto a blanco
            lblHuespedId = CrearLabel("Huésped ID:", 50, 310);
            lblFechaEntrada = CrearLabel("Fecha de Ingreso:", 50, 350);
            lblFechaSalida = CrearLabel("Fecha de Salida:", 50, 390);
            lblCantidadPersonas = CrearLabel("Cantidad de Personas:", 50, 430);
            lblCantidadNoches = CrearLabel("Cantidad de Noches:", 50, 470);
            lblTotalCancelar = CrearLabel("Total a Pagar:", 50, 510);

            // Campos de ingreso de datos (Columna de datos)
            txtHuespedId = CrearTextBox(300, 310);
            txtCantidadPersonas = CrearTextBox(300, 430);
            txtCantidadNoches = CrearTextBox(300, 470, true);
            txtTotalCancelar = CrearTextBox(300, 510, true);

            // Campos de fecha (Columna de fechas)
            dtpFechaEntrada = CrearDateTimePicker(300, 350); // Alineamos la fecha de ingreso
            dtpFechaSalida = CrearDateTimePicker(300, 390);  // Alineamos la fecha de salida

            // Botones (Centrados y separados)
            btnConfirmar = CrearBoton("Confirmar", 150, 580);
            btnCancelar = CrearBoton("Cancelar", 450, 580);

            btnConfirmar.Click += BtnConfirmar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            Controls.Add(lblTituloHospedaje);
            Controls.Add(lblPrecio);
            Controls.Add(lblUbicacion);
            Controls.Add(lblCapacidad);
            Controls.Add(lblHabitaciones);
            Controls.Add(lblDescripcion);
            Controls.Add(lblHospedajeId);
            Controls.Add(lblHuespedId);
            Controls.Add(lblFechaEntrada);
            Controls.Add(lblFechaSalida);
            Controls.Add(lblCantidadPersonas);
            Controls.Add(lblCantidadNoches);
            Controls.Add(lblTotalCancelar);

            Controls.Add(txtHuespedId);
            Controls.Add(txtCantidadPersonas);
            Controls.Add(txtCantidadNoches);
            Controls.Add(txtTotalCancelar);

            Controls.Add(dtpFechaEntrada);
            Controls.Add(dtpFechaSalida);
            Controls.Add(btnConfirmar);
            Controls.Add(btnCancelar);
        }

        // Métodos de ayuda para crear controles
        private Label CrearLabel(string texto, int x, int y)
        {
            var label = new Label
            {
                Text = texto,
                Location = new System.Drawing.Point(x, y),
                AutoSize = true
            };
            EstilosUI.AplicarEstiloLabel(label);
            return label;
        }

        private TextBox CrearTextBox(int x, int y, bool readonlyText = false)
        {
            var textBox = new TextBox
            {
                Location = new System.Drawing.Point(x, y),
                Width = 250,
                ReadOnly = readonlyText
            };
            EstilosUI.AplicarEstiloTextBox(textBox);
            return textBox;
        }

        private DateTimePicker CrearDateTimePicker(int x, int y)
        {
            var dateTimePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(x, y),
                Width = 250
            };
            EstilosUI.AplicarEstiloDateTimePicker(dateTimePicker);
            dateTimePicker.ValueChanged += ActualizarDatos;
            return dateTimePicker;
        }

        private Button CrearBoton(string texto, int x, int y)
        {
            var boton = new Button
            {
                Text = texto,
                Location = new System.Drawing.Point(x, y),
                Width = 150,   // Botón más grande
                Height = 40    // Más alto
            };
            EstilosUI.AplicarEstiloBoton(boton);
            return boton;
        }

        private void ActualizarDatos(object sender, EventArgs e)
        {
            int cantidadNoches = (dtpFechaSalida.Value - dtpFechaEntrada.Value).Days;
            txtCantidadNoches.Text = cantidadNoches.ToString();

            double precioPorNocheDouble = (double)hospedajeSeleccionado.PrecioPorNoche;
            double totalCancelar = cantidadNoches * precioPorNocheDouble;
            txtTotalCancelar.Text = totalCancelar.ToString("C");
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            // Obtener la cantidad de personas ingresadas
            int cantidadPersonas = int.Parse(txtCantidadPersonas.Text);

            // Verificar si la cantidad de personas excede la capacidad máxima
            if (cantidadPersonas > hospedajeSeleccionado.CapacidadPersonas)
            {
                // Mostrar mensaje de error
                CustomMessageBoxForm.Mostrar(this, $"La capacidad máxima del hospedaje es de {hospedajeSeleccionado.CapacidadPersonas} personas. No se puede reservar para {cantidadPersonas} personas.");
                return;  // Detener el proceso si la validación no se cumple
            }

            // Mostrar modal mientras se procesa la reserva
            ModalBackgroundForm modal = new ModalBackgroundForm();
            modal.ShowDialog();

            int huespedId = int.Parse(txtHuespedId.Text);
            DateTime fechaEntrada = dtpFechaEntrada.Value;
            DateTime fechaSalida = dtpFechaSalida.Value;
            int cantidadNoches = int.Parse(txtCantidadNoches.Text);
            double totalCancelar = double.Parse(txtTotalCancelar.Text, System.Globalization.NumberStyles.Currency);

            // Crear la reserva en la base de datos
            CrearReserva(huespedId, hospedajeSeleccionado.Id, fechaEntrada, fechaSalida, cantidadPersonas, cantidadNoches);

            // Generar PDF de la reserva
            PdfForm.GenerarPDF(huespedId, hospedajeSeleccionado.Nombre, cantidadPersonas, fechaEntrada, fechaSalida, cantidadNoches, totalCancelar);

            // Cerrar el modal y mostrar confirmación
            modal.Close();
            CustomMessageBoxForm.Mostrar(this, "Reserva confirmada y PDF generado.");
        }


        private void CrearReserva(int huespedId, int hospedajeId, DateTime fechaEntrada, DateTime fechaSalida, int cantidadPersonas, int cantidadNoches)
        {
            try
            {
                var conexion = new Conexion();
                conexion.UsarConexion(conn =>
                {
                    string query = "INSERT INTO reservas (huesped_id, hospedaje_id, fecha_entrada, fecha_salida, cantidad_personas, cantidad_noches) " +
                                   "VALUES (@huesped_id, @hospedaje_id, @fecha_entrada, @fecha_salida, @cantidad_personas, @cantidad_noches)";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@huesped_id", huespedId);
                        cmd.Parameters.AddWithValue("@hospedaje_id", hospedajeId);
                        cmd.Parameters.AddWithValue("@fecha_entrada", fechaEntrada);
                        cmd.Parameters.AddWithValue("@fecha_salida", fechaSalida);
                        cmd.Parameters.AddWithValue("@cantidad_personas", cantidadPersonas);
                        cmd.Parameters.AddWithValue("@cantidad_noches", cantidadNoches);

                        cmd.ExecuteNonQuery();
                    }
                });

                CustomMessageBoxForm.Mostrar(this, "Reserva almacenada correctamente.");
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Mostrar(this, $"Error al almacenar la reserva: {ex.Message}");
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
