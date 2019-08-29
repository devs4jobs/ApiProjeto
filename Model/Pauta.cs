namespace Model
{
    //essa minha classe pauta é a pauta em si que em algum momento vai entrar em votação em alguma sessão.
    //quando todo mundo que tiver na sessão votar nessa pauta ela finaliza.
    public class Pauta : Base
    {
        public string Descricao { get; set; }
        public bool Concluida { get; set; }
        public string Resultado { get; set; }
    }
}
