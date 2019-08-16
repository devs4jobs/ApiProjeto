using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class PautaCore
    {

        private readonly Conectionn _context;
    
        public PautaCore(Conectionn context)
        {
            _context = context;
            _context.Pautas = context.Set<Pauta>();
        }
        public Pauta Cadastrar(Pauta pauta) { _context.Pautas.Add(pauta); _context.SaveChanges(); return pauta; }
        public List<Pauta> Procurar() { var lstPautas = _context.Pautas.OrderBy(p => p.DataCadastro).ToList(); return lstPautas; }
        public Pauta ProcurarPorId(string id) => _context.Pautas.SingleOrDefault(p => p.Id.ToString().Equals(id));
        public void Deletar(string id) { _context.Pautas.Remove(_context.Pautas.SingleOrDefault(p => p.Id.ToString().Equals(id))); _context.SaveChanges(); }
        public Pauta Atualizar(Pauta item)
        {
            if (!Exists(item.Id)) return null;

            // Pega o estado atual do registro no banco
            // seta as alterações e salva
            var result = _context.Pautas.SingleOrDefault(b => b.Id == item.Id);
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }
        //esse metodo eu não ultilizo no meu Controller eu só criei para ver se existe um cliente com o ID que a pessoa inseriu !!
        public bool Exists(Guid id)
        {
            return _context.Pautas.Any(b => b.Id.Equals(id));
        }
    }
}