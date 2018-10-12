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
    class Zoo
    {
        public List<Penguin> arr;
        public List<Aviary> arra;
        public static int textx2 = 600;
        public static int texty2 = 5;
        public static int wx;
        public static int wy;
        public static bool Terminate = false;
        public int scrx;
        public int scry;
        int scrwx;
        int scrwy;
        Bitmap bn;
        int bnsizewx;
        int bnsizewy;
        System.Windows.Forms.Timer timer1;
        delegate void TimerDef(object sender, EventArgs e);
        event TimerDef Added;

        //---------------
        private static int zoox, zooy, zoowx, zoowy, zoodx;

        public Zoo()
        {
            Zoo.zoox = 0;
            Zoo.zooy = 40;
            Zoo.zoowx = 150;
            Zoo.zoowy = 150;
            Zoo.zoodx = 10;
            Zoo.wx = 1600;
            Zoo.wy = 1600;

            scrx = 0;
            scry = 0;

            arr = new List<Penguin>();
            arr.Add(new Penguin());
            arr.Add(new Speedy());
            arr.Add(new Killer());
            arr.Add(new Speedy("Sergio"));
            arr.Add(new Speedy("Jack"));
            arr.Add(new Penguin("Bill"));
            arr.Add(new Penguin("Oscar"));

            arra = new List<Aviary>();
            arra.Add(new Aviary(this, 200, 200));
            arra.Add(new Aviary(this, 200, 700));
            arra.Add(new Aviary(this, 200, 1200));
            arra.Add(new Aviary(this, 700, 200));
            arra.Add(new Aviary(this, 700, 700));
            arra.Add(new Aviary(this, 700, 1200));
            arra.Add(new Aviary(this, 1200, 200));
            arra.Add(new Aviary(this, 1200, 700));
            arra.Add(new Aviary(this, 1200, 1200));

            bn = new Bitmap("penguin.png");
            bnsizewx = bn.Size.Width;
            bnsizewy = bn.Size.Height;

            Added += Zoo_TimeTick1;
            Added += Zoo_TimeTick2;
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(Added);
            timer1.Interval = 10000;
        }

        public void Save(StreamWriter sw)
        {
            sw.WriteLine("Zoo");
            sw.WriteLine(arr.Count);
            foreach (var VARIABLE in arr)
            {
                VARIABLE.Save(sw);
            }
            sw.WriteLine("Aviary");
            sw.WriteLine(arra.Count);
            foreach (var VARIABLE in arra)
            {
                VARIABLE.Save(sw);
            }

            lock (Penguins.form1)
            {
                sw.WriteLine("{0}, {1}, {2}, {3}", Penguins.already, zoox, zooy, zoodx);
            }
        }

        public int Load(StreamReader sr)
        {
            string s = sr.ReadLine();
            if (s != "Zoo") return -1;
            foreach (var VAR in arr.ToArray())
            {
                arr.Remove(VAR);
            }
            s = sr.ReadLine();
            int k = Convert.ToInt32(s);
            for (int i = 0; i < k; i++)
            {
                s = sr.ReadLine();
                if (s == "Penguin")
                {
                    arr.Add(new Penguin());
                    arr[arr.Count - 1].Load(sr);
                }
                if (s == "Speedy")
                {
                    arr.Add(new Speedy());
                    arr[arr.Count - 1].Load(sr);
                }
                if (s == "Killer")
                {
                    arr.Add(new Killer());
                    arr[arr.Count - 1].Load(sr);
                }
            }
            s = sr.ReadLine();
            if (s != "Aviary") return -2;
            foreach (var VARIABLE in arra)
            {
                foreach (var i in VARIABLE.pen.ToArray())
                {
                    VARIABLE.pen.Remove(i);
                }
            }
            s = sr.ReadLine();
            int kk = Convert.ToInt32(s);
            for (int i = 0; i < kk; i++)
            {
                arra.Add(new Aviary(this));
                arra[arra.Count - 1].Load(sr);
            }

            s = sr.ReadLine();
            string[] ars = s.Split(',');
            if (ars.Length != 4) return -3; 
            lock (Penguins.form1)
            {
                Penguins.already = Convert.ToBoolean(ars[0]);
                zoox = Convert.ToInt32(ars[1]);
                zooy = Convert.ToInt32(ars[2]);
                zoodx = Convert.ToInt32(ars[3]);
            }

            return 0;
        }

        public static void ZooMove()
        {
            while(!Zoo.Terminate)
            {
                {
                    if (((Zoo.zoox + Zoo.zoodx) < 0) || ((Zoo.zoox + Zoo.zoowx + Zoo.zoodx) > Zoo.wx))     
                    {
                        Zoo.zoox = -Zoo.zoodx;
                    }
                    else
                    {
                        Zoo.zoox += Zoo.zoodx;
                    }
                    Penguins.form1.form_Invalidate();
                    Thread.Sleep(100);
                }
                Penguins.form1.form_Invalidate();
                Thread.Sleep(100);
            }
        }

        public void Zoo_TimeTick1(object sender, EventArgs e)
        {
            foreach (var VAR in arra.ToArray())
            {
                if (VAR.pen.Count != 0)
                {
                    foreach (var VARIABLE in VAR.pen.ToArray())
                    {
                        VARIABLE.GetOut();
                        arr.Add(VARIABLE);
                        VAR.pen.Remove(VARIABLE);
                        VAR.empty = true;
                        //timer1.Stop();
                    }
                }
            }
        }

        public void Zoo_TimeTick2(object sender, EventArgs e)
        {
            foreach (var VAR in arra)
            {
                    foreach (var VARIABLE in VAR.pen)
                    {
                        timer1.Stop();
                    }
            }
        }

        public void Zoo_TimerTick(EventArgs e)
       {
           foreach (var VAR in arr.ToArray())
           {
               if (VAR.Inside == false && (bool)VAR.GetIn == false)
               {
                   VAR.Move((Program.rnd.Next(1, 4) - 2) * (Penguin.imagewx / 8),
                           (Program.rnd.Next(1, 4) - 2) * (Penguin.imagewy / 8));
                   foreach (var VARIABLE in arr.ToArray())
                   {
                       if ((VARIABLE / VAR) && VARIABLE.Inside==false)
                       {
                           if (VARIABLE is Killer == true && VAR.speed == 1) 
                           {
                               arr.Remove(VAR);
                               VARIABLE.Kills[0]++;
                               VARIABLE.energy -= 2;
                               VARIABLE.weight -= 0.2;
                           }

                           if (VARIABLE.speed == 2)
                           {
                               VAR.Move(-100, -100);
                               VAR.energy -= 2;
                               VAR.weight -= 0.2;
                               VARIABLE.Move(100, 100);
                               VARIABLE.energy -= 2;
                               VARIABLE.weight -= 0.2;
                           }
                       }
                   }
                   foreach (var i in arra.ToArray())
                   {
                       if (i.CheckInside(VAR))
                       {
                           i.empty = false;
                           VAR.Inside = true;
                           VAR.x = i.ax + 25;
                           VAR.y = i.ay + 25;
                           i.AddPenguin(VAR);
                           arr.Remove(VAR);
                           timer1.Enabled = true;
                           timer1.Start();
                       }
                   }
               }
           }
       }

        public void SetVScroll(int v) 
        {
            scry = v;
        }

        public void SetHScroll(int v)
        {
            scrx = v;
        }

        public void SetScreenSize(int _wx, int _wy)
        {
            scrwx = _wx;
            scrwy = _wy;
        }

        public void Draw(Graphics gc, bool windowed)
        {
            if (windowed)
            {
               
                Font g = new Font("Arial", 14);
                Brush b = new SolidBrush(Color.Maroon);
                Pen p = new Pen(Color.Maroon, 2);
                Point[] P = new Point[] 
                { new Point(780 - scrx, 40 - scry),
                new Point(825 - scrx, 40-scry),
                new Point(825 - scrx, 60-scry),
                new Point(780-scrx, 60-scry)};
                RectangleF r = new RectangleF((780 - scrx), (40 - scry), (870 - scrx), (70 - scry));
                gc.DrawPolygon(p, P);
                gc.DrawString("ZOO", g, b, r);
                gc.DrawLine(new Pen(b, 5), (780 - scrx), (40 - scry), (780 - scrx), (100 - scry));
                gc.DrawImage(bn, (zoox - scrx), (zooy - scry), zoowx, zoowy);
                foreach (Aviary el in arra)
                    el.Draw(gc, true, scrx, scry, scrwx, scrwy);
                foreach (Penguin el in arr)
                    el.Draw(gc, true, scrx, scry, scrwx, scrwy);
            }
            else
            {
                
                Font g = new Font("Arial", 14);
                Brush b = new SolidBrush(Color.Maroon);
                Pen p = new Pen(Color.Maroon, 2);
                Point[] P = new Point[] 
                { new Point(780, 40),
                new Point(825, 40),
                new Point(825, 60),
                new Point(780, 60)};
                RectangleF r = new RectangleF(780, 40, 870, 70);
                gc.DrawPolygon(p, P);
                gc.DrawString("ZOO", g, b, r);
                gc.DrawLine(new Pen(b, 5), 780, 40, 780, 100);
                gc.DrawImage(bn, zoox, zooy, zoowx, zoowy);
                foreach (Aviary el in arra)
                    el.Draw(gc, false, scrx, scry, scrwx, scrwy);
                foreach (Penguin el in arr)
                    el.Draw(gc, false, scrx, scry, scrwx, scrwy);
            }                        
        }

        public void Adjust_Viewport(double partX, double partY)
        {
            double dx = (Zoo.wx * partX) - ((double)scrwx) * 0.5;
            scrx = Convert.ToInt32(dx);
            if (scrx < 0) scrx = 0;
            if ((scrx + scrwx) > Zoo.wx) scrx = Zoo.wx - scrwx;
                  
            double dy = (Zoo.wy * partY) - ((double)scrwy) * 0.5;
            scry = Convert.ToInt32(dy);
            if (scry < 0) scry = 0;
            if ((scry + scrwy) > Zoo.wy) scry = Zoo.wy - scrwy;
        }

        public void Zoo_MouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (Penguin el in arr)
                    el.MouseClick(e.Location.X, e.Location.Y, scrx, scry, scrwx, scrwy);
            }
        }

        public void Zoo_Keydown(KeyEventArgs e)
        {
            const int dx = 10;
            const int dy = 10;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    foreach (Penguin i in arr)
                        if (i != null && i.Inside == false) i.Move(0, -dy);
                    break;
                case Keys.Left:
                    foreach (Penguin i in arr)
                        if (i != null && i.Inside == false) i.Move(-dx, 0);
                    break;
                case Keys.Right:
                    foreach (Penguin i in arr)
                        if (i != null && i.Inside == false) i.Move(dx, 0);
                    break;
                case Keys.Down:
                    foreach (Penguin i in arr)
                        if (i != null && i.Inside == false) i.Move(0, dy);
                    break;
                case Keys.C:
                    foreach (var VAR in arr.ToArray())
                    {
                        if (VAR.isActive() == true && VAR is Killer == true && VAR.Inside == false)
                        {
                            Killer k = new Killer();
                            k = (Killer)(VAR as Killer).Clone();
                            k.x += 20;
                            k.y += 20;
                            k.Inactive();
                            arr.Add(k);
                        }
                    }
                    break;
                case Keys.Delete:
                    for (int i = 0; i < arr.Count; i++)
                    {
                        if (arr[i].isActive())
                        {
                            arr.Remove(arr[i]);
                            i--;
                        }
                            
                    }
                    break;
                case Keys.Escape:
                    for (int i = 0; i < arr.Count; i++)
                    {
                        if (arr[i].isActive())
                        {
                            arr[i].Inactive();
                        }
                    }
                    break;
                case Keys.Insert:
                    {
                        Form2 NewPenguin = new Form2();
                        if (NewPenguin.ShowDialog() == DialogResult.OK)
                        {
                            if (NewPenguin.TypeOfMicroObject == 1)
                            {
                                Penguin p = new Penguin(NewPenguin.MyName, NewPenguin.Weight, NewPenguin.Energy,
                                                        NewPenguin.Active, NewPenguin.X, NewPenguin.Y);
                                arr.Add(p);
                            }
                            
                            if (NewPenguin.TypeOfMicroObject == 2)
                            {
                                Speedy p = new Speedy (NewPenguin.MyName, NewPenguin.Weight, NewPenguin.Energy,
                                                        NewPenguin.Active, NewPenguin.X, NewPenguin.Y);
                                arr.Add(p);
                            }
                             
                            if (NewPenguin.TypeOfMicroObject == 3)
                            {
                                Killer p = new Killer (NewPenguin.MyName, NewPenguin.Weight, NewPenguin.Energy,
                                                        NewPenguin.Active, NewPenguin.X, NewPenguin.Y);
                                arr.Add(p);
                            }
                        }
                    }
                    break;
            }
        }
    }
}