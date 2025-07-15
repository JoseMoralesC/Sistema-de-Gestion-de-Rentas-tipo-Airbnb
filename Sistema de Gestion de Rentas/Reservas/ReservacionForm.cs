using Npgsql;
using Sistema_de_Gestion_de_Rentas.Data;
using Sistema_de_Gestion_de_Rentas.Forms;
using Sistema_de_Gestion_de_Rentas.Reservas;
using Sistema_de_Gestion_de_Rentas.Services;
using System;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Reservas
{
    public class ReservacionForm : Form
    {
        private Hospedaje hospedajeSeleccionado;
        private ReservaHospedajeService _reservaHospedajeService;

        private int provinciaId;  // NUEVO campo para guardar provinciaId

        // Componentes del formulario
        private Label lblTituloHospedaje, lblPrecio, lblUbicacion, lblCapacidad, lblHabitaciones, lblDescripcion;
        private Label lblHuespedId, lblHospedajeId, lblFechaEntrada, lblFechaSalida, lblCantidadPersonas, lblCantidadNoches, lblTotalCancelar;
        private DateTimePicker dtpFechaEntrada, dtpFechaSalida;
        private TextBox txtCantidadPersonas, txtCantidadNoches, txtTotalCancelar, txtHuespedId;
        private Button btnConfirmar, btnCancelar;

        // Constructor actualizado que recibe el ID del hospedaje y el provinciaId
        public ReservacionForm(int idHospedaje, int provinciaId)
        {
            this.provinciaId = provinciaId;
            this.hospedajeSeleccionado = HospedajeLogic.ObtenerHospedajePorID(idHospedaje);

            if (hospedajeSeleccionado == null)
            {
                CustomMessageBoxForm.Mostrar(this, "Hospedaje no encontrado.");
                this.Close();
                return;
            }

            _reservaHospedajeService = new ReservaHospedajeService();

            ConfigurarFormulario();
            InicializarControles();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Formulario de Reservación";
            this.Size = new System.Drawing.Size(800, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void InicializarControles()
        {
            EstilosUI.AplicarEstiloFormulario(this);

            lblTituloHospedaje = CrearLabel($"Hospedaje: {hospedajeSeleccionado.Nombre}", 50, 30);
            lblPrecio = CrearLabel($"Precio por noche: {hospedajeSeleccionado.PrecioPorNoche:C}", 50, 70);
            lblUbicacion = CrearLabel($"Ubicación: {hospedajeSeleccionado.Ubicacion}", 50, 110);
            lblCapacidad = CrearLabel($"Capacidad: {hospedajeSeleccionado.CapacidadPersonas} personas", 50, 150);
            lblHabitaciones = CrearLabel($"Habitaciones: {hospedajeSeleccionado.Habitaciones}", 50, 190);
            lblDescripcion = CrearLabel($"Descripción: {hospedajeSeleccionado.Descripcion}", 50, 230);

            lblHospedajeId = CrearLabel($"Hospedaje ID: {hospedajeSeleccionado.Id}", 50, 270);
            lblHospedajeId.ForeColor = System.Drawing.Color.White;

            lblHuespedId = CrearLabel("Huésped ID:", 50, 310);
            lblFechaEntrada = CrearLabel("Fecha de Ingreso:", 50, 350);
            lblFechaSalida = CrearLabel("Fecha de Salida:", 50, 390);
            lblCantidadPersonas = CrearLabel("Cantidad de Personas:", 50, 430);
            lblCantidadNoches = CrearLabel("Cantidad de Noches:", 50, 470);
            lblTotalCancelar = CrearLabel("Total a Pagar:", 50, 510);

            txtHuespedId = CrearTextBox(300, 310);
            txtCantidadPersonas = CrearTextBox(300, 430);
            txtCantidadNoches = CrearTextBox(300, 470, true);
            txtTotalCancelar = CrearTextBox(300, 510, true);

            dtpFechaEntrada = CrearDateTimePicker(300, 350);
            dtpFechaSalida = CrearDateTimePicker(300, 390);

            btnConfirmar = CrearBoton("Confirmar", 150, 580);
            btnCancelar = CrearBoton("Cancelar", 450, 580);

            btnConfirmar.Click += BtnConfirmar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            Controls.AddRange(new Control[] {
                lblTituloHospedaje, lblPrecio, lblUbicacion, lblCapacidad, lblHabitaciones, lblDescripcion,
                lblHospedajeId, lblHuespedId, lblFechaEntrada, lblFechaSalida,
                lblCantidadPersonas, lblCantidadNoches, lblTotalCancelar,
                txtHuespedId, txtCantidadPersonas, txtCantidadNoches, txtTotalCancelar,
                dtpFechaEntrada, dtpFechaSalida, btnConfirmar, btnCancelar
            });
        }

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
                Width = 150,
                Height = 40
            };
            EstilosUI.AplicarEstiloBoton(boton);
            return boton;
        }

        private void ActualizarDatos(object sender, EventArgs e)
        {
            if (dtpFechaEntrada.Value > dtpFechaSalida.Value)
            {
                CustomMessageBoxForm.Mostrar(this, "La fecha de entrada no puede ser posterior a la fecha de salida.");
                dtpFechaEntrada.Value = dtpFechaSalida.Value; // Resetea para evitar inconsistencia
                return;
            }

            int cantidadNoches = (dtpFechaSalida.Value - dtpFechaEntrada.Value).Days;
            txtCantidadNoches.Text = cantidadNoches.ToString();

            double precioPorNoche = (double)hospedajeSeleccionado.PrecioPorNoche;
            double totalCancelar = cantidadNoches * precioPorNoche;
            txtTotalCancelar.Text = totalCancelar.ToString("C");
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtHuespedId.Text) || string.IsNullOrWhiteSpace(txtCantidadPersonas.Text))
                {
                    CustomMessageBoxForm.Mostrar(this, "Por favor, complete todos los campos.");
                    return;
                }

                if (!int.TryParse(txtHuespedId.Text, out int huespedId))
                {
                    CustomMessageBoxForm.Mostrar(this, "El ID del huésped debe ser un número válido.");
                    return;
                }

                if (!int.TryParse(txtCantidadPersonas.Text, out int cantidadPersonas))
                {
                    CustomMessageBoxForm.Mostrar(this, "La cantidad de personas debe ser un número válido.");
                    return;
                }

                if (cantidadPersonas > hospedajeSeleccionado.CapacidadPersonas)
                {
                    CustomMessageBoxForm.Mostrar(this, $"La capacidad máxima del hospedaje es de {hospedajeSeleccionado.CapacidadPersonas} personas.");
                    return;
                }

                ModalBackgroundForm modal = new ModalBackgroundForm();
                modal.Show();

                DateTime fechaEntrada = dtpFechaEntrada.Value;
                DateTime fechaSalida = dtpFechaSalida.Value;
                int cantidadNoches = int.Parse(txtCantidadNoches.Text);

                var reserva = new Reserva
                {
                    HuespedId = huespedId,
                    HospedajeId = hospedajeSeleccionado.Id,
                    FechaEntrada = fechaEntrada,
                    FechaSalida = fechaSalida,
                    CantidadPersonas = cantidadPersonas,
                    CantidadNoches = cantidadNoches,
                    ProvinciaId = this.provinciaId  // <-- asignar provinciaId aquí
                };

                _reservaHospedajeService.CrearReserva(reserva);

                modal.Close();
                CustomMessageBoxForm.Mostrar(this, "Reserva confirmada");
                this.Close();

            }
            catch (PostgresException pgEx)
            {
                string mensaje = $"Mensaje: {pgEx.Message}\n" +
                                 $"Detalle: {pgEx.Detail}\n" +
                                 $"Código: {pgEx.SqlState}\n" +
                                 $"Dónde: {pgEx.Where}\n" +
                                 $"Pista: {pgEx.Hint}\n" +
                                 $"Posición: {pgEx.Position}";

                CustomMessageBoxForm.Mostrar(this, $" Error de PostgreSQL:\n\n{mensaje}");
            }
            catch (Exception ex)
            {
                string mensajeErrorCompleto = ObtenerMensajeCompletoExcepcion(ex);
                CustomMessageBoxForm.Mostrar(this, $"Error general al procesar la reserva:\n{mensajeErrorCompleto}");
            }
        }


        private string ObtenerMensajeCompletoExcepcion(Exception ex)
        {
            string mensaje = ex.Message;
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                mensaje += "\n-> " + inner.Message;
                inner = inner.InnerException;
            }
            return mensaje;
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
