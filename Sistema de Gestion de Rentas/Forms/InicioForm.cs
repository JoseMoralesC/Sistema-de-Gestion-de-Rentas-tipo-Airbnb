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
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                CustomMessageBoxForm.Mostrar("No se encontró la imagen de fondo.");
            }

            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;

            Label lblTitulo = new Label
            {
                Font = new Font("Arial", 32, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true
            };
            Controls.Add(lblTitulo);
            lblTitulo.Location = new Point((Screen.PrimaryScreen.Bounds.Width - lblTitulo.PreferredWidth) / 2, 100);

            Size tamañoBoton = new Size(250, 60);
            int borderRadius = 15;

            btnIniciarSesion = new Button
            {
                Text = "Iniciar Sesión",
                Size = tamañoBoton,
                Font = new Font("Arial", 14, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
            };
            btnIniciarSesion.FlatAppearance.BorderSize = 0;
            btnIniciarSesion.Location = new Point((Screen.PrimaryScreen.Bounds.Width - btnIniciarSesion.Width) / 2, 200);
            btnIniciarSesion.Click += BtnIniciarSesion_Click;
            Controls.Add(btnIniciarSesion);
            AplicarBordesRedondeados(btnIniciarSesion, borderRadius);

            btnRegistrarse = new Button
            {
                Text = "Registrarse",
                Size = tamañoBoton,
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

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = tamañoBoton,
                Font = new Font("Arial", 14, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 20, 60),
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
            var background = new ModalBackgroundForm
            {
                StartPosition = FormStartPosition.Manual,
                Size = this.ClientSize,
                Location = this.PointToScreen(Point.Empty),
                Owner = this,
                TopMost = false
            };
            background.Show();

            var loginForm = new LoginForm
            {
                StartPosition = FormStartPosition.CenterParent,
                Owner = this
            };

            loginForm.ShowDialog(this);
            background.Close();
        }

        private void BtnRegistrarse_Click(object sender, EventArgs e)
        {
            var background = new ModalBackgroundForm
            {
                StartPosition = FormStartPosition.Manual,
                Size = this.ClientSize,
                Location = this.PointToScreen(Point.Empty),
                Owner = this,
                TopMost = false
            };
            background.Show();

            var registroForm = new RegistroHuespedForm
            {
                StartPosition = FormStartPosition.CenterParent,
                Owner = this
            };

            registroForm.FormClosed += (s, args) =>
            {
                background.Close();
            };

            registroForm.ShowDialog(this);
        }
    }
}
