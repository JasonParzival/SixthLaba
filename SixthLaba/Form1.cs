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
    // Необходимо реализовать систему управления частицами на базе статей Система частиц часть 1 и часть 2.
    // В остальных случаях вы делаете одно или комбинацию желтых/красных задач.
    // Реализовать точку-счетчик частиц, попадая в которую частица умирает, а на точке пишется сколько частиц она уже собрала.
    // при клике левой кнопкой мыши добавлять новый счетчик
    // при клике правой кнопкой мыши, удалять счетчик 

    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter; // добавим поле для эмиттера

        List<CounterPoint> counterPoints = new List<CounterPoint>();

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            this.emitter = new Emitter 
            {
                Direction = 0,
                Spreading = 6,
                SpeedMin = 5,
                SpeedMax = 10,
                ColorFrom = Color.Gold,
                ColorTo = Color.FromArgb(0, Color.Red),
                ParticlesPerTick = 15,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
            };

            emitters.Add(this.emitter); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState(); // тут теперь обновляем эмиттер

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black); // А ЕЩЕ ЧЕРНЫЙ ФОН СДЕЛАЮ
                emitter.Render(g); // а тут теперь рендерим через эмиттер

                foreach (var counter in counterPoints)
                {
                    counter.Render(g);
                }
            }

            picDisplay.Invalidate();
        }

        private void tbDirection_Scroll(object sender, EventArgs e)
        {
            emitter.Direction = tbDirection.Value; // направлению эмиттера присваиваем значение ползунка 
            lblDirection.Text = $"{tbDirection.Value}°"; // добавил вывод значения
        }

        private void picDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var counter = new CounterPoint
                {
                    X = e.X,
                    Y = e.Y,
                };
                counterPoints.Add(counter);
                emitter.impactPoints.Add(counter);
            }
            else if (e.Button == MouseButtons.Right)
            {
                // От конца списка счетчиков
                for (int i = counterPoints.Count - 1; i >= 0; i--)
                {
                    var point = counterPoints[i];

                    if (Math.Abs(point.X - e.X) < 30 && Math.Abs(point.Y - e.Y) < 30)
                    {
                        counterPoints.Remove(point);
                        emitter.impactPoints.Remove(point);
                        break;
                    }
                }
            }
        }
    }
}
