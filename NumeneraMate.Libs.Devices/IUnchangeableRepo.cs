using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Libs.Devices
{
    // Interface for loading data from different locations: XML, JSON, Excel, SQLLite
    public interface IUnchangeableRepo<T>
    {
        List<T> GetAllItems();
        Task<List<T>> GetAllItemsAsync();
        T GetItem(string name);
        Task<T> GetItemAsync(string name);
    }
}
