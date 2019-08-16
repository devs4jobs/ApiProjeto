using Microsoft.EntityFrameworkCore;
using Model;
using Model.Db;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Core
{
    public class PautaCore
    {
        //Classe para as regras de negócio e lógica.

        // Referencia ao Contexto(db)
        private EleicaoContext _eleicaoContext { get; set; }

        //Construtor contendo exigindo o contexto(db)
        public PautaCore(EleicaoContext EleicaoContext)
        {
            _eleicaoContext = EleicaoContext;
            _eleicaoContext.Pautas = _eleicaoContext.Set<Pauta>();
        }
        //Método para o cadastro de pautas.
        public Pauta Cadastrar(Pauta pauta)
        {
            _eleicaoContext.Pautas.Add(pauta);
            _eleicaoContext.SaveChanges();
            return pauta;
        }
        //Método para buscar uma pauta pelo Id
        public Pauta AcharUm(string id) => _eleicaoContext.Pautas.FirstOrDefault(c => c.Id.ToString() == id);
        //Método para buscar todas as pautas registradas
        public List<Pauta> AcharTodos() => _eleicaoContext.Pautas.ToList();
        //Método para atualizar os dados de uma pauta
        public Pauta Atualizar(Pauta pauta)
        {
            if (!_eleicaoContext.Pautas.Any(b => b.Id.Equals(pauta.Id)))
                return null;

            var umaPauta = _eleicaoContext.Pautas.FirstOrDefault(c => c.Id == pauta.Id);

            if (umaPauta != null)
            {
                try
                {
                    _eleicaoContext.Entry(umaPauta).CurrentValues.SetValues(pauta);
                    _eleicaoContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return pauta;
        }
        //Método para deletar uma pauta pelo id.
        public void DeletarUm(string id)
        {
            var umaPauta = _eleicaoContext.Pautas.FirstOrDefault(c => c.Id.ToString() == id);
            _eleicaoContext.Pautas.Remove(umaPauta);
            _eleicaoContext.SaveChanges();
        }
        //Método para vefificar o id
        public bool VerificaId(string id) => _eleicaoContext.Pautas.Any(c => c.Id.ToString() == id);
    }
}