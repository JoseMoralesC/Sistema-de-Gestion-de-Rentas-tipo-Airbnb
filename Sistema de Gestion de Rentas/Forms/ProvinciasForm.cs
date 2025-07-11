using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Npgsql;
using Sistema_de_Gestion_de_Rentas.Data;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public partial class ProvinciasForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int l, int t, int r, int b, int w, int h);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wp, int lp);

        private Label lblTitulo;
        private DataGridView dgvProvincias;
        private Button btnAgregar, btnEditar, btnEliminar, btnActualizar, btnCerrar;

        public ProvinciasForm()
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
                Text = "Provincias de Costa Rica",
                AutoSize = true,
                Location = new Point(20, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20, FontStyle.Bold)
            };
            EstilosUI.AplicarEstiloLabel(lblTitulo);
            Controls.Add(lblTitulo);

            dgvProvincias = new DataGridView
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
            EstilosUI.AplicarEstiloDataGridView(dgvProvincias);
            Controls.Add(dgvProvincias);

            int btnHeight = 50, spacing = 15;
            int baseY = dgvProvincias.Bottom + 20;
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
                // conn.Open(); // Solo si es necesario
                string query = "SELECT id, id_provincia, nombre FROM provincias ORDER BY id";
                using var cmd = new NpgsqlCommand(query, conn);
                using var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                dgvProvincias.DataSource = dt;

                dgvProvincias.Columns["id"].HeaderText = "ID";
                dgvProvincias.Columns["id_provincia"].HeaderText = "Código";
                dgvProvincias.Columns["nombre"].HeaderText = "Nombre de provincia";
                dgvProvincias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MostrarCustomMessageBox("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            using var input = new ProvinciaInputForm("Agregar Provincia") { Owner = this };
            input.ShowDialog(this);

            if (input.DialogResult == DialogResult.OK)
            {
                try
                {
                    using var conn = new Conexion().ObtenerConexion();
                    string q = "INSERT INTO provincias (id_provincia, nombre) VALUES (@cod, @nom)";
                    using var cmd = new NpgsqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("cod", input.CodigoProvincia);
                    cmd.Parameters.AddWithValue("nom", input.ProvinciaNombre);
                    cmd.ExecuteNonQuery();
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MostrarCustomMessageBox("Error al agregar: " + ex.Message, "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProvincias.SelectedRows.Count == 0)
            {
                MostrarCustomMessageBox("Selecciona una provincia.", "Atención", MessageBoxButtons.OK);
                return;
            }

            var row = dgvProvincias.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["id"].Value);
            int cod = Convert.ToInt32(row.Cells["id_provincia"].Value);
            string nom = row.Cells["nombre"].Value.ToString();

            using var input = new ProvinciaInputForm("Editar Provincia", cod, nom) { Owner = this };
            input.ShowDialog(this);

            if (input.DialogResult == DialogResult.OK)
            {
                try
                {
                    using var conn = new Conexion().ObtenerConexion();
                    string q = "UPDATE provincias SET id_provincia = @cod, nombre = @nom WHERE id = @id";
                    using var cmd = new NpgsqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("cod", input.CodigoProvincia);
                    cmd.Parameters.AddWithValue("nom", input.ProvinciaNombre);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MostrarCustomMessageBox("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProvincias.SelectedRows.Count == 0)
            {
                MostrarCustomMessageBox("Selecciona una provincia.", "Atención", MessageBoxButtons.OK);
                return;
            }

            var row = dgvProvincias.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["id"].Value);
            string nom = row.Cells["nombre"].Value.ToString();

            try
            {
                using var conn = new Conexion().ObtenerConexion();
                string q = "DELETE FROM provincias WHERE id = @id";
                using var cmd = new NpgsqlCommand(q, conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                CargarDatos();

                // Mensaje de confirmación después de eliminar
                MostrarCustomMessageBox($"Provincia '{nom}' eliminada.", "Información", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MostrarCustomMessageBox("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private DialogResult MostrarCustomMessageBox(string mensaje, string titulo = "Mensaje", MessageBoxButtons botones = MessageBoxButtons.OK)
        {
            // Siempre mostramos un mensaje con botón OK solo
            return CustomMessageBoxForm.Mostrar(this, mensaje);
        }
    }
}
