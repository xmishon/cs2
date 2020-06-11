using System;
using System.Drawing;

namespace MyGame
{
    class Asteroid : BaseObject, ICloneable
    {
        public int Power { get; set; }
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Brown, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }

        /// <summary>
        /// Вызывает метод Recreate() родительского класса, а затем переписывает координату X
        /// </summary>
        public override void Recreate()
        {
            base.Recreate();
            Pos.X = Game.Width + Size.Width;
        }

        /// <summary>
        /// Создаёт копию астероида
        /// </summary>
        /// <returns>Asteroid</returns>
        public object Clone()
        {
            // создаём копию нашего астероида
            Asteroid asteroid = new Asteroid(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height));
            // Не забываем скопировать новому астероиду Power нашего астероида
            asteroid.Power = Power;

            return asteroid;
        }
    }
}
