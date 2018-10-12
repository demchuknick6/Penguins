using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace lab_3
{

    class Speedy : Penguin
    {
        Bitmap b;
        int bsizewx;
        int bsizewy;
        public Speedy(string name = "Private", double weight = 20.0, int e = 60,
            bool active = false, int x = 0, int y = 0, int speed = 2, bool Inside = false)
            : base(name, weight, e, active, x, y, speed, Inside)
        {
            GetIn = false;
            b = new Bitmap("speedy.png");
            bsizewx = b.Size.Width;
            bsizewy = b.Size.Height;
        }

        public override void Save(StreamWriter sw)
        {
            base.Save(sw);
        }

        public override void Load(StreamReader sr)
        {
            base.Load(sr);
        }

        public override void Draw(Graphics gc, bool windowed, int scrx, int scry, int scrwx, int scrwy)
        {
            if (windowed)
            {
                gc.DrawImage(b, (x - scrx) + Speedy.imagex, (y - scry) + Speedy.imagey, Speedy.imagewx, Speedy.imagewy);
                Font f = new Font("Arial", 12, FontStyle.Bold);
                Point[] Pt = new Point[]
                {new Point((x-scrx) + Speedy.imagex + Speedy.imagewx/2-10, (y-scry) + Speedy.imagey-10),
                new Point((x-scrx) + Speedy.imagex + Speedy.imagewx/2+10, (y-scry) + Speedy.imagey-10),
                new Point((x-scrx) + Speedy.imagex + Speedy.imagewx/2, (y-scry) + Speedy.imagey)};
                if (active)
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Black, (x - scrx) + textx1, (y - scry) + texty1);
                    Brush p = new SolidBrush(Color.Black);
                    gc.FillPolygon(p, Pt);
                }
                else
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Blue, (x - scrx) + textx1, (y - scry) + texty1);
                    Brush p = new SolidBrush(Color.Blue);
                    gc.FillPolygon(p, Pt);
                }
            }
            else
            {
                gc.DrawImage(b, x + Speedy.imagex, y + Speedy.imagey, Speedy.imagewx, Speedy.imagewy);
                Font f = new Font("Arial", 12, FontStyle.Bold);
                Point[] Pt = new Point[]
                {new Point(x + Speedy.imagex + Speedy.imagewx/2-10, y + Speedy.imagey-10),
                new Point(x + Speedy.imagex + Speedy.imagewx/2+10, y + Speedy.imagey-10),
                new Point(x + Speedy.imagex + Speedy.imagewx/2, y + Speedy.imagey)};
                if (active)
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Black, x + textx1, y + texty1);
                    Brush p = new SolidBrush(Color.Black);
                    gc.FillPolygon(p, Pt);
                }
                else
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Blue, x + textx1, y + texty1);
                    Brush p = new SolidBrush(Color.Blue);
                    gc.FillPolygon(p, Pt);
                }
            }   
        }
    }
}