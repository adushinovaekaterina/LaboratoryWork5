using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лабораторная_работа__5.Objects
{
    class BaseObject
    {
        // определяем объект, у него будет точка расположения в виде  
        public float X; // координат X 
        public float Y; // и Y
        public float Angle; // и угол поворота

        // поле делегат, к которому можно будет привязать реакцию на события
        public Action<BaseObject, BaseObject> OnOverlap;

        // конструктор
        public BaseObject(float x, float y, float angle)
        {
            X = x;
            Y = y;
            Angle = angle;
        }
        // метод для формирования матрицы трансформаций
        public Matrix GetTransform()
        {
            Matrix matrix = new Matrix();
            matrix.Translate(X, Y);
            matrix.Rotate(Angle);

            return matrix;
        }

        // ключевое слово virtual нужно, чтобы метод можно
        // было переопределить в классах наследниках

        // виртуальный метод для отрисовки
        public virtual void Render(Graphics g)
        {

        }

        public virtual GraphicsPath GetGraphicsPath()
        {
            // пока возвращаем пустую форму
            return new GraphicsPath();
        }

        // т.к пересечение учитывает толщину линий и матрицы трансформаций,
        // то для того, чтобы определить пересечение объекта с другим объектом,
        // надо передать туда объект Graphics
        public virtual bool Overlaps(BaseObject obj, Graphics g)
        {
            // берем информацию о форме
            GraphicsPath path1 = this.GetGraphicsPath();
            GraphicsPath path2 = obj.GetGraphicsPath();

            // применяем к объектам матрицы трансформации
            path1.Transform(this.GetTransform());
            path2.Transform(obj.GetTransform());

            // используем класс Region, который позволяет определить 
            // пересечение объектов в данном графическом контексте
            Region region = new Region(path1);
            region.Intersect(path2); // пересекаем формы
            return !region.IsEmpty(g); // если полученная форма не пуста, то значит было пересечение
        }

        // создавая поле делегат, надо создать метод,
        // с помощью которого будет вызываться это событие
        public virtual void Overlap(BaseObject obj)
        {
            // если к полю есть привязанные методы, то
            if (this.OnOverlap != null)
            {
                this.OnOverlap(this, obj); // вызываем их
            }
        }
    }
}
