using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace Counter.models
{
    public class CounterModel : INotifyPropertyChanged
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

     string _name = "Licznik";
     int _value;
     int _initialValue;          
     string _colorHex = "#FFCC00";

        public string Name
    {
        get => _name;
        set { if (_name != value) { _name = value; OnPropertyChanged(); } }
    }

    public int Value
    {
        get => _value;
        set { if (_value != value) { _value = value; OnPropertyChanged(); } }
    }

    public int InitialValue
    {
        get => _initialValue;
        set { if (_initialValue != value) { _initialValue = value; OnPropertyChanged(); } }
    }

    public string ColorHex
    {
        get => _colorHex;
        set { if (_colorHex != value) { _colorHex = value; OnPropertyChanged(); } }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string p = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
}
}
