using Core.util;
using FluentValidation;
using Model;
using System;
using System.Linq;

namespace Core
{
    public class PautaCore : AbstractValidator<Pauta>
    {
        private Pauta _Pauta { get; set; }
        private Sistema Db { get; set; }
        public PautaCore()
        {

            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (Db == null)
                Db = new Sistema();
        }
        public PautaCore(Pauta pauta)
        {
            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if(Db == null)
                Db = new Sistema();

            _Pauta = pauta;

            RuleFor(e => e.Descricao)
                .MinimumLength(15)
                .NotNull()
                .WithMessage("O descrição deve ser preenchida");
        }

        public Retorno CadastroPauta()
        {

            var results = Validate(_Pauta);

            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };

            if (Db.Pautas.Find(c => c.Descricao == _Pauta.Descricao) != null)
                return new Retorno() { Status = false, Resultado = "Pauta ja cadastrada" };

            Db.Pautas.Add(_Pauta);
            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = _Pauta };
        }

        public Retorno PorData(string date, string time)
        {
            //Testa se os dados são datas
            if (!DateTime.TryParse(date, out DateTime date1) && !DateTime.TryParse(time, out DateTime time1))
                return new Retorno() { Status = false, Resultado = "Dados Invalidos" };

            //Caso Data final seja nula ou errada
            if (!DateTime.TryParse(time, out time1))
                return new Retorno() { Status = true, Resultado = Db.Pautas.Where(x => x.DataCadastro >= date1) };

            //Caso Data inicial seja nula ou errada
            if (!DateTime.TryParse(date, out date1))
                return new Retorno() { Status = true, Resultado = Db.Pautas.Where(x => x.DataCadastro <= time1) };

            return new Retorno() { Status = true, Resultado = Db.Pautas.Where(x => x.DataCadastro >= date1 && x.DataCadastro <= time1) };
        }

        public Retorno PorPagina(int NPagina, string Direcao, int TPagina)
        {
            if (Direcao.ToLower() == "asc" && NPagina >= 1 && TPagina >= 1)
                return new Retorno() { Status = true, Resultado = Db.Pautas.OrderBy(x => x.Descricao).Skip((NPagina - 1) * TPagina).Take(TPagina).ToList() };

            if (Direcao.ToLower() == "des" && NPagina >= 1 && TPagina >= 1)
                return new Retorno() { Status = true, Resultado = Db.Pautas.OrderByDescending(x => x.Descricao).Skip((NPagina - 1) * TPagina).Take(TPagina).ToList() };

            //se paginação é não é possivel
            return new Retorno() { Status = false, Resultado = "Digite as propriedades corretas" };
        }

        public Retorno ID(Guid id)
        {
            var Pauta = Db.Pautas.SingleOrDefault(e => e.Id == id);

            if (Pauta == null)
                return new Retorno() { Status = false, Resultado = "Pauta não existe" };

           return new Retorno() { Status = true, Resultado = Pauta };
        }
        public Retorno Lista()=> new Retorno() { Status = true, Resultado = Db.Pautas };
        
        public Retorno AtualizaPauta()
        {
            var pauta = Db.Pautas.SingleOrDefault(c => c.Id == _Pauta.Id);

            if (pauta == null)
                return new Retorno { Status = false, Resultado = "Pauta não existe" };

            if (_Pauta.Descricao != null) pauta.Descricao = _Pauta.Descricao;

            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = pauta };
        }
        public Retorno DeletaPauta(Guid id)
        {
            _Pauta = Db.Pautas.SingleOrDefault(c => c.Id == id);

            if (_Pauta == null)
                return new Retorno() { Status = false, Resultado = "Eleitor não existe" };

            Db.Pautas.Remove(_Pauta);
            file.ManipulacaoDeArquivos(false,Db);

            return new Retorno() { Status = true, Resultado = _Pauta };
        }
    }
}
