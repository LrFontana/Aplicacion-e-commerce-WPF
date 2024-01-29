using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // Propiedad.
        public event PropertyChangedEventHandler PropertyChanged;

        // Metodo.
        public void OnPropertyChanged(string porpName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(porpName));
        }

    }
}
