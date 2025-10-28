using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Counter.models;
using SQLite;

namespace Counter.models
{
    public class CounterStore : ICounterStore
    {
        readonly string _dbPath;
        SQLiteAsyncConnection _db;

        public CounterStore()
        {

            _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "counters.db3");

        }

        public async Task InitAsync()
        {
            if (_db != null) return;
            _db = new SQLiteAsyncConnection(_dbPath,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            await _db.CreateTableAsync<CounterModel>();

            async Task EnsureColumn(string colName, string sqlType, string defaultSql)
            {
                var info = await _db.ExecuteScalarAsync<int>(
                    $"SELECT COUNT(*) FROM pragma_table_info('CounterModel') WHERE name='{colName}'");
                if (info == 0)
                    await _db.ExecuteAsync($"ALTER TABLE CounterModel ADD COLUMN {colName} {sqlType} {defaultSql}");
            }
            await EnsureColumn("InitialValue", "INTEGER", "DEFAULT 0");
            await EnsureColumn("ColorHex", "TEXT", "DEFAULT '#FFCC00'");
        }

        public async Task<List<CounterModel>> GetAllAsync()
        {
            await InitAsync();
            return await _db.Table<CounterModel>().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task AddAsync(CounterModel item)
        {
            await InitAsync();
            await _db.InsertAsync(item);
        }

        public async Task UpdateAsync(CounterModel item)
        {
            await InitAsync();
            await _db.UpdateAsync(item);
        }

        public async Task DeleteAsync(string id)
        {
            await InitAsync();
            await _db.Table<CounterModel>().DeleteAsync(x => x.Id == id);
        }

        public async Task<CounterModel?> GetByIdAsync(string id)
        {
            await InitAsync();
            return await _db.Table<CounterModel>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

    }
}
