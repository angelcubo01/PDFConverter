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
        /// <summary>
        /// Abre la interfaz gráfica de Windows para selecionar los archivos PDF que se van a cargar y sus ubicaciones
        /// </summary>
        /// <param name="multiArchivo"> Indica si necesitamos 1 o más archivos, TRUE = más de 1</param>
        /// <returns></returns>

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

        /// <summary>
        ///  Abre la interfaz gráfica de Windows para guardar el archivo generado con las opciones indicadas
        /// </summary>
        /// <param name="permanente">Indica si queremos crear un archivo temporal o uno definitivo</param>
        /// <param name="abrir"> Indica si queremos abrir el archivo generado</param>
        /// <param name="documento"> Indica el archivo que se va a guardar</param>
        /// <returns></returns>
        public string GuardarArchivo(bool permanente, bool abrir, PdfDocument documento)
        {
            if (documento == null)
            {
                return "null";
            }
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

        /// <summary>
        /// Une los archivos de la lista pasada como parametro en un unico archivo PdfDocument
        /// </summary>
        /// <param name="rutas"> List de las rutas de los archivos a unir</param>
        /// <returns></returns>
        public PdfDocument UnirPDF(List<String> rutas)
        {
            try
            {
                PdfDocument outputDocument = new PdfDocument();
                foreach (string file in rutas)
                {
                    

                    PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Modify);
                   
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                       
                        PdfPage page = inputDocument.Pages[idx];
                       
                        outputDocument.AddPage(page);
                    }

                }
                return outputDocument;
            }
            catch
            {
                MessageBox.Show("Algún archivo no se puede leer porque está cifrado o protegido", "PDFTools",
                                                MessageBoxButton.OK, MessageBoxImage.Error);

                return null;
            }

        }
        /// <summary>
        /// Transforma el pdf de la ruta indicada por parametro y genera un pdfDocument con dos diapositivas por página
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public PdfDocument DosDiapositivasEnUna(string ruta)
        {
            PdfDocument outputDocument = new PdfDocument();
           
            outputDocument.PageLayout = PdfPageLayout.SinglePage;

            XGraphics gfx;
            XRect box;
            try
            {
                XPdfForm form = XPdfForm.FromFile(ruta);

                for (int idx = 0; idx < form.PageCount; idx += 2)
                {

                    PdfPage page = outputDocument.AddPage();
                    page.Orientation = PageOrientation.Portrait;
                    double width = page.Width;
                    double height = page.Height;

                    int rotate = page.Elements.GetInteger("/Rotate");

                    gfx = XGraphics.FromPdfPage(page);


                    form.PageNumber = idx + 1;

                    box = new XRect(30, 30, width - 60, (height / 2) - 60);

                    gfx.DrawImage(form, box);



                    if (idx + 1 < form.PageCount)
                    {

                        form.PageNumber = idx + 2;

                        box = new XRect(30, (height / 2) + 30, width - 60, (height / 2) - 60);

                        gfx.DrawImage(form, box);


                    }
                }
                return outputDocument;
            }
            catch
            {
                MessageBox.Show("Algún archivo no se puede leer porque está cifrado o protegido", "PDFTools",
                                                MessageBoxButton.OK, MessageBoxImage.Error);

                return null;
            }
          

        }
    }
}
