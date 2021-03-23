using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Apps.ConsoleApp
{
    // Interface for loading data from different locations: XML, JSON, Excel, SQLLite
    // and for saving data (current devices list for example) to different locations: XML, JSON, Excel, SQLite
    // IRepo<Cypher> -> XMLRepoBase<Cypher> -> CypherRepo
    public interface IRepo<T>
    {
        bool IsChangesProhibited { get; }
        List<T> GetAll();
        T GetOne(string name);
        void Add(T Entity);
        void AddRange(T Entity);
        void Delete(string name);
        void Save();
    }
}
