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

        public float GravitationX = 0;
        public float GravitationY = 1; 

        public List<IImpactPoint> impactPoints = new List<IImpactPoint>();

        public int X; // Координата X центра эмиттера, будем ее использовать вместо MousePositionX
        public int Y; // Соответствующая координата Y 
        public int Direction = 0; // Вектор направления в градусах куда сыпет эмиттер
        public int Spreading = 360; // Разброс частиц относительно Direction
        public int SpeedMin = 1; // Начальная минимальная скорость движения частицы
        public int SpeedMax = 10; // Начальная максимальная скорость движения частицы
        public int RadiusMin = 2; // Минимальный радиус частицы
        public int RadiusMax = 10; // Максимальный радиус частицы
        public int LifeMin = 20; // Минимальное время жизни частицы
        public int LifeMax = 100; // Максимальное время жизни частицы

        public int ParticlesPerTick = 1; // Количество частиц

        public Color ColorFrom = Color.White; // Начальный цвет частицы
        public Color ColorTo = Color.FromArgb(0, Color.Black); // Конечный цвет частиц

        public virtual Particle CreateParticle()
        {
            var particle = new ParticleColorful();
            particle.FromColor = ColorFrom;
            particle.ToColor = ColorTo;

            return particle;
        }

        // Функцию обновления состояния системы
        public void UpdateState()
        {
            int particlesToCreate = ParticlesPerTick; // Счётчик, сколько частиц нам создавать за тик

            foreach (var particle in particles)
            {
                if (particle.Life <= 0)
                {
                    if (particlesToCreate > 0)
                    {
                        particlesToCreate -= 1; 
                        ResetParticle(particle); 
                    }
                }
                else
                {
                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;


                    particle.Life -= 1;
                    foreach (var point in impactPoints)
                    {
                        point.ImpactParticle(particle);
                    }

                    // Гравитация воздействует на вектор скорости, поэтому пересчитываем его
                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;
                }
            }

            while (particlesToCreate >= 1)
            {
                particlesToCreate -= 1;
                var particle = CreateParticle();
                ResetParticle(particle);
                particles.Add(particle);
            }
        }

        public void Render(Graphics g)
        {
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }

            foreach (var point in impactPoints)
            {
                point.Render(g);
            }
        }


        public int ParticlesCount = 500; //

        public virtual void ResetParticle(Particle particle)
        {
            particle.Life = Particle.rand.Next(LifeMin, LifeMax);

            particle.X = X;
            particle.Y = Y;

            var direction = Direction
                + (double)Particle.rand.Next(Spreading)
                - Spreading / 2;

            var speed = Particle.rand.Next(SpeedMin, SpeedMax);

            particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

            particle.Radius = Particle.rand.Next(RadiusMin, RadiusMax);
        }
    }

    public class TopEmitter : Emitter
    {
        public int Width; // Длина экрана

        public override void ResetParticle(Particle particle)
        {
            base.ResetParticle(particle);

            particle.X = Particle.rand.Next(Width);
            particle.Y = 0;

            particle.SpeedY = 1;
            particle.SpeedX = Particle.rand.Next(-2, 2); 
        }
    }
}
