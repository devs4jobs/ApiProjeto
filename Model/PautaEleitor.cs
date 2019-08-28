using System;
namespace Model
{
    //essa minha classe PautaEleitor serve para fazer a junção de um eleitor a uma pauta e ela já é o voto em si.
    public class PautaEleitor
    {
        public Guid PautaId { get; set; }
        public Guid EleitorId { get; set; }
        public bool Votou { get; set; }
        public string Voto { get; set; }
    }
}
