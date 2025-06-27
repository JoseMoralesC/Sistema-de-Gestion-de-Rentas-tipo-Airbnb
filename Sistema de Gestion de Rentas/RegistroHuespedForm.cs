using System;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Data;

namespace Sistema_de_Gestion_de_Rentas
{
    public partial class RegistroHuespedForm : Form
    {
        public RegistroHuespedForm()
        {
            InitializeComponent();
            this.Load += RegistroHuespedForm_Load;

            // Evento para el botón guardar (si ya existe el botón btnGuardar en el diseñador)
            // btnGuardar.Click += BtnGuardar_Click;
        }

        private void RegistroHuespedForm_Load(object sender, EventArgs e)
        {
            try
            {
                Conexion conexion = new Conexion();
                var conn = conexion.ObtenerConexion();
                MessageBox.Show("Conexión exitosa a la base de datos.");
                conn.Close();

                // Llamamos al DAO para crear la tabla de huéspedes
                // Se ejecuta solo una ves al iniciar el formulario
                // HuespedDAO.CrearTablaHuespedes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión:\n" + ex.Message);
            }
        }

        // Este método lo puedes usar más adelante para el botón "Guardar":
        /*
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Aquí iría la lógica para guardar huéspedes
        }
        */
    }
}
