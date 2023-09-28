using CP3405Game.Business;
using CP3405Game.DATA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;

namespace CP3405Game.GUI
{
    public partial class GamePageUpdate : Form
    {
        /// <summary>
        /// The Room Number
        /// </summary>
        private string roomNumber;

        /// <summary>
        /// owner or player 
        /// </summary>
        private int ownOrGuest;

        /// <summary>
        /// The charactor data 
        /// </summary>
        private ArrayList data_1;

        /// <summary>
        /// the owner charactor
        /// </summary>
        private int ownTarget;

        /// <summary>
        /// the player charactor
        /// </summary>
        private int enemyTarget;

        /// <summary>
        /// trying times 
        /// </summary>
        private int tryTimes;

        /// <summary>
        /// the card set are playing 
        /// </summary>
        private int cardSetID;

        MediaPlayer backGroundMusic = new MediaPlayer();

        /// <summary>
        /// should we play the background
        /// </summary>
        private bool backMusic;

        /// <summary>
        /// 1:check ready or set ready
        /// 2:start game
        /// 3:check score
        /// </summary>
        private int condition;


        /// <summary>
        /// The play page 
        /// </summary>
        /// <param name="roomNumb">room number</param>
        /// <param name="ownOrGuest">0=owner, 1=guest</param>
        /// <param name="cardSetID">the card set playing now</param>
        public GamePageUpdate(string roomNumb, int ownOrGuest, int cardSetID)
        {
            InitializeComponent();
            this.roomNumber = roomNumb;
            this.ownOrGuest = ownOrGuest;
            this.cardSetID = cardSetID;
        }

        private void GamePageUpdate_Load(object sender, EventArgs e)
        {
            set_Pic();

            btn_sound.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\Sound.png");

            backGroundMusic.Open(new Uri(Directory.GetCurrentDirectory() + @"\Sound\"+cardSetID+"\\background.mp3", 
                UriKind.RelativeOrAbsolute));
            backGroundMusic.Play();
            backMusic = true;
            backGroundMusic.MediaEnded+= (s, a) =>
            {
                backGroundMusic.Position = TimeSpan.Zero; // 重置播放位置到开始
                backGroundMusic.Play(); // 重新播放
            };
            /*
            btnGuess.FlatAppearance.BorderSize = 0;
            btnSurrender.FlatAppearance.BorderSize = 0;
            btnExit.FlatAppearance.BorderSize = 0;
            btn_readyOrStart.FlatAppearance.BorderSize = 0;
            */
            

            data_1 = new ArrayList();
            ownTarget = 0;
            enemyTarget = 0;
            btnGuess.Enabled = false;
            this.Text = "Room Number : " + roomNumber + "                           " + (ownOrGuest == 0 ? "Owner" : "Player");

            lab_RoomNumb.Text = roomNumber + "  " + (ownOrGuest == 0 ? "Owner" : "Player");

            if (ownOrGuest == 0)
            {
               // btn_readyOrStart.Text = "Check Ready!";
                btn_readyOrStart.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\brc.png");
                condition = 1;
            }
            else
            {
                //btn_readyOrStart.Text = "Ready!";
                btn_readyOrStart.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\brr.png");
                condition = 1;
            }

            // 创建一个数据源
            List<KeyValuePair<string, int>> data_sex = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("None", 3),
                new KeyValuePair<string, int>("Male", 0),
                new KeyValuePair<string, int>("Female", 1)
            };

            readIO();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (backMusic)
            {
                SQSLogic.buttonSound("button",cardSetID);
            }
            backGroundMusic.Stop();
            this.Close();
        }

        private void btnSurrender_Click(object sender, EventArgs e)
        {
            if (backMusic)
            {
                SQSLogic.buttonSound("button", cardSetID); ;
            }
            SQSConnect.sendMessageSQS("EG|" + ownOrGuest + "|" + roomNumber);

            SQSLogic.SQSGetData(ownOrGuest, roomNumber, "EG");

            if (Convert.ToInt32(lab_urScore.Text) > Convert.ToInt32(lab_EnemyScore.Text))
            {
                MessageBox.Show("You Win!!");
            }
            else if (Convert.ToInt32(lab_urScore.Text) < Convert.ToInt32(lab_EnemyScore.Text))
            {
                MessageBox.Show("You Lose!!");
            }
            else
            {
                MessageBox.Show("No one will be the winner!");
            }
            backGroundMusic.Stop();
            this.Close();
        }

