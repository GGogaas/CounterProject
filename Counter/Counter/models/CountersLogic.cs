using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Counter.models;

namespace Counter.models;

public class CountersLogic
{
    readonly ICounterStore _store;
    

    public ObservableCollection<CounterModel> Items { get; } = new();

    public ICommand AddCommand { get; }
    public ICommand OpenCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand IncCommand { get; }
    public ICommand DecCommand { get; }

    public CountersLogic(ICounterStore store)
    {
        _store = store;

        AddCommand = new Command(async () =>
        {
            
            var name = await Shell.Current.DisplayPromptAsync(
                "Nowy licznik",
                "Podaj nazwę licznika:",
                accept: "OK",
                cancel: "Anuluj",
                placeholder: "Nazwa Licznika",
                maxLength: 50);

            if (name is null) return; 

            
            int startValue = 0;
            while (true)
            {
                var startStr = await Shell.Current.DisplayPromptAsync(
                    "Wartość początkowa",
                    "Podaj liczbę całkowitą:",
                    accept: "OK",
                    cancel: "Anuluj",
                    placeholder: "0",
                    keyboard: Keyboard.Numeric);

                if (startStr is null) return;

                if (int.TryParse(startStr, out startValue))
                    break;

                await Shell.Current.DisplayAlert("Błąd", "Wpisz poprawną liczbę całkowitą.", "OK");
            }

            var choice = await Shell.Current.DisplayActionSheet(
                "Kolor licznika", "Anuluj", null, "Żółty", "Niebieski", "Zielony", "Czerwony", "Fioletowy");
                if (choice is "Anuluj" or null) return;

            string hex = choice switch
            {
                "Żółty" => "#FFCC00",
                "Niebieski" => "#88CCFF",
                "Zielony" => "#B4E197",
                "Czerwony" => "#FF9A9A",
                "Fioletowy" => "#D0B3FF",
                _ => "#FFCC00"
            };

            var item = new CounterModel { Name = name, InitialValue = startValue, Value = startValue, ColorHex = hex };
            await _store.AddAsync(item);            
            Items.Add(item);

            
            var vm = new EditCounterLogic(item, isNew: false, _store);
            var page = new Counter.views.EditCounterPage(vm);
            await Shell.Current.Navigation.PushAsync(page);
        });


        OpenCommand = new Command<CounterModel>(async item =>
        {
            if (item is null) return;
            var copy = new CounterModel
            {
                Id = item.Id,
                Name = item.Name,
                Value = item.Value,
                InitialValue = item.InitialValue, 
                ColorHex = item.ColorHex          
            }; var vm = new EditCounterLogic(copy, isNew: false, _store);
            vm.Saved += async (_, __) =>
            {
                var idx = Items.IndexOf(Items.First(x => x.Id == item.Id));
                Items[idx].Name = vm.Item.Name;
                Items[idx].Value = vm.Item.Value;
                await _store.UpdateAsync(Items[idx]); 
            };
            var page = new Counter.views.EditCounterPage(vm);
            await Shell.Current.Navigation.PushAsync(page);
        });

        DeleteCommand = new Command<CounterModel>(async item =>
        {
            if (item is null) return;
            Items.Remove(item);
            await _store.DeleteAsync(item.Id);
        });

        
        IncCommand = new Command<CounterModel>(async item => { if (item == null) return; item.Value++; await _store.UpdateAsync(item); });
        DecCommand = new Command<CounterModel>(async item => { if (item == null) return; item.Value--; await _store.UpdateAsync(item); });
    }

    public async Task RefreshAsync()
    {
        var all = await _store.GetAllAsync();
        Items.Clear();
        foreach (var c in all) Items.Add(c);

        var first = all.FirstOrDefault();
    }
}
