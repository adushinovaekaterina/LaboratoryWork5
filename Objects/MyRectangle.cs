using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лабораторная_работа__5.Objects
{
    // наследуем BaseObject
    class MyRectangle : BaseObject
    {
        // создаем конструктор с тем же набором параметров что и в BaseObject
        // base(x, y, angle) - вызывает конструктор родительского класса
        public MyRectangle(float x, float y, float angle) : base(x, y, angle)
        {

        }

        // переопределяем Render
        public override void Render(Graphics g)
        {
            // сместим влево-вверх на половину ширины и высоты, чтобы центр был смещен в центр объекта 
            g.FillRectangle(new SolidBrush(Color.Yellow), -25, -15, 50, 30); // заливаем цвет (сначала фон)
            g.DrawRectangle(new Pen(Color.Red, 2), -25, -15, 50, 30); // рисуем прямоугольную рамку (потом рамка)
        }
    }
}
