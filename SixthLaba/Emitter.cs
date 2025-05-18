using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixthLaba
{
    public class Emitter
    {
        List<Particle> particles = new List<Particle>();
        public int MousePositionX;
        public int MousePositionY;

        public float GravitationX = 0;
        public float GravitationY = 0; // пусть гравитация будет силой один пиксель за такт, нам хватит

        public List<Point> gravityPoints = new List<Point>(); // тут буду хранится точки притяжения

        // добавил функцию обновления состояния системы
        public void UpdateState()
        {
            foreach (var particle in particles)
            {
                particle.Life -= 1;  // не трогаем
                if (particle.Life < 0)
                {
                    // тоже не трогаем
                    particle.Life = 20 + Particle.rand.Next(100);
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;

                    /* это убираем
                    particle.Direction = Particle.rand.Next(360);
                    particle.Speed = 1 + Particle.rand.Next(10);
                    */

                    /* ЭТО ДОБАВЛЯЮ, тут сброс состояния частицы */
                    var direction = (double)Particle.rand.Next(360);
                    var speed = 1 + Particle.rand.Next(10);

                    particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
                    particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);
                    /* конец ЭТО ДОБАВЛЯЮ  */

                    // это не трогаем
                    particle.Radius = 2 + Particle.rand.Next(10);
                }
                else
                {
                    /* это все убираем, тут у нас старый пересчет положения частицы в пространстве 
                    var directionInRadians = particle.Direction / 180 * Math.PI;
                    particle.X += (float)(particle.Speed * Math.Cos(directionInRadians));
                    particle.Y -= (float)(particle.Speed * Math.Sin(directionInRadians));
                    */

                    // сделаем сначала для одной точки
                    // и так считаем вектор притяжения к точке
                    /*float gX = gravityPoints[0].X - particle.X;
                    float gY = gravityPoints[0].Y - particle.Y;*/

                    // каждая точка по-своему воздействует на вектор скорости
                    foreach (var point in gravityPoints)
                    {
                        float gX = point.X - particle.X;
                        float gY = point.Y - particle.Y;

                        // считаем квадрат расстояния между частицей и точкой r^2
                        float r2 = (float)Math.Max(100, gX * gX + gY * gY); // ограничил
                        float M = 100; // сила притяжения к точке, пусть 100 будет

                        // пересчитываем вектор скорости с учетом притяжения к точке
                        particle.SpeedX += (gX) * M / r2;
                        particle.SpeedY += (gY) * M / r2;
                    }

                    // гравитация воздействует на вектор скорости, поэтому пересчитываем его
                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;

                    // и добавляем новый, собственно он даже проще становится, 
                    // так как теперь мы храним вектор скорости в явном виде и его не надо пересчитывать
                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;
                }
            }

            // добавил генерацию частиц
            // генерирую не более 10 штук за тик
            for (var i = 0; i < 10; ++i)
            {
                if (particles.Count < 500) // пока частиц меньше 500 генерируем новые
                {
                    // а у тут уже наш новый класс используем
                    var particle = new ParticleColorful();
                    // ну и цвета меняем
                    particle.FromColor = Color.Yellow;
                    particle.ToColor = Color.FromArgb(0, Color.Magenta);
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;
                    particles.Add(particle);
                }
                else
                {
                    break; // а если частиц уже 500 штук, то ничего не генерирую
                }
            }
        }

        public void Render(Graphics g)
        {
            // ну тут так и быть уж сам впишу...
            // это то же самое что на форме в методе Render
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }

            // рисую точки притяжения красными кружочками
            foreach (var point in gravityPoints)
            {
                g.FillEllipse(
                    new SolidBrush(Color.Red),
                    point.X - 5,
                    point.Y - 5,
                    10,
                    10
                );
            }
        }
    }
}
