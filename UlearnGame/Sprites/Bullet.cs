using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UlearnGame.Sprites
{
    public class Bullet : Sprite, ICollidable //стрельба в игре
    {
        private float _timer;

        public Explosion Explosion;
        

        public float LifeSpan { get; set; } // время жизни

        public Vector2 Velocity { get; set; } //векторная скорость

        public Bullet(Texture2D texture)
          : base(texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= LifeSpan)
                IsRemoved = true; //удаляем

            Position += Velocity * 2; // задаём движение /////////////////////////////
        }

        public void OnCollide(Sprite sprite) //Столконовения:
        {
            //Пули не сталкиваются друг с другом
            if (sprite is Bullet) 
                return;

            //Враги не могут стрелять друг в друга
            if (sprite is Enemy && this.Parent is Enemy) 
                return;

            //Игроки не могут стрелять друг в друга
            if (sprite is Player && this.Parent is Player) 
                return;


            //Нельзя попасть в игрока, если он мертв
            if (sprite is Player && ((Player)sprite).IsDead) 
                return;


            //попадпние врага в игрока
            if (sprite is Enemy && this.Parent is Player) 
            {
                IsRemoved = true;
                AddExplosion(); //происходит взрыв
            }

            //попадпние игрока во врага
            if (sprite is Player && this.Parent is Enemy) 
            {
                IsRemoved = true;
                AddExplosion();
            }
        }

        public void AddExplosion() //взрыв
        {
            if (Explosion == null)
                return;

            var explosion = Explosion.Clone() as Explosion;
            explosion.Position = this.Position;

            Children.Add(explosion);
        }
    }
}
