﻿using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class EleitorCore
    {
        private Eleitor _eleitor { get; set; }
        
        
        public EleitorCore( Eleitor eleitor)
        {
            _eleitor = eleitor;
        }

        public EleitorCore()
        {

        }

        public Eleitor Achar(string id )
        {
            return null;
        }
        
        public Eleitor AcharTodos()
        {
            return null;
        }
        public void DeletarUm(string id)
        {

        }

        public void Atualizar(string id)
        {

        }

    }
}
