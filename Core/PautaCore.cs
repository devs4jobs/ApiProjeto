using Core.util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
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

            if(_Pauta.Concluida==true) pauta.Concluida = _Pauta.Concluida;
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
