using Model;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Core.util;

namespace Core
{
    public class EleitorCore : AbstractValidator<Eleitor>
    {
        private Eleitor _eleitor { get; set; }
        public EleitorCore() { }
        public EleitorCore( Eleitor eleitor)
        {
            _eleitor = eleitor;

            RuleFor(e => e.Documento)
                .Length(11,11)
                .NotNull()
                .WithMessage("Cpf inválido");

            RuleFor(e => e.Nome)
                .MinimumLength(3)
                .NotNull()
                .WithMessage("O nome deve ser preenchido e deve ter o mínimo de 3 caracteres");
        }

        public Retorno CadastroEleitor() {

            var results = Validate(_eleitor);

            // Se o modelo é inválido retorno false
            if(!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors};

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true,null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if(db.sistema.Eleitores.Find(c=>c.Documento==_eleitor.Documento)!=null)
                return new Retorno() { Status = false, Resultado = "Usuario ja cadastrado" };

            db.sistema.Eleitores.Add(_eleitor);
            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = _eleitor};
        }

        public Retorno ID(Guid id)
        {
            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true, null);

            var ELEITOR = db.sistema.Eleitores.Find(x => x.Id == id);

            return new Retorno() { Status = true, Resultado =ELEITOR};
        }
        public Retorno Lista()
        {
            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true, null);

            return new Retorno() { Status = true, Resultado = db.sistema.Eleitores};
        }

    }
}
