using Npgsql;
using Sistema_de_Gestion_de_Rentas.Reservas;
using Sistema_de_Gestion_de_Rentas.Data;
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
        private Label lblHuespedId;  // Nueva etiqueta
        private Label lblHospedajeId;  // Nueva etiqueta
        private DateTimePicker dtpFechaEntrada;
        private DateTimePicker dtpFechaSalida;
        private TextBox txtCantidadPersonas;
        private TextBox txtCantidadNoches;
        private TextBox txtTotalCancelar;  // Campo para mostrar el Total a Pagar
        private TextBox txtHuespedId;  // Campo para ingresar Huesped ID
        private TextBox txtHospedajeId;  // Campo para ingresar Hospedaje ID
        private Button btnConfirmar;
        private Button btnCancelar;

        private Hospedaje hospedajeSeleccionado;

        // Constructor que recibe solo el ID del hospedaje
        public ReservacionForm(int idHospedaje)
        {
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
            this.Size = new System.Drawing.Size(450, 600);  // Aumentar el tamaño para los nuevos campos
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InicializarControles()
        {
            // Configurar los controles existentes
            lblTituloHospedaje = new Label
            {
                Text = $"Hospedaje: {hospedajeSeleccionado.Nombre}",
                Location = new System.Drawing.Point(50, 30),
                AutoSize = true
            };
            Controls.Add(lblTituloHospedaje);

            lblPrecio = new Label
            {
                Text = $"Precio por noche: {hospedajeSeleccionado.PrecioPorNoche:C}",
                Location = new System.Drawing.Point(50, 70),
                AutoSize = true
            };
            Controls.Add(lblPrecio);

            lblUbicacion = new Label
            {
                Text = $"Ubicación: {hospedajeSeleccionado.Ubicacion}",
                Location = new System.Drawing.Point(50, 110),
                AutoSize = true
            };
            Controls.Add(lblUbicacion);

            lblCapacidad = new Label
            {
                Text = $"Capacidad: {hospedajeSeleccionado.CapacidadPersonas} personas",
                Location = new System.Drawing.Point(50, 150),
                AutoSize = true
            };
            Controls.Add(lblCapacidad);

            lblHabitaciones = new Label
            {
                Text = $"Habitaciones: {hospedajeSeleccionado.Habitaciones}",
                Location = new System.Drawing.Point(50, 190),
                AutoSize = true
            };
            Controls.Add(lblHabitaciones);

            lblDescripcion = new Label
            {
                Text = $"Descripción: {hospedajeSeleccionado.Descripcion}",
                Location = new System.Drawing.Point(50, 230),
                AutoSize = true
            };
            Controls.Add(lblDescripcion);

            // NUEVO: Agregar etiquetas y campos de texto para Huesped ID y Hospedaje ID
            lblHuespedId = new Label
            {
                Text = "Huésped ID:",
                Location = new System.Drawing.Point(50, 270),
                AutoSize = true
            };
            Controls.Add(lblHuespedId);

            txtHuespedId = new TextBox
            {
                Location = new System.Drawing.Point(150, 270),
                Width = 200
            };
            Controls.Add(txtHuespedId);

            lblHospedajeId = new Label
            {
                Text = "Hospedaje ID:",
                Location = new System.Drawing.Point(50, 310),
                AutoSize = true
            };
            Controls.Add(lblHospedajeId);

            txtHospedajeId = new TextBox
            {
                Text = $"{hospedajeSeleccionado.Id}",  // Muestra el ID del hospedaje automáticamente
                Location = new System.Drawing.Point(150, 310),
                Width = 200,
                ReadOnly = true  // Este campo es de solo lectura
            };
            Controls.Add(txtHospedajeId);

            // Configurar los controles para las fechas y cantidad de personas
            dtpFechaEntrada = new DateTimePicker
            {
                Location = new System.Drawing.Point(50, 350),
                Width = 200
            };
            dtpFechaEntrada.ValueChanged += ActualizarDatos;  // Actualiza los datos cuando cambian las fechas
            Controls.Add(dtpFechaEntrada);

            dtpFechaSalida = new DateTimePicker
            {
                Location = new System.Drawing.Point(50, 390),
                Width = 200
            };
            dtpFechaSalida.ValueChanged += ActualizarDatos;  // Actualiza los datos cuando cambian las fechas
            Controls.Add(dtpFechaSalida);

            txtCantidadPersonas = new TextBox
            {
                Location = new System.Drawing.Point(50, 430),
                Width = 200
            };
            Controls.Add(txtCantidadPersonas);

            txtCantidadNoches = new TextBox
            {
                Location = new System.Drawing.Point(50, 470),
                Width = 200,
                ReadOnly = true  // Campo solo de lectura
            };
            Controls.Add(txtCantidadNoches);

            // NUEVO: Campo para el "Total a cancelar"
            Label lblTotalCancelar = new Label
            {
                Text = "To:",
                Location = new System.Drawing.Point(50, 510),
                AutoSize = true
            };
            Controls.Add(lblTotalCancelar);

            txtTotalCancelar = new TextBox
            {
                Location = new System.Drawing.Point(150, 510),
                Width = 200,
                ReadOnly = true  // Campo solo de lectura
            };
            Controls.Add(txtTotalCancelar);

            btnConfirmar = new Button
            {
                Text = "Confirmar",
                Location = new System.Drawing.Point(50, 550),
                Width = 100
            };
            btnConfirmar.Click += BtnConfirmar_Click;
            Controls.Add(btnConfirmar);

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(200, 550),
                Width = 100
            };
            btnCancelar.Click += BtnCancelar_Click;
            Controls.Add(btnCancelar);
        }

        private void ActualizarDatos(object sender, EventArgs e)
        {
            // Calculamos la cantidad de noches basada en las fechas
            int cantidadNoches = (dtpFechaSalida.Value - dtpFechaEntrada.Value).Days;
            txtCantidadNoches.Text = cantidadNoches.ToString();

            // Realizamos la conversión explícita de decimal a double para evitar el error
            double precioPorNocheDouble = (double)hospedajeSeleccionado.PrecioPorNoche;  // Conversión de decimal a double

            // Calculamos el total a pagar basado en las noches y el precio por noche
            double totalCancelar = cantidadNoches * precioPorNocheDouble;

            // Mostramos el total a cancelar en formato de moneda
            txtTotalCancelar.Text = totalCancelar.ToString("C");
        }


        // En tu clase ReservacionForm.cs
        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            // Obtener todos los datos de la reserva
            int huespedId = int.Parse(txtHuespedId.Text);
            int cantidadPersonas = int.Parse(txtCantidadPersonas.Text);
            DateTime fechaEntrada = dtpFechaEntrada.Value;
            DateTime fechaSalida = dtpFechaSalida.Value;
            int cantidadNoches = int.Parse(txtCantidadNoches.Text);
            double totalCancelar = double.Parse(txtTotalCancelar.Text, System.Globalization.NumberStyles.Currency);  // Convertimos a double

            // Llamar al método para crear la reserva
            CrearReserva(huespedId, hospedajeSeleccionado.Id, fechaEntrada, fechaSalida, cantidadPersonas, cantidadNoches);

            // Llamar al método de la clase PdfForm para generar el PDF
            PdfForm.GenerarPDF(huespedId, hospedajeSeleccionado.Nombre, cantidadPersonas, fechaEntrada, fechaSalida, cantidadNoches, totalCancelar);

            // Mensaje de confirmación
            MessageBox.Show("Reserva confirmada y PDF generado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // En tu clase ReservacionForm.cs
        private void CrearReserva(int huespedId, int hospedajeId, DateTime fechaEntrada, DateTime fechaSalida, int cantidadPersonas, int cantidadNoches)
        {
            try
            {
                var conexion = new Conexion();  // Usamos la clase Conexion para obtener la conexión
                conexion.UsarConexion(conn =>
                {
                    string query = "INSERT INTO reservas (huesped_id, hospedaje_id, fecha_entrada, fecha_salida, cantidad_personas, cantidad_noches) " +
                                   "VALUES (@huesped_id, @hospedaje_id, @fecha_entrada, @fecha_salida, @cantidad_personas, @cantidad_noches)";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        // Definir los parámetros para la consulta
                        cmd.Parameters.AddWithValue("@huesped_id", huespedId);
                        cmd.Parameters.AddWithValue("@hospedaje_id", hospedajeId);
                        cmd.Parameters.AddWithValue("@fecha_entrada", fechaEntrada);
                        cmd.Parameters.AddWithValue("@fecha_salida", fechaSalida);
                        cmd.Parameters.AddWithValue("@cantidad_personas", cantidadPersonas);
                        cmd.Parameters.AddWithValue("@cantidad_noches", cantidadNoches);

                        // Ejecutar la consulta para insertar la reserva
                        cmd.ExecuteNonQuery();
                    }
                });

                // Si todo salió bien
                MessageBox.Show("Reserva almacenada correctamente en la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Manejar cualquier error
                MessageBox.Show($"Error al almacenar la reserva: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
