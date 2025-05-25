using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixthLaba
{
    public abstract class IImpactPoint
    {
        public float X;
        public float Y;

        // Абстрактный метод с помощью которого будем изменять состояние частиц
        // например притягивать
        public abstract void ImpactParticle(Particle particle);

        // Базовый класс для отрисовки точечки
        public virtual void Render(Graphics g)
        {
            
        }
    }

    public class CounterPoint : IImpactPoint
    {
        public int Count = 0; // Счетчик собранных частиц
        public int Radius = 30; // Радиус области сбора
        public Color Color = Color.DeepSkyBlue; // Базовый цвет точки

        public override void ImpactParticle(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;
            double r = Math.Sqrt(gX * gX + gY * gY);

            if (r + particle.Radius < Radius)
            {
                particle.Life = 0;
                Count++; 
            }
        }

        public override void Render(Graphics g)
        {
            var color = Color.Pink;

            g.FillEllipse(new SolidBrush(color), X - Radius, Y - Radius, Radius * 2, Radius * 2);

            g.DrawEllipse(
                new Pen(Color.Red, 3.0f),
                X - Radius,
                Y - Radius,
                Radius * 2,
                Radius * 2
            );

            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(Count.ToString(), new Font("Arial", 12), Brushes.Black, X + 2, Y + 2, stringFormat);
        }
    }

}
