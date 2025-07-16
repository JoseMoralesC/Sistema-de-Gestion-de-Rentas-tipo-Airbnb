using Sistema_de_Gestion_de_Rentas.Controls;
using Sistema_de_Gestion_de_Rentas.ProvinciaForms;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using Sistema_de_Gestion_de_Rentas.Data;  

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public class PanelHuespedForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private Button btnCerrarSesion;
        private Button btnPerfil;
        private Button btnNosotros;
        private Button btnIrPanelAdmin;  

        private TableLayoutPanel provinciasTable;
        private Label lblBienvenida;
        private Label lblMensaje;
        private PictureBox banner;

        public PanelHuespedForm()
        {
            ConfigurarFormulario();
            InicializarControles();
            CrearBotonAdmin(); 
            CargarProvincias();
        }

        private void ConfigurarFormulario()
        {
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
        }

        private void InicializarControles()
        {
           
            btnCerrarSesion = new Button();
            EstilosPanelHuesped.EstiloBotonCerrarSesion(btnCerrarSesion);
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.Click += BtnCerrarSesion_Click;
            Controls.Add(btnCerrarSesion);

            
            btnPerfil = new Button();
            EstilosPanelHuesped.EstiloBotonCerrarSesion(btnPerfil);
            btnPerfil.Text = "Perfil";
            btnPerfil.Click += BtnPerfil_Click;
            Controls.Add(btnPerfil);

            
            btnNosotros = new Button();
            EstilosPanelHuesped.EstiloBotonCerrarSesion(btnNosotros);
            btnNosotros.Text = "Nosotros";
            btnNosotros.Click += BtnNosotros_Click;
            Controls.Add(btnNosotros);

           
            lblBienvenida = new Label();
            EstilosPanelHuesped.EstiloLabelBienvenida(lblBienvenida);
            lblBienvenida.Text = "Bienvenido a su panel de usuario";  
            lblBienvenida.AutoSize = true;
            lblBienvenida.TextAlign = ContentAlignment.TopCenter;
            lblBienvenida.Location = new Point((this.ClientSize.Width / 2) - (lblBienvenida.Width / 2), 50);
            Controls.Add(lblBienvenida);

            
            banner = new PictureBox();
            EstilosPanelHuesped.EstiloBanner(banner, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "banner.jpg"), 200, this.ClientSize.Width - 100);
            banner.Location = new Point(50, 180);
            Controls.Add(banner);

            
            lblMensaje = new Label();
            EstilosPanelHuesped.EstiloLabelMensaje(lblMensaje);
            lblMensaje.Location = new Point(50, banner.Bottom + 10);
            Controls.Add(lblMensaje);

            
            provinciasTable = new TableLayoutPanel();
            EstilosPanelHuesped.EstiloTableLayoutPanelProvincias(provinciasTable);
            provinciasTable.Location = new Point(50, lblMensaje.Bottom + 20);
            provinciasTable.Size = new Size(this.ClientSize.Width - 100, 520);
            provinciasTable.ColumnCount = 4;
            provinciasTable.RowCount = 2;

            for (int i = 0; i < 4; i++)
                provinciasTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            for (int i = 0; i < 2; i++)
                provinciasTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            provinciasTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            Controls.Add(provinciasTable);

            
            btnCerrarSesion.Location = new Point(50, this.ClientSize.Height - 100);
            btnPerfil.Location = new Point((this.ClientSize.Width / 2) - (btnPerfil.Width / 2), this.ClientSize.Height - 100);
            btnNosotros.Location = new Point(this.ClientSize.Width - btnNosotros.Width - 50, this.ClientSize.Height - 100);

            
            this.Resize += (s, e) =>
            {
                banner.Width = this.ClientSize.Width - 100;
                lblMensaje.Size = new Size(this.ClientSize.Width - 100, 30);
                provinciasTable.Size = new Size(this.ClientSize.Width - 100, 520);

                
                btnCerrarSesion.Location = new Point(50, this.ClientSize.Height - 100);
                btnPerfil.Location = new Point((this.ClientSize.Width / 2) - (btnPerfil.Width / 2), this.ClientSize.Height - 100);
                btnNosotros.Location = new Point(this.ClientSize.Width - btnNosotros.Width - 50, this.ClientSize.Height - 100);

                
                if (btnIrPanelAdmin != null)
                {
                    btnIrPanelAdmin.Location = new Point(this.ClientSize.Width - btnIrPanelAdmin.Width - 20, 50);  // 20px de margen desde la derecha
                }
            };
        }

        private void CrearBotonAdmin()
        {
           
            if (SesionUsuario.Rol.ToLower() == "admin")
            {
                
                btnIrPanelAdmin = new Button
                {
                    Text = "Panel Admin",
                    Size = new Size(300, 50), 
                    BackColor = Color.FromArgb(30, 30, 30),
                    ForeColor = Color.White
                };
                EstilosUI.AplicarEstiloBoton(btnIrPanelAdmin);

                
                btnIrPanelAdmin.Location = new Point(this.ClientSize.Width - btnIrPanelAdmin.Width - 20, 50);  // 20px de margen desde la derecha

                btnIrPanelAdmin.Click += BtnIrPanelAdmin_Click;
                Controls.Add(btnIrPanelAdmin);
            }
        }

        private void BtnIrPanelAdmin_Click(object sender, EventArgs e)
        {
            
            var panelAdminForm = new PanelAdministradorForm();
            panelAdminForm.Show();
            this.Hide(); 
        }

        private void CargarProvincias()
        {
            var provincias = new Dictionary<string, int>
            {
                { "Alajuela", 2 },
                { "Cartago", 3 },
                { "San Jose", 1 },
                { "Heredia", 4 },
                { "Puntarenas", 6 },
                { "Guanacaste", 5 },
                { "Limon", 7 }
            };

            provinciasTable.Controls.Clear();

            int totalCeldas = provinciasTable.RowCount * provinciasTable.ColumnCount;
            int i = 0;

            foreach (var provincia in provincias)
            {
                ProvinciaCard card = new ProvinciaCard
                {
                    Titulo = provincia.Key,
                    Imagen = CargarImagenProvincia(provincia.Key.ToLower().Replace(" ", ""))
                };

                card.CardClick += (s, e) => AbrirFormularioProvincia(provincia.Key, provincia.Value);
                card.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                provinciasTable.Controls.Add(card, i % 4, i / 4);
                i++;
            }

           
            ProvinciaCard cardCostaRica = new ProvinciaCard
            {
                Titulo = "Costa Rica",
                Imagen = CargarImagenProvincia("costarica") 
            };

            cardCostaRica.CardClick += (s, e) => AbrirFormularioCostaRica();
            cardCostaRica.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            provinciasTable.Controls.Add(cardCostaRica, i % 4, i / 4);
        }

        private void AbrirFormularioCostaRica()
        {
           
            CustomMessageBoxForm.Mostrar(this, "¿Desea ver el mapa de Costa Rica?");

            if (DialogResult.OK == DialogResult.OK)
            {
                
                CostaRicaForm costaRicaForm = new CostaRicaForm();
                costaRicaForm.Show();
            }
        }

        private Image CargarImagenProvincia(string nombreArchivo)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", $"{nombreArchivo}.jpg");
            return File.Exists(ruta) ? Image.FromFile(ruta) : null;
        }

        private void AbrirFormularioProvincia(string provincia, int provinciaId)
        {
            CustomMessageBoxForm.Mostrar(this, $"¿Deseas abrir los hospedajes de {provincia}?");

            if (DialogResult.OK == DialogResult.OK)
            {
                switch (provincia)
                {
                    case "Cartago":
                        CartagoForm cartagoForm = new CartagoForm(provinciaId);
                        cartagoForm.Show();
                        break;

                    case "San Jose":
                        SanJoseForm sanJoseForm = new SanJoseForm(provinciaId);
                        sanJoseForm.Show();
                        break;

                    case "Alajuela":
                        AlajuelaForm AlajuelaForm = new AlajuelaForm(provinciaId);
                        AlajuelaForm.Show();
                        break;

                    case "Heredia":
                        HerediaForm HerediaForm = new HerediaForm(provinciaId);
                        HerediaForm.Show();
                        break;

                    case "Guanacaste":
                        GuanacasteForm GuanacasteForm = new GuanacasteForm(provinciaId);
                        GuanacasteForm.Show();
                        break;

                    case "Puntarenas":
                        PuntarenasForm PuntarenasForm = new PuntarenasForm(provinciaId);
                        PuntarenasForm.Show();
                        break;

                    case "Limon":
                        LimonForm LimonForm = new LimonForm(provinciaId);
                        LimonForm.Show();
                        break;

                    default:
                        CustomMessageBoxForm.Mostrar(this, $"Formulario no disponible para {provincia}.");
                        break;
                }
            }
        }

        private void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            Hide();
            var inicioForm = new InicioForm();
            inicioForm.Show();
            Close();
        }


        private void BtnPerfil_Click(object sender, EventArgs e)
        {
            var perfilForm = new PerfilForm();
            perfilForm.ShowDialog();
        }

        private void BtnNosotros_Click(object sender, EventArgs e)
        {
            NosotrosForm nosotrosForm = new NosotrosForm();
            nosotrosForm.Show();
        }
    }
}
