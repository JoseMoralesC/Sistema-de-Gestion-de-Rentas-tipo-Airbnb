using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Services;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class RegistroHuespedForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private System.Windows.Forms.Timer fadeInTimer;

        private TextBox txtIdentificacion, txtUsuario, txtContrasena, txtNombre,
                        txtPrimerApellido, txtSegundoApellido, txtCorreo,
                        txtTelefono, txtPaisOrigen;

        private Button btnGuardar;
        private Button btnCerrar;

        public RegistroHuespedForm()
        {
            ConfigurarFormulario();
            InicializarUI();

            // Aplicar estilos a controles creados
            AplicarEstilos();

            // Animación fade-in
            Opacity = 0;
            fadeInTimer = new System.Windows.Forms.Timer();
            fadeInTimer.Interval = 1;
            fadeInTimer.Tick += (s, e) =>
            {
                if (Opacity < 1)
                    Opacity += 0.05;
                else
                    fadeInTimer.Stop();
            };
            fadeInTimer.Start();
        }

        private void AplicarEstilos()
        {
            // Botones
            EstilosUI.AplicarEstiloBoton(btnGuardar);
            EstilosUI.AplicarEstiloBoton(btnCerrar);

            // TextBoxes
            EstilosUI.AplicarEstiloTextBox(txtIdentificacion);
            EstilosUI.AplicarEstiloTextBox(txtUsuario);
            EstilosUI.AplicarEstiloTextBox(txtContrasena);
            EstilosUI.AplicarEstiloTextBox(txtNombre);
            EstilosUI.AplicarEstiloTextBox(txtPrimerApellido);
            EstilosUI.AplicarEstiloTextBox(txtSegundoApellido);
            EstilosUI.AplicarEstiloTextBox(txtCorreo);
            EstilosUI.AplicarEstiloTextBox(txtTelefono);
            EstilosUI.AplicarEstiloTextBox(txtPaisOrigen);
        }

        private void ConfigurarFormulario()
        {
            Text = "";
            Size = new Size(550, 650);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;

            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            Padding = new Padding(2);
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
                    Location = new Point(30, top + 5),
                    Size = new Size(150, 30),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular)
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

            // Asignar referencias
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
                Size = new Size(220, 50)
            };
            btnGuardar.Click += BtnGuardar_Click;
            Controls.Add(btnGuardar);

            // Botón Cerrar (justo a la derecha de Guardar)
            btnCerrar = new Button
            {
                Text = "Cerrar",
                Location = new Point(btnGuardar.Right + 20, btnGuardar.Top),
                Size = new Size(220, 50),
                BackColor = Color.FromArgb(220, 20, 60), // Rojo un poco más suave
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => this.Close();
            Controls.Add(btnCerrar);
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
