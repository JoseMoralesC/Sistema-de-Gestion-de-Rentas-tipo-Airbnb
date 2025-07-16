using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_de_Gestion_de_Rentas.Forms
{
    public static class EstilosPanelHuesped
    {
     
        public static void EstiloBotonCerrarSesion(Button btn)
        {
            btn.Text = "Cerrar Sesión";
            btn.Size = new Size(200, 50);
            btn.Font = new Font("Arial", 12, FontStyle.Regular);
            btn.BackColor = Color.FromArgb(220, 20, 60);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }


        public static void EstiloLabelBienvenida(Label lbl)
        {
            lbl.Text = "Bienvenido a su Panel de Usuario";
            lbl.Font = new Font("Arial", 26, FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new Point(50, 120);  
            lbl.ForeColor = Color.FromArgb(50, 50, 50);  
        }

        
        public static void EstiloBanner(PictureBox pic, string imagePath, int height, int width)
        {
            pic.Image = Image.FromFile(imagePath);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Height = height;
            pic.Width = width;
        }


        public static void EstiloLabelMensaje(Label lbl)
        {
            lbl.Text = "Elige una de nuestras 7 provincias como tu próximo destino turístico";
            lbl.Font = new Font("Arial", 14, FontStyle.Italic);
            lbl.AutoSize = false;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Size = new Size(200, 30);  
            lbl.ForeColor = Color.Gray;  
        }

        public static void EstiloTableLayoutPanelProvincias(TableLayoutPanel panel)
        {
            panel.ColumnCount = 4;
            panel.RowCount = 2;
            panel.AutoScroll = true;
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel.Size = new Size(800, 520);  
            panel.ColumnStyles.Clear();
            panel.RowStyles.Clear();


            for (int i = 0; i < 4; i++)
            {
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            }

            for (int i = 0; i < 2; i++)
            {
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            }
        }
    }
}
