using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Лабораторная_работа__5.Objects
{
    class BaseObject
    {
        // определяем объект, у него будет точка расположения в виде  
        public float X; // координат X 
        public float Y; // и Y
        public float Angle; // и угол поворота

        public Random Random = new Random();

        // если нужно абстрагировать вызов какой-либо функции от ее реализации, делегат есть класс, но параметр этого класса - метод

        // делегат Action представляет некоторое действие, которое ничего не возвращает

        // поле делегат, к которому можно будет привязать реакцию на события
        // делегат для вызова метода объекта при пересечении
        public Action<BaseObject, BaseObject> OnOverlap; // ~ ссылается на метод void SomeMethod (BaseObject p, BaseObject obj)

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
            matrix.Translate(X, Y); // применяет указанный вектор смещения к объекту Matrix, добавляя вектор смещения в начало

            matrix.Rotate(Angle); // поворот по часовой стрелке вокруг начала координат на указанный угол

            return matrix;
        }
        // виртуальный метод для отрисовки
        public virtual void Render(Graphics g)
        {

        }

        // метод для установки случайной позиции
        public virtual void RandomPosition(int xMax, int yMax)
        {
            // приравниваем к координате X случайное число от 20 до x-20
            X = Random.Next(20, xMax - 20);
            // приравниваем к координате Y случайное число от 20 до y-20
            Y = Random.Next(20, yMax - 20);
        }

        // класс GraphicsPath представляет последовательность соединенных линий и кривых

        // метод для получения пути к объекту
        public virtual GraphicsPath GetGraphicsPath() 
        {
            // пока возвращаем пустую форму
            return new GraphicsPath();
        }

        // т.к пересечение учитывает толщину линий и матрицы трансформаций,
        // то для того, чтобы определить пересечение объекта с другим объектом,
        // надо передать туда объект Graphics

        // метод для проверки пересечения объектов
        public virtual bool Overlaps(BaseObject obj, Graphics g)
        {
            // берем информацию о форме

            // получаем расположение первого объекта
            GraphicsPath path1 = this.GetGraphicsPath();
            // получаем расположение второго объекта
            GraphicsPath path2 = obj.GetGraphicsPath();

            // transform - применяем к объектам матрицы трансформации

            // получаем координаты первого объекта
            path1.Transform(this.GetTransform());
            // получаем координаты второго объекта
            path2.Transform(obj.GetTransform());

            // используем класс Region, который позволяет определить 
            // пересечение объектов в данном графическом контексте
            
            // этот класс описывает внутреннюю часть графической
            // формы, состоящей из прямоугольников и контуров

            // объявляем область пересечения
            Region region = new Region(path1);
            region.Intersect(path2); // пересекаем формы, метод для получения пересечения
                                     // последовательностей, т.е. общих для обоих наборов элементов

            // IsEmpty - true, если внутренняя часть области Region является
            // пустой при применении преобразования, связанного с параметром g

            return !region.IsEmpty(g); // если полученная форма не пуста, то значит было пересечение
        }

        // создавая поле делегат, надо создать метод,
        // с помощью которого будет вызываться это событие

        // действия при пересечении, вызов делегата
        public virtual void Overlap(BaseObject obj)
        {
            // this используется когда в метод нужно передать ссылку на текущий объект

            // если к полю есть привязанные методы, то
            if (this.OnOverlap != null)
            {
                this.OnOverlap(this, obj); // вызываем их
            }
        }
    }
}