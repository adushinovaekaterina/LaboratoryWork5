using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лабораторная_работа__5.Objects
{
    class Player : BaseObject
    {
        // поле делегат, к которому можно будет привязать реакцию на события
        public Action<Marker> OnMarkerOverlap;
        public Action<GreenCircle>? OnGreenCircleOverlap;

        // чтобы сделать движение игрока более плавным, надо чтобы
        // пересчет его направление происходил через ускорение
        public float vX, vY; // поля под вектор скорости для плавного движение игрока
        public int score = 0; // очки

        // конструктор
        public Player(float x, float y, float angle) : base(x, y, angle)
        {

        }
        // отрисовка объекта
        public override void Render(Graphics g)
        {
            // рисуем кружочек с синим фоном
            g.FillEllipse(
                new SolidBrush(Color.DeepSkyBlue),
                -15, -15,
                30, 30
                );

            // очерчиваем кружочку рамку
            g.DrawEllipse(
                new Pen(Color.Black, 2),
                -15, -15,
                30, 30
                );

            // рисуем палочку, указывающую направления игрока
            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);
        }

        // получение пути объекту
        public override GraphicsPath GetGraphicsPath()
        {
            GraphicsPath path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        // создавая поле делегат, надо создать метод,
        // с помощью которого будет вызываться это событие

        // пересечение с объектами
        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj is Marker)
            {
                OnMarkerOverlap(obj as Marker);
            }

            if (obj is GreenCircle)
            {
                OnGreenCircleOverlap?.Invoke((GreenCircle)obj);
            }
        }
    }
}
