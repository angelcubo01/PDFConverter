using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PDFConverter
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private String[] archivosUnir;
        private String archivoFinal;

        private void origenbtn_Click(object sender, RoutedEventArgs e)
        {
            err.Children.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Pdf Files|*.pdf";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.ShowDialog();
            archivosUnir = openFileDialog.FileNames;
            if (archivosUnir.Length.Equals(0))
            {
                TextBlock linea = new TextBlock();
                linea.Text = "No hay archivos selecionados";
                err.Children.Add(linea);

            }
            foreach (String file in archivosUnir)
            {
                TextBlock linea = new TextBlock();
                linea.Text = "Se ha añadido el archivo: " + file;
                err.Children.Add(linea);
            }

        }

        private void destinobtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "ArchivoFusionado.pdf";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "Pdf Files|*.pdf";
            saveFileDialog.ShowDialog();
            archivoFinal = saveFileDialog.FileName;
            TextBlock linea = new TextBlock();
            linea.Text = "El archivo se va a guardar en :  " + archivoFinal;
            err.Children.Add(linea);
        }

        private void unirTemp_Checked(object sender, RoutedEventArgs e)
        {
            destinoBtn.IsEnabled = false;
        }

        private void unirDefinitivo_Checked(object sender, RoutedEventArgs e)
        {
            destinoBtn.IsEnabled = true;
        }

        private void unirbtn_Click(object sender, RoutedEventArgs e)
        {
            if (destinoBtn.IsEnabled == true) //SE GUARDA EN LA DIRECCIÓN
            {
                guardarEnLocal();
            }
            else
            {
                abrirArchivo();
            }
        }

        private void abrirArchivo()
        {

            string archivoSalida=Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+Path.DirectorySeparatorChar+"pdfConverterTemp.pdf";
    
            if(archivosUnir != null)
            {
                if (archivosUnir.Length > 0)
                {
                    UnirPDF.Merge(archivoSalida, archivosUnir);
                    TextBlock linea = new TextBlock();
                    linea.Text = "Se ha creado archivo temporal, se abre el visor: " + archivoSalida;
                    err.Children.Add(linea);
                    try
                    {
                        System.Diagnostics.Process.Start("chrome.exe", archivoSalida);
                    }
                    catch
                    {
                        System.Diagnostics.Process.Start("firefox.exe", archivoSalida);
                    }
                }
                

                
            }
            else
            {
                TextBlock linea = new TextBlock();
                linea.Text = "No se ha guardado el archivo porque no hay ningún PDF origen";
                err.Children.Add(linea);
            }

        }

        private void guardarEnLocal()
        {
            if(archivosUnir != null && archivoFinal!= null)
            {
                if (archivosUnir.Length > 0 && archivoFinal.Length>0)
                {
                    UnirPDF.Merge(archivoFinal, archivosUnir);
                    TextBlock linea = new TextBlock();
                    linea.Text = "Archivo fusionado correctamente: " + archivoFinal;
                    err.Children.Add(linea);


                }
            }
            
            else
            {
                TextBlock linea = new TextBlock();
                linea.Text = "No se ha guardado el archivo porque no hay ningún PDF origen o final" ;
                err.Children.Add(linea);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/pdfConverterTemp.pdf"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/pdfConverterTemp.pdf");
            }
        }

        private void Ayuda_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://descargas.angelpicado.me/windows/pdfconvertor.html");
        }
    }
}
