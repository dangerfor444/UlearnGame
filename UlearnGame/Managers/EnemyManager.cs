using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UlearnGame.Models;
using UlearnGame.Sprites;

namespace UlearnGame.Managers
{
    public class EnemyManager //логика врагов
    {
        private float _timer;


        private float _timerSpawn;


        private List<Texture2D> _textures;

          

        public bool CanAdd { get; set; } // можем добавить или не можем добавить
        public Bullet Bullet { get; set; } //умеем стрелять

        public int MaxEnemies { get; set; } //максимальное число врагов

        public float SpawnTimer { get; set; } //частота появления врагов

        public float IntensitySpawn1 { get; set; } // изменение частота появления вргов

        public float IntensitySpawn2 { get; set; } // изменение частота появления вргов

        public float IntensitySpawn3 { get; set; } // изменение частота появления вргов

        public float IntensitySpawn4 { get; set; } // изменение частота появления вргов

        public EnemyManager(ContentManager content)
        {
            _textures = new List<Texture2D>()
            {
                content.Load<Texture2D>("monster11"),
                content.Load<Texture2D>("monster2"),
                content.Load<Texture2D>("monster3"),
            };

            

            IntensitySpawn1 = 35f;
            IntensitySpawn2 = 60f;
            IntensitySpawn3 = 120f;
            IntensitySpawn4 = 240f;
            MaxEnemies = 10;
            SpawnTimer = 2.5f;

            var texture = _textures[Game1.Random.Next(0, _textures.Count)];

                                 

        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            _timerSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;


            CanAdd = false;

            if (_timer > SpawnTimer)
            {
                CanAdd = true;

                _timer = 0f;
            }

            if (_timerSpawn > IntensitySpawn1)
            {
                SpawnTimer = 2f;
            }

            if (_timerSpawn > IntensitySpawn2)
            {
                SpawnTimer = 1.7f;
            }

            if (_timerSpawn > IntensitySpawn3)
            {
                SpawnTimer = 1.2f;
            }

            if (_timerSpawn > IntensitySpawn4)
            {
                SpawnTimer = 0.5f;
            }
        }





        public Enemy GetEnemy()
        {
            var texture = _textures[Game1.Random.Next(0, _textures.Count)];
            return new Enemy(texture)
            {
                Colour = Color.White,
                Bullet = Bullet,
                Health = 2,
                Layer = 0.2f,
                Position = new Vector2(Game1.ScreenWidth + texture.Width, Game1.Random.Next(0, Game1.ScreenHeight)),
                Speed = 2 + (float)Game1.Random.NextDouble(),
                ShootingTimer = 1.5f + (float)Game1.Random.NextDouble(),
            };

        }

    }
}
