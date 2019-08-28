namespace Model
{
    //essa minha classe eleitor é o eleitor mesmo aqui eu guardo todos os dados do eleitor.
    public class Eleitor : Base
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }
    }
}
