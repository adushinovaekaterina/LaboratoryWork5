using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Лабораторная_работа__5.Objects
{
    class Circle : BaseObject
    {
        public int Timer; // таймер

        // делегат Action представляет некоторое действие, которое ничего не возвращает
        public Action<Circle> TimerOver; // делегат по окончании таймера // ~ void SomeMethod (BaseObject p, BaseObject obj)        

        // конструктор
        public Circle(float x, float y, float angle) : base(x, y, angle) 
        {
            Timer = 100;
        }

        // рендер зеленого круга
        public override void Render(Graphics g)
        {
            // закраска эллипса на "холсте" g
            g.FillEllipse(new SolidBrush(Color.Green), -15, -15, 30, 30);
        }

        // метод для получения пути к объекту
        public override GraphicsPath GetGraphicsPath()
        {
            GraphicsPath path = base.GetGraphicsPath(); // вызываем метод GetGraphicsPath базового класса
            path.AddEllipse(-15, -15, 30, 30); // добавляем эллипс
            return path; // возвращаем путь
        }

        // метод для вычитания единицы у таймера
        public void Less()
        {
            Timer--;
            // если таймер стал равен нулю
            if (Timer == 0)
            {
                TimerOver.Invoke(this); // вызываем делегат
            }
        }
    }
}
