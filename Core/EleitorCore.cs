﻿using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Core
{
    public class EleitorCore : AbstractValidator<Eleitor>
    {

        private Eleitor _eleitor { get; set; }
        public EleitorCore(Eleitor eleitor)
        {

            _eleitor = eleitor;

            RuleFor(e => e.Documento)
                .Length(11, 11)
                .NotNull()
                .WithMessage("Cpf inválido");

            RuleFor(e => e.Nome)
                .MinimumLength(3)
                .NotNull()
                .WithMessage("O nome deve ser preenchido e deve ter o mínimo de 3 caracteres");
        }

        public EleitorCore() { }


        public Retorno CadastroEleitor()
        {

            var results = Validate(_eleitor);
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };

            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Eleitores.Exists(x => x.Documento == _eleitor.Documento))
            {

                return new Retorno() { Status = true, Resultado = "CPF já cadastrado" };
            }
            db.sistema.Eleitores.Add(_eleitor);

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = _eleitor };
        }

        public Retorno ExibirEleitorId(string id)
        {

            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            var resultado = arquivo.sistema.Eleitores.Where(x => x.Id == new Guid(id));
            return new Retorno() { Status = true, Resultado = resultado };
        }

        public Retorno ExibirEleitorDataCadastro(string dataCadastro)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            var resultado = arquivo.sistema.Eleitores.Where(x => x.DataCadastro.ToString("ddMMyyyy").Equals(dataCadastro));
            return new Retorno() { Status = true, Resultado = resultado };
        }

        public Retorno ExibirTodos(int page, int sizePage)
        {
            
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            List<Eleitor> thirdPage = GetPage(arquivo.sistema.Eleitores, page, sizePage);

            return new Retorno() { Status = true, Resultado = thirdPage };
        }

        public Retorno DeletarEleitorId(string id)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            var resultado = arquivo.sistema.Eleitores.Remove(arquivo.sistema.Eleitores.Find(s => s.Id == new Guid(id)));

            file.ManipulacaoDeArquivos(false, arquivo.sistema);

            return new Retorno() { Status = true, Resultado = null };
        }

        public Retorno AtualizarId(Eleitor novo, string id)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            var velho = arquivo.sistema.Eleitores.Find(s => s.Id == new Guid(id));
            var troca = TrocaDados(novo, velho);

            arquivo.sistema.Eleitores.Add(troca);
            arquivo.sistema.Eleitores.Remove(velho);

            file.ManipulacaoDeArquivos(false, arquivo.sistema);

            return new Retorno() { Status = true, Resultado = troca };
        }

        public Eleitor TrocaDados(Eleitor novo, Eleitor velho)
        {
            if (velho.Nome == null) novo.Nome = velho.Nome;
            if (velho.Documento == null) novo.Documento = velho.Documento;
            if (velho.Sexo == null) novo.Sexo = velho.Sexo;
            if (velho.Idade == 0) novo.Idade = velho.Idade;
            velho.DataCadastro = novo.DataCadastro;
            novo.Id = velho.Id;
            return novo;
        }


        List<Eleitor> GetPage(List<Eleitor> list, int page, int pageSize)
        {
            if (page <= 0)
                return new List<Eleitor>();
            return list.Skip(page - 1 * pageSize).Take(pageSize).ToList();
        }
    }
}
