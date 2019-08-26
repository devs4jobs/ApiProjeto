namespace Model
{
    public class Pauta : Base
    {
        public string Descricao { get; set; }
        public bool Concluida { get; set; }

        public void Trocar(Pauta pauta)
        {
            Descricao = pauta.Descricao;
            DataCadastro = pauta.DataCadastro;
            Concluida = pauta.Concluida;
        }
    }
}
