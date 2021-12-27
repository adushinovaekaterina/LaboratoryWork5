using System;
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
        List<BaseObject> objects = new List<BaseObject>(); // список объектов
        Player player; // поле под игрока
        Marker marker; // поле под точку, в которую двигается игрок
        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0); // создаем экземпляр класса игрока в центре экрана
            // реакция на пересечение
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };
            // реакция на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0); // создаем экземпляр класса точки, в которую двигается игрок

            objects.Add(marker); // добавляем в список объектов точку, в которую двигается игрок
            objects.Add(player); // добавляем в список объектов игрока

            // добавляем в список объектов два прямоугольника
            objects.Add(new MyRectangle(50, 50, 0));
            objects.Add(new MyRectangle(100, 100, 45));
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

            // пересчитываем пересечения
            foreach (BaseObject obj in objects.ToList()) // objects.ToList() создает копию списка и позволяет
                                                         // модифицировать оригинальный objects прямо из цикла foreach
            {
                // если есть пересечение с игроком
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj); // игрок пересекся с объектом
                    obj.Overlap(player); // и объект пересекся с игроком
                }
            }
            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // если marker не нулевой
            if (marker != null)
            {
                // расчитываем вектор между игроком и маркером
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                // находим его длину
                float length = (float)(Math.Sqrt(dx * dx + dy * dy));
                dx /= length; // нормализуем координаты
                dy /= length;

                // пересчитываем координаты игрока
                player.X += dx * 2;
                player.Y += dy * 2;
            }

            // запрашиваем обновление pbMain
            // это вызовет метод pbMain_Paint по новой
            pbMain.Invalidate();
        }

        // событие клика мыши
        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // если маркер не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0); // создаем маркер по клику
                objects.Add(marker); // добавляем в список объектов
            }
            // меняем положение маркера кликом мыши
            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
