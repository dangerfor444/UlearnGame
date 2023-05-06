using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace UlearnGame.Models
{
    public class Input //действия в игре
    {
        public Keys Up { get; set; }

        public Keys Down { get; set; }

        public Keys Left { get; set; }

        public Keys Right { get; set; }

        public Keys Shoot { get; set; }
    }
}
