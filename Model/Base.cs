using System;
namespace Model
{
    //essa minha classe base foi criada só para ser herdada pelo fato dos atributos serem genericos e mais de uma classe precisar desses atributos.
    //eu deixo ela abstrata pq eu só quero herda ela e ela não pode ser instanciada.
    public abstract class Base
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
