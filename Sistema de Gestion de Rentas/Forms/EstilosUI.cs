using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class EstilosUI
{
    [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    private static extern IntPtr CreateRoundRectRgn(
        int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
        int nWidthEllipse, int nHeightEllipse);

    public static void AplicarEstiloBoton(Button boton)
    {
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.BackColor = Color.FromArgb(30, 144, 255);
        boton.ForeColor = Color.White;
        boton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        boton.Size = new Size(220, 50);
        boton.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, boton.Width, boton.Height, 15, 15));
        boton.MouseEnter += (s, e) => boton.BackColor = Color.FromArgb(65, 105, 225);
        boton.MouseLeave += (s, e) => boton.BackColor = Color.FromArgb(30, 144, 255);
    }

    public static void AplicarEstiloTextBox(TextBox txt)
    {
        txt.Font = new Font("Segoe UI", 10);
        txt.BorderStyle = BorderStyle.FixedSingle;
        txt.BackColor = Color.WhiteSmoke;
        txt.ForeColor = Color.Black;
        txt.Padding = new Padding(5);
    }
}
