﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Лабораторная_работа__5.Objects;

namespace Лабораторная_работа__5
{
    public partial class Form1 : Form
    {
        MyRectangle myRect; // поле под наш прямоугольник
        List<BaseObject> objects = new List<BaseObject>(); // список объектов
        public Form1()
        {
            InitializeComponent();

            // положим в список объектов два прямоугольника
            objects.Add(new MyRectangle(50, 50, 0));
            objects.Add(new MyRectangle(100, 100, 45));

            //myRect = new MyRectangle(100, 100, 45); // создаем экземпляр класса
        }
        // событие срабатывает, когда происходит отрисовка формы,
        // как правило в момент появления формы на экране

        // взаимодействие с графикой осуществляется через специальный объект типа Graphics,
        // с помощью которого можно рисовать разные квадратики, кружочки, линии

        // из события Paint доступ к этому объекту происходит через объект PaintEventArgs e
        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; // вытаскиваем объект графики из события
            g.Clear(Color.White); // заливаем фон белым цветом

            // выводим объекты, теперь если захочется добавлять еще объекты,
            // нужно прописать их в конструкторе
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }

            //g.Transform = myRect.GetTransform(); // вызываем метод класса BaseObject,
            // (от которого наследован класс MyRectangle)
            // для формирования матрицы трансформаций


            //myRect.Render(g); // теперь так рисуем (рендерим)            
        }
    }
}