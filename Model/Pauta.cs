namespace Model
{
    public class Pauta : Base
    {
        // Model de pauta herdando da base
        public string Descricao { get; set; }
        public bool Concluida { get; set; }



        public void TrocandoDados(Pauta pauta)
        {
            Descricao = pauta.Descricao;
            DataCadastro = pauta.DataCadastro;
        }

    }
}
