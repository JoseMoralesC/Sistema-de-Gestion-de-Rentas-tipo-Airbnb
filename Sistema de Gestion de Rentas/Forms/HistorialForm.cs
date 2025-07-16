using Sistema_de_Gestion_de_Rentas.Data;
using Sistema_de_Gestion_de_Rentas.Reservas;
using Sistema_de_Gestion_de_Rentas.Services;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using iText.Kernel.Colors;
using SystemColor = System.Drawing.Color;  // Alias para System.Drawing.Color
using iTextColor = iText.Kernel.Colors.Color;  // Alias para iText.Kernel.Colors.Color

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
            // Aumentamos el tamaño del formulario para darle más espacio
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = SystemColor.FromArgb(30, 30, 40);  // Color de fondo
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 700);  // Aumentamos tanto el ancho como el alto
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void InicializarControles()
        {
            // Título del formulario
            lblTitulo = new Label
            {
                Text = "Historial de Reservas",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = SystemColor.White,  // Color del texto
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };
            EstilosUI.AplicarEstiloLabel(lblTitulo);
            Controls.Add(lblTitulo);

            // Panel de reservas
            panelReservas = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoScroll = true,
                Width = this.ClientSize.Width,
                Height = this.ClientSize.Height - 180,  // Ajustamos la altura para los botones
                Location = new Point(0, 60),
                Padding = new Padding(20),
            };
            Controls.Add(panelReservas);

            // Botones de "Imprimir", "Actualizar" y "Cerrar"
            var panelBotones = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,  // Alineamos los botones horizontalmente
                Dock = DockStyle.Bottom,
                Height = 60,  // Altura fija para los botones
                Padding = new Padding(10),
                Margin = new Padding(0),
                AutoSize = true
            };

            // Botón Imprimir
            btnImprimir = new Button
            {
                Text = "Imprimir",
                Width = 140,
                Height = 40,
            };
            EstilosUI.AplicarEstiloBoton(btnImprimir);
            btnImprimir.Click += BtnImprimir_Click;
            panelBotones.Controls.Add(btnImprimir);

            // Botón Actualizar
            btnActualizar = new Button
            {
                Text = "Actualizar",
                Width = 140,
                Height = 40,
            };
            EstilosUI.AplicarEstiloBoton(btnActualizar);
            btnActualizar.Click += (s, e) => CargarHistorial();
            panelBotones.Controls.Add(btnActualizar);

            // Botón Cerrar
            btnCerrar = new Button
            {
                Text = "Cerrar",
                Width = 140,
                Height = 40,
            };
            EstilosUI.AplicarEstiloBoton(btnCerrar);
            btnCerrar.Click += (s, e) => this.Close();
            panelBotones.Controls.Add(btnCerrar);

            Controls.Add(panelBotones);
        }

        private void CargarHistorial()
        {
            panelReservas.Controls.Clear();

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
                        ForeColor = SystemColor.White,
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
                    string lugar = ReservaDAO.ObtenerLugarPorHospedajeId(reserva.HospedajeId);
                    if (string.IsNullOrEmpty(lugar))
                    {
                        lugar = "Lugar no disponible";
                    }

                    Label lblReserva = new Label
                    {
                        Text = $"Lugar: {lugar}\n" +
                               $"Reserva: {reserva.FechaEntrada:dd/MM/yyyy} a {reserva.FechaSalida:dd/MM/yyyy}\n" +
                               $"{reserva.CantidadPersonas} persona(s), {reserva.CantidadNoches} noche(s)",
                        Font = new Font("Segoe UI", 12),
                        ForeColor = SystemColor.White,
                        AutoSize = false,
                        Width = panelReservas.Width - 40,
                        Height = 130,
                        Padding = new Padding(10),
                        Margin = new Padding(5),
                        BackColor = SystemColor.FromArgb(50, 50, 60),
                        TextAlign = ContentAlignment.TopLeft
                    };
                    panelReservas.Controls.Add(lblReserva);

                    Button btnActualizar = new Button
                    {
                        Text = "Cancelar",
                        Width = 120,
                        Height = 40,
                        Margin = new Padding(5),
                        Tag = reserva
                    };
                    EstilosUI.AplicarEstiloBoton(btnActualizar);
                    btnActualizar.Click += BtnActualizar_Click;
                    panelReservas.Controls.Add(btnActualizar);
                }
            }
            catch (InvalidOperationException ex)
            {
                CustomMessageBoxForm.MostrarOpciones("Error al cargar el historial: " + ex.Message, "Aceptar", "");
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            var reserva = (ReservaHistorialDTO)((Button)sender).Tag;

            DialogResult result = MessageBox.Show(
                "¿Estás seguro de que deseas cancelar esta reserva?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                bool actualizacionExitosa = ReservaDAO.ActualizarEstadoReserva(reserva.Id, false);

                if (actualizacionExitosa)
                {
                    CustomMessageBoxForm.MostrarOpciones("La reserva ha sido cancelada exitosamente.", "Aceptar", "");
                    CargarHistorial();
                }
                else
                {
                    CustomMessageBoxForm.MostrarOpciones("Hubo un problema al intentar cancelar la reserva. Por favor, inténtalo de nuevo.", "Aceptar", "");
                }
            }
            else
            {
                CustomMessageBoxForm.MostrarOpciones("La cancelación fue cancelada.", "Aceptar", "");
            }
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                int idHuesped = SesionUsuario.ObtenerIdComoInt();
                var reservas = ReservaDAO.ObtenerReservasPorHuespedId(idHuesped);

                if (reservas == null || reservas.Count == 0)
                {
                    CustomMessageBoxForm.MostrarOpciones("No tienes reservas para imprimir.", "Aceptar", "");
                    return;
                }

                ExportarHistorialReservasPDF(reservas);
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.MostrarOpciones($"Error al generar el PDF: {ex.Message}", "Aceptar", "");
            }
        }

        private void ExportarHistorialReservasPDF(List<ReservaHistorialDTO> reservas)
        {
            string pdfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Historial_Reservas.pdf");

            // Intentamos eliminar el archivo si ya existe
            if (File.Exists(pdfPath))
            {
                try
                {
                    File.Delete(pdfPath);  // Eliminar el archivo antes de crearlo
                }
                catch (IOException ex)
                {
                    CustomMessageBoxForm.MostrarOpciones($"No se pudo eliminar el archivo existente: {ex.Message}", "Aceptar", "");
                    return;
                }
            }

            try
            {
                // Crear el documento PDF
                using (var writer = new PdfWriter(pdfPath))
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);
                    // Código de creación de PDF aquí

                    CustomMessageBoxForm.MostrarOpciones($"El archivo PDF ha sido exportado correctamente a {pdfPath}", "Aceptar", "");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.MostrarOpciones($"Error al generar el PDF: {ex.Message}", "Aceptar", "");
            }
        }
    }
}
