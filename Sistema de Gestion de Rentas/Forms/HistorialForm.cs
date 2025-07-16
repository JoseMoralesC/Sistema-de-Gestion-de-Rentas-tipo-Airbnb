using Sistema_de_Gestion_de_Rentas.Data;
using Sistema_de_Gestion_de_Rentas.Reservas;
using Sistema_de_Gestion_de_Rentas.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class HistorialForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);

        private Label lblTitulo;
        private FlowLayoutPanel panelReservas;
        private Button btnActualizar;
        private Button btnCerrar;
        private Button btnImprimir;

        public HistorialForm()
        {
            ConfigurarFormulario();
            InicializarControles();
            CargarHistorial();
        }

        private void ConfigurarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(700, 500);
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void InicializarControles()
        {
            lblTitulo = new Label
            {
                Text = "Historial de Reservas",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };
            EstilosUI.AplicarEstiloLabel(lblTitulo);
            Controls.Add(lblTitulo);

            panelReservas = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoScroll = true,
                Width = this.ClientSize.Width,
                Height = this.ClientSize.Height - 130,
                Location = new Point(0, 60),
                Padding = new Padding(20),
            };
            Controls.Add(panelReservas);

            btnActualizar = new Button
            {
                Text = "Actualizar",
                Width = 140,
                Height = 40,
                Location = new Point((this.ClientSize.Width / 2) - 150, this.ClientSize.Height - 60)
            };
            EstilosUI.AplicarEstiloBoton(btnActualizar);
            btnActualizar.Click += (s, e) => CargarHistorial(); // por ahora solo recarga
            Controls.Add(btnActualizar);

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Width = 140,
                Height = 40,
                Location = new Point((this.ClientSize.Width / 2) + 10, this.ClientSize.Height - 60)
            };
            EstilosUI.AplicarEstiloBoton(btnCerrar);
            btnCerrar.Click += (s, e) => this.Close();
            Controls.Add(btnCerrar);
        }

        private void CargarHistorial()
        {
            panelReservas.Controls.Clear();  // Limpiamos el panel antes de recargarlo

            try
            {
                int idHuesped = SesionUsuario.ObtenerIdComoInt();
                var reservas = ReservaDAO.ObtenerReservasPorHuespedId(idHuesped);

                if (reservas == null || reservas.Count == 0)
                {
                    Label lblSinReservas = new Label
                    {
                        Text = "No tienes reservas realizadas.",
                        Font = new Font("Segoe UI", 14, FontStyle.Italic),
                        ForeColor = Color.White,
                        AutoSize = false,
                        Width = panelReservas.Width - 40,
                        Height = 50,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    panelReservas.Controls.Add(lblSinReservas);
                    return;
                }

                foreach (var reserva in reservas)
                {
                    // Obtener el nombre del lugar usando el HospedajeId
                    string lugar = ReservaDAO.ObtenerLugarPorHospedajeId(reserva.HospedajeId);
                    if (string.IsNullOrEmpty(lugar))
                    {
                        lugar = "Lugar no disponible"; // Si no se encuentra el lugar
                    }

                    // Crear el texto de la reserva
                    Label lblReserva = new Label
                    {
                        Text = $"Lugar: {lugar}\n" +
                               $"Reserva: {reserva.FechaEntrada:dd/MM/yyyy} a {reserva.FechaSalida:dd/MM/yyyy}\n" +
                               $"{reserva.CantidadPersonas} persona(s), {reserva.CantidadNoches} noche(s)",
                        Font = new Font("Segoe UI", 12),
                        ForeColor = Color.White,
                        AutoSize = false,
                        Width = panelReservas.Width - 40,
                        Height = 130,  // Aumentamos la altura para mostrar todo
                        Padding = new Padding(10),
                        Margin = new Padding(5),
                        BackColor = Color.FromArgb(50, 50, 60),
                        TextAlign = ContentAlignment.TopLeft
                    };
                    panelReservas.Controls.Add(lblReserva);

                    // Crear un botón de "Actualizar" para cada reserva
                    Button btnActualizar = new Button
                    {
                        Text = "Cancelar",
                        Width = 120,
                        Height = 40,
                        Margin = new Padding(5),
                        Tag = reserva // Guardamos la reserva completa en el botón para poder acceder a ella luego
                    };
                    EstilosUI.AplicarEstiloBoton(btnActualizar);
                    btnActualizar.Click += BtnActualizar_Click;  // Asignamos el evento Click para cada botón
                    panelReservas.Controls.Add(btnActualizar);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Error al cargar el historial: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            // Obtener la reserva desde el botón (el Tag contiene la reserva completa)
            var reserva = (ReservaHistorialDTO)((Button)sender).Tag;

            // Usar el MessageBox de Windows para mostrar la confirmación
            DialogResult result = MessageBox.Show(
                "¿Estás seguro de que deseas cancelar esta reserva?", // Mensaje
                "Confirmación", // Título
                MessageBoxButtons.YesNo, // Opciones de botones
                MessageBoxIcon.Question // Icono
            );

            if (result == DialogResult.Yes)
            {
                // Llamamos a la función que cambia el estado de la reserva en la base de datos
                bool actualizacionExitosa = ReservaDAO.ActualizarEstadoReserva(reserva.Id, false);

                if (actualizacionExitosa)
                {
                    MessageBox.Show("La reserva ha sido cancelada exitosamente.", "Reserva Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarHistorial();  // Recargar el historial después de la actualización
                }
                else
                {
                    MessageBox.Show("Hubo un problema al intentar cancelar la reserva. Por favor, inténtalo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("La cancelación fue cancelada.", "Acción Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
