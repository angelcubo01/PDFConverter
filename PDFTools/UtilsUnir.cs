using Microsoft.Win32;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace PDFTools
{
    class UtilsUnir
    {


        public string[] CargarArchivos(bool multiArchivo)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (multiArchivo)
            {
                openFileDialog.Multiselect = true;
            }
            else
            {
                openFileDialog.Multiselect = false;
            }

            openFileDialog.Filter = "Pdf Files|*.pdf";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.ShowDialog();
            return openFileDialog.FileNames;
        }
        public string GuardarArchivo(bool permanente, bool abrir, PdfDocument documento)
        {
            if (permanente)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog.Filter = "Pdf Files|*.pdf";
                if (saveFileDialog.ShowDialog() == true)
                {
                    documento.Save(saveFileDialog.FileName);
                }
                if (abrir)
                {
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
            else
            {
                string archivoSalida = Path.GetTempFileName() + ".pdf";
                documento.Save(archivoSalida);
                if (abrir)
                {
                    System.Diagnostics.Process.Start(archivoSalida);
                }
                return archivoSalida;

            }
            return null;
        }
        public PdfDocument UnirPDF(List<String> rutas)
        {
            try
            {
                PdfDocument outputDocument = new PdfDocument();
                foreach (string file in rutas)
                {
                    // Open the document to import pages from it.

                    PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                    // Iterate pages
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        // Get the page from the external document...
                        PdfPage page = inputDocument.Pages[idx];
                        // ...and add it to the output document.
                        outputDocument.AddPage(page);
                    }

                }
                return outputDocument;
            }
            catch
            {

                MessageBox.Show("Algún archivo no se puede leer porque está cifrado o protegido, la aplicación se cierra", "PDFTools",
                                                 MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

        }

        public PdfDocument DosDiapositivasEnUna(string ruta)
        {
            PdfDocument outputDocument = new PdfDocument();
            // Show single pages
            // (Note: one page contains two pages from the source document)
            outputDocument.PageLayout = PdfPageLayout.SinglePage;

            XGraphics gfx;
            XRect box;

            // Open the external document as XPdfForm object
            XPdfForm form = XPdfForm.FromFile(ruta);

            for (int idx = 0; idx < form.PageCount; idx += 2)
            {
                // Add a new page to the output document
                PdfPage page = outputDocument.AddPage();
                page.Orientation = PageOrientation.Portrait;
                double width = page.Width;
                double height = page.Height;

                int rotate = page.Elements.GetInteger("/Rotate");

                gfx = XGraphics.FromPdfPage(page);

                // Set page number (which is one-based)
                form.PageNumber = idx + 1;

                box = new XRect(30, 30, width - 60, (height / 2) - 60);
                // Draw the page identified by the page number like an image
                gfx.DrawImage(form, box);



                if (idx + 1 < form.PageCount)
                {
                    // Set page number (which is one-based)
                    form.PageNumber = idx + 2;

                    box = new XRect(30, (height / 2) + 30, width - 60, (height / 2) - 60);
                    // Draw the page identified by the page number like an image
                    gfx.DrawImage(form, box);


                }
            }
            return outputDocument;
        }
    }
}
