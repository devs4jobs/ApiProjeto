using System.Collections.Generic;

namespace Model
{
    public class Sessao : Base
    {
        public string nomeSessao;
        public bool Encerrada;
        public string dataTermino;
        public List<Voto> votoSessao { get; set; } = new List<Voto>();
        public List<Eleitor> eleitoresSessao { get; set; } = new List<Eleitor>();
        public List<Pauta> pautasSessao { get; set; } = new List<Pauta>();
    }
}