        private void btn_readyOrStart_Click(object sender, EventArgs e)
        {
            if (backMusic)
            {
                SQSLogic.buttonSound("button",cardSetID);
            }


            if (ownOrGuest == 0)
            {
                //check ready 
                if (condition == 1)
                {
                    //btn_readyOrStart.Text = "Check Ready!";
                    btn_readyOrStart.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\brr.png");
                    string tempN = SQSLogic.SQSGetData(ownOrGuest, roomNumber, "RD");
                    if (tempN.StartsWith("RD|0|"))
                    {
                        //btn_readyOrStart.Text = "Start!";
                        btn_readyOrStart.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\brs.png");
                        condition = 2;
                        return;
                    }
                }
                else if (condition == 2)
                {
                    txt_Message.Text = "";
                    SQSConnect.sendMessageSQS("SG|" + ownOrGuest + "|" + roomNumber);
                    string tempM = SQSLogic.SQSGetData(ownOrGuest, roomNumber, "SG");

                    if (ownOrGuest == 0 && tempM.StartsWith("SG|0|") && tempM.Length > 30)
                    {
                        setPic(tempM.Split('|')[3].Trim(), ownOrGuest);
                    }
                    else if (ownOrGuest == 1 && tempM.StartsWith("SG|1|") && tempM.Length > 30)
                    {
                        setPic(tempM.Split('|')[3].Trim(), ownOrGuest);
                    }
                }
                else if (condition == 3)
                {
                    //check game result
                    string tempResult = SQSLogic.SQSGetData(ownOrGuest, roomNumber, "IG");

                    if (tempResult.StartsWith("IG|" + ownOrGuest + "|" + roomNumber) || tempResult.StartsWith("IG|0|"))
                    {
                        txt_Message.Text += Environment.NewLine + "Your:" + tempResult.Split('|')[3] + "     Your Enemy:" + tempResult.Split('|')[4];
                        if (Convert.ToInt32(tempResult.Split('|')[3]) < Convert.ToInt32(tempResult.Split('|')[4]))
                        {
                            MessageBox.Show("Your Win this game!");
                            backGroundMusic.Pause();
                            lab_EnemyScore.Text = (Convert.ToInt32(lab_EnemyScore.Text) + 1).ToString();
                            if (backMusic)
                            {
                                SQSLogic.buttonSound("win", cardSetID);
                                backGroundMusic.Play();
                            }

                        }
                        else if (Convert.ToInt32(tempResult.Split('|')[3]) > Convert.ToInt32(tempResult.Split('|')[4]))
                        {
                            MessageBox.Show("Your lose this game!");
                            backGroundMusic.Pause();
                            lab_urScore.Text = (Convert.ToInt32(lab_urScore.Text) + 1).ToString();
                            if (backMusic)
                            {
                                SQSLogic.buttonSound("lose", cardSetID);
                                backGroundMusic.Play();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your guys far equate in this game!");
                        }
                        condition = 2;
                       // btn_readyOrStart.Text = "Start!";
                        btn_readyOrStart.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\brs.png");
                    }

                }
            }
            else if (ownOrGuest == 1)
            {

                if (condition == 1)
                {
                    SQSConnect.sendMessageSQS("RD|" + ownOrGuest + "|" + roomNumber);
                    //btn_readyOrStart.Text = "Start!";
                    btn_readyOrStart.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\brs.png");
                    condition = 2;
                }
                else if (condition == 2)
                {
                    txt_Message.Text = "";
                    string tempM = SQSLogic.SQSGetData(ownOrGuest, roomNumber, "SG");

                    if (ownOrGuest == 0 && tempM.StartsWith("SG|0|") && tempM.Length > 30)
                    {
                        setPic(tempM.Split('|')[3].Trim(), ownOrGuest);
                    }
                    else if (ownOrGuest == 1 && tempM.StartsWith("SG|1|") && tempM.Length > 30)
                    {
                        setPic(tempM.Split('|')[3].Trim(), ownOrGuest);
                    }
                }
                else if (condition == 3)
                {
                    //check game result
                    string tempResult = SQSLogic.SQSGetData(ownOrGuest, roomNumber, "IG");

                    if (tempResult.StartsWith("IG|" + ownOrGuest + "|" + roomNumber))
                    {
                        txt_Message.Text += Environment.NewLine + "Your Score:" + tempResult.Split('|')[4] + "     Your Enemy:" + tempResult.Split('|')[3];
                        if (Convert.ToInt32(tempResult.Split('|')[4]) < Convert.ToInt32(tempResult.Split('|')[3]))
                        {
                            MessageBox.Show("Your Win this game!");
                            if (backMusic)
                            {
                                SQSLogic.buttonSound("win", cardSetID);
                                backGroundMusic.Play();
                            }
                            lab_EnemyScore.Text = (Convert.ToInt32(lab_EnemyScore.Text) + 1).ToString();
                        }
                        else if (Convert.ToInt32(tempResult.Split('|')[4]) > Convert.ToInt32(tempResult.Split('|')[3]))
                        {
                            MessageBox.Show("Your lose this game!");
                            if (backMusic)
                            {
                                SQSLogic.buttonSound("lose", cardSetID);
                                backGroundMusic.Play();
                            }
                            lab_urScore.Text = (Convert.ToInt32(lab_urScore.Text) + 1).ToString();
                        }
                        else
                        {
                            MessageBox.Show("Your guys far equate in this game!");
                        }
                    }
                    condition = 2;
                    //btn_readyOrStart.Text = "Start!";
                    btn_readyOrStart.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\brs.png");
                }
            }

            if (Convert.ToInt32(lab_urScore.Text) + Convert.ToInt32(lab_EnemyScore.Text)== 5)
            {
                if (Convert.ToInt32(lab_urScore.Text) > Convert.ToInt32(lab_EnemyScore.Text))
                {
                    MessageBox.Show("You Lose!!");
                }
                else if (Convert.ToInt32(lab_urScore.Text) < Convert.ToInt32(lab_EnemyScore.Text))
                {
                    MessageBox.Show("You Win!!");
                }
                else
                {
                    MessageBox.Show("No one will be the winner!");
                }
                backGroundMusic.Stop();
                this.Close();
            }
        }

        /// <summary>
        /// read charactor information from cbook and write in detail
        /// </summary>
        public void readIO()
        {
            // 使用 ReadAllLines 方法读取文件的所有行
            string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\cBook\"+ cardSetID + ".txt");
            
            //set listbox
            ArrayList detail_1_t = new ArrayList();
            ArrayList detail_2_t = new ArrayList();

            foreach (string line in lines)
            {
                string[] tempLine = line.Trim().Split('|')[1].Split('-');

                for (int i = 1; i < tempLine.Length - 1; i++)
                {
                    string[] temp = tempLine[i].Split('_');

                    detail_1_t.Add(temp[0].ToString().ToUpper());
                    detail_2_t.Add(temp[1].ToString().ToUpper());
                }
            }

            ArrayList detail_1 = new ArrayList(detail_1_t.Cast<object>().ToHashSet().ToList());
            ArrayList detail_2 = new ArrayList(detail_2_t.Cast<object>().ToHashSet().ToList());

            lb_detail_1.DataSource = detail_1;

            lb_detail_2.DataSource = detail_2;

        }

        /// <summary>
        /// read data and set pic to the charactor ground
        /// </summary>
        /// <param name="message_T">message from sqs</param>
        /// <param name="ownOrGuest">0:owner 1:guest</param>
        public void setPic(string message_T, int ownOrGuest)
        {
            data_1 = new ArrayList();

            PictureBox[] ownBox = new PictureBox[]
            {
                PB_own1, PB_own2, PB_own3, PB_own4, PB_own5, PB_own6,
                PB_own7, PB_own8, PB_own9, PB_own10, PB_own11, PB_own12,
                PB_own13, PB_own14, PB_own15, PB_own16, PB_own17, PB_own18,
                PB_own19, PB_own20, PB_own21, PB_own22, PB_own23, PB_own24,
            };
            string[] message_arr = message_T.Split('#');

            string[] data_1T = message_arr[2].Split(',');

            if (ownOrGuest == 0)
            {
                ownTarget = Convert.ToInt32(message_arr[0]);
                enemyTarget = Convert.ToInt32(message_arr[1]);
                PicHome.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\"+cardSetID+"\\" + ownTarget + ".png");
            }
            else
            {
                ownTarget = Convert.ToInt32(message_arr[1]);
                enemyTarget = Convert.ToInt32(message_arr[0]);
                PicHome.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\"+cardSetID+"\\" + ownTarget + ".png");
            }

            for (int i = 0; i < data_1T.Length; i++)
            {
                ownBox[i].Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\" + cardSetID + "\\" + data_1T[i] + ".png");
                data_1.Add(data_1T[i]);
            }
            setAllClean(true);
        }

        /// <summary>
        /// check the key word player selected 
        /// find out the pic and change it to X
        /// </summary>
        /// <param name="keyWord">key word player selected</param>
        private void deletePicOwn(string keyWord)
        {
            PictureBox[] ownBox = new PictureBox[]
            {
            PB_own1, PB_own2, PB_own3, PB_own4, PB_own5, PB_own6,
            PB_own7, PB_own8, PB_own9, PB_own10, PB_own11, PB_own12,
            PB_own13, PB_own14, PB_own15, PB_own16, PB_own17, PB_own18,
            PB_own19, PB_own20, PB_own21, PB_own22, PB_own23, PB_own24,
            };

            string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\cBook\"+cardSetID+".txt");
            ArrayList tempList = new ArrayList();
            ArrayList finalList = new ArrayList();
            // 1 | Adam - white_hair - brown_eyes - red_shirt - white_skin - 1

            //check owner charactor has this key or not 
            // if yes ,then kill all without this key
            // if not ,then kill all with this key
            ArrayList ownerDetail = new ArrayList();
            foreach (string line in lines)
            {
                if (enemyTarget == Convert.ToInt32(line.Trim().Split('|')[0]))
                {
                    string[] tempLine = line.Trim().Split('|')[1].Split('-');
                    for (int i = 1; i < tempLine.Length - 1; i++)
                    {
                        ownerDetail.Add(tempLine[i].Trim());
                    }
                }
            }
            bool ownerDetailOn = false;

            foreach (string detail in ownerDetail)
            {
                if (keyWord == detail.ToLower())
                {
                    ownerDetailOn = true;
                }
            }

            //find out all charactor has the key word information
            foreach (string line in lines)
            {
                string[] tempLine = line.Trim().Split('|')[1].Split('-');

                for (int i = 1; i < tempLine.Length - 1; i++)
                {
                    if (keyWord == tempLine[i].ToLower())
                    {
                        tempList.Add(line.Trim().Split('|')[0]);
                    }
                }
            }

            //check the charactor in guess part has in the first arr
            for (int i = 0; i < data_1.Count; i++)
            {
                if (ownerDetailOn && !tempList.Contains(data_1[i]))
                {
                    finalList.Add(i);
                }
                else if (!ownerDetailOn && tempList.Contains(data_1[i]))
                {
                    finalList.Add(i);
                }
            }

            //   txt_Message.Text += Environment.NewLine + keyWord.ToUpper() + "        " + (ownerDetailOn == true ? "Yes" : "No");
            txt_Message.Text += Environment.NewLine + "Q: Does my targer character have " + keyWord.Replace('_', ' ') + " ?";
            txt_Message.Text += Environment.NewLine + "A: " + (ownerDetailOn == true ? "Yes" : "No");

            //put them in delete
            foreach (var item in finalList)
            {
                ownBox[Convert.ToInt32(item)].Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\D.png");
            }
            SQSLogic.buttonSound("switch", cardSetID);
        }

        private void btnGuess_Click(object sender, EventArgs e)
        {
            if (backMusic)
            {
                SQSLogic.buttonSound("button", cardSetID);
            }
            deletePicOwn(lb_detail_1.Text.ToLower() + "_" + lb_detail_2.Text.ToLower());
            tryTimes += 1;
        }

        private void PB_own_Click(object sender, EventArgs e)
        {
            if (condition == 2)
            {
                PictureBox clickedPic = (PictureBox)sender;

                int tryingWay = Convert.ToInt32(clickedPic.Name.Remove(0, 6));

                if (Convert.ToInt32(data_1[tryingWay - 1]) == enemyTarget)
                {
                    tryTimes += 1;
                    MessageBox.Show("You find the person!");
                    SQSConnect.sendMessageSQS("IG|" + ownOrGuest + "|" + roomNumber + "|" + tryTimes);
                    setAllClean(false);
                    condition = 3;
                  // btn_readyOrStart.Text = "check Result!";
                    btn_readyOrStart.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\"+cardSetID+"\\brc.png");
                }
                else
                {
                    tryTimes += 2;
                    MessageBox.Show("The person is wrong!");
                }
            }



        }

        /// <summary>
        /// Initialize data,and clean all pic
        /// </summary>
        /// <param name="cleanOrNot">set state playing or waiting . 0-waiting 1-playing</param>
        public void setAllClean(bool cleanOrNot)
        {
            tryTimes = 0;
            if (!cleanOrNot)
            {
                btnGuess.Enabled = false;
                btn_readyOrStart.Enabled = true;
                cleanAllPic();
               // lab_Gaming.Text = "Waiting!";
            }
            else
            {
                btnGuess.Enabled = true;
                btn_readyOrStart.Enabled = false;
              //  lab_Gaming.Text = "Playing!";
            }
        }

        /// <summary>
        /// clean all pic and let them display nothing
        /// </summary>
        private void cleanAllPic()
        {
            PictureBox[] ownBox = new PictureBox[]
            {
            PB_own1, PB_own2, PB_own3, PB_own4, PB_own5, PB_own6,
            PB_own7, PB_own8, PB_own9, PB_own10, PB_own11, PB_own12,
            PB_own13, PB_own14, PB_own15, PB_own16, PB_own17, PB_own18,
            PB_own19, PB_own20, PB_own21, PB_own22, PB_own23, PB_own24,PicHome
            };

            for (int i = 0; i < ownBox.Count(); i++)
            {
                ownBox[i].Image = null;
            }
        }

        private void lb_detail_1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // 如果一个新的项被选中
            if (e.NewValue == CheckState.Checked)
            {
                // 取消选中其他所有项
                for (int i = 0; i < lb_detail_1.Items.Count; i++)
                {
                    if (i != e.Index) // 不取消刚刚选中的这一项
                    {
                        lb_detail_1.SetItemChecked(i, false);
                    }
                }
            }
        }

