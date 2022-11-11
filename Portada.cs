using System.ComponentModel;

namespace PDFTools
{
    class Portada : INotifyPropertyChanged
    {
        private string nombreAsignaturaPriv;
        private string nombreApellidosPriv;
        private string cursoPriv;
        private string centroPriv;
        private System.Windows.Media.Color colorPriv;
        private bool esNegroPriv;
        public event PropertyChangedEventHandler PropertyChanged;

        public Portada(string nombreAsignatura, string nombreApellidos, string centro, string curso, System.Windows.Media.Color color, bool esNegro)
        {
            nombreAsignaturaPriv = nombreAsignatura;
            nombreApellidosPriv = nombreApellidos;
            cursoPriv = curso;
            centroPriv = centro;
            colorPriv = color;
            esNegroPriv = esNegro;
        }
        public string NombreAsignatura
        {
            get { return nombreAsignaturaPriv; }
            set { nombreAsignaturaPriv = value; OnPropertyChanged("NombreAsignatura"); }
        }
        public string NombreApellidos
        {
            get { return nombreApellidosPriv; }
        }
        public string Curso
        {
            get { return cursoPriv; }
        }
        public string Centro
        {
            get { return centroPriv; }
        }
        public System.Windows.Media.Color Color
        {
            get { return colorPriv; }
        }
        public bool EsNegro
        {
            get { return esNegroPriv; }
        }
        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
