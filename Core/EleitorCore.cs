using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class EleitorCore
    {
        private Conectionn _context;
        public EleitorCore(Conectionn context)
        {
            _context = context;
            _context.Eleitores = context.Set<Eleitor>();
        }

        public Eleitor Cadastrar(Eleitor cliente) { _context.Eleitores.Add(cliente); _context.SaveChanges(); return cliente; }
        public List<Eleitor> Procurar() { var lstProdutos = _context.Eleitores.OrderBy(p => p.DataCadastro).ToList(); return lstProdutos; }
        public Eleitor ProcurarPorId(string id) => _context.Eleitores.SingleOrDefault(p => p.Id.ToString().Equals(id));
        public void Deletar(string id) { _context.Eleitores.Remove(_context.Eleitores.SingleOrDefault(p => p.Id.ToString().Equals(id))); _context.SaveChanges(); }
        public Eleitor Atualizar(Eleitor item)
        {
            if (!Exists(item.Id)) return null;

            // Pega o estado atual do registro no banco
            // seta as alterações e salva
            var result = _context.Eleitores.SingleOrDefault(b => b.Id == item.Id);
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
    
        public bool Exists(Guid id)
        {
            return _context.Eleitores.Any(b => b.Id.Equals(id));
        }
    }
}
