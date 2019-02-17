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
        private Image mainMenuBackGround;   //Background Base Image
        private Mod displayMode;            //Mode of the display
        private bool modUpdate = true;   
        private Button[] displayButtons;    //Current Buttons on active Form


        public Display()
        {
            this.Text = "Tourna Build 0.01";
            mainMenuBackGround = Image.FromFile("C:/Users/Aaron/Source/Repos/aml2296/Tournament_Seeder/ConsoleApp1/menu.jpg");

            this.Size = mainMenuBackGround.Size;
            this.CenterToScreen();
            displayMode = Mod.main;
        }

        public void Run()
        {
            if (modUpdate)
            {
                Trash();
                switch (displayMode)
                {
                    case Mod.main:
                        BuildMainMenu();
                        break;
                    case Mod.data:
                        BuildDataMenu();
                        break;
                    case Mod.compare:
                        BuildCompareMenu();
                        break;
                }
            }
        }
        private Mod BuildMainMenu()
        {
            Button dataB = new Button();
            Button compareB = new Button();
            displayButtons = new Button[2] { dataB, compareB };
            displayMode = Mod.main;


            this.BackgroundImage = mainMenuBackGround;
            displayButtons[0].BackColor = Color.DarkOrchid;
            displayButtons[1].BackColor = Color.LightCoral;

            displayButtons[0].Text = "DATA";
            displayButtons[1].Text = "COMPARE";

            displayButtons[0].DialogResult = DialogResult.OK;
            displayButtons[1].DialogResult = DialogResult.OK;

            int ratioWidth = BackgroundImage.Size.Width / 26;
            int ratioHeight = BackgroundImage.Size.Height / 14;
            displayButtons[0].SetBounds(ratioWidth * 2, ratioHeight * 2, ratioWidth * 9, ratioHeight * 4);
            displayButtons[1].SetBounds(ratioWidth * 2, ratioHeight * 8, ratioWidth * 9, ratioHeight * 4);

            displayButtons[0].MouseUp += new MouseEventHandler(ToData);
            displayButtons[1].MouseUp += new MouseEventHandler(ToComp);

            for (int i = 0; i < displayButtons.Length; i++)
                this.Controls.Add(displayButtons[i]);


            return RunLoop(Mod.main);
        }
        private Mod BuildDataMenu()
        {
            this.BackgroundImage = null;
            this.BackColor = Color.White;

            displayButtons = new Button[1] { new Button() };

            int ratioWidth = mainMenuBackGround.Size.Width / 26;
            int ratioHeight = mainMenuBackGround.Size.Height / 14;
            displayButtons[0].SetBounds(ratioWidth * 2, ratioHeight * 2, ratioWidth * 9, ratioHeight * 4);

            displayButtons[0].BackColor = Color.Red;

            displayButtons[0].Text = "MENU";

            displayButtons[0].DialogResult = DialogResult.OK;

            displayButtons[0].MouseUp += new MouseEventHandler(ToMain);

            for (int i = 0; i < displayButtons.Length; i++)
                this.Controls.Add(displayButtons[i]);


            return RunLoop(Mod.data);
        }
        private Mod BuildCompareMenu()
        {
            this.BackgroundImage = null;
            this.BackColor = Color.AliceBlue;

            displayButtons = new Button[1] { new Button() };

            int ratioWidth = mainMenuBackGround.Size.Width / 26;
            int ratioHeight = mainMenuBackGround.Size.Height / 14;
            displayButtons[0].SetBounds(ratioWidth * 2, ratioHeight * 2, ratioWidth * 9, ratioHeight * 4);

            displayButtons[0].BackColor = Color.DarkBlue;

            displayButtons[0].Text = "MENU";

            displayButtons[0].DialogResult = DialogResult.OK;

            displayButtons[0].MouseUp += new MouseEventHandler(ToMain);

            for (int i = 0; i < displayButtons.Length; i++)
                this.Controls.Add(displayButtons[i]);


            return RunLoop(Mod.compare);
        }
        private Mod RunLoop(Mod currentMod)
        {
            do
                this.ShowDialog();
            while (displayMode == currentMod);
            return displayMode;
        }

        public Mod GetMode
        {
            get
                {
                return displayMode;
            }
        }

        private void ToData(object sender, System.EventArgs e)
        {
            displayMode = Mod.data;
            modUpdate = true;
        }
        private void ToComp(object sender, System.EventArgs e)
        {
            displayMode = Mod.compare;
            modUpdate = true;
        }
        private void ToMain(object sender, System.EventArgs e)
        {
            displayMode = Mod.main;
            modUpdate = true;
        }
        private void Trash()
        {
            if (displayButtons != null)
            {
                int len = displayButtons.Length;
                for (int i = 0; i < len; i++)
                    displayButtons[i].Dispose();
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

