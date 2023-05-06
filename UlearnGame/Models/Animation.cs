using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using UlearnGame.Managers;

namespace UlearnGame.Models
{
    public class Animation : ICloneable // общий класс для всех анимаций
    {
        public int CurrentFrame { get; set; } //текущий кадр

        public int FrameCount { get; private set; } 

        public int FrameHeight { get { return Texture.Height; } } //Высота рамки

        public float FrameSpeed { get; set; } //Скорость рамки

        public int FrameWidth { get { return Texture.Width / FrameCount; } } //Ширина рамки

        public bool IsLooping { get; set; }

        public Texture2D Texture { get; private set; } 

        public Animation(Texture2D texture, int frameCount)
        {
            Texture = texture;

            FrameCount = frameCount;

            IsLooping = true;

            FrameSpeed = 0.2f;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
