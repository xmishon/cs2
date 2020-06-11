using System;
using System.Drawing;

namespace MyGame
{
    class Aid : BaseObject
    {
        public Aid(Point pos, Point dir, Size size) : base(pos, dir, size) { }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Green, Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawString("+", SystemFonts.DefaultFont, Brushes.White, new Point(Pos.X + 5, Pos.Y + 3));
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Recreate();
        }

        /// <summary>
        /// Перемещает аптечку в новые координаты в правой границе экрана, имитируя пересоздание аптечки
        /// </summary>
        public override void Recreate()
        {
            Pos.Y = new Random().Next(0, Game.Height);
            Pos.X = Game.Width + Size.Width;
        }
    }
}
