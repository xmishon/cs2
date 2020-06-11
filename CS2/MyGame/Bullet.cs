using System;
using System.Drawing;

namespace MyGame
{
    class Bullet : BaseObject
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size) { }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Двигает пулю по оси X.
        /// Когда пуля достигает конца экрана, она генерится в начале экрана на случайной высоте
        /// </summary>
        public override void Update()
        {
            Pos.X = Pos.X + 8;
        }
    }
}
