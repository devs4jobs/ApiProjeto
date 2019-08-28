using System.Collections.Generic;
namespace Model
{
    //esse minha classe sessão serve para armazenar pautas e eleitores que vão votar nessas pautas.
    //quando todas as pautas da minha sessão for concluida eu finalizo a sessão pelo status.
    public class Sessao : Base
    {
        public List<Pauta> lstPautas { get; set; } = new List<Pauta>();
        public List<Eleitor> lstEleitores { get; set; } = new List<Eleitor>();
        public bool Status { get; set; }
    }
}
