using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ConsoleApp1
{
    class display : Form
    {
        public display()
        {
            this.CenterToScreen();
            this.Text = "YOOOOooooo";

            Button myButton = new Button();
            myButton.Location = new Point(0, 10);
            myButton.Text = "AH";
            myButton.BackColor = Color.Aqua;


            this.Controls.Add(myButton);
        }
        public void start()
        {
            this.ShowDialog();
        }

    }
}

