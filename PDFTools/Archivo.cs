using System.ComponentModel;

namespace PDFTools
{
    class Archivo : INotifyPropertyChanged
    {
        private bool borrarPriv;
        private string rutaPriv;
        public event PropertyChangedEventHandler PropertyChanged;

        public Archivo(string ruta)
        {
            rutaPriv = ruta;
        }
        public Archivo(string ruta, bool borrar)
        {
            rutaPriv = ruta;
            borrarPriv = borrar;
        }
        public Archivo(Archivo arch)
        {
            rutaPriv = arch.Ruta;
            borrarPriv = arch.Borrar;
        }
        public string Ruta
        {
            get { return rutaPriv; }
            set { rutaPriv = value; OnPropertyChanged("Ruta"); }
        }
        public bool Borrar
        {
            get { return borrarPriv; }
            set { borrarPriv = value; OnPropertyChanged("Borrar"); }
        }

        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

    }
}