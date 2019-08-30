using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class Base
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        //método genérico para paginar minhas listas
        public List<T> GetPage<T>(List<T> list, int page, int pageSize)
        {
            if (page <= 0)
                return new List<T>();
            return list.Skip(page - 1 * pageSize).Take(pageSize).ToList();
        }
    }

}

   
