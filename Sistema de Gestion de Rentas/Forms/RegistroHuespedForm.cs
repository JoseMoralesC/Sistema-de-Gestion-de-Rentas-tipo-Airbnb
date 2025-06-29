using System;
using System.Drawing;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Services;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class RegistroHuespedForm : Form
    {
        // Campos de entrada
        private TextBox txtIdentificacion, txtUsuario, txtContrasena, txtNombre,
                        txtPrimerApellido, txtSegundoApellido, txtCorreo,
                        txtTelefono, txtPaisOrigen;

        private Button btnGuardar;

        public RegistroHuespedForm()
        {
            ConfigurarFormulario();
            InicializarUI();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                HuespedService.Inicializar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar la base de datos:\n" + ex.Message);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "Registro de Huéspedes";
            Size = new Size(600, 700);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }

        private void InicializarUI()
        {
            string[] etiquetas = {
                "Identificación", "Usuario", "Contraseña", "Nombre",
                "Primer Apellido", "Segundo Apellido", "Correo",
                "Teléfono", "País de Origen"
            };

            TextBox[] campos = new TextBox[etiquetas.Length];
            int top = 20;

            for (int i = 0; i < etiquetas.Length; i++)
            {
                Label label = new Label
                {
                    Text = etiquetas[i] + ":",
                    Location = new Point(30, top),
                    Size = new Size(150, 30)
                };
                Controls.Add(label);

                campos[i] = new TextBox
                {
                    Location = new Point(200, top),
                    Size = new Size(300, 30)
                };
                Controls.Add(campos[i]);

                top += 50;
            }

            // Asignar referencias a campos
            txtIdentificacion = campos[0];
            txtUsuario = campos[1];
            txtContrasena = campos[2];
            txtNombre = campos[3];
            txtPrimerApellido = campos[4];
            txtSegundoApellido = campos[5];
            txtCorreo = campos[6];
            txtTelefono = campos[7];
            txtPaisOrigen = campos[8];

            // Botón Guardar
            btnGuardar = new Button
            {
                Text = "Guardar Huésped",
                Location = new Point(200, top + 20),
                Size = new Size(200, 40)
            };
            btnGuardar.Click += BtnGuardar_Click;
            Controls.Add(btnGuardar);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                HuespedService.GuardarHuesped(
                    txtIdentificacion.Text.Trim(),
                    txtUsuario.Text.Trim(),
                    txtContrasena.Text.Trim(),
                    txtNombre.Text.Trim(),
                    txtPrimerApellido.Text.Trim(),
                    txtSegundoApellido.Text.Trim(),
                    txtCorreo.Text.Trim(),
                    txtTelefono.Text.Trim(),
                    txtPaisOrigen.Text.Trim()
                );

                MessageBox.Show("Huésped guardado exitosamente.");
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar:\n" + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            txtIdentificacion.Clear();
            txtUsuario.Clear();
            txtContrasena.Clear();
            txtNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            txtCorreo.Clear();
            txtTelefono.Clear();
            txtPaisOrigen.Clear();
        }
    }
}