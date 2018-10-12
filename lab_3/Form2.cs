using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_3
{
    public partial class Form2 : Form
    {
        public string MyName
        {
            get
            {
                string str = textBox1.Text;
                if (str.Length < 1) str = "Anonymous";
                return str;
            }
        }

        public double Weight
        {
            get
            {
                double w;
                try
                {
                    w = Convert.ToDouble(textBox2.Text);
                    if ((w < 10.0) || (w > 50.0)) w = 21.0;
                }
                catch (Exception e)
                {
                    w = 21.0;
                }
                return w;
            }
        }

        public int Energy
        {
            get
            {
                int d;
                try
                {
                    d = Convert.ToInt16(textBox3.Text);
                    if ((d < 0) || (d > 100)) d = 50;
                }
                catch (Exception e)
                {
                    d = 100;
                }
                return d;
            }
        }

        public int X
        {
            get
            {
                int _x;
                try
                {
                    _x = Convert.ToInt16(textBox4.Text);
                }
                catch (Exception e)
                {
                    _x = 0;
                }
                return _x;
            }
        }

        public int Y
        {
            get
            {
                int _y;
                try
                {
                    _y = Convert.ToInt16(textBox5.Text);
                }
                catch (Exception e)
                {
                    _y = 0;
                }
                return _y;
            }
        }

        public bool Active
        {
            get
            {
                return checkBox1.Checked;
            }
        }

        public int TypeOfMicroObject
        {
            get
            {
                if (radioButton1.Checked == true)
                {
                    return 1;
                }
                if (radioButton2.Checked == true)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
        }

        public Form2()
        {
            InitializeComponent();
        }


    }
}
