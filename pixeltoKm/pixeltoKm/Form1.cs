using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pixeltoKm
{
    public partial class Form1 : Form
    {
        public Form1()
        {//1100 534
            InitializeComponent();
            Location = new Point(400, 200);
            Text = "Cal";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int unit = Convert.ToInt32(textBox1.Text);
                label1.Text = $"{unit * 0.000005704:f3} km2";


            }
            catch (FormatException) { }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int unit = Convert.ToInt32(textBox2.Text);
                int unit1 = Convert.ToInt32(textBox3.Text);
                textBox1.Text = $"{unit * unit1}";

            }
            catch (FormatException) { }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int unit = Convert.ToInt32(textBox2.Text);
                int unit1 = Convert.ToInt32(textBox3.Text);
                textBox1.Text = $"{unit * unit1}";

            }
            catch (FormatException) { }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3.Clear();

        }
    }
}
