using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Npgsql;
using Sistema_de_Gestion_de_Rentas.Data;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class HospedajesForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int l, int t, int r, int b, int w, int h);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wp, int lp);

        private Label lblTitulo;
        private DataGridView dgvHospedajes;
        private Button btnAgregar, btnEditar, btnEliminar, btnActualizar, btnCerrar;

        public HospedajesForm()
        {
            ConfigurarFormulario();
            CargarDatos();
        }

        private void ConfigurarFormulario()
        {
            SuspendLayout();

            Text = "";
            Size = new Size(1200, 750);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(25, 25, 35);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, 0x112, 0xf012, 0);
                }
            };

            lblTitulo = new Label
            {
                Text = "Gestión de Hospedajes",
                AutoSize = true,
                Location = new Point(20, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20, FontStyle.Bold)
            };
            Controls.Add(lblTitulo);

            dgvHospedajes = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(ClientSize.Width - 40, ClientSize.Height - 220),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.FromArgb(40, 40, 50),
                ForeColor = Color.White,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(70, 130, 180),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.Black,
                    BackColor = Color.White,
                    
                },
                RowHeadersVisible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            EstilosUI.AplicarEstiloDataGridView(dgvHospedajes);
            Controls.Add(dgvHospedajes);

            
            int btnHeight = 45, spacing = 15;
            int totalBtn = 5;
            int btnWidth = (ClientSize.Width - 40 - spacing * (totalBtn - 1)) / totalBtn;
            int baseY = ClientSize.Height - btnHeight - 20;

           
            int totalBtnWidth = btnWidth * totalBtn + spacing * (totalBtn - 1);
            int offsetX = (ClientSize.Width - totalBtnWidth) / 2;

            btnAgregar = new Button { Text = "Agregar", Size = new Size(btnWidth, btnHeight), Location = new Point(offsetX + 0 * (btnWidth + spacing), baseY) };
            btnEditar = new Button { Text = "Editar", Size = new Size(btnWidth, btnHeight), Location = new Point(offsetX + 1 * (btnWidth + spacing), baseY) };
            btnEliminar = new Button { Text = "Eliminar", Size = new Size(btnWidth, btnHeight), Location = new Point(offsetX + 2 * (btnWidth + spacing), baseY) };
            btnActualizar = new Button { Text = "Actualizar", Size = new Size(btnWidth, btnHeight), Location = new Point(offsetX + 3 * (btnWidth + spacing), baseY) };
            btnCerrar = new Button { Text = "Cerrar", Size = new Size(btnWidth, btnHeight), Location = new Point(offsetX + 4 * (btnWidth + spacing), baseY) };

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

            ResumeLayout(false);
            PerformLayout();
        }


        private void CargarDatos()
        {
            try
            {
                using var conn = new Conexion().ObtenerConexion();
                const string query = @"
                    SELECT h.id, h.provincia_id, h.nombre, h.ubicacion, h.precio_por_noche, h.capacidad_personas,
                           h.habitaciones, h.descripcion
                    FROM hospedajes h
                    ORDER BY h.id";
                using var cmd = new NpgsqlCommand(query, conn);
                using var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                dgvHospedajes.DataSource = dt;
                dgvHospedajes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar hospedajes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            var inputForm = new HospedajeInputForm("Agregar Hospedaje")
            {
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true
            };

            if (inputForm.ShowDialog(this) == DialogResult.OK)
            {
                GuardarHospedaje(inputForm);
                CargarDatos();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvHospedajes.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvHospedajes.SelectedRows[0].Cells["id"].Value);
                var datosHospedaje = ObtenerHospedajePorId(id);

                var inputForm = new HospedajeInputForm(
                    "Editar Hospedaje",
                    datosHospedaje.ProvinciaId,
                    datosHospedaje.Nombre,
                    datosHospedaje.Ubicacion,
                    datosHospedaje.PrecioPorNoche,
                    datosHospedaje.CapacidadPersonas,
                    datosHospedaje.Habitaciones,
                    datosHospedaje.Descripcion
                )
                {
                    StartPosition = FormStartPosition.CenterParent,
                    TopMost = true
                };

                if (inputForm.ShowDialog(this) == DialogResult.OK)
                {
                    ActualizarHospedaje(id, inputForm);
                    CargarDatos();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un hospedaje para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvHospedajes.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvHospedajes.SelectedRows[0].Cells["id"].Value);
                var confirm = MessageBox.Show("¿Está seguro de eliminar este hospedaje?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    EliminarHospedaje(id);
                    CargarDatos();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un hospedaje para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GuardarHospedaje(HospedajeInputForm form)
        {
            using var conn = new Conexion().ObtenerConexion();
            string query = @"INSERT INTO hospedajes (provincia_id, nombre, ubicacion, precio_por_noche, capacidad_personas, habitaciones, descripcion)
                             VALUES (@provincia_id, @nombre, @ubicacion, @precio, @capacidad, @habitaciones, @descripcion)";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("provincia_id", form.ProvinciaId);
            cmd.Parameters.AddWithValue("nombre", form.Nombre);
            cmd.Parameters.AddWithValue("ubicacion", form.Ubicacion);
            cmd.Parameters.AddWithValue("precio", form.PrecioPorNoche);
            cmd.Parameters.AddWithValue("capacidad", form.CapacidadPersonas);
            cmd.Parameters.AddWithValue("habitaciones", form.Habitaciones);
            cmd.Parameters.AddWithValue("descripcion", form.Descripcion);
            cmd.ExecuteNonQuery();
        }

        private void ActualizarHospedaje(int id, HospedajeInputForm form)
        {
            using var conn = new Conexion().ObtenerConexion();
            string query = @"UPDATE hospedajes SET provincia_id=@provincia_id, nombre=@nombre, ubicacion=@ubicacion, 
                             precio_por_noche=@precio, capacidad_personas=@capacidad, habitaciones=@habitaciones, descripcion=@descripcion
                             WHERE id=@id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("provincia_id", form.ProvinciaId);
            cmd.Parameters.AddWithValue("nombre", form.Nombre);
            cmd.Parameters.AddWithValue("ubicacion", form.Ubicacion);
            cmd.Parameters.AddWithValue("precio", form.PrecioPorNoche);
            cmd.Parameters.AddWithValue("capacidad", form.CapacidadPersonas);
            cmd.Parameters.AddWithValue("habitaciones", form.Habitaciones);
            cmd.Parameters.AddWithValue("descripcion", form.Descripcion);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }

        private void EliminarHospedaje(int id)
        {
            using var conn = new Conexion().ObtenerConexion();
            string query = "DELETE FROM hospedajes WHERE id = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }

        private (int ProvinciaId, string Nombre, string Ubicacion, decimal PrecioPorNoche, int CapacidadPersonas, int Habitaciones, string Descripcion) ObtenerHospedajePorId(int id)
        {
            using var conn = new Conexion().ObtenerConexion();
            string query = @"SELECT provincia_id, nombre, ubicacion, precio_por_noche, capacidad_personas, habitaciones, descripcion 
                             FROM hospedajes WHERE id = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (
                    ProvinciaId: reader.GetInt32(0),
                    Nombre: reader.GetString(1),
                    Ubicacion: reader.GetString(2),
                    PrecioPorNoche: reader.GetDecimal(3),
                    CapacidadPersonas: reader.GetInt32(4),
                    Habitaciones: reader.GetInt32(5),
                    Descripcion: reader.GetString(6)
                );
            }
            else
            {
                throw new Exception("Hospedaje no encontrado.");
            }
        }
    }
}
