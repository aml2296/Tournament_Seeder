using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ConsoleApp1
{
    class Display : Form
    {
        Image mainMenuBackGround;
        mod displayMode;
        public Display()
        {
            this.Text = "Tourna Build 0.01";
            mainMenuBackGround = Image.FromFile("C:/Users/Aaron/Source/Repos/aml2296/Tournament_Seeder/ConsoleApp1/menu.jpg");

            this.Size = mainMenuBackGround.Size;
            this.CenterToScreen();
            displayMode = mod.main;
        }
        public void update()
        {
            this.ShowDialog();
        }
        public void BuildMainMenu()
        {
            Button dataB = new Button();
            Button compareB = new Button();

            this.BackgroundImage = mainMenuBackGround;
            dataB.BackColor = Color.DarkOrchid;
            compareB.BackColor = Color.LightCoral;

            dataB.Text = "DATA";
            compareB.Text = "COMPARE";

            int ratioWidth = BackgroundImage.Size.Width / 26;
            int ratioHeight = BackgroundImage.Size.Height / 14;
            dataB.SetBounds(ratioWidth * 2, ratioHeight * 2, ratioWidth * 9, ratioHeight * 4);
            compareB.SetBounds(ratioWidth * 2, ratioHeight * 8, ratioWidth * 9, ratioHeight * 4);

            this.Controls.Add(dataB);
            this.Controls.Add(compareB);
            displayMode = mod.main;
        }
        public void BuildDataMenu()
        {
            this.BackgroundImage = null;
            this.BackColor = Color.DarkOrchid;

            Button exit = new Button();
            this.AcceptButton = exit;
            this.CancelButton = exit;
        }


        public mod getMode
        {
            get
                {
                return displayMode;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Display
            // 
            this.ClientSize = new System.Drawing.Size(939, 475);
            this.Name = "display";
            this.ResumeLayout(false);

        }
    }
}

