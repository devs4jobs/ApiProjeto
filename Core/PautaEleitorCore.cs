using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;

namespace Core
{
    public class PautaEleitorCore : AbstractValidator<PautaEleitor>
    {
        private PautaEleitor _pautaEleitor { get; set; }
        public PautaEleitorCore(PautaEleitor pautaeleitor)
        {
            _pautaEleitor = pautaeleitor;
            //o id do eleitor não pode ser nulo !
            RuleFor(e => e.EleitorId)
                .NotNull()
                .WithMessage("o Id do Eleitor não pode ser nulo.");
            //o id da pauta não pode ser nulo ! 
            RuleFor(e => e.PautaId)
                .NotNull()
                .WithMessage("o id da pauta não pode ser nulo.");
            //o voto só pode ser a favor ou contra 
            RuleFor(e => e.Voto.ToLower().Trim())
                .NotNull()
                .Must(voto => voto.Equals("a favor") || voto.Equals("contra"))
                .WithMessage("O voto não pode ser nulo e só pode ser 'a favor' ou 'contra'.");

        }
        public PautaEleitorCore() { }

        public Retorno CadastrarPautaEleitor()
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            var results = Validate(_pautaEleitor);


            //se não tiver uma pauta com o Idpauta inserido ou um Eleitor com ideleitor inserido então eu retorno uma menssagem de erro pro usuario !
            if (!db.sistema.Pautas.Exists(p => p.Id.Equals(_pautaEleitor.PautaId)) || !db.sistema.Eleitores.Exists(e => e.Id.Equals(_pautaEleitor.EleitorId)))
                return new Retorno() { Status = false, Resultado = "Não exite uma Pauta/Eleitor com esses ID's na Base de Dados." };


            //deve haver uma sessão com essa pauta! 
            var ValidarVotoPauta = db.sistema.Sessao.SingleOrDefault(lst => lst.lstPautas.SingleOrDefault(p => p.Id.Equals(_pautaEleitor.PautaId)) != null);

            // o Id do eleitor inserido deve estar ná sessão que tem aquela determinada pauta !
            if (!ValidarVotoPauta.lstEleitores.Exists(e => e.Id.Equals(_pautaEleitor.EleitorId)))
                return new Retorno() { Status = false, Resultado = "Não exite um Eleitor com esses ID na Sessão da Pauta inserida." };

            //a pauta tenque esta aberta e não pode ser nula.
            if (ValidarVotoPauta.Status || ValidarVotoPauta == null)
                new Retorno() { Status = false, Resultado = "Essa Sessão não é válida você só pode votar se a sessão estiver aberta e conter a pauta e o eleitor lá." };

            // Se o modelo é inválido retorno false
            if (!results.IsValid) return new Retorno { Status = false, Resultado = results.Errors.Select(m => m.ErrorMessage).ToList() };

            //se não tiver o sistema instanciado no json eu declaro aqui ! 
            if (db.sistema == null) db.sistema = new Sistema();

         

            //se passou por todas validações , o eleitor votou na tal pauta e eu registo o voto na lista de EleitoresPauta ! e salvo !!
            _pautaEleitor.Votou = true;
            db.sistema.EleitoresPauta.Add(_pautaEleitor);
            
            //Quantidade de votos existentes naquela pauta. 
            var VotosNaPauta = db.sistema.EleitoresPauta.Where(p => p.PautaId.Equals(_pautaEleitor.PautaId)).ToList().Count();
            
            //quantidades de eleitores que devem votar na sessão .
            var QtdEleitores = ValidarVotoPauta.lstEleitores.Count();

            //se os eleitores votaram na pauta então  eu seleciono a pauta lá da lista de pautas da sessão e dou a Pauta como concluida.
            if(QtdEleitores == VotosNaPauta)
            {
                var PautaSelecionada = ValidarVotoPauta.lstPautas.Single(p => p.Id.Equals(_pautaEleitor.PautaId));
                PautaSelecionada.Concluida = true;
            }

            //aqui eu vejo se todas as pautas foram encerradas
            if (ValidarVotoPauta.lstPautas.All(p => p.Concluida.Equals(true)))
                ValidarVotoPauta.Status = true;

            file.ManipulacaoDeArquivos(false, db.sistema);

            //eu retorno pro usuario o volto dele como ficou salvo no sistem ! 
            return new Retorno() { Status = true, Resultado = _pautaEleitor };


        }
        //EU CRIEI ESSE METODO PARA PODER EXIBIR  VOTO(PautaEleitor) POR ID.
        public Retorno ProcurarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return db.sistema.EleitoresPauta.Any(e => e.PautaId.ToString().Equals(id)) ? new Retorno() { Status = true, Resultado = db.sistema.EleitoresPauta.SingleOrDefault(e => e.PautaId.ToString().Equals(id)) } : new Retorno() { Status = false, Resultado = "Não existe uma PautaEleitor com esse ID." };
        }

        //EU CRIEI ESSE METODO PARA PODER EXIBIR  TODOS VOTOS(PautaEleitor) POR ID.
        public Retorno ProcurarTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return db.sistema.EleitoresPauta.Any() ? new Retorno() { Status = true, Resultado = db.sistema.EleitoresPauta } : new Retorno() { Status = false, Resultado = "Não existe nenhum elemento." };

        }

        //Deletar Votos(PautaEleitor) por ID.
        public Retorno DeletarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (db.sistema.EleitoresPauta.Exists(e => e.PautaId.ToString().Equals(id)))
            {
                db.sistema.EleitoresPauta.Remove(db.sistema.EleitoresPauta.Single(e => e.PautaId.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);

                return new Retorno() { Status = true, Resultado = "PautaEleitor Deletada com sucesso." };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma PautaEleitor com esse ID ,nada foi deletado." };

        }

        //Metodo PARA ATUALIZAR O VOTO
        public Retorno AtualizarPorID(string id, PautaEleitor pautaeleitor)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();


            if (db.sistema.EleitoresPauta.Exists(e => e.PautaId.ToString().Equals(id)))
            {
                var elementoAtualizado = TrocaDadosPautas(pautaeleitor, db.sistema.EleitoresPauta.SingleOrDefault(e => e.PautaId.ToString().Equals(id)));
                db.sistema.EleitoresPauta.Add(elementoAtualizado);
                db.sistema.EleitoresPauta.Remove(db.sistema.EleitoresPauta.FirstOrDefault(e => e.PautaId.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = elementoAtualizado };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma PautaEleitor com esse ID, Nenhuma Pauta foi atualizado." };

        }

        //ESSE METODO EU SÓ ULTILIZO PARA TROCAR OS DADOS QUE A PAUTAELEITOR TEM PELO QUE A PESSOA INSERIU.
        protected PautaEleitor TrocaDadosPautas(PautaEleitor inserida, PautaEleitor Existente)
        {

            if (inserida.Voto == null) inserida.Voto = Existente.Voto;

            if (inserida.Votou == false) inserida.Votou = Existente.Votou;

            inserida.PautaId = Existente.PautaId;

            inserida.EleitorId = Existente.EleitorId;

            return inserida;
        }

    }

}

