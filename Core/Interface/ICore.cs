using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interface
{
    public interface ICore<T>
    {
        T Cadastrar(T obj);
        T Achar(string id);
        T AcharTodos();
        T Atualizar(string id);
        void DeletarUm(string id);

    }
}
