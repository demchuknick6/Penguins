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
using System.Windows;



namespace lab_3
{
    class Penguin : ICloneable, MicroObjInMacro
    {
        public string name;
        public double weight;
        public int speed;
        public int[] Kills = new int[1];
        public int energy;
        public bool Inside;
        public int x, y;
        public bool active;
        public object GetIn;  //object (unboxing in class Zoo)
        public static int imagex = 0;
        public static int imagey = 0;
        public static int imagewx = 150;
        public static int imagewy = 150;
        protected static int textx1 = imagex - 10;
        protected static int texty1 = imagey - 30;
        Bitmap b;
        int bsizewx;
        int bsizewy;
        public int Energy
        {
            get
            {
                return energy;
            }
            set
            {
                if (value > 100)
                {
                    energy = 100;
                }
                if (value < 0)
                {
                    energy = 0;
                }
                if (value >= 0 && value <= 100)
                {
                    energy = value;
                }
            }
        }

        public Penguin(string name = "Skipper", double weight = 19.0, int e = 50, 
            bool active = false, int x = 0, int y = 0, int speed = 1, bool Inside = false)
        {
            this.name = name;
            this.weight = weight;
            Energy = e;
            this.x = (x == 0) ? (Program.rnd.Next() % 1400) : (x);
            this.y = (y == 0) ? (Program.rnd.Next() % 1400) : (y);
            this.active = active;
            this.speed = speed;
            this.Inside = Inside;
            GetIn = false;
            b = new Bitmap("normal.png");
            bsizewx = b.Size.Width;
            bsizewy = b.Size.Height;
        }

        public virtual void Save(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            sw.WriteLine(name);
            sw.WriteLine(weight);
            sw.WriteLine(Energy);
            sw.WriteLine(active);
            sw.WriteLine(x);
            sw.WriteLine(y);
            sw.WriteLine(speed);
            sw.WriteLine(Inside);
        }

        public virtual void Load(StreamReader sr)
        {
            this.name = sr.ReadLine();
            weight = Convert.ToDouble(sr.ReadLine());
            Energy = Convert.ToInt32(sr.ReadLine());
            active = Convert.ToBoolean(sr.ReadLine());
            x = Convert.ToInt32(sr.ReadLine());
            y = Convert.ToInt32(sr.ReadLine());
            speed = Convert.ToInt32(sr.ReadLine());
            Inside = Convert.ToBoolean(sr.ReadLine());
        }

        public bool InAviary(int rx, int ry, int rwx, int rwy)
        {
            if (x < rx - 20) return false;
            if ((x - 20 + Penguin.imagewx) > (rx + rwx)) return false;
            if (y < ry - 20) return false;
            if ((y - 20 + Penguin.imagewy) > (ry + rwy)) return false;

            return true;
        }

        public static bool operator /(Penguin obj1, Penguin obj2)
        {
            if (obj1.name != obj2.name)
            {
                Rectangle rectangle1 = new Rectangle(obj1.x+50, obj1.y+50, Penguin.imagewx-50, Penguin.imagewy-50);
                Rectangle rectangle2 = new Rectangle(obj2.x+50, obj2.y+50, Penguin.imagewx-50, Penguin.imagewy-50);
                if (rectangle1.IntersectsWith(rectangle2) && obj1.isActive() == true && obj2.isActive() == true &&
                    obj1.Inside == false && obj2.Inside == false)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isActive()
        {
            return active;
        }

        public void Inactive()
        {
            active = false;
        }

        public void Move(int dx, int dy)
        {
            if (active)
            {
                x += (speed * dx);
                y += (speed * dy);
                if (x > 5 && x + imagewx < 1600 && y > 50 && y + imagewy < 1600)
                {
                }
                else
                {
                    x -= (speed * dx);
                    y -= (speed * dy);
                }
            }
        }

        public void MouseClick(int mx, int my, int scrx, int scry, int scrwx, int scrwy)
        {
            mx += scrx;
            my += scry;
            if ((mx < x) || (mx > (x + Penguin.imagex + Penguin.imagewx)) ||
                 (my < y) || (my > (y + Penguin.imagey + Penguin.imagewy)))
            {
                return;
            }
            active = !active;
        }

        public virtual void Draw(Graphics gc, bool windowed, int scrx, int scry, int scrwx, int scrwy)
        {
            if (windowed)
            {
                gc.DrawImage(b, (x - scrx) + Penguin.imagex, (y - scry) + Penguin.imagey, Penguin.imagewx, Penguin.imagewy);
                Font f = new Font("Arial", 12, FontStyle.Bold);
                Point[] Pt = new Point[]
                {new Point((x-scrx) + Penguin.imagex + Penguin.imagewx/2-10, (y-scry) + Penguin.imagey-10),
                new Point((x-scrx) + Penguin.imagex + Penguin.imagewx/2+10, (y-scry) + Penguin.imagey-10),
                new Point((x-scrx) + Penguin.imagex + Penguin.imagewx/2, (y-scry) + Penguin.imagey)};
                if (active)
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Black, (x - scrx) + textx1, (y - scry) + texty1);
                    Brush p = new SolidBrush(Color.Black);
                    gc.FillPolygon(p, Pt);
                }
                else
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Red, (x - scrx) + textx1, (y - scry) + texty1);
                    Brush p = new SolidBrush(Color.Red);
                    gc.FillPolygon(p, Pt);
                }
            }
            else
            {
                gc.DrawImage(b, x + Penguin.imagex, y + Penguin.imagey, Penguin.imagewx, Penguin.imagewy);
                Font f = new Font("Arial", 12, FontStyle.Bold);
                Point[] Pt = new Point[]
                {new Point(x + Penguin.imagex + Penguin.imagewx/2-10, y + Penguin.imagey-10),
                new Point(x + Penguin.imagex + Penguin.imagewx/2+10, y + Penguin.imagey-10),
                new Point(x + Penguin.imagex + Penguin.imagewx/2, y + Penguin.imagey)};
                if (active)
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Black, x + textx1, y + texty1);
                    Brush p = new SolidBrush(Color.Black);
                    gc.FillPolygon(p, Pt);
                }
                else
                {
                    gc.DrawString(name + " - " + weight + " кг, " + Energy + "%", f, Brushes.Red, x + textx1, y + texty1);
                    Brush p = new SolidBrush(Color.Red);
                    gc.FillPolygon(p, Pt);
                }
            }
        }

        public void Print()
        {
            Console.Write("{0} - пiнгвiн з вагою {1:0.0}кг енергiєю {2}, показники: ", name, weight, Energy);
        }

        public object Clone()
        {
            Penguin tmp = (Penguin)this.MemberwiseClone();
            tmp.Kills = (int[])this.Kills.Clone();
            return tmp;
        }

        public static Penguin operator ++(Penguin obj)
        {
            obj.weight += 0.2;
            obj.Energy += 2;
            return obj;
        }

        public void GetOut()
        {
            x += 200;
            y += 200;
            Inside = false;
            GetIn = false;
        }
    }

    public interface MicroObjInMacro
    {
        void GetOut();
    }

}