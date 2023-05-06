using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UlearnGame.Sprites
{
    public interface ICollidable //неизбежные соприкосновения в игре
    {
        void OnCollide(Sprite sprite);
    }
}
