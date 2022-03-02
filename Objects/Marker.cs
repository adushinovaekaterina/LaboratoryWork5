using System.Drawing;
using System.Drawing.Drawing2D;

namespace Лабораторная_работа__5.Objects
{
    class Marker : BaseObject
    {
        // конструктор
        public Marker(float x, float y, float angle) : base(x, y, angle)
        {

        }

        // рендер маркера
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Red), -3, -3, 6, 6);
            g.DrawEllipse(new Pen(Color.Red, 2), -6, -6, 12, 12);
            g.DrawEllipse(new Pen(Color.Red, 2), -10, -10, 20, 20);
        }

        // метод для получения пути к объекту
        public override GraphicsPath GetGraphicsPath()
        {
            GraphicsPath path = base.GetGraphicsPath(); // вызываем метод GetGraphicsPath базового класса
            path.AddEllipse(-3, -3, 6, 6); // добавляем эллипс
            return path; // возвращаем путь
        }
    }
}