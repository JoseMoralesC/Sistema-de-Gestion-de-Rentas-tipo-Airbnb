using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class OpcionAdminForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private Button btnPanelHuesped, btnPanelAdmin, btnCerrar;
        private string nombreAdmin;

        public OpcionAdminForm(string nombre)
        {
            nombreAdmin = nombre;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Configuración de la ventana
            this.FormBorderStyle = FormBorderStyle.None;  // Eliminar la barra de título
            this.BackColor = Color.FromArgb(50, 50, 50);  // Fondo gris oscuro
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar el formulario
            this.Size = new Size(850, 300); // Tamaño adecuado para los botones y el texto

            // Aplicar bordes redondeados a la ventana
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 30, 30)); // Esquinas redondeadas

            // Etiqueta de bienvenida (centrada en la parte superior)
            Label lblBienvenida = new Label
            {
                Text = $"Bienvenido, {nombreAdmin}!",
                Location = new Point((Width - 300) / 2, 50),  // Centrado horizontalmente
                Size = new Size(300, 30),
                ForeColor = Color.White,  // Cambiar color de texto a blanco
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lblBienvenida);

            // Botón Panel Huesped
            btnPanelHuesped = new Button
            {
                Text = "Huesped",
                Size = new Size(200, 50),  // Botón grande
                Location = new Point(100, 150)  // Primer botón a la izquierda
            };
            EstilosUI.AplicarEstiloBoton(btnPanelHuesped);
            btnPanelHuesped.Click += BtnPanelHuesped_Click;
            Controls.Add(btnPanelHuesped);

            // Botón Panel Administrador
            btnPanelAdmin = new Button
            {
                Text = "Administrador",
                Size = new Size(200, 50),  // Botón grande
                Location = new Point(320, 150)  // Botón al centro
            };
            EstilosUI.AplicarEstiloBoton(btnPanelAdmin);
            btnPanelAdmin.Click += BtnPanelAdmin_Click;
            Controls.Add(btnPanelAdmin);

            // Botón Cerrar
            btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = new Size(200, 50),  // Botón grande
                Location = new Point(540, 150)  // Botón a la derecha
            };
            EstilosUI.AplicarEstiloBoton(btnCerrar);
            btnCerrar.Click += BtnCerrar_Click;
            Controls.Add(btnCerrar);
        }

        private void BtnPanelHuesped_Click(object sender, EventArgs e)
        {
            // Acción al hacer clic en "Panel Huesped"
            var panelHuesped = new PanelHuespedForm();
            panelHuesped.Show();
            this.Close(); // Cierra el formulario actual
        }

        private void BtnPanelAdmin_Click(object sender, EventArgs e)
        {
            // Acción al hacer clic en "Panel Administrador"
            var panelAdmin = new PanelAdministradorForm();
            panelAdmin.Show();
            this.Close(); // Cierra el formulario actual
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            // Cierra solo el formulario OpcionAdmin y regresa al formulario principal
            this.Close();
        }
    }
}
