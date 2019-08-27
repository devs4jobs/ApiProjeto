using Model;
using FluentValidation;
using Core.util;
using System.Linq;

namespace Core
{
    public class PautaEleitorCore : AbstractValidator<PautaEleitor>
    {
        private PautaEleitor _pautaeleitor;
        public Sistema db { get; set; }
        public PautaEleitorCore()
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        }
        public PautaEleitorCore(PautaEleitor pautaeleitor)

        {
            _pautaeleitor = pautaeleitor;
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();

            RuleFor(e => e.EleitorId).NotEmpty().WithMessage("O eleitor Id nao pode ser vazio");
            RuleFor(e => e.PautaId).NotEmpty().WithMessage("a pauta id nao pode ser vazia");
            RuleFor(a => a.Voto.ToUpper()).NotNull().Must(a => a == "A FAVOR" || a == "CONTRA").WithMessage($"Campo Inválido.");

        }

        // Método para cadastro do voto.
        public Retorno Votar()
        {
            var results = Validate(_pautaeleitor);

            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors.Select(c => c.ErrorMessage).ToList() };

            // procura uma sessao
            var umaSessao = db.Sessoes.SingleOrDefault(c => c.LstPautas.SingleOrDefault(e => e.Id == _pautaeleitor.PautaId) != null);

            // checa se a sessao é valida
            if (umaSessao == null || umaSessao.Status)
                return new Retorno { Status = false, Resultado = "Essa Sessão é invalida!" };

           // checa se o eleitor existe nessa sessao
            if(!(umaSessao.LstEleitores.SingleOrDefault(c => c.Id == _pautaeleitor.EleitorId) != null))
                return new Retorno { Status = false, Resultado = "Esse eleitor não existe" };

            // adciona na lista d
            db.EleitoresPauta.Add(_pautaeleitor);

            // busca a quantidade de votos da pauta a ser votada
            var qtdVotos = db.EleitoresPauta.Where(a => a.PautaId == _pautaeleitor.PautaId).ToList().Count();

            // busca a quantiade de votos eleitores na sessao;
            var qdtEleitores = umaSessao.LstEleitores.Count();

            // busco a pauta e comparo com a quantaidade de para fazer a mudança no status dela;
            if (qtdVotos == qdtEleitores)
            {
                
                var umaPauta = umaSessao.LstPautas.SingleOrDefault(d => d.Id == _pautaeleitor.PautaId);

                umaPauta.Concluida = true;

            }

            // Checo se a sessao foi finalizada, se sim ja faço um novo retorno.
            if (umaSessao.LstPautas.All(e => e.Concluida))
            {
                umaSessao.Status = true;

                file.ManipulacaoDeArquivos(false, db);

                return new Retorno { Status = true, Resultado = $"Essa Sessao foi Finalizada! : Status: {umaSessao.Status} ID {umaSessao.Id}" };
            }

            file.ManipulacaoDeArquivos(false, db);
            return new Retorno() { Status = true, Resultado = _pautaeleitor };
        }

        // Método para buscar uma pauta
        public Retorno AcharUm(string id)
        {
            if (!db.EleitoresPauta.Any(p => p.PautaId.ToString() == id)) 
                return new Retorno() { Status = false, Resultado = null };

            var UmaPautaEleitor = db.EleitoresPauta.Find(c => c.PautaId.ToString() == id);
            return new Retorno() { Status = true, Resultado = UmaPautaEleitor };
        }

        // Método para retornar todas as pautas
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.EleitoresPauta.OrderBy(c => c.PautaId) };
        

        // Método para deletar por id
        public Retorno DeletarId(string id)
        {
           
           var umaPautaEleitor = db.EleitoresPauta.Find(c => c.EleitorId.ToString() == id);

            db.EleitoresPauta.Remove(umaPautaEleitor);

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = null };

        }

        // Método para atualizar a pauta por id
        public Retorno AtualizarUm(string id, PautaEleitor pautaeleitor)
        {
            var umaPautaEleitor = db.EleitoresPauta.Find(c => c.PautaId.ToString() == id);
        
            if (pautaeleitor.Voto != null)
                umaPautaEleitor.Voto = pautaeleitor.Voto;

            if (pautaeleitor.Votou != false)
                umaPautaEleitor.Votou = pautaeleitor.Votou;

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = umaPautaEleitor };
        }
   
    }
}
