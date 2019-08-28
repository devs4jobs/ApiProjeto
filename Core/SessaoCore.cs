using Core.util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class SessaoCore : AbstractValidator<Sessao>
    {
        private Sessao _Sessao { get; set; }
        private Sistema Db { get; set; }
        public SessaoCore()
        {
            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (Db == null)
                Db = new Sistema();
        }
        public SessaoCore(Sessao sessao)
        {
            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (Db == null)
                Db = new Sistema();

            _Sessao = sessao;

            RuleFor(e => e.Eleitores)
                .NotNull()
                .WithMessage("Eleitores não pode ser nulo");

            RuleForEach(e => e.Eleitores)
                .Must(temp => Db.Eleitores.SingleOrDefault(check => check.Id == temp.Id) != null)
                .WithMessage($"Eleitor com o ID:{_Sessao.Eleitores.SingleOrDefault(temp => Db.Eleitores.SingleOrDefault(check => check.Id == temp.Id) == null).Id.ToString()} não conta na base de dados");

            RuleFor(e => e.Pautas)
                .NotNull()
                .WithMessage("Pautas não pode ser nulo");

            RuleForEach(e=>e.Pautas)
                .Must(temp => Db.Pautas.SingleOrDefault(check => check.Id == temp.Id) != null)
                .WithMessage($"Pauta com o ID:{_Sessao.Pautas.SingleOrDefault(temp=>Db.Pautas.SingleOrDefault(check=>check.Id==temp.Id)==null).Id.ToString()} não conta na base de dados");
        }

        public Retorno IniciarSessao()
        {

            var results = Validate(_Sessao);
            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors.Select(c => c.ErrorMessage) };

            _Sessao.Status = true;
            _Sessao.Eleitores.ForEach(c => c.Trocar(Db.Eleitores.SingleOrDefault(d => d.Id == c.Id)));
            _Sessao.Pautas.ForEach(c => c.Trocar(Db.Pautas.SingleOrDefault(d => d.Id == c.Id)));

            if (_Sessao.Pautas.Where(c => c.Concluida == true) != null)
                return new Retorno() { Status = false, Resultado =("Essas pautas ja foram  finalizadas tire-as e inicie a Sessão novamente",_Sessao.Pautas.Where(c => c.Concluida == true)) };

            Db.Sessaos.Add(_Sessao);

            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = _Sessao };
        }

        public Retorno ID(Guid id)
        {
            var sessao = Db.Sessaos.SingleOrDefault(e => e.Id == id);

            if (sessao == null)
                return new Retorno() { Status = false, Resultado = "Sessão não existe" };

            return new Retorno() { Status = true, Resultado = sessao };
        }

        public Retorno PorPagina(int NPagina,string Direcao,int TPagina)
        {
            if (Direcao.ToLower() == "asc" && NPagina >= 1 && TPagina >= 1)
                return new Retorno() { Status = true, Resultado = Db.Sessaos.OrderBy(x => x.Status).Skip((NPagina - 1) * TPagina).Take(TPagina).ToList() };

            if (Direcao.ToLower() == "des" && NPagina >= 1 && TPagina >= 1)
                return new Retorno() { Status = true, Resultado = Db.Sessaos.OrderByDescending(x => x.Status).Skip((NPagina - 1) * TPagina).Take(TPagina).ToList() };

            //se paginação é não é possivel
            return new Retorno() { Status = false, Resultado = "Digite as propriedades corretas" };
        }

        public Retorno PorData(string date, string time)
        {
            //Testa se os dados são datas
            if (!DateTime.TryParse(date, out DateTime date1) && !DateTime.TryParse(time, out DateTime time1))
                return new Retorno() { Status = false, Resultado = "Dados Invalidos" };

            //Caso Data final seja nula ou errada
            if (!DateTime.TryParse(time, out time1))
                return new Retorno() { Status = true, Resultado = Db.Sessaos.Where(x => x.DataCadastro >= date1) };

            //Caso Data inicial seja nula ou errada
            if (!DateTime.TryParse(date, out date1))
                return new Retorno() { Status = true, Resultado = Db.Sessaos.Where(x => x.DataCadastro <= time1) };

            return new Retorno() { Status = true, Resultado = Db.Sessaos.Where(x => x.DataCadastro >= date1 && x.DataCadastro <= time1) };
        }

        public Retorno Lista() => new Retorno() { Status = true, Resultado = Db.Sessaos };
        public Retorno AdicionaPauta(Guid id,Pauta pauta)
        {
            var Sessao = Db.Sessaos.SingleOrDefault(c => c.Id == id);
            if (Sessao == null)
                return new Retorno() { Status = false, Resultado = "Sessão não existe" };

            pauta = Db.Pautas.SingleOrDefault(c => c.Id == pauta.Id);
            if (pauta == null)
                return new Retorno() { Status = false, Resultado = "Pauta não existe" };

            Sessao.Pautas.Add(pauta);

            return new Retorno() { Status = true, Resultado = Sessao };
        }

        public Retorno AdicionaEleitor(Guid id, Eleitor eleitor)
        {
            var Sessao = Db.Sessaos.SingleOrDefault(c => c.Id == id);
            if (Sessao == null)
                return new Retorno() { Status = false, Resultado = "Sessão não existe" };

            eleitor = Db.Eleitores.SingleOrDefault(c => c.Id == eleitor.Id);
            if (eleitor == null)
                return new Retorno() { Status = false, Resultado = "Pauta não existe" };

            Sessao.Eleitores.Add(eleitor);

            return new Retorno() { Status = true, Resultado = Sessao };
        }
    }
}
