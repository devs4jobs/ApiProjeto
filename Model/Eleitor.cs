using System;

namespace Model
{
    public class Eleitor : Base
    {
        //Model de eleitor herdando da base.
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }
    }
}
