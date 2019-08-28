namespace Core.util
{
    //essa classe retorno serve para eu retornar algo positivo se o status for true ou badrequest se o status for false , o Resultado é dinamico ai eu retorna qualquer coisa.
    public class Retorno
    {
        public bool Status { get; set; }
        public dynamic Resultado { get; set; }
    }
}
