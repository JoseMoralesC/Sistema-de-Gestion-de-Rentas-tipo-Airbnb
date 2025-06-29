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
                        txtTelefono;

        private ComboBox cbPaisOrigen;

        private Button btnGuardar;
        private Button btnCerrar;

        public RegistroHuespedForm()
        {
            ConfigurarFormulario();
            InicializarUI();
            AplicarEstilos();

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

        private void ConfigurarFormulario()
        {
            Text = "";
            Size = new Size(720, 750);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(45, 45, 48);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            Padding = new Padding(2);
        }

        private void InicializarUI()
        {
            string[] etiquetas = {
                "Identificación", "Usuario", "Contraseña", "Nombre",
                "1° Apellido", "2° Apellido", "Correo", "Teléfono", "País"
            };

            TextBox[] campos = new TextBox[8]; // solo los 8 primeros serán TextBox
            int top = 30;
            int labelWidth = 160;
            int spacing = 25;
            int extraOffset = 40;
            int textBoxX = 40 + labelWidth + spacing + extraOffset;

            for (int i = 0; i < etiquetas.Length; i++)
            {
                Label label = new Label
                {
                    Text = etiquetas[i] + ":",
                    Location = new Point(30, top + 5),
                    Size = new Size(labelWidth, 30)
                };
                EstilosUI.AplicarEstiloLabel(label);
                Controls.Add(label);

                if (i < 8)
                {
                    campos[i] = new TextBox
                    {
                        Location = new Point(textBoxX, top),
                        Size = new Size(415, 35)
                    };
                    Controls.Add(campos[i]);
                    EstilosUI.AplicarEstiloTextBox(campos[i]);
                }
                else
                {
                    cbPaisOrigen = new ComboBox
                    {
                        Location = new Point(textBoxX, top),
                        Size = new Size(415, 35),
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Font = new Font("Segoe UI", 11)
                    };
                    cbPaisOrigen.Items.AddRange(new string[] {
                        "Costa Rica", "México", "Argentina", "Colombia",
                        "Chile", "Perú", "España", "Estados Unidos", "Otro"
                    });
                    Controls.Add(cbPaisOrigen);
                }

                top += 55;
            }

            txtIdentificacion = campos[0];
            txtUsuario = campos[1];
            txtContrasena = campos[2];
            txtNombre = campos[3];
            txtPrimerApellido = campos[4];
            txtSegundoApellido = campos[5];
            txtCorreo = campos[6];
            txtTelefono = campos[7];

            // Botón Guardar
            btnGuardar = new Button
            {
                Text = "Guardar Huésped",
                Size = new Size(250, 50),
                Location = new Point((Width - 250) / 2, top + 30)
            };
            EstilosUI.AplicarEstiloBoton(btnGuardar);
            btnGuardar.Click += BtnGuardar_Click;
            Controls.Add(btnGuardar);

            // Botón Cerrar
            btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = new Size(250, 50),
                Location = new Point((Width - 250) / 2, btnGuardar.Bottom + 15),
                BackColor = Color.FromArgb(220, 20, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCerrar.Width, btnCerrar.Height, 15, 15));
            btnCerrar.Click += (s, e) => this.Close();
            Controls.Add(btnCerrar);
        }

        private void AplicarEstilos()
        {
            EstilosUI.AplicarEstiloBoton(btnGuardar);
            EstilosUI.AplicarEstiloBoton(btnCerrar);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string paisSeleccionado = cbPaisOrigen.SelectedItem?.ToString() ?? "";

                HuespedService.GuardarHuesped(
                    txtIdentificacion.Text.Trim(),
                    txtUsuario.Text.Trim(),
                    txtContrasena.Text.Trim(),
                    txtNombre.Text.Trim(),
                    txtPrimerApellido.Text.Trim(),
                    txtSegundoApellido.Text.Trim(),
                    txtCorreo.Text.Trim(),
                    txtTelefono.Text.Trim(),
                    paisSeleccionado
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
            cbPaisOrigen.SelectedIndex = -1;
        }
    }
}
