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
           
            this.FormBorderStyle = FormBorderStyle.None;  
            this.BackColor = Color.FromArgb(50, 50, 50);  
            this.StartPosition = FormStartPosition.CenterScreen; 
            this.Size = new Size(850, 300); 

            
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 30, 30)); // Esquinas redondeadas

           
            Label lblBienvenida = new Label
            {
                Text = $"Bienvenido, {nombreAdmin}!",
                Location = new Point((Width - 300) / 2, 50),  
                Size = new Size(300, 30),
                ForeColor = Color.White,  
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lblBienvenida);

            
            btnPanelHuesped = new Button
            {
                Text = "Huesped",
                Size = new Size(200, 50),  
                Location = new Point(100, 150)  
            };
            EstilosUI.AplicarEstiloBoton(btnPanelHuesped);
            btnPanelHuesped.Click += BtnPanelHuesped_Click;
            Controls.Add(btnPanelHuesped);

            
            btnPanelAdmin = new Button
            {
                Text = "Administrador",
                Size = new Size(200, 50),  
                Location = new Point(320, 150)  
            };
            EstilosUI.AplicarEstiloBoton(btnPanelAdmin);
            btnPanelAdmin.Click += BtnPanelAdmin_Click;
            Controls.Add(btnPanelAdmin);

            
            btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = new Size(200, 50), 
                Location = new Point(540, 150)  
            };
            EstilosUI.AplicarEstiloBoton(btnCerrar);
            btnCerrar.Click += BtnCerrar_Click;
            Controls.Add(btnCerrar);
        }

        private void BtnPanelHuesped_Click(object sender, EventArgs e)
        {
            
            var panelHuesped = new PanelHuespedForm();
            panelHuesped.Show();
            this.Close(); 
        }

        private void BtnPanelAdmin_Click(object sender, EventArgs e)
        {
           
            var panelAdmin = new PanelAdministradorForm();
            panelAdmin.Show();
            this.Close(); 
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
