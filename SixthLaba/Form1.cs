using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SixthLaba
{
    public partial class Form1 : Form
    {
        // собственно список, пока пустой
        List<Particle> particles = new List<Particle>();
        public Form1()
        {
            InitializeComponent();

            // привязал изображение
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            // генерирую 500 частиц
            for (var i = 0; i < 500; ++i)
            {
                var particle = new Particle();
                // переношу частицы в центр изображения
                particle.X = picDisplay.Image.Width / 2;
                particle.Y = picDisplay.Image.Height / 2;
                // добавляю список
                particles.Add(particle);
            }
        }

        int counter = 0; // добавлю счетчик чтобы считать вызовы функции
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++; // увеличиваю значение счетчика каждый вызов
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.White); // добавил очистку
                // рисую на изображении сколько насчитал
                g.DrawString(
                    counter.ToString(), // значения счетчика в виде строки
                    new Font("Arial", 12), // шрифт
                    new SolidBrush(Color.Black), // цвет
                    new PointF
                    { // по центру экрана
                        X = picDisplay.Image.Width / 2,
                        Y = picDisplay.Image.Height / 2
                    }
                );
            }

            // обновить picDisplay
            picDisplay.Invalidate();

        }
    }
}
