using Npgsql;
using Sistema_de_Gestion_de_Rentas.Data;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public partial class HospedajeInputForm : Form
    {
        // Propiedades públicas
        public int ProvinciaId { get; private set; }
        public string Nombre { get; private set; }
        public string Ubicacion { get; private set; }
        public decimal PrecioPorNoche { get; private set; }
        public int CapacidadPersonas { get; private set; }
        public int Habitaciones { get; private set; }
        public string Descripcion { get; private set; }
        public string Estado { get; private set; }

        private ComboBox cbProvincias;
        private TextBox txtNombre, txtUbicacion, txtDescripcion;
        private NumericUpDown nudPrecio, nudCapacidad, nudHabitaciones;
        private Button btnAceptar, btnCancelar;

        // Constructor que acepta parámetros (nuevo y existente)
        public HospedajeInputForm(string titulo,
            int provinciaId = 0, string nombre = "", string ubicacion = "",
            decimal precioPorNoche = 0, int capacidad = 1, int habitaciones = 1,
            string descripcion = "")
        {
            Text = titulo;
            Size = new Size(500, 600);  // Reducido para reflejar que no hay ComboBox de estado
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(25, 25, 35);

            // Título centrado
            Label lblTitulo = new Label
            {
                Text = titulo,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 50
            };
            Controls.Add(lblTitulo);

            // Controles
            int y = 60, spacing = 45;
            cbProvincias = new ComboBox { Location = new Point(20, y), Width = 440, DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(new Label { Text = "Provincia:", ForeColor = Color.White, Location = new Point(20, y - 20) });
            Controls.Add(cbProvincias);
            y += spacing;

            txtNombre = AddLabeledTextBox("Nombre:", ref y);
            txtUbicacion = AddLabeledTextBox("Ubicación:", ref y);

            nudPrecio = AddLabeledNumericUpDown("Precio por noche:", precioPorNoche, ref y, 0, 1000000, 0.01M);
            nudCapacidad = AddLabeledNumericUpDown("Capacidad personas:", capacidad, ref y, 1, 1000, 1);
            nudHabitaciones = AddLabeledNumericUpDown("Habitaciones:", habitaciones, ref y, 1, 1000, 1);

            txtDescripcion = new TextBox
            {
                Location = new Point(20, y),
                Size = new Size(440, 60),
                Multiline = true
            };
            Controls.Add(new Label { Text = "Descripción:", ForeColor = Color.White, Location = new Point(20, y - 20) });
            Controls.Add(txtDescripcion);
            y += 70;

            // Eliminar el ComboBox de estado, ya no es necesario
            // cbEstado = new ComboBox { Location = new Point(20, y), Width = 440, DropDownStyle = ComboBoxStyle.DropDownList };
            // cbEstado.Items.AddRange(new string[] { "Disponible", "Reservado" });
            // Controls.Add(new Label { Text = "Estado:", ForeColor = Color.White, Location = new Point(20, y - 20) });
            // Controls.Add(cbEstado);
            // y += spacing;

            btnAceptar = new Button { Text = "Aceptar", Location = new Point(100, y), Size = new Size(120, 35) };
            btnCancelar = new Button { Text = "Cancelar", Location = new Point(260, y), Size = new Size(120, 35) };
            Controls.Add(btnAceptar);
            Controls.Add(btnCancelar);

            // Eventos
            btnAceptar.Click += BtnAceptar_Click;
            btnCancelar.Click += (_, __) => this.DialogResult = DialogResult.Cancel;

            // Carga provincias de DB
            LoadProvincias();

            // Inicializa valores
            SelectProvinciaById(provinciaId);
            txtNombre.Text = nombre;
            txtUbicacion.Text = ubicacion;
            nudPrecio.Value = precioPorNoche >= nudPrecio.Minimum && precioPorNoche <= nudPrecio.Maximum ? precioPorNoche : nudPrecio.Minimum;
            nudCapacidad.Value = capacidad >= nudCapacidad.Minimum && capacidad <= nudCapacidad.Maximum ? capacidad : (decimal)nudCapacidad.Minimum;
            nudHabitaciones.Value = habitaciones >= nudHabitaciones.Minimum && habitaciones <= nudHabitaciones.Maximum ? habitaciones : (decimal)nudHabitaciones.Minimum;
            txtDescripcion.Text = descripcion;

            // Estado se asigna como "Disponible" por defecto
            Estado = "Disponible";  // Valor fijo de estado
        }

        private void LoadProvincias()
        {
            using var conn = new Conexion().ObtenerConexion();
            using var cmd = new NpgsqlCommand("SELECT id_provincia, nombre FROM provincias ORDER BY nombre", conn);
            var dt = new DataTable();
            using (var reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
            }
            cbProvincias.DataSource = dt;
            cbProvincias.DisplayMember = "nombre";
            cbProvincias.ValueMember = "id_provincia";
        }

        private void SelectProvinciaById(int provinciaId)
        {
            if (provinciaId == 0) return;
            for (int i = 0; i < cbProvincias.Items.Count; i++)
            {
                var drv = cbProvincias.Items[i] as DataRowView;
                if (drv != null && Convert.ToInt32(drv["id_provincia"]) == provinciaId)
                {
                    cbProvincias.SelectedIndex = i;
                    break;
                }
            }
        }

        private TextBox AddLabeledTextBox(string label, ref int y)
        {
            Controls.Add(new Label { Text = label, ForeColor = Color.White, Location = new Point(20, y - 20) });
            var txt = new TextBox { Location = new Point(20, y), Width = 440 };
            Controls.Add(txt);
            y += 45;
            return txt;
        }

        private NumericUpDown AddLabeledNumericUpDown(string label, decimal defaultVal, ref int y,
            decimal min = 0, decimal max = 1000000, decimal step = 0.5M)
        {
            Controls.Add(new Label { Text = label, ForeColor = Color.White, Location = new Point(20, y - 20) });
            var nud = new NumericUpDown
            {
                Location = new Point(20, y),
                Width = 440,
                DecimalPlaces = step < 1 ? 2 : 0,
                Minimum = min,
                Maximum = max,
                Value = defaultVal >= min && defaultVal <= max ? defaultVal : min,
                Increment = step
            };
            Controls.Add(nud);
            y += 45;
            return nud;
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (cbProvincias.SelectedIndex < 0
                || string.IsNullOrWhiteSpace(txtNombre.Text)
                || string.IsNullOrWhiteSpace(txtUbicacion.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProvinciaId = (int)cbProvincias.SelectedValue;
            Nombre = txtNombre.Text.Trim();
            Ubicacion = txtUbicacion.Text.Trim();
            PrecioPorNoche = nudPrecio.Value;
            CapacidadPersonas = (int)nudCapacidad.Value;
            Habitaciones = (int)nudHabitaciones.Value;
            Descripcion = txtDescripcion.Text.Trim();

            // Estado se establece como "Disponible" por defecto
            Estado = "Disponible";

            DialogResult = DialogResult.OK;
        }
    }
}
