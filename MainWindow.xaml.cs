using PdfSharp.Pdf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace PDFTools
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Archivo> listaArchivos;
        private ObservableCollection<Portada> listaPortadas;
        private Archivo archivoUnico;
        public string[] portada;
        public MainWindow()
        {
            InitializeComponent();
            listaArchivos = new ObservableCollection<Archivo>();
            ListaUnir.ItemsSource = listaArchivos;
            listaPortadas = new ObservableCollection<Portada>();
            ListaPortadas.ItemsSource = listaPortadas;

        }
        string ficheroDatos = @"c:\PdfTools\datos.bin";
        string nombreApellidoGlobal = "";
        string centroGlobal = "";
        string cursoGlobal = "";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(ficheroDatos))
            {
                StreamReader lector = new StreamReader(ficheroDatos);

                nombreApellidoGlobal = lector.ReadLine();
                TuNombreApellidos.Text = nombreApellidoGlobal;
                TuNombreApellidosCONST.Text = nombreApellidoGlobal;

                centroGlobal = lector.ReadLine();
                Centro.Text = centroGlobal;
                CentroCONST.Text = centroGlobal;

                cursoGlobal = lector.ReadLine();
                Curso.Text = cursoGlobal;
                CursoCONST.Text = cursoGlobal;

                lector.Close();
            }
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


        /***** TERCERA FUNCIONALIDAD ***** CREAR PORTADAS*****/
        private void ANadirPortada_Click(object sender, RoutedEventArgs e)
        {
            if(NombreAsignatura.Text == "" || TuNombreApellidos.Text == "" || Centro.Text == "" || Curso.Text == "")
            {
                MessageBox.Show("Tienes que rellenar todos los campos", "PDFTools",
                                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                
                bool negro;
                if ((bool)NegroPortadaTexto.IsChecked) { negro = true; }
                else { negro = false; }
                System.Windows.Media.Color col = ColorPortada.SelectedColor;
                listaPortadas.Add(new Portada(NombreAsignatura.Text, TuNombreApellidos.Text, Centro.Text, Curso.Text, col, negro));
            }
        }

        private void ListaPortadas_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ListaPortadas.SelectedValue != null)
            {
                borrarPortadaCreada.IsEnabled = true;
                Portada portadaSelecionada = (Portada)ListaPortadas.SelectedValue;
                NombreAsignatura.Text = portadaSelecionada.NombreAsignatura;
                TuNombreApellidos.Text = portadaSelecionada.NombreApellidos;
                Centro.Text = portadaSelecionada.Centro;
                Curso.Text = portadaSelecionada.Curso;

                ColorPortada.SelectedColor = portadaSelecionada.Color;
                if (portadaSelecionada.EsNegro == true)
                {
                    NegroPortadaTexto.IsChecked = true;
                }
                else
                {
                    BlancoPortadaTexto.IsChecked = true;
                }
            }
            else
            {
                borrarPortadaCreada.IsEnabled = false;
            }
            
        }

        private void BorrarPortadaCreada_Click(object sender, RoutedEventArgs e)
        {
            Portada portadaSelecionada = (Portada)ListaPortadas.SelectedValue;
            listaPortadas.Remove(portadaSelecionada);
            NombreAsignatura.Text = "";
            TuNombreApellidos.Text = nombreApellidoGlobal;
            Centro.Text = centroGlobal;
            Curso.Text = cursoGlobal;
            System.Windows.Media.Color col = System.Windows.Media.Color.FromRgb(0, 0, 0);
            ColorPortada.SelectedColor = col;
            NegroPortadaTexto.IsChecked = true;
        }

        private void GenerarPortadas_Click(object sender, RoutedEventArgs e)
        {
            if(listaPortadas.Count > 0)
            {
                UtilsPortadas.generarPortadas(listaPortadas);
            }
            else
            {
                MessageBox.Show("Introduce alguna portada para generar", "PDFTools",
                                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void CrearConstantes_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(ficheroDatos))
            {
                string directorio = @"c:\PdfTools";
                Directory.CreateDirectory(directorio);
            }
            StreamWriter escritor = new StreamWriter(ficheroDatos);

            escritor.WriteLine(TuNombreApellidosCONST.Text);
            nombreApellidoGlobal = TuNombreApellidosCONST.Text;
            TuNombreApellidos.Text = nombreApellidoGlobal;

            escritor.WriteLine(CentroCONST.Text);
            centroGlobal = CentroCONST.Text;
            Centro.Text = centroGlobal;

            escritor.WriteLine(CursoCONST.Text);
            cursoGlobal = CursoCONST.Text;
            Curso.Text = cursoGlobal;
            

            escritor.Close();

            MessageBox.Show("Se ha creado los datos predeterminados correctamente", "PDFTools",
                                              MessageBoxButton.OK, MessageBoxImage.Information);
        }

        
    }
}
