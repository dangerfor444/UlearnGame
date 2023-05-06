using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using UlearnGame.Managers;
using UlearnGame.Sprites;

namespace UlearnGame.States
{
    public class GameState : State //окно игры
    {
        

        private List<Texture2D> _textures;

        private List<Sprite> _sprites;

        private List<Player> _players;

        private EnemyManager _enemyManager;

        private SpriteFont _font;

        private ScoreManager _scoreManager;
      
        private SoundEffect ShootSoundPlayer1;

        private SoundEffect ShootSoundPlayer2;

        public SoundEffect DamageSound;

        private float _shootTimer = 0;

        public int PlayerCount;

        public GameState(Game1 game, ContentManager content)
     : base(game, content)
        {
            
        }

       

        public override void LoadContent()
        {
            MediaPlayer.Stop();


            ShootSoundPlayer1 = _content.Load<SoundEffect>("ShootSound");
            ShootSoundPlayer2 = _content.Load<SoundEffect>("ShootSoundPlayer2");
            DamageSound = _content.Load<SoundEffect>("ShootSoundPlayer2");


            _textures = new List<Texture2D>()
            {
                _content.Load<Texture2D>("backroad"),
                _content.Load<Texture2D>("Game"),
            };

            var texture = _textures[Game1.Random.Next(0, _textures.Count)];

           

            var playerTexture1 = _content.Load<Texture2D>("Player");
            var playerTexture2 = _content.Load<Texture2D>("pixil-frame-0");
            var bulletTexture = _content.Load<Texture2D>("Bullet");

            _font = _content.Load<SpriteFont>("Font");

            _scoreManager = ScoreManager.Load();

            _sprites = new List<Sprite>()
            {
                new Sprite(texture)
                {
                  Layer = 0.0f,
                  Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
                }
            };

            var bulletPrefab = new Bullet(bulletTexture)
            {
                
                Explosion = new Explosion(new Dictionary<string, Models.Animation>()
                {
                    { "Explode", new Models.Animation(_content.Load<Texture2D>("Explosion"), 3) { FrameSpeed = 0.1f, } }
                })
                {
                    Layer = 0.5f,
                }
            };

            if (PlayerCount >= 1)
            {
                _sprites.Add(new Player(playerTexture1)
                {
                    Colour = Color.White,
                    Position = new Vector2(100, 200),
                    Layer = 0.3f,
                    Bullet = bulletPrefab,
                    Input = new Models.Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D,
                        Shoot = Keys.Space,
                    },
                    Health = 20,
                    Score = new Models.Score()
                    {
                        PlayerName = "Игрок 1",
                        Value = 0,
                    },
                });
            }

            if (PlayerCount >= 2)
            {
                _sprites.Add(new Player(playerTexture2)
                {
                    Colour = Color.White,
                    Position = new Vector2(100, 400),
                    Layer = 0.4f,
                    Bullet = bulletPrefab,
                    Input = new Models.Input()
                    {
                        Up = Keys.Up,
                        Down = Keys.Down,
                        Left = Keys.Left,
                        Right = Keys.Right,
                        Shoot = Keys.Enter,
                    },
                    Health = 20,
                    Score = new Models.Score()
                    {
                        PlayerName = "Игрок 2",
                        Value = 0,
                    },
                });
            }

            _players = _sprites.Where(c => c is Player).Select(c => (Player)c).ToList();

            _enemyManager = new EnemyManager(_content)
            {
                Bullet = bulletPrefab,
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            foreach (var sprite in _sprites)
                sprite.Update(gameTime);

            _enemyManager.Update(gameTime);
            if (_enemyManager.CanAdd && _sprites.Where(c => c is Enemy).Count() < _enemyManager.MaxEnemies)
            {
                _sprites.Add(_enemyManager.GetEnemy());
                
            }

            //добавляем звук выстрела
            _shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _shootTimer > 0.25f)
            {
                ShootSoundPlayer1.Play();
                SoundEffect.MasterVolume = 0.1f;
                _shootTimer = 0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _shootTimer > 0.25f)
            {
                ShootSoundPlayer2.Play();
                SoundEffect.MasterVolume = 0.1f;
                _shootTimer = 0f;
            }

           

        }

        public override void PostUpdate(GameTime gameTime)
        {
            var collidableSprites = _sprites.Where(c => c is ICollidable);

            foreach (var spriteA in collidableSprites)
            {
                foreach (var spriteB in collidableSprites)
                {
                    // Ничего не делайте, если это один и тот же спрайт
                    if (spriteA == spriteB)
                        continue;

                    if (!spriteA.CollisionArea.Intersects(spriteB.CollisionArea))
                        continue;

                    if (spriteA.Intersects(spriteB))
                        ((ICollidable)spriteA).OnCollide(spriteB);
                }
            }
            //Добавьте дочерние спрайты в список спрайтов 
            int spriteCount = _sprites.Count;
            for (int i = 0; i < spriteCount; i++)
            {
                var sprite = _sprites[i];
                foreach (var child in sprite.Children)
                    _sprites.Add(child);

                sprite.Children = new List<Sprite>();

            }

            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }

            //Если все игроки мертвы, мы сохраняем результаты и возвращаемся в состояние рекордов
            if (_players.All(c => c.IsDead))
            {
                foreach (var player in _players)
                    _scoreManager.Add(player.Score);

                ScoreManager.Save(_scoreManager);

                _game.ChangeState(new HighscoresState(_game, _content));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var sprite in _sprites)
                sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            float x = 40f;
            foreach (var player in _players)
            {
                spriteBatch.DrawString(_font,  player.Score.PlayerName, new Vector2(x, 10f), Color.Red);
                spriteBatch.DrawString(_font, "Здоровье: " + player.Health, new Vector2(x, 30f), Color.Red);
                spriteBatch.DrawString(_font, "Счёт: " + player.Score.Value, new Vector2(x, 50f), Color.Red);

                x += 150;
            }
            spriteBatch.End();
        }
    }
}