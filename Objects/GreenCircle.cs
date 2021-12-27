using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лабораторная_работа__5.Objects
{
    class GreenCircle : BaseObject
    {
        public Action<GreenCircle> OnTimerEnd;
        public int Timer; // таймер

        // конструктор
        public GreenCircle(float x, float y, float angle) : base(x, y, angle) 
        {
            Timer = 100;
        }


        // отрисовка объекта
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Green), -15, -15, 30, 30);
        }

        // получение пути к объекту
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }
        // вычитание единицы у таймера
        public void DecreaceTimer()
        {
            Timer--;
            if (Timer == 0)
            {
                OnTimerEnd?.Invoke(this);
            }
        }
    }
}
