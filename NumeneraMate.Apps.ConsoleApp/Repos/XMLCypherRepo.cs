using NumeneraMate.Libs.Devices;
using NumeneraMate.Support.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Apps.ConsoleApp
{
    public class XMLCypherRepo : IRepo<Cypher>
    {
        public bool IsChangesProhibited { get; }
        string _xmlFileName;

        public XMLCypherRepo(bool isChangesProhibited, string xmlFileName)
        {
            IsChangesProhibited = isChangesProhibited;
            _xmlFileName = xmlFileName;
        }

        public List<Cypher> GetAll()
        {
            return NumeneraXML.DeserializeCyphersListFromXML(_xmlFileName);
        }

        public Cypher GetOne(string name)
        {
            throw new NotImplementedException();
        }

        public void Add(Cypher Entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(Cypher Entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string name)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
