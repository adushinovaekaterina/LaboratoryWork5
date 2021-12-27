﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лабораторная_работа__5.Objects
{
    class Marker : BaseObject
    {
        public Marker(float x, float y, float angle) : base(x, y, angle)
        {

        }
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Red), -3, -3, 6, 6);
            g.DrawEllipse(new Pen(Color.Red, 2), -6, -6, 12, 12);
            g.DrawEllipse(new Pen(Color.Red, 2), -10, -10, 20, 20);

        }

        // метод для описания формы для маркера
        public override GraphicsPath GetGraphicsPath()
        {
            GraphicsPath path = base.GetGraphicsPath();
            path.AddEllipse(-3, -3, 6, 6); // используем самый маленький круг в центре маркера
            return path;
        }
    }
}
