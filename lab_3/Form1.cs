using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace lab_3
{
    public partial class Penguins : Form
    {
        BufferedGraphicsContext bgc;
        BufferedGraphics bg;
        Bitmap buffered_bitmap;
        private static int viewport_bx;
        private static int viewport_by;
        private static int viewport_wx;
        private static int viewport_wy;  
        Zoo vin;
        private MyVScrollBar myVScrollBar;
        private MyHScrollBar myHScrollBar;
        private static int ScrollBarWidth=25;
        Timer timer = new Timer();
        public static bool already = false;
        public static Penguins form1;
        delegate void Zoo_Proc();
        static Zoo_Proc myproc = new Zoo_Proc(Zoo.ZooMove);
        IAsyncResult iar;


        public void Organize_Graphics(Graphics g)
        {
            lock (Penguins.form1)
            {
                g.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height);
                vin.Draw(g, true);

                Graphics gbmp = Graphics.FromImage(buffered_bitmap);
                gbmp.FillRectangle(Brushes.White, 0, 0, Zoo.wx, Zoo.wy);
                vin.Draw(gbmp, false);

                g.DrawImage(buffered_bitmap, Penguins.viewport_bx, Penguins.viewport_by, Penguins.viewport_wx, Penguins.viewport_wy);
                g.DrawRectangle(Pens.Black, Penguins.viewport_bx, Penguins.viewport_by, Penguins.viewport_wx, Penguins.viewport_wy);
            }            
        }

        public Penguins()
        {
            InitializeComponent();
            Penguins.form1 = this;
            vin = new Zoo();

            this.DoubleBuffered = false;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint , true);    
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
                
            bgc = BufferedGraphicsManager.Current;
            bgc.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            bg = bgc.Allocate(this.CreateGraphics(),new Rectangle(0, 0, this.Width, this.Height));
                
            Penguins.viewport_bx = this.Width - 234;
            Penguins.viewport_by = 25;
            Penguins.viewport_wx = 200;
            Penguins.viewport_wy = 200;

            buffered_bitmap = new Bitmap(Zoo.wx, Zoo.wy);

            Organize_Graphics(bg.Graphics);

            myVScrollBar = new MyVScrollBar();
            myVScrollBar.Left = this.ClientRectangle.Size.Width - Penguins.ScrollBarWidth;
            myVScrollBar.Top = 25;
            myVScrollBar.Height = this.ClientRectangle.Size.Height - Penguins.ScrollBarWidth;
            myVScrollBar.Width = Penguins.ScrollBarWidth;
            myVScrollBar.ValueChanged += this.vScrollBar1_ValueChanged;
            myVScrollBar.Anchor = AnchorStyles.Right;
            this.Controls.Add(myVScrollBar);

            myHScrollBar = new MyHScrollBar();
            myHScrollBar.Left =0;
            myHScrollBar.Top=this.ClientRectangle.Size.Height-Penguins.ScrollBarWidth;
            myHScrollBar.Height = Penguins.ScrollBarWidth;
            myHScrollBar.Width = this.ClientRectangle.Size.Width - Penguins.ScrollBarWidth;
            myHScrollBar.ValueChanged += this.hScrollBar1_ValueChanged;
            myHScrollBar.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.Controls.Add(myHScrollBar);

            myHScrollBar.Minimum = 0;
            myHScrollBar.Maximum = Zoo.wx-750;
            myVScrollBar.Minimum = 0;
            myVScrollBar.Maximum = Zoo.wy-750;

            timer.Tick += new EventHandler(timer_Tick); 
            timer.Interval = 100;                
            timer.Enabled = true;              
            timer.Start(); 
        }

        delegate void delegate_form_Invalidate();

        public void form_Invalidate()
        {
            bool InvokeRequired;
            lock (Penguins.form1)
            {
                InvokeRequired = Penguins.form1.InvokeRequired;
            }

            if (!InvokeRequired)
            {
                Organize_Graphics(bg.Graphics);
                bg.Render(Graphics.FromHwnd(this.Handle));
            }
            else
            {
                delegate_form_Invalidate d = new delegate_form_Invalidate(form_Invalidate);
                this.Invoke(d);
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            lock (Penguins.form1)
            {
                vin.Zoo_TimerTick(e);
                Organize_Graphics(bg.Graphics);
                bg.Render(Graphics.FromHwnd(this.Handle));
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            vin.SetScreenSize(this.Width, this.Height); 
            bg.Render(e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            vin.SetScreenSize(this.Width, this.Height);

            if (e.Button == MouseButtons.Left)
            {
                if ((e.Location.X > Penguins.viewport_bx) &&
                    (e.Location.X < (Penguins.viewport_bx + Penguins.viewport_wx)) &&
                    (e.Location.Y > Penguins.viewport_by) &&
                    (e.Location.Y < (Penguins.viewport_by + Penguins.viewport_wy))
                    )
                {
                    double partX = ((double)(e.Location.X - Penguins.viewport_bx)) / ((double)Penguins.viewport_wx);
                    double partY = ((double)(e.Location.Y - Penguins.viewport_by)) / ((double)Penguins.viewport_wy);
                    vin.Adjust_Viewport(partX, partY);
                }
                else
                    vin.Zoo_MouseClick(e);
            }
            Organize_Graphics(bg.Graphics);
            bg.Render(Graphics.FromHwnd(this.Handle));
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Penguins.already)
            {
                Penguins.already = true;

                iar = Penguins.myproc.BeginInvoke(null, null);
            }
            vin.SetScreenSize(this.Width, this.Height);
            vin.Zoo_Keydown(e);
            Organize_Graphics(bg.Graphics);
            bg.Render(Graphics.FromHwnd(this.Handle));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override bool IsInputKey(Keys keyData)
        {            
            if ((keyData == Keys.Up) || (keyData == Keys.Down)
                 || (keyData == Keys.Left) || keyData == Keys.Right)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            vin.SetVScroll(myVScrollBar.Value);
            vin.SetScreenSize(this.Width, this.Height);
            Organize_Graphics(bg.Graphics);
            bg.Render(Graphics.FromHwnd(this.Handle));
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            vin.SetHScroll(myHScrollBar.Value);
            vin.SetScreenSize(this.Width, this.Height);
            Organize_Graphics(bg.Graphics);
            bg.Render(Graphics.FromHwnd(this.Handle));
        }

        private void serializeTotxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
              SaveFileDialog saveFileDialog1 = new SaveFileDialog();
              saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
              saveFileDialog1.RestoreDirectory = false;
              if (saveFileDialog1.ShowDialog() == DialogResult.OK)
              {
                  string fileToWrite = saveFileDialog1.FileName;
                  StreamWriter myWriter = new StreamWriter(fileToWrite);
                  vin.Save(myWriter);
                  myWriter.Close();
              }
        } 

        private void deserializeFromTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileToRead = openFileDialog1.FileName;
                StreamReader myReader = new StreamReader(fileToRead);
                bool needstart = Penguins.already;
                vin.Load(myReader);
                myReader.Close();
            }
        }

        private void slowlyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer.Start(); 
            timer.Enabled = false;
            timer.Interval = 500;
            timer.Enabled = true;
        }

        private void fastToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            timer.Start(); 
            timer.Enabled = false;
            timer.Interval = 100;
            timer.Enabled = true;
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop(); 
        }
       
    }

    public class MyVScrollBar: VScrollBar
    {
        protected override bool IsInputKey(Keys keyData)
        {
            if ( (keyData == Keys.Up) || (keyData == Keys.Down)
                 || (keyData == Keys.Left) || keyData == Keys.Right )
            {
                return false;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }
    }

    public class MyHScrollBar : HScrollBar
    {
        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData == Keys.Up) || (keyData == Keys.Down)
                 || (keyData == Keys.Left) || keyData == Keys.Right)
            {
                return false;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }


    }
}
