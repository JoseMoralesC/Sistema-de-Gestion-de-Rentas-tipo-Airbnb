using System;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Data;

namespace Sistema_de_Gestion_de_Rentas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Conexion conexion = new Conexion();
                var conn = conexion.ObtenerConexion();
                MessageBox.Show("Conexión exitosa a la base de datos.");
                conn.Close();

                // Llamamos al DAO para crear la tabla
                HuespedDAO.CrearTablaHuespedes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión:\n" + ex.Message);
            }
        }
    }
}
