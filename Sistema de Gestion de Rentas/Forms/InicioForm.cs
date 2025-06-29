using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class InicioForm : Form
    {
        private Button btnIniciarSesion;
        private Button btnRegistrarse;
        private Button btnCerrar;

        public InicioForm()
        {
            string rutaFondo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Inicio_Fondo.jpg");

            if (File.Exists(rutaFondo))
            {
                this.BackgroundImage = Image.FromFile(rutaFondo);
                this.BackgroundImageLayout = ImageLayout.Stretch; // o Zoom si prefieres
            }
            else
            {
                MessageBox.Show("No se encontr� la imagen de fondo.");
            }

            // Configuraci�n de pantalla completa y sin borde
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;

            // T�tulo centrado
            Label lblTitulo = new Label
            {
                //Text = "�Bienvenido!",
                Font = new Font("Arial", 32, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true
            };
            Controls.Add(lblTitulo);
            lblTitulo.Location = new Point((Screen.PrimaryScreen.Bounds.Width - lblTitulo.PreferredWidth) / 2, 100);

            // Tama�o y radio para botones
            Size tama�oBoton = new Size(250, 60);
            int borderRadius = 15;

            // Bot�n Iniciar Sesi�n
            btnIniciarSesion = new Button
            {
                Text = "Iniciar Sesi�n",
                Size = tama�oBoton,
                Font = new Font("Arial", 14, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(70, 130, 180), // SteelBlue
                ForeColor = Color.White,
            };
            btnIniciarSesion.FlatAppearance.BorderSize = 0;
            btnIniciarSesion.Location = new Point((Screen.PrimaryScreen.Bounds.Width - btnIniciarSesion.Width) / 2, 200);
            btnIniciarSesion.Click += BtnIniciarSesion_Click;
            Controls.Add(btnIniciarSesion);
            AplicarBordesRedondeados(btnIniciarSesion, borderRadius);

            // Bot�n Registrarse
            btnRegistrarse = new Button
            {
                Text = "Registrarse",
                Size = tama�oBoton,
                Font = new Font("Arial", 14, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
            };
            btnRegistrarse.FlatAppearance.BorderSize = 0;
            btnRegistrarse.Location = new Point((Screen.PrimaryScreen.Bounds.Width - btnRegistrarse.Width) / 2, 280);
            btnRegistrarse.Click += BtnRegistrarse_Click;
            Controls.Add(btnRegistrarse);
            AplicarBordesRedondeados(btnRegistrarse, borderRadius);

            // Bot�n Cerrar (debajo de los otros dos)
            btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = tama�oBoton,
                Font = new Font("Arial", 14, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 20, 60), // Crimson
                ForeColor = Color.White,
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Location = new Point((Screen.PrimaryScreen.Bounds.Width - btnCerrar.Width) / 2, 360);
            btnCerrar.Click += (s, e) => Application.Exit();
            Controls.Add(btnCerrar);
            AplicarBordesRedondeados(btnCerrar, borderRadius);
        }

        private void AplicarBordesRedondeados(Button boton, int borderRadius)
        {
            boton.Paint += (s, e) =>
            {
                var path = new GraphicsPath();
                path.AddArc(0, 0, borderRadius * 2, borderRadius * 2, 180, 90);
                path.AddArc(boton.Width - borderRadius * 2, 0, borderRadius * 2, borderRadius * 2, 270, 90);
                path.AddArc(boton.Width - borderRadius * 2, boton.Height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 0, 90);
                path.AddArc(0, boton.Height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 90, 90);
                path.CloseAllFigures();
                boton.Region = new Region(path);
            };
            boton.Invalidate();
        }

        private void BtnIniciarSesion_Click(object sender, EventArgs e)
        {
            var loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void BtnRegistrarse_Click(object sender, EventArgs e)
        {
            // Fondo oscuro
            Form fondoOscuro = new ModalBackgroundForm
            {
                StartPosition = FormStartPosition.Manual,
                Size = this.ClientSize,
                Location = this.PointToScreen(Point.Empty),
                Owner = this
            };

            fondoOscuro.Show(); // Mostrar fondo

            var registroForm = new RegistroHuespedForm
            {
                StartPosition = FormStartPosition.CenterScreen, // o CenterParent si prefieres
                TopMost = true // se mantiene arriba
            };

            // Mostrar como formulario no modal (Show), y manejar el cierre nosotros
            registroForm.FormClosed += (s, args) =>
            {
                fondoOscuro.Close(); // Cerrar fondo oscuro al cerrar modal
            };

            registroForm.Show(); // No bloquea
        }
    }
}
