using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UlearnGame.Sprites
{
    public class Enemy : Mob //враги 
    {
        public Enemy(Texture2D texture) : base(texture)
        {
            Speed = 2f; //скорость врага           
        }

        private float _timer;

        public float ShootingTimer = 1.75f; //период выстрелов

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= ShootingTimer)
            {
                Shoot(-5f);
                _timer = 0;
            }

            Position += new Vector2(-Speed, 0);

            //если враг находится за пределами левой части экрана
            if (Position.X < -_texture.Width)
                IsRemoved = true;
        }

        public override void OnCollide(Sprite sprite)
        {
            //Если враг врежется в игрока, который все еще жив
            if (sprite is Player && !((Player)sprite).IsDead)
            {
                ((Player)sprite).Score.Value++;

                // полностью убрать 
                IsRemoved = true;
            }
               

            //Если враг попадёт под пулю, которая принадлежит игроку
            if (sprite is Bullet && ((Bullet)sprite).Parent is Player)
            {
                Health--;

                if (Health <= 0)
                {
                    IsRemoved = true;
                    ((Player)sprite.Parent).Score.Value++;
                }
            }
        }
    }
}
