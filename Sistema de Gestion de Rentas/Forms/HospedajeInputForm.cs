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
        // Ya no es necesario el DllImport para el redondeo de esquinas
        // [DllImport("user32.dll", CharSet = CharSet.Auto)]
        // public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

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

        public HospedajeInputForm(string titulo,
            int provinciaId = 0, string nombre = "", string ubicacion = "",
            decimal precioPorNoche = 0, int capacidad = 1, int habitaciones = 1,
            string descripcion = "")
        {
            // Inicialización del formulario
            Text = titulo;
            Size = new Size(600, 900);  // Aumento la altura del formulario
            FormBorderStyle = FormBorderStyle.None;  // No bordes
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(25, 25, 35);

            // Eliminamos la región redondeada para mantener las esquinas rectas
            // this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));

            // Título centrado
            Label lblTitulo = new Label
            {
                Text = titulo,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),  // Tamaño ajustado
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 60
            };
            Controls.Add(lblTitulo);

            int y = 80, spacing = 60;  // Espacio entre elementos
            cbProvincias = new ComboBox { Location = new Point(20, y), Width = 540, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblProvincia = new Label { Text = "Provincia:", ForeColor = Color.White, Location = new Point(20, y - 30), Font = new Font("Segoe UI", 10, FontStyle.Regular) };  // Etiqueta ajustada
            Controls.Add(lblProvincia);
            Controls.Add(cbProvincias);
            lblProvincia.Width = cbProvincias.Width;  // Ajustamos el ancho de la etiqueta
            y += spacing;

            y += 20;  // Espacio adicional
            txtNombre = AddLabeledTextBox("Nombre:", ref y);
            y += 20;  // Espacio adicional
            txtUbicacion = AddLabeledTextBox("Ubicación:", ref y);

            y += 20;  // Espacio adicional
            nudPrecio = AddLabeledNumericUpDown("Precio por noche:", precioPorNoche, ref y, 0, 1000000, 0.01M);
            y += 20;  // Espacio adicional
            nudCapacidad = AddLabeledNumericUpDown("Capacidad personas:", capacidad, ref y, 1, 1000, 1);
            y += 20;  // Espacio adicional
            nudHabitaciones = AddLabeledNumericUpDown("Habitaciones:", habitaciones, ref y, 1, 1000, 1);

            y += 20;  // Espacio adicional
            txtDescripcion = new TextBox
            {
                Location = new Point(20, y),
                Size = new Size(540, 80),  // Ajustamos el tamaño de la caja de texto
                Multiline = true
            };
            var lblDescripcion = new Label { Text = "Descripción:", ForeColor = Color.White, Location = new Point(20, y - 30), Font = new Font("Segoe UI", 10, FontStyle.Regular) };  // Etiqueta ajustada
            Controls.Add(lblDescripcion);
            Controls.Add(txtDescripcion);
            lblDescripcion.Width = txtDescripcion.Width;  // Ajustamos el ancho de la etiqueta
            y += 100;

            btnAceptar = new Button { Text = "Aceptar", Location = new Point(100, y), Size = new Size(160, 40) };
            btnCancelar = new Button { Text = "Cancelar", Location = new Point(280, y), Size = new Size(160, 40) };

            EstilosUI.AplicarEstiloBoton(btnAceptar);
            EstilosUI.AplicarEstiloBoton(btnCancelar);

            Controls.Add(btnAceptar);
            Controls.Add(btnCancelar);

            // Eventos de botones
            btnAceptar.Click += BtnAceptar_Click;
            btnCancelar.Click += (_, __) => this.DialogResult = DialogResult.Cancel;

            // Carga de provincias
            LoadProvincias();

            // Inicializa valores
            SelectProvinciaById(provinciaId);
            txtNombre.Text = nombre;
            txtUbicacion.Text = ubicacion;
            nudPrecio.Value = precioPorNoche >= nudPrecio.Minimum && precioPorNoche <= nudPrecio.Maximum ? precioPorNoche : nudPrecio.Minimum;
            nudCapacidad.Value = capacidad >= nudCapacidad.Minimum && capacidad <= nudCapacidad.Maximum ? capacidad : (decimal)nudCapacidad.Minimum;
            nudHabitaciones.Value = habitaciones >= nudHabitaciones.Minimum && habitaciones <= nudHabitaciones.Maximum ? habitaciones : (decimal)nudHabitaciones.Minimum;
            txtDescripcion.Text = descripcion;

            // Estado por defecto
            Estado = "Disponible";
        }

        // Método para cargar las provincias desde la base de datos
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

        // Método para seleccionar la provincia por id
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

        // Método para agregar un TextBox con su etiqueta
        private TextBox AddLabeledTextBox(string label, ref int y)
        {
            var lbl = new Label { Text = label, ForeColor = Color.White, Location = new Point(20, y - 30), Font = new Font("Segoe UI", 10, FontStyle.Regular) };  // Etiqueta ajustada
            var txt = new TextBox { Location = new Point(20, y), Width = 540, Height = 35 };  // Ajustamos la altura del TextBox
            Controls.Add(lbl);
            Controls.Add(txt);
            lbl.Width = txt.Width;  // Ajustamos el ancho de la etiqueta
            y += 80;  // Separación adicional
            return txt;
        }

        // Método para agregar un NumericUpDown con su etiqueta
        private NumericUpDown AddLabeledNumericUpDown(string label, decimal defaultVal, ref int y,
            decimal min = 0, decimal max = 1000000, decimal step = 0.5M)
        {
            var lbl = new Label { Text = label, ForeColor = Color.White, Location = new Point(20, y - 30), Font = new Font("Segoe UI", 10, FontStyle.Regular) };  // Etiqueta ajustada
            var nud = new NumericUpDown
            {
                Location = new Point(20, y),
                Width = 540,
                Height = 35,
                DecimalPlaces = step < 1 ? 2 : 0,
                Minimum = min,
                Maximum = max,
                Value = defaultVal >= min && defaultVal <= max ? defaultVal : min,
                Increment = step
            };
            Controls.Add(lbl);
            Controls.Add(nud);
            lbl.Width = nud.Width;  // Ajustamos el ancho de la etiqueta
            y += 80;  // Separación adicional
            return nud;
        }

        // Evento de clic en "Aceptar"
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

            Estado = "Disponible";  // Valor fijo de estado

            DialogResult = DialogResult.OK;
        }
    }
}
