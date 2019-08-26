using System;

namespace Model
{
    public class Eleitor : Base
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }

        public void Trocar(Eleitor eleitor)
        {
            Nome = eleitor.Nome;
            DataCadastro = eleitor.DataCadastro;
            Documento = eleitor.Documento;
            Sexo = eleitor.Sexo;
            Idade = eleitor.Idade;
        }
    }
}
