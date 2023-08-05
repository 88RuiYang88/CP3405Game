using CP3405Game.Business;
using CP3405Game.DATA;
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
    public partial class LoginPage : Form
    {
        string RoomId = string.Empty;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            //发送数据到SQS 等待信息
            SQSConnect.sendMessageSQS("NRN|0");

            Console.WriteLine(SQSConnect.MessageIDNow);

           string tempRoom= SQSLogic.SQSGetData(0, "");

            if (tempRoom.IndexOf("NRN|")>=0)
            {
                RoomId = tempRoom.Split('|')[2];
                GamePage gamePage = new GamePage(RoomId, 0);
                gamePage.Show();
               // this.Close();
            }
            else
            {
                MessageBox.Show("Something Wrong,Please try again!", "Error", MessageBoxButtons.OK);
            }

        }

        private void btn_join_Click(object sender, EventArgs e)
        {
            if (txt_RoomNumber.Text.Length>0)
            {
                RoomId = txt_RoomNumber.Text;
                SQSConnect.sendMessageSQS("ER|1|"+RoomId);

               string message =  SQSLogic.SQSGetData(1,"");

                if (message.Split('|')[3].ToString().ToLower()=="y")
                {
                    GamePage gamePage  = new GamePage(RoomId,1);
                    gamePage.Show();
                   // this.Close();
                }
                else if (message.Split('|')[3].ToString().ToLower() == "n")
                {
                    MessageBox.Show("The Room number is wrong!Please check again!", "Error", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Something Wrong,Please try again!", "Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Please Enter Room Number!", "Error",MessageBoxButtons.OK);
            }
        }
    }
}
