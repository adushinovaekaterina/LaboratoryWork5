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
    }
}
