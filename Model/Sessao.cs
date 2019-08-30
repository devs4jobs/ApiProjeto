using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class Sessao : Base
    {
        // instancia de uma votação 

        public List<Pauta> LstPautas { get; set; } = new List<Pauta>();
        public List<Eleitor> LstEleitores { get; set; } = new List<Eleitor>();
        public bool Status { get; set; } = false;
        public DateTime DataFim { get; set; } = DateTime.Now.AddDays(1);

        // meotodo para validaçao das pautas.
        public bool ValidaPauta()
        {
            foreach (var item in LstPautas)
            {
                if (item.Concluida)
                    return false;
            }
               
            return true;
        }
    }
}

