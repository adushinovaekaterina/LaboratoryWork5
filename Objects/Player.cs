using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Лабораторная_работа__5.Objects
{
    class Player : BaseObject
    {
        public int tally = 0; // очки

        // делегат Action представляет некоторое действие, которое ничего не возвращает

        // поле делегат, к которому можно будет привязать реакцию на события
        public Action<Marker> OnMarkerOverlap; // пересечение с маркером // ~ void SomeMethod (Marker m)
        public Action<Circle> OnCircleOverlap; // пересечение с кругом // ~ void SomeMethod (Circle c)

        // чтобы сделать движение игрока более плавным, надо чтобы
        // пересчет его направления происходил через ускорение
        public float vX, vY; // поля под вектор скорости для плавного движение игрока        

        // конструктор
        public Player(float x, float y, float angle) : base(x, y, angle)
        {

        }
        // рендер игрока
        public override void Render(Graphics g)
        {
            // рисуем кружочек с фиолетовым фоном
            g.FillEllipse(new SolidBrush(Color.DarkViolet),-15, -15, 30, 30);

            // очерчиваем кружочку рамку
            g.DrawEllipse(new Pen(Color.Black, 2),-15, -15, 30, 30);

            // рисуем палочку, указывающую направление игрока
            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);
        }

        // метод для получения пути к объекту
        public override GraphicsPath GetGraphicsPath()
        {
            GraphicsPath path = base.GetGraphicsPath(); // вызываем метод GetGraphicsPath базового класса
            path.AddEllipse(-15, -15, 30, 30); // добавляем эллипс
            return path; // возвращаем путь
        }

        // создавая поле делегат, надо создать метод,
        // с помощью которого будет вызываться это событие

        // пересечение с объектами
        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj); // вызываем метод Overlap базового класса

            // если объект является маркером
            if (obj is Marker)
            {
                OnMarkerOverlap(obj as Marker); // as — выполняет операцию приведения типов объекта к заданному типу                                                
            }

            // если объект является зеленым кругом
            if (obj is Circle)
            {
                // вызов делегата. Если делегат принимает параметры, то в метод Invoke передаются значения для этих параметров
                OnCircleOverlap.Invoke((Circle)obj);
            }
        }
    }
}