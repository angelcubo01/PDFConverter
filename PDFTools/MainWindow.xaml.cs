using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;



namespace PDFTools
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Archivo> listaArchivos;
        public MainWindow()
        {
            InitializeComponent();
            listaArchivos = new ObservableCollection<Archivo>();
            ListaUnir.ItemsSource = listaArchivos;
            
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/pdfConverterTemp.pdf"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/pdfConverterTemp.pdf");
            }
        }

        private void Btn_agregarUnir_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Pdf Files|*.pdf";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.ShowDialog();
            foreach (string file in openFileDialog.FileNames)
            {
                listaArchivos.Add(new Archivo(file,false));
            }
        }
        private void BorrarUnir_Checked(object sender, RoutedEventArgs e)
        { 
            for (int i = 0; i<listaArchivos.Count; i++)
            {
                if (listaArchivos[i].Borrar == true)
                {
                    listaArchivos.Remove(listaArchivos[i]);
                }
            }
        }
        private void BorrarTodoUnir_Click(object sender, RoutedEventArgs e)
        {
            listaArchivos.Clear();
        }


        private void ListaUnir_DragOver(object sender, DragEventArgs e)
        {
            
        }

        private void ListaUnir_DragLeave(object sender, DragEventArgs e)
        {

        }


        

        private void ListaUnir_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Btn_crearUnir_Click(object sender, RoutedEventArgs e)
        {
            List<string> rutas = new List<string>();
            foreach (Archivo arch in listaArchivos)
            {
                rutas.Add(arch.Ruta);
            }
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
            if (GuardarUnir.IsChecked == true)
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "ArchivoFusionado.pdf";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog.Filter = "Pdf Files|*.pdf";
                if (saveFileDialog.ShowDialog() == true)
                {
                    outputDocument.Save(saveFileDialog.FileName);
                }
            }
            else
            {

                string archivoSalida=Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+ System.IO.Path.DirectorySeparatorChar+"pdfConverterTemp.pdf";
                outputDocument.Save(archivoSalida);
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

        
    }
}
