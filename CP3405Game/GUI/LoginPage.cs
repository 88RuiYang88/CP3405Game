using CP3405Game.Business;
using CP3405Game.DATA;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CP3405Game.GUI
{
    public partial class LoginPage : Form
    {
        string RoomId = string.Empty;



        /// <summary>
        //card set
        //1 = Oldversion
        //2= Baldur's gate
        //3= Pokemon
        //4= ThreeKindom
        /// </summary>
        int cardset;

        /// <summary>
        /// set the border width
        /// </summary>
        int borderWidth = 3;

        /// <summary>
        ///  set the border empty width 
        /// </summary>
         int borderWidthEmpty = 0;


        /// <summary>
        /// set the border color
        /// </summary>
        Color borderColor = Color.Gold; 


        public LoginPage()
        {
            InitializeComponent();
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            //Play Button sound
            SQSLogic.buttonSound("button", cardset);
         
            //Button view change 
            btn_New.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\nw0.png");
            btn_New.FlatStyle = FlatStyle.Flat;
            btn_New.BackColor = Color.Transparent;

            //Send Message to SQS New Room
            SQSConnect.sendMessageSQS("NRN|0|"+cardset);

            //get Message report New Room
           string tempRoom= SQSLogic.SQSGetData(0, "", "NRN");

            //check New Room Logic And Split Room ID 
            if (tempRoom.IndexOf("NRN|")>=0)
            {
                //Ok to get in 
                RoomId = tempRoom.Split('|')[2];
                //start page and post data to the page(room id , owner,card set )
                GamePageUpdate gamePage = new GamePageUpdate(RoomId, 0, cardset);
                gamePage.Show();
            }
            else
            {
                //If cannot get in the room , display error message
                MessageBox.Show("Something Wrong,Please try again!", "Error", MessageBoxButtons.OK);
            }

        }

        private void btn_join_Click(object sender, EventArgs e)
        {
            //Play Button sound
            SQSLogic.buttonSound("button", cardset);

            //Button view change 
            btn_join.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\jg0.png");
            btn_join.FlatStyle = FlatStyle.Flat;
            btn_join.BackColor = Color.Transparent;

            //Check the player enter the the room ID
            if (txt_RoomNumber.Text.Length>0)
            {
                RoomId = txt_RoomNumber.Text;

                //send message to SQS about enter room with room id
                SQSConnect.sendMessageSQS("ER|1|"+RoomId);

                //waiting for the message back
               string message =  SQSLogic.SQSGetData(1,"", "ER");

                //Check the room is ok to join in 
                if (message.Split('|')[4].ToString().ToLower()=="y")
                {
                    //open the window of game page
                    GamePageUpdate gamePage = new GamePageUpdate(RoomId, 1,Convert.ToInt32( message.Split('|')[2]));
                    gamePage.Show();
                }
                else if (message.Split('|')[4].ToString().ToLower() == "n")
                {
                    //if the message with no means the room number wrong 
                    MessageBox.Show("The Room number is wrong!Please check again!", "Error", MessageBoxButtons.OK);
                }
                else
                {  //if the back message is wrong ,will dis play wrong.
                    MessageBox.Show("Something Wrong,Please try again!", "Error", MessageBoxButtons.OK);
                }
            }
            else
            { //with no room number
                MessageBox.Show("Please Enter Room Number!", "Error",MessageBoxButtons.OK);
            }
        }

        private void pic1_Click(object sender, EventArgs e)
        {
            //set card Set
            cardset = 1;

            //play button music
            SQSLogic.buttonSound("button", cardset);

            //change the background image 
            this.FindForm().BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\"+cardset+"\\lb.png");

            //flash the pic images
            pic1.Invalidate();
            pic2.Invalidate();
            pic3.Invalidate();
            pic4.Invalidate();
        }

        private void pic2_Click(object sender, EventArgs e)
        {
            //set card Set
            cardset = 2;

            //play button music
            SQSLogic.buttonSound("button", cardset);

            //change the background image 
            this.FindForm().BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardset + "\\lb.png");

            //flash the pic images
            pic1.Invalidate();
            pic2.Invalidate();
            pic3.Invalidate();
            pic4.Invalidate();
        }

        private void pic3_Click(object sender, EventArgs e)
        {
            //set card Set
            cardset = 3;

            //play button music
            SQSLogic.buttonSound("button", cardset);

            //change the background image 
            this.FindForm().BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardset + "\\lb.png");

            //flash the pic images
            pic1.Invalidate();
            pic2.Invalidate();
            pic3.Invalidate();
            pic4.Invalidate();
        }

        private void pic4_Click(object sender, EventArgs e)
        {
            //set card Set
            cardset = 4;

            //play button music
            SQSLogic.buttonSound("button", cardset);

            //change the background image 
            this.FindForm().BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardset + "\\lb.png");

            //flash the pic images
            pic1.Invalidate();
            pic2.Invalidate();
            pic3.Invalidate();
            pic4.Invalidate();
        }


        /// <summary>
        /// make a movment to show the pic
        /// </summary>
        private void changePicClick(int PicNumber,PaintEventArgs e)
        {
            if (cardset==PicNumber)
            {
                ControlPaint.DrawBorder(e.Graphics, pic1.DisplayRectangle,
                          borderColor, borderWidth, ButtonBorderStyle.Solid,
                          borderColor, borderWidth, ButtonBorderStyle.Solid,
                          borderColor, borderWidth, ButtonBorderStyle.Solid,
                          borderColor, borderWidth, ButtonBorderStyle.Solid);
            }
            else
            {
                ControlPaint.DrawBorder(e.Graphics, pic1.DisplayRectangle,
borderColor, borderWidthEmpty, ButtonBorderStyle.Solid,
borderColor, borderWidthEmpty, ButtonBorderStyle.Solid,
borderColor, borderWidthEmpty, ButtonBorderStyle.Solid,
borderColor, borderWidthEmpty, ButtonBorderStyle.Solid);
            }
        }

        private void pic1_Paint(object sender, PaintEventArgs e)
        {         
            changePicClick(1,e);
        }
        private void pic2_Paint(object sender, PaintEventArgs e)
        {
            changePicClick(2,e);
        }
        private void pic3_Paint(object sender, PaintEventArgs e)
        {
            changePicClick(3,e);
        }
        private void pic4_Paint(object sender, PaintEventArgs e)
        {
            changePicClick(4,e) ;
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {
            //Initialization the card set 
            cardset = 1;

            //Initialization background
            this.FindForm().BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardset + "\\lb.png");

            //Initialization the button back ground
            btn_New.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\nw0.png");
            btn_join.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\jg0.png");
        }

        /// <summary>
        /// set New Room Button mouse movement.
        /// </summary>
        private void btn_New_MouseLeave(object sender, EventArgs e)
        {
            btn_New.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\nw0.png");
            btn_New.FlatStyle = FlatStyle.Flat;
            btn_New.BackColor = Color.Transparent;
        }

        /// <summary>
        /// set join Room Button mouse movement.
        /// </summary>
        private void btn_join_MouseLeave(object sender, EventArgs e)
        {
            btn_join.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\jg0.png");
            btn_join.FlatStyle = FlatStyle.Flat;
            btn_join.BackColor = Color.Transparent;
        }

        /// <summary>
        /// set New Room Button mouse movement.
        /// </summary>
        private void btn_New_MouseEnter(object sender, EventArgs e)
        {
            btn_New.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\nw1.png");
            btn_New.FlatStyle = FlatStyle.Flat;
            btn_New.BackColor = Color.Transparent;
            btn_New.BackgroundImageLayout = ImageLayout.Stretch;
        }

        /// <summary>
        /// set join Room Button mouse movement.
        /// </summary>
        private void btn_join_MouseEnter(object sender, EventArgs e)
        {
            btn_join.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\jg1.png");
            btn_join.FlatStyle = FlatStyle.Flat;
            btn_join.BackColor = Color.Transparent;
            btn_join.BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
