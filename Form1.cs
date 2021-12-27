using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public Form1()
        {
            InitializeComponent();
            myRect = new MyRectangle(0, 0, 0); // создаем экземпляр класса
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
            myRect.Render(g); // теперь так рисуем            
        }
    }
}
