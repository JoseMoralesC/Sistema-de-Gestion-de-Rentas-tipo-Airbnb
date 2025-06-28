using System;
using System.Windows.Forms;
using Sistema_de_Gestion_de_Rentas.Forms;

namespace Sistema_de_Gestion_de_Rentas
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
      
            Application.Run(new InicioForm());

        }
    }
}
