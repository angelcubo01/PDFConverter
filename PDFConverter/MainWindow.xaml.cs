using Microsoft.Win32;
using System;
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
        String[] archiv;

        private void origenbtn_Click(object sender, RoutedEventArgs e)
        {
            err.Children.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Pdf Files|*.pdf";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.ShowDialog();
            archiv = openFileDialog.FileNames;
            if (archiv.Length.Equals(0))
            {
                TextBlock linea = new TextBlock();
                linea.Text = "No hay archivos selecionados";
                err.Children.Add(linea);

            }
            foreach (String file in archiv)
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
            TextBlock linea = new TextBlock();
            linea.Text = "Se va a fusionar los archivos";
            err.Children.Add(linea);
            UnirPDF.Merge(saveFileDialog.FileName, archiv);
            linea.Text = "Se ha creado el archivo (UNIDO) : " + saveFileDialog.FileName;

        }
    }
}
