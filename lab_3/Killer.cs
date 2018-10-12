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

    class Killer : Speedy
    {
        Bitmap b;
        int bsizewx;
        int bsizewy;
        public Killer(string name = "Rico", double weight = 21.0, int e = 70,
            bool active = false, int x = 0, int y = 0, int speed = 3, bool Inside = false)
            : base(name, weight, e, active, x, y, speed, Inside)
        {
            GetIn = false;
            b = new Bitmap("killer.png");
            bsizewx = b.Size.Width;
            bsizewy = b.Size.Height;
            Kills[0] = 0;
        }

        public override void Save(StreamWriter sw)
        {
            base.Save(sw);
            sw.WriteLine(Kills[0]);
        }

        public override void Load(StreamReader sr)
        {
            base.Load(sr);
            Kills[0] = Convert.ToInt32(sr.ReadLine());
        }

        public override void Draw(Graphics gc, bool windowed, int scrx, int scry, int scrwx, int scrwy)
        {
            if (windowed)
            {
                gc.DrawImage(b, (x - scrx) + Killer.imagex, (y - scry) + Killer.imagey, Killer.imagewx, Killer.imagewy);
                Font f = new Font("Arial", 12, FontStyle.Bold);
                Point[] Pt = new Point[]
                {new Point((x-scrx) + Killer.imagex + Killer.imagewx/2-10, (y-scry) + Killer.imagey-10),
                new Point((x-scrx) + Killer.imagex + Killer.imagewx/2+10, (y-scry) + Killer.imagey-10),
                new Point((x-scrx) + Killer.imagex + Killer.imagewx/2, (y-scry) + Killer.imagey)};
                if (active)
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%, " + Kills[0] + " kill(s)", f, Brushes.Black, (x - scrx - 10) + textx1, (y - scry) + texty1);
                    Brush p = new SolidBrush(Color.Black);
                    gc.FillPolygon(p, Pt);
                }
                else
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%, " + Kills[0] + " kill(s)", f, Brushes.Green, (x - scrx - 10) + textx1, (y - scry) + texty1);
                    Brush p = new SolidBrush(Color.Green);
                    gc.FillPolygon(p, Pt);
                }
            }
            else
            {
                gc.DrawImage(b, x + Killer.imagex, y + Killer.imagey, Killer.imagewx, Killer.imagewy);
                Font f = new Font("Arial", 12, FontStyle.Bold);
                Point[] Pt = new Point[]
                {new Point(x + Killer.imagex + Killer.imagewx/2-10, y + Killer.imagey-10),
                new Point(x + Killer.imagex + Killer.imagewx/2+10, y + Killer.imagey-10),
                new Point(x + Killer.imagex + Killer.imagewx/2, y + Killer.imagey)};
                if (active)
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Black, x + textx1, y + texty1);
                    Brush p = new SolidBrush(Color.Black);
                    gc.FillPolygon(p, Pt);
                }
                else
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Green, x + textx1, y + texty1);
                    Brush p = new SolidBrush(Color.Green);
                    gc.FillPolygon(p, Pt);
                }
            }
        }
    }
}
