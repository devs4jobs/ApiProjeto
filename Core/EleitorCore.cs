using Model;
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

    }
}
