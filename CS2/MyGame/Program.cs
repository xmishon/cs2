using System;
using System.Windows.Forms;

namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form()
            {
                Width = 800,
                Height = 600

            };
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}
