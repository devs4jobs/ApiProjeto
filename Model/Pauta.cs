using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Pauta : Base
    {
        // Model de pauta herdando da base
        public string Descricao { get; set; }
        public bool Concluida { get; set; }
    }
}
