using Microsoft.EntityFrameworkCore;
using Model;
using Model.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class EleitorCore
    {
        //Classe para as regras de negócio e lógica.

        // Referencia ao Contexto(db)
        private EleicaoContext _eleicaoContext { get; set; }
        //Construtor contendo exigindo o contexto(db).
        public EleitorCore(EleicaoContext eleicaoContext)
        {
            _eleicaoContext = eleicaoContext;
            _eleicaoContext.Eleitores = _eleicaoContext.Set<Eleitor>();
        }
        // Método para fazer o cadastro de um eleitor.
        public Eleitor Cadastrar(Eleitor eleitor)
        {
            _eleicaoContext.Eleitores.Add(eleitor);
            _eleicaoContext.SaveChanges();
            return eleitor;
        }
        // Método para buscar um  eleitor.
        public Eleitor AcharUm(string id) => _eleicaoContext.Eleitores.FirstOrDefault(c => c.Id.ToString() == id);
        // Método para listar todos eleitores.
        public List<Eleitor> AcharTodos() => _eleicaoContext.Eleitores.ToList();
        // Método para atualizar os dados um eleitor.
        public Eleitor Atualizar(Eleitor eleitor)
        {
            if (!_eleicaoContext.Eleitores.Any(b => b.Id.Equals(eleitor.Id)))
                return null;
          
            var umEleitor = _eleicaoContext.Eleitores.FirstOrDefault(c => c.Id == eleitor.Id);

            if (umEleitor != null)
            {
                try
                {
                    _eleicaoContext.Entry(umEleitor).CurrentValues.SetValues(eleitor);
                    _eleicaoContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return eleitor;
        }
        //Método para deletar um eleitor.
        public void DeletarUm(string id)
        {
            var umEleitor = _eleicaoContext.Eleitores.FirstOrDefault(c => c.Id.ToString() == id);
            _eleicaoContext.Eleitores.Remove(umEleitor);
            _eleicaoContext.SaveChanges();
        }
    }
}