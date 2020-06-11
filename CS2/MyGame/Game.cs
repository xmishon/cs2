using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace MyGame
{
    static class Game
    {
        public static BufferedGraphics Buffer;
        public static BaseObject[] _objs;
        public static Random Rnd = new Random();
        public static event Action<string> LogMessage;

        private static BufferedGraphicsContext _context;
        private static List<Bullet> _bullets = new List<Bullet>();
        private static List<Asteroid> _asteroids = new List<Asteroid>();
        private static int _asteroidsCount;
        private static Ship _ship;
        private static Aid _aid;
        private static Timer _timer;
        private static int _points;

        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }
        
        static Game() { }

        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики
            Graphics g;
            // Предосатвяет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаём объект (поверхность рисования) и связываем его с формой
            // Запомниает размеры формы
            Width = form.ClientSize.Width;
            if (Width > 1000) throw new ArgumentOutOfRangeException("Width", "Ширина экрана больше допустимого значения (1000)");
            Height = form.ClientSize.Height;
            if (Height > 1000) throw new ArgumentOutOfRangeException("Height", "Высота экрана больше допустимого значения (1000)");

            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            _timer = new Timer();
            _timer.Interval = 40;
            _timer.Start();
            _timer.Tick += Timer_Tick;

            //Добавляем обработчики событий
            form.KeyDown += Form_KeyDown;

            Ship.MessageDie += Finish; // Подписываем на событие MessageDie класса Ship обработчик - метод Finish() класса Game (текущего класса)

            Logger.setFilename("Log.txt"); // Задаём имя файла для логирования
            LogMessage += Logger.logConsole; // Подписываем обработчики в классе Logger на событие логирования
            LogMessage += Logger.logFile;

            _asteroidsCount = 5;

            Load();
        }
        
        /// <summary>
        /// Выполняется при загрузке формы
        /// </summary>
        public static void Load()
        {
            _objs = new BaseObject[20];
            _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(20, 15)); // создали корабль
            _aid = new Aid(new Point(1000, Rnd.Next(0, Game.Height)), new Point(-5, 0), new Size(20, 20)); // создали аптечку
            for (var i = 0; i < _objs.Length; i++) //создаём звёзды
            {
                int r = Rnd.Next(2, 6); // влияет на скорость передвижения звезды
                _objs[i] = new Star(new Point(1000, Rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }
            createAsteroids(_asteroidsCount); // создаём коллекцию астероидов размером _asteroidsCount (на старте пять астероидов)
            _points = 0;
            LogMessage("Игра началась!");
        }

        /// <summary>
        /// Отрисовывает все элементы на сцене
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (BaseObject a in _asteroids)
                a?.Draw();
            foreach (BaseObject bullet in _bullets)
                bullet?.Draw();
            _ship?.Draw();
            _aid?.Draw();
            if (_ship != null)
                Buffer.Graphics.DrawString("Energy: " + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Graphics.DrawString("Points: " + _points, SystemFonts.DefaultFont, Brushes.White, 400, 0);
            Buffer.Render();
        }

        /// <summary>
        /// Обновляет положения объектов на сцене запуская метод Update() каждого из объектов.
        /// Проверяет пересечения объектов.
        /// Выполняется каждый кадр (по событию таймера, раз в 40 миллисекунд)
        /// </summary>
        public static void Update()
        {
            if (_asteroids.Count < 1)
            {
                _asteroidsCount++;
                createAsteroids(_asteroidsCount);
            }
            foreach (BaseObject obj in _objs) obj.Update();
            foreach (BaseObject bullet in _bullets) bullet.Update();
            _aid?.Update();
            if (_aid.Collision(_ship)) // проверяем, столкнулась ли аптечка с кораблём
            {
                _aid.Recreate();
                _ship.EnergyHigh(10);
            }
            for (var i = 0; i < _asteroids.Count; i++)
            {
                _asteroids[i].Update();
                for (int j = 0; j < _bullets.Count; j++)
                {
                    if (i > -1 && _bullets[j].Collision(_asteroids[i]))
                    {
                        LogMessage("Уничтожен астероид c мощностью: " + _asteroids[i].Power);
                        System.Media.SystemSounds.Hand.Play();
                        _asteroids.RemoveAt(i);
                        i--;
                        _points++;
                        _bullets.RemoveAt(j);
                        j--;
                    }
                    if (i < 0)
                        break; // если уничтожили последний астероид, прерываем обход пуль
                }
                    
                if (i < 0) continue; // если уничтожили последний астероид, прерываем обход астероидов
                if (!_ship.Collision(_asteroids[i])) continue;
                _ship?.EnergyLow(Rnd.Next(5, 15));
                _asteroids[i].Recreate();
                LogMessage("Астероид попал в корабль.");
                System.Media.SystemSounds.Asterisk.Play();
                if (_ship.Energy <= 0) _ship?.Die();
            }
        }

        /// <summary>
        /// Выполняет ряд действий при проигрыше
        /// </summary>
        public static void Finish()
        {
            _timer?.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.WhiteSmoke, 200, 100);
            Buffer.Render();
            LogMessage("Игра окончена!");
        }

        /// <summary>
        /// Обрабатывает событие тика таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки
        /// По нажатию Control - вылетает пуля. По нажатию стрелок "Вверх" и "Вниз" корабль перемещается в соответствующем направлении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                _bullets.Add(getNewBullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4)));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        /// <summary>
        /// Возвращает новый объект Bullet в координатах pos
        /// </summary>
        /// <param name="pos">координаты, в которых объект будет создан</param>
        /// <returns></returns>
        private static Bullet getNewBullet(Point pos)
        {
            return new Bullet(pos, new Point(5, 0), new Size(4, 2));
        }

        /// <summary>
        /// Создаёт новую коллекцию астероидов указанного размера
        /// </summary>
        /// <param name="count">Размер коллекции</param>
        private static void createAsteroids(int count)
        {
            _asteroids = new List<Asteroid>();
            for(int i = 0; i < count; i++)
            {
                int r = Rnd.Next(25, 60); // влияет на размер астероида
                _asteroids.Add(new Asteroid(new Point(1000, Rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r)));
            }
        }
    }
}
