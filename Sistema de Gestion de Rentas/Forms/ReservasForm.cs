using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Npgsql;
using Sistema_de_Gestion_de_Rentas.Data;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public partial class ReservasForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int l, int t, int r, int b, int w, int h);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wp, int lp);

        private Label lblTitulo;
        private DataGridView dgvReservas;
        private Button btnAgregar, btnEditar, btnEliminar, btnActualizar, btnCerrar;

        public ReservasForm()
        {
            InitializeComponent();
            ConfigurarFormulario();
            CargarDatos();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(25, 25, 35);
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            this.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0x112, 0xf012, 0);
                }
            };

            lblTitulo = new Label
            {
                Text = "Reservas",
                AutoSize = true,
                Location = new Point(20, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20, FontStyle.Bold)
            };
            EstilosUI.AplicarEstiloLabel(lblTitulo);
            Controls.Add(lblTitulo);

            dgvReservas = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(ClientSize.Width - 40, ClientSize.Height - 250),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.FromArgb(40, 40, 50),
                ForeColor = Color.White,
                EnableHeadersVisualStyles = false
            };
            EstilosUI.AplicarEstiloDataGridView(dgvReservas);
            Controls.Add(dgvReservas);

            int btnHeight = 50, spacing = 15;
            int baseY = dgvReservas.Bottom + 20;
            int totalBtn = 5;
            int btnWidth = (ClientSize.Width - 40 - spacing * (totalBtn - 1)) / totalBtn;

            btnAgregar = new Button { Text = "Agregar", Size = new Size(btnWidth, btnHeight), Location = new Point(20, baseY) };
            btnEditar = new Button { Text = "Editar", Size = new Size(btnWidth, btnHeight), Location = new Point(20 + (btnWidth + spacing), baseY) };
            btnEliminar = new Button { Text = "Eliminar", Size = new Size(btnWidth, btnHeight), Location = new Point(20 + 2 * (btnWidth + spacing), baseY) };
            btnActualizar = new Button { Text = "Actualizar", Size = new Size(btnWidth, btnHeight), Location = new Point(20 + 3 * (btnWidth + spacing), baseY) };
            btnCerrar = new Button { Text = "Cerrar", Size = new Size(btnWidth, btnHeight), Location = new Point(20 + 4 * (btnWidth + spacing), baseY) };

            foreach (var b in new[] { btnAgregar, btnEditar, btnEliminar, btnActualizar, btnCerrar })
            {
                EstilosUI.AplicarEstiloBoton(b);
                Controls.Add(b);
                b.BringToFront();
            }

            btnAgregar.Click += BtnAgregar_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnActualizar.Click += (s, e) => CargarDatos();
            btnCerrar.Click += (s, e) => Close();
        }

        private void CargarDatos()
        {
            try
            {
                using var conn = new Conexion().ObtenerConexion();
                string query = "SELECT id, huesped_id, hospedaje_id, fecha_entrada, fecha_salida, cantidad_personas, cantidad_noches, estado FROM reservas ORDER BY id";
                using var cmd = new NpgsqlCommand(query, conn);
                using var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);


                dt.Columns.Add("EstadoTexto", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    bool estado = Convert.ToBoolean(row["estado"]);
                    row["EstadoTexto"] = estado ? "Reservado" : "Cancelada";
                }

                dgvReservas.DataSource = dt;

                dgvReservas.Columns["id"].HeaderText = "ID";
                dgvReservas.Columns["huesped_id"].HeaderText = "Huésped ID";
                dgvReservas.Columns["hospedaje_id"].HeaderText = "Hospedaje ID";
                dgvReservas.Columns["fecha_entrada"].HeaderText = "Fecha Entrada";
                dgvReservas.Columns["fecha_salida"].HeaderText = "Fecha Salida";
                dgvReservas.Columns["cantidad_personas"].HeaderText = "Cantidad Personas";
                dgvReservas.Columns["cantidad_noches"].HeaderText = "Cantidad Noches";
                dgvReservas.Columns["EstadoTexto"].HeaderText = "Estado";


                dgvReservas.Columns["estado"].Visible = false;

                dgvReservas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MostrarCustomMessageBox("Error al cargar datos: " + ex.Message, "Error");
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            using var input = new ReservaInputForm("Agregar Reserva") { Owner = this };
            input.ShowDialog(this);

            if (input.DialogResult == DialogResult.OK)
            {
                try
                {
                    using var conn = new Conexion().ObtenerConexion();
                    string q = "INSERT INTO reservas (huesped_id, hospedaje_id, fecha_entrada, fecha_salida, cantidad_personas, cantidad_noches, estado) VALUES (@huesped, @hospedaje, @entrada, @salida, @personas, @noches, @estado)";
                    using var cmd = new NpgsqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("huesped", input.HuespedID);
                    cmd.Parameters.AddWithValue("hospedaje", input.HospedajeID);
                    cmd.Parameters.AddWithValue("entrada", input.FechaEntrada);
                    cmd.Parameters.AddWithValue("salida", input.FechaSalida);
                    cmd.Parameters.AddWithValue("personas", input.CantidadPersonas);
                    cmd.Parameters.AddWithValue("noches", input.CantidadNoches);
                    cmd.Parameters.AddWithValue("estado", input.Estado);  // Booleano
                    cmd.ExecuteNonQuery();
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MostrarCustomMessageBox("Error al agregar: " + ex.Message, "Error");
                }
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvReservas.SelectedRows.Count == 0)
            {
                MostrarCustomMessageBox("Selecciona una reserva.", "Atención");
                return;
            }

            var row = dgvReservas.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["id"].Value);
            int huespedID = Convert.ToInt32(row.Cells["huesped_id"].Value);
            int hospedajeID = Convert.ToInt32(row.Cells["hospedaje_id"].Value);
            DateTime fechaEntrada = Convert.ToDateTime(row.Cells["fecha_entrada"].Value);
            DateTime fechaSalida = Convert.ToDateTime(row.Cells["fecha_salida"].Value);
            int cantidadPersonas = Convert.ToInt32(row.Cells["cantidad_personas"].Value);
            int cantidadNoches = Convert.ToInt32(row.Cells["cantidad_noches"].Value);
            bool estado = Convert.ToBoolean(row.Cells["estado"].Value);

            using var input = new ReservaInputForm("Editar Reserva", huespedID, hospedajeID, fechaEntrada, fechaSalida, cantidadPersonas, cantidadNoches) { Owner = this };
            input.ShowDialog(this);

            if (input.DialogResult == DialogResult.OK)
            {
                try
                {
                    using var conn = new Conexion().ObtenerConexion();
                    string q = "UPDATE reservas SET huesped_id = @huesped, hospedaje_id = @hospedaje, fecha_entrada = @entrada, fecha_salida = @salida, cantidad_personas = @personas, cantidad_noches = @noches, estado = @estado WHERE id = @id";
                    using var cmd = new NpgsqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("huesped", input.HuespedID);
                    cmd.Parameters.AddWithValue("hospedaje", input.HospedajeID);
                    cmd.Parameters.AddWithValue("entrada", input.FechaEntrada);
                    cmd.Parameters.AddWithValue("salida", input.FechaSalida);
                    cmd.Parameters.AddWithValue("personas", input.CantidadPersonas);
                    cmd.Parameters.AddWithValue("noches", input.CantidadNoches);
                    cmd.Parameters.AddWithValue("estado", input.Estado);  // Booleano
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MostrarCustomMessageBox("Error al actualizar: " + ex.Message, "Error");
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvReservas.SelectedRows.Count == 0)
            {
                MostrarCustomMessageBox("Selecciona una reserva.", "Atención");
                return;
            }

         
            MostrarCustomMessageBox("¿Estás seguro de que quieres eliminar esta reserva?", "Confirmación");

            try
            {
                var row = dgvReservas.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells["id"].Value);

                using var conn = new Conexion().ObtenerConexion();
                string q = "DELETE FROM reservas WHERE id = @id";
                using var cmd = new NpgsqlCommand(q, conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MostrarCustomMessageBox("Error al eliminar: " + ex.Message, "Error");
            }
        }

        private void MostrarCustomMessageBox(string mensaje, string titulo)
        {
            CustomMessageBoxForm customMessageBox = new CustomMessageBoxForm(mensaje, titulo);
            customMessageBox.ShowDialog(this); 
        }
    }
}