        private void lb_detail_2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // 如果一个新的项被选中
            if (e.NewValue == CheckState.Checked)
            {
                // 取消选中其他所有项
                for (int i = 0; i < lb_detail_2.Items.Count; i++)
                {
                    if (i != e.Index) // 不取消刚刚选中的这一项
                    {
                        lb_detail_2.SetItemChecked(i, false);
                    }
                }
            }
        }

        private void lb_detail_1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = lb_detail_1.IndexFromPoint(e.Location);
            if (index != -1)
            {
                // 反转当前项的选中状态
                bool currentState = lb_detail_1.GetItemChecked(index);
                lb_detail_1.SetItemChecked(index, !currentState);
            }
        }

        private void lb_detail_2_MouseClick(object sender, MouseEventArgs e)
        {
            int index = lb_detail_2.IndexFromPoint(e.Location);
            if (index != -1)
            {
                // 反转当前项的选中状态
                bool currentState = lb_detail_2.GetItemChecked(index);
                lb_detail_2.SetItemChecked(index, !currentState);
            }
        }

        private void btn_sound_Click(object sender, EventArgs e)
        {
            //backGroundMusic.Pause();'
            if (backMusic)
            {
                backGroundMusic.Pause();
                backMusic=false;
                btn_sound.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\"+cardSetID+"\\noSound.png");
            }
            else
            {
                backGroundMusic.Play();
                backMusic = true;
                btn_sound.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\Sound.png");
            }
        }

        /// <summary>
        /// set different image with card set id.
        /// include windows background,
        /// pic background,etc
        /// </summary>
        public void set_Pic()
        {
            this.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\"+cardSetID+"\\background.png");
            pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\hi.png");
            pictureBox2.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\pl.png");
            pictureBox3.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\source.png");
            btnGuess.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\bg.png");
            btnSurrender.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\bs.png");
            btn_readyOrStart.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\brr.png");
            btnExit.BackgroundImage = Image.FromFile(Directory.GetCurrentDirectory() + @"\Pic\" + cardSetID + "\\be.png");

        }

    }
}
