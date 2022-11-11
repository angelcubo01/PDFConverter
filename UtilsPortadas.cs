using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PDFTools
{
    class UtilsPortadas
    {
        internal static void generarPortadas(ObservableCollection<Portada> listaPortadas)
        {
            FolderBrowserDialog carpetaDestino = new FolderBrowserDialog();
            carpetaDestino.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
          
            if (carpetaDestino.ShowDialog() == DialogResult.OK)
            {
                string destinoPath = carpetaDestino.SelectedPath;
                
                foreach (Portada port in listaPortadas)
                {
                    
                    PdfDocument document = new PdfDocument();
                    document.Info.Title = port.NombreAsignatura;

                    
                    PdfPage page = document.AddPage();
                   
                    
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    double paginaEnCmAncho = page.Width / 21;
                    double paginaEnCmAlto = page.Height / 29.7;
                 
                    XFont font = new XFont("Times New Roman", 20);
                   
                    XRect rectanguloFondo = new XRect(paginaEnCmAncho*6,paginaEnCmAlto*6,paginaEnCmAncho*15, paginaEnCmAlto*15.7);
                    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(port.Color.A, port.Color.R, port.Color.G, port.Color.B)), rectanguloFondo);
                 
                    XBrush brocha;
                    if (port.EsNegro == true)
                    {
                        brocha = XBrushes.Black;
                    }
                    else
                    {
                        brocha = XBrushes.White;
                    }
                   
                    XRect rectanguloTitulo = new XRect(paginaEnCmAncho*8,paginaEnCmAlto*8,paginaEnCmAncho*12, 0);
                    gfx.DrawString(port.NombreAsignatura, font, brocha, rectanguloTitulo);

                    XFont font2 = new XFont("Arial", 14);
                    XRect rectanguloNombre = new XRect(paginaEnCmAncho*8,paginaEnCmAlto*18,paginaEnCmAncho*12, 0);
                    gfx.DrawString(port.NombreApellidos, font2, brocha, rectanguloNombre);

                    string centroCurso = port.Centro + " | " + port.Curso;
                    XRect rectanguloCentro = new XRect(paginaEnCmAncho*8,paginaEnCmAlto*20,paginaEnCmAncho*12, 0);
                    gfx.DrawString(centroCurso, font2, brocha, rectanguloCentro);

                    string filename = destinoPath + "/" + port.NombreAsignatura + ".pdf";
                    document.Save(filename);
                    
                }
                System.Windows.MessageBox.Show("Se han creado las portadas", "PDFTools",
                                              MessageBoxButton.OK, MessageBoxImage.Information);
            }

           
        }
    }
}
