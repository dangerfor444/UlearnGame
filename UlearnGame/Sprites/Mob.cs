using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UlearnGame.Sprites
{
    public class Mob : Sprite, ICollidable // общий класс для всего что движется в игре
    {
        public int Health { get; set; } //хп

        public Bullet Bullet { get; set; } //умеем стрелять

        public float Speed; //скорость

        public Mob(Texture2D texture) : base(texture)
        {
        }

        public void Shoot(float speed)
        {
            if (Bullet == null)
                return;

            
            var bullet = Bullet.Clone() as Bullet;
            bullet.Position = this.Position;
            bullet.Colour = this.Colour;
            bullet.Layer = 0.1f;
            bullet.LifeSpan = 5f;
            bullet.Velocity = new Vector2(speed, 0f);
            bullet.Parent = this;

            Children.Add(bullet);
        }

        public virtual void OnCollide(Sprite sprite)
        {
            throw new NotImplementedException();
        }
       
    }
}
