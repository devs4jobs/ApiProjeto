using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class SeedingData 
    {
        private APIContext _api;

        public SeedingData(APIContext context)
        {
            _api = context;
        }

        public void Seed()
        {
            if (_api.Eleitors.Any() ||
                _api.Pautas.Any() ||
                _api.PautaEleitors.Any())
            {
                return; // DB has been seeded
            }
            _api.SaveChanges();
        }

    }
}
