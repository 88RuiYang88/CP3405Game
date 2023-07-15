using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CP3405Game.GUI
{
    public partial class GamePage : Form
    {
        public GamePage()
        {
            InitializeComponent();
        }

        private void GamePage_Load(object sender, EventArgs e)
        {
            PicHome.Image = Image.FromFile(@"F:\VS2019\working\CP3405Game\CP3405Game\charactor\Adam-white_hair-brown_eyes-red_shirt-white_skin-1.png");
           // PicHome.SizeMode = PictureBoxSizeMode.Zoom;
        }


    }
}
