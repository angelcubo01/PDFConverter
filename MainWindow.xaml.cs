using PdfSharp.Pdf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace PDFTools
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Archivo> listaArchivos;
        private Archivo archivoUnico;
        public string[] portada;
        public MainWindow()
        {
            InitializeComponent();
            listaArchivos = new ObservableCollection<Archivo>();
            ListaUnir.ItemsSource = listaArchivos;

        }



        private void Btn_agregarUnir_Click(object sender, RoutedEventArgs e) //BOTON DE CARGA DE UnirPDF
        {
            string[] archivos;
            UtilsUnir unir = new UtilsUnir();
            archivos = unir.CargarArchivos(true);
            foreach (string file in archivos)
            {
                listaArchivos.Add(new Archivo(file, false));
            }
        }
        private void Opciones_Checked(object sender, RoutedEventArgs e) //CHECKBOX DE OPCIONES DE UnirPDF
        {
 
            for (int i = 0; i < listaArchivos.Count; i++)
            {
               
                if (listaArchivos[i].Subir == true && i!=0)
                {
                    listaArchivos.Move(i, i - 1);
                    listaArchivos[i-1].Subir = false;
                }
                if(listaArchivos[i].Bajar == true && i != listaArchivos.Count-1)
                {
                    listaArchivos.Move(i, i + 1);
                    listaArchivos[i+1].Bajar = false;
                }
                listaArchivos[0].Subir = false;
                listaArchivos[listaArchivos.Count-1].Bajar = false;
                if (listaArchivos[i].Borrar == true)
                {
                    listaArchivos.Remove(listaArchivos[i]);

                }
            }
        }
        private void BorrarTodoUnir_Click(object sender, RoutedEventArgs e) //BOTON DE BORRAR TODO DE UnirPDF
        {
            listaArchivos.Clear();
        }


        private void Btn_crearUnir_Click(object sender, RoutedEventArgs e) //BOTON DE CREAR ARCHIVO DE UnirPDF
        {
            UtilsUnir unir = new UtilsUnir();
            string ruta_archTemp;
            List<string> rutas = new List<string>();
            rutas.Clear();

            foreach (Archivo arch in listaArchivos)
            {
                rutas.Add(arch.Ruta);
            }
            if (rutas.Count == 0)
            {

                MessageBox.Show("No hay ningún archivo que unir", "ERROR!!",
                                 MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Este proceso podría tardar varios segundos, por favor se paciente", "PDFTools",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
            PdfDocument outputDocument = new PdfDocument();
            outputDocument = unir.UnirPDF(rutas);
            ruta_archTemp = unir.GuardarArchivo(false, false, outputDocument);
            if ((bool)DiapositivasUnir.IsChecked)
            {
                outputDocument = unir.DosDiapositivasEnUna(ruta_archTemp);
                ruta_archTemp = unir.GuardarArchivo(false, false, outputDocument);
            }
            if ((bool)PortadaUnir.IsChecked)
            {
                if (portada != null)
                {
                    rutas.Clear();
                    rutas.Add(portada[0]);
                    rutas.Add(ruta_archTemp);
                    outputDocument = unir.UnirPDF(rutas);
                }
            }
            if (GuardarUnir.IsChecked == true)
            {
                unir.GuardarArchivo(true, true, outputDocument);
            }
            else
            {
                unir.GuardarArchivo(false, true, outputDocument);
            }
            listaArchivos.Clear();
            PortadaUnir.IsChecked = false;
            DiapositivasUnir.IsChecked = false;
        }

        private void Btn_PortadaUnir_Click(object sender, RoutedEventArgs e) //CARGA EL ARCHIVO DE LA PORTADA DE UnirPDF
        {
            UtilsUnir unir = new UtilsUnir();
            portada = unir.CargarArchivos(false);
            if (portada.Length == 0)
            {
                MessageBox.Show("No hay ningún archivo de portada", "ERROR!!",
                                 MessageBoxButton.OK, MessageBoxImage.Error);
                PortadaUnir.IsChecked = false;
            }
            else
            {
                PortadaUnirNombre.Text = System.IO.Path.GetFileName(portada[0]);
            }

        }

        private void PortadaUnir_Checked(object sender, RoutedEventArgs e) //COMPRUEBA SI HAY QUE AÑADIR PORTADA O NO DE UnirPDF
        {
            if ((bool)PortadaUnir.IsChecked)
            {
                Btn_PortadaUnir.IsEnabled = true;
                PortadaUnirNombre.Opacity = 1;
            }
            else
            {
                Btn_PortadaUnir.IsEnabled = false;
                PortadaUnirNombre.Opacity = 0.5;
                PortadaUnirNombre.Text = "Ninguna portada selecionada";
            }
        }

 
        /***** SEGUNDA FUNCIONALIDAD ***** DOS DIAPOSITIVAS POR PDF*****/

        private void Btn_agregar2DP_Click(object sender, RoutedEventArgs e) //BOTON DE AGREAGAR ARCHIVO EN 2 DIAP POR PAGINA
        {
            string[] archivos;
            UtilsUnir unir = new UtilsUnir();
            archivos = unir.CargarArchivos(false);

            if (archivos.Length > 0)
            {
                archivoUnico = new Archivo(archivos[0], false);
                Nombre2DP.Text = "El archivo selecionado es " + System.IO.Path.GetFileName(archivoUnico.Ruta);
            }
        }

        private void Btn_guardar2DP_Click(object sender, RoutedEventArgs e) //BOTON DE GUARDAR ARCHIVO EN 2 DIAP POR PAGINA
        {

            if (archivoUnico.Ruta.Length != 0)
            {
                UtilsUnir unir = new UtilsUnir();
                PdfDocument outputDocument = new PdfDocument();
                outputDocument = unir.DosDiapositivasEnUna(archivoUnico.Ruta);
                if (Guardar2DP.IsChecked == true)
                {
                    unir.GuardarArchivo(true, true, outputDocument);
                }
                else
                {
                    unir.GuardarArchivo(false, true, outputDocument);
                }
            }

        }

        
    }
}
