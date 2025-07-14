// PdfForm.cs
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using System;
using System.IO;

namespace Sistema_de_Gestion_de_Rentas.Reservas
{
    public class PdfForm
    {
        public static void GenerarPDF(int huespedId, string nombreHospedaje, int cantidadPersonas, DateTime fechaEntrada, DateTime fechaSalida, int cantidadNoches, double totalCancelar)
        {
            // Definir la ruta de la carpeta donde se guardará el PDF (carpeta de documentos del usuario)
            string carpetaDestino = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SistemaDeGestionDeRentas", "Pdf");

            // Crear la carpeta si no existe
            if (!Directory.Exists(carpetaDestino))
            {
                Directory.CreateDirectory(carpetaDestino);
            }

            // Crear el nombre del archivo PDF con el formato deseado
            string nombreArchivoPdf = $"Reserva_{huespedId}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";

            // Ruta completa donde se guardará el PDF
            string rutaPdf = Path.Combine(carpetaDestino, nombreArchivoPdf);

            // Crear el objeto PDF
            using (var writer = new PdfWriter(rutaPdf))
            using (var pdf = new PdfDocument(writer))
            {
                // Crear el documento en blanco
                var document = new Document(pdf);

                // Agregar título
                document.Add(new Paragraph("Resumen de su Reserva")
                                .SetFontSize(20)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontColor(ColorConstants.BLACK));

                // Agregar información de la reserva
                document.Add(new Paragraph($"Huésped ID: {huespedId}")
                                .SetFontSize(12));
                document.Add(new Paragraph($"Hospedaje: {nombreHospedaje}")
                                .SetFontSize(12));
                document.Add(new Paragraph($"Fecha de Entrada: {fechaEntrada.ToShortDateString()}")
                                .SetFontSize(12));
                document.Add(new Paragraph($"Fecha de Salida: {fechaSalida.ToShortDateString()}")
                                .SetFontSize(12));
                document.Add(new Paragraph($"Cantidad de Noches: {cantidadNoches}")
                                .SetFontSize(12));
                document.Add(new Paragraph($"Cantidad de Personas: {cantidadPersonas}")
                                .SetFontSize(12));
                document.Add(new Paragraph($"Total a Pagar: {totalCancelar:C}")
                                .SetFontSize(12));

                // Agregar mensaje de agradecimiento
                document.Add(new Paragraph("\n\nGracias por preferirnos y utilizar nuestra herramienta para reservar su hospedaje.")
                                .SetFontSize(12)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontColor(ColorConstants.BLACK));

                // Agregar nota sobre la política de cancelación
                document.Add(new Paragraph("\nRecuerde que si necesita cancelar su reserva deberá comunicarse con el administrador en un plazo mínimo de 24 horas antes de la fecha elegida.")
                                .SetFontSize(12)
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetFontColor(ColorConstants.RED));

                document.Add(new Paragraph("De lo contrario, perderá su reservación.")
                                .SetFontSize(12)
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetFontColor(ColorConstants.RED));

                // Cerrar el documento
                document.Close();
            }

            // Mostrar la ubicación del PDF generado
            System.Windows.Forms.MessageBox.Show($"El PDF ha sido generado correctamente en la ubicación: {rutaPdf}", "PDF Generado", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
