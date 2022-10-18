using System.ComponentModel;

namespace PDFTools
{
    class Archivo : INotifyPropertyChanged
    {
        private bool borrarPriv;
        private bool subirPriv;
        private bool bajarPriv;
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
        public bool Subir
        {
            get { return subirPriv; }
            set { subirPriv = value; OnPropertyChanged("Subir"); }
        }
        public bool Bajar
        {
            get { return bajarPriv; }
            set { bajarPriv = value; OnPropertyChanged("Bajar"); }
        }

        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

    }
}