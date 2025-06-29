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
        // El tamaño del botón lo defines en el formulario, aquí no lo fijamos
        boton.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, boton.Width, boton.Height, 15, 15));
        boton.MouseEnter += (s, e) => boton.BackColor = Color.FromArgb(65, 105, 225);
        boton.MouseLeave += (s, e) => boton.BackColor = Color.FromArgb(30, 144, 255);
    }

    public static void AplicarEstiloTextBox(TextBox txt)
    {
        txt.Font = new Font("Segoe UI", 11);
        txt.BackColor = Color.White;
        txt.ForeColor = Color.Black;
        txt.BorderStyle = BorderStyle.None;
        txt.Multiline = false;   // Si sólo quieres una línea
        txt.Height = 35;
        txt.TextAlign = HorizontalAlignment.Left;
        // Redondear bordes
        txt.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txt.Width, txt.Height, 10, 10));
        txt.Padding = new Padding(8, 5, 8, 5);
    }

    public static void AplicarEstiloLabel(Label lbl)
    {
        lbl.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        lbl.ForeColor = Color.White;
        lbl.BackColor = Color.Transparent;
    }
}
