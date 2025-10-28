using Counter.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Counter.models;

public interface ICounterStore
{
    Task InitAsync();
    Task<List<CounterModel>> GetAllAsync();
    Task<CounterModel?> GetByIdAsync(string id);

    Task AddAsync(CounterModel item);
    Task UpdateAsync(CounterModel item);
    Task DeleteAsync(string id);


}