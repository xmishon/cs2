using System;
using System.Drawing;

namespace MyGame
{
    abstract class BaseObject : ICollision
    {
        public delegate void Message();

        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        protected BaseObject(Point pos, Point dir, Size size)
        {
            if (pos.X > 2 * Game.Width || pos.X < -2 * Game.Width // если позиция находится сильно за пределами экрана
                || pos.Y > 2 * Game.Height || pos.Y < -2 * Game.Height
                || dir.X > 100 || dir.X < -100 // или если направление имеет слишком большие величины (слишком большая скорость)
                || dir.Y > 100 || dir.Y < -100
                || size.Height > (Game.Height / 2) || size.Height < 0 // или если размер создаваемого объекта такой, что будет занимать больше половины экрана
                || size.Width > (Game.Width / 2) || size.Width < 0)
                throw new GameObjectException("Неправильно заданы позиция, направление движения или размер объекта"); // выбрасываем исключение

                Pos = pos;
            Dir = dir;
            Size = size;
        }
        public abstract void Draw();

        public abstract void Update();

        /// <summary>
        /// Перемещает объект в новые случайные координаты, как бы создавая его заново
        /// </summary>
        public virtual void Recreate()
        {
            Random r = new Random();
            Pos.X = r.Next(0, Game.Width);
            Pos.Y = r.Next(0, Game.Height);
        }

        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);

        public Rectangle Rect => new Rectangle(Pos, Size);
    }
}
