using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Лабораторная_работа__5.Objects;

namespace Лабораторная_работа__5
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new List<BaseObject>(); // список объектов            
        Player player; // поле под игрока
        Marker marker; // поле под точку, в которую двигается игрок
        Circle[] circles; // массив зеленых кругов 
        public Form1()
        {
            // метод, который формирует поля на форме, добавляет свойства, все то, что находится в Form1.Designer.cs
            InitializeComponent();

            circles = new Circle[2]; // создаем два зеленых круга
            for (int i = 0; i < circles.Length; i++)
            {
                circles[i] = new Circle(0, 0, 0); // создаем объект класса Circle
                circles[i].RandomPosition(pbMain.Width, pbMain.Height); // располагаем зеленый круг в случайной точке на форме
                objects.Add(circles[i]); // добавляем в список объектов зеленый круг
            }

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0); // создаем объект класса Player в центре экрана

            // реакция на пересечение с маркером или с зеленым кругом
            player.OnOverlap = (p, obj) => txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;

            //player.OnOverlap = delegate (BaseObject p, BaseObject obj)
            //{
            //    txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            //};

            // реакция на пересечение с маркером 
            player.OnMarkerOverlap = (m) =>
            {
                objects.Remove(m); // удаляем из списка объектов маркер
                marker = null; // делаем маркер "несозданным", дальше идут проверки на то, создан ли маркер
            };

            //player.OnMarkerOverlap = delegate (Marker m)
            //{
            //    objects.Remove(m); // удаляем из списка объектов маркер
            //    marker = null; // делаем маркер "несозданным", дальше идут проверки на то, создан ли маркер
            //};

            // реакция на пересечение с зеленым кругом
            player.OnCircleOverlap = (c) =>
            {
                player.tally++; // увеличиваем очки
                c.RandomPosition(pbMain.Width, pbMain.Height); // устанавливаем рандомное положение зеленого круга
                c.Timer = 100; // ставим таймер равным ста
            };

            //player.OnCircleOverlap = delegate (Circle c)
            //{
            //    player.tally++; // увеличиваем очки
            //    c.RandomPosition(pbMain.Width, pbMain.Height); // устанавливаем рандомное положение зеленого круга
            //    c.Timer = 100; // ставим таймер равным ста
            //};

            // создаем экземпляр класса точки, в которую двигается игрок
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            objects.Add(marker); // добавляем в список объектов точку, в которую двигается игрок
            objects.Add(player); // добавляем в список объектов игрока
        }
        // событие Paint срабатывает, когда происходит отрисовка формы,
        // как правило, в момент появления формы на экране

        // взаимодействие с графикой осуществляется через специальный объект типа Graphics,
        // с помощью которого можно рисовать разные квадратики, кружочки, линии

        // sender предоставляет ссылку на объект, который вызвал событие.
        // Из события Paint доступ к объекту Graphics происходит через объект PaintEventArgs e
        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; // вытаскиваем объект графики из события
            g.Clear(Color.White); // заливаем фон белым цветом

            updatePlayer(); // вызываем метод для более плавного движения игрока

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
            // перебираем и рендерим объекты
            foreach (BaseObject obj in objects)
            {
                // Transform возвращает или задает копию геометрического
                // мирового преобразования для данного объекта Graphics
                g.Transform = obj.GetTransform();

                // если объект является кругом
                if (obj is Circle) 
                {
                    Circle circle = (Circle)obj; // явно преобразуем объект в зеленый круг
                    circle.Timer--; // уменьшаем значения таймера
                    if (circle.Timer == 0) // если таймер у круга стал равен нулю, то
                    {
                        circle.RandomPosition(pbMain.Width, pbMain.Height); // устанавливаем рандомную позицию у круга
                        circle.Timer = 100; // ставим таймер равным ста
                    }
                    // числа таймера
                    g.DrawString(circle.Timer.ToString(), new Font("INK Free", 10), new SolidBrush(Color.Green), 10, 10);
                }
                obj.Render(g); // рендерим через объект
            }
            labelTally.Text = $"Очки: {player.tally}"; // выводим очки
        }

        // метод для более плавного движения игрока
        private void updatePlayer()
        {
            // если маркер создан
            if (marker != null)
            {
                // расчитываем вектор между игроком и маркером
                float dx = marker.X - player.X; // по оси X
                float dy = marker.Y - player.Y; // по оси Y
                
                float length = (float)(Math.Sqrt(dx * dx + dy * dy)); // находим расстояние от игрока до маркера            

                // нормализуем координаты
                dx /= length; // деление расстояния по x на общее расстояние
                dy /= length; // деление расстояния по y на общее расстояние 

                // используем вектор dx, dy
                // как вектор ускорения, точнее вектор притяжения,
                // который притягивает игрока к маркеру

                // векторная скорость по X увеличивается на 0.5 по расстоянию по x
                player.vX += dx * 0.5f;
                // векторная скорость по Y увеличивается на 0.5 по расстоянию по y
                player.vY += dy * 0.5f;

                // расчитываем угол поворота игрока 
                player.Angle = (float)(90 - Math.Atan2(player.vX, player.vY) * 180 / Math.PI);
            }

            // тормозящий момент, нужен чтобы, когда игрок
            // достигнет маркера произошло постепенное замедление

            // векторная скорость по X уменьшается на 0.1 от этой скорости
            player.vX += -player.vX * 0.1f;
            // векторная скорость по Y уменьшается на 0.1 от этой скорости
            player.vY += -player.vY * 0.1f;

            // пересчет позиции игрока с помощью вектора скорости

            // к координате X добавляется векторная скорость по X
            player.X += player.vX;
            // к координате Y добавляется векторная скорость по Y
            player.Y += player.vY;
        }

        // object sender – объект, от которого пришло событие

        // e передает объект, относящийся к обрабатываемому событию
        // EventArgs - класс, дающий возможность передать какую-нибудь
        // дополнительную информацию обработчику

        // метод, вызывающийся через некоторый промежуток времени и пересчитывающий
        // позицию объектов в соответствии с задуманной логикой
        private void timer1_Tick(object sender, EventArgs e)
        {
            // запрашиваем обновление pbMain
            // это вызовет метод pbMain_Paint по новой

            pbMain.Invalidate(); // когда программа хочет обновить то, что отображается, она не может
                                 // напрямую нарисовать обновленное изображение на экране,
                                 // вместо этого она сообщает Windows, что область должна быть обновлена
        }

        // object sender – объект, от которого пришло событие, в нашем случае это будет клик мыши

        // MouseEventArgs e – специфические свойства события, в клике нет особых свойств,
        // а событие клика мыши могут содержать дополнительную информацию
        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // если маркер не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0); // создаем маркер по клику
                objects.Add(marker); // добавляем в список объектов маркер
            }

            // меняем положение маркера кликом мыши

            // заносим положение мыши в переменные для хранения положения маркера
            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сделать приложение с обработкой событий по методичке  " + "\n\n" +
                             "    •  Реализовать новый объект, который будет исчезать при пересечении с игроком и появляться на новом месте" + "\n\n" +
                             "    •  Реализовать вывод очков. Увеличивать количество очков при пересечении с объектом, добавленным в предыдущем пункте. Дополнительно добавить на поле несколько зеленых кругов." + "\n\n" +
                             "    •  Добавить счетчик к зеленому объекту. Если игрок не успел добраться до объекта, то переместить его. Сам счетчик хранить в зеленом кружке, событие конца отсчета должен генерировать зеленый кружок.");
        }
    }
}