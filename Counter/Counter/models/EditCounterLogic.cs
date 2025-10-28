
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Counter.models;

public class EditCounterLogic
{
    public CounterModel Item { get; }
    public bool IsNew { get; }
    readonly ICounterStore _store;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand IncCommand { get; }
    public ICommand DecCommand { get; }

    public ICommand ResetCommand { get; }

    public ICommand PickColorCommand { get; }  
    public ICommand DeleteCommand { get; }

    public event EventHandler Saved;

    public EditCounterLogic(CounterModel item, bool isNew, ICounterStore store)
    {
        Item = item;
        IsNew = isNew;
        _store = store;

        
        IncCommand = new Command(async () => { Item.Value++; await _store.UpdateAsync(Item); });
        DecCommand = new Command(async () => { Item.Value--; await _store.UpdateAsync(Item); });

        SaveCommand = new Command(async () => { Saved?.Invoke(this, EventArgs.Empty); await Shell.Current.Navigation.PopAsync(); });
        CancelCommand = new Command(async () => await Shell.Current.Navigation.PopAsync());

        ResetCommand = new Command(async () =>
        {
            var fromDb = await _store.GetByIdAsync(Item.Id);
            var init = fromDb?.InitialValue ?? Item.InitialValue;
            Item.Value = init;
            await _store.UpdateAsync(Item);
        });

        PickColorCommand = new Command(async () =>
        {
            var choice = await Shell.Current.DisplayActionSheet(
                "Kolor licznika", "Anuluj", null,
                "Żółty", "Niebieski", "Zielony", "Czerwony", "Fioletowy");
            if (choice is null || choice == "Anuluj") return;

            Item.ColorHex = choice switch
            {
                "Żółty" => "#FFCC00",
                "Niebieski" => "#88CCFF",
                "Zielony" => "#B4E197",
                "Czerwony" => "#FF9A9A",
                "Fioletowy" => "#D0B3FF",
                _ => Item.ColorHex
            };
            await _store.UpdateAsync(Item);
        });

        DeleteCommand = new Command(async () =>
        {
            var ok = await Shell.Current.DisplayAlert("Usuń licznik",
                $"Usunąć „{Item.Name}”?", "Usuń", "Anuluj");
            if (!ok) return;

            await _store.DeleteAsync(Item.Id);
            await Shell.Current.Navigation.PopAsync();
        
        });

    }
}
