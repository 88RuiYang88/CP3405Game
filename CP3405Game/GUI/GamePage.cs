using Amazon.Runtime;
using CP3405Game.Business;
using CP3405Game.DATA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CP3405Game.GUI
{
    public partial class GamePage : Form
    {
        private string roomNumber;

        private int ownOrGuest;

        private ArrayList data_1;

        private int ownTarget;

        private int enemyTarget;

        private int tryTimes;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomNumb">room number </param>
        /// <param name="ownOrGuest">0=owner, 1=guest </param>
        public GamePage(string roomNumb, int ownOrGuest)
        {
            InitializeComponent();
            this.roomNumber = roomNumb;
            this.ownOrGuest = ownOrGuest;
        }

        private  void GamePage_Load(object sender, EventArgs e)
        {
            data_1 = new ArrayList();
            ownTarget = 0;
            enemyTarget = 0;

          //  setPic("17#1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24|17#1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24");

            this.Text = "Room Number : "+ roomNumber;
            lab_RoomNumb.Text = roomNumber;

            if (ownOrGuest==0)
            {
                // btn_readyOrStart.Text = "Start!";
                // btn_readyOrStart.Enabled = false;
                btn_readyOrStart.Text = "Check Ready!";
                //try
                //{
                //    string result = await SQSGetDataAsync(0);
                //    // 处理result或其他后续操作
                //}
                //catch (Exception ex)
                //{
                //    // 处理异常
                //    Debug.WriteLine(ex.Message);
                //}

            }
            else
            {
                btn_readyOrStart.Text = "Ready!";
            }

            // 创建一个数据源
            List<KeyValuePair<string, int>> data_sex = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("None", 3),
                new KeyValuePair<string, int>("Male", 0),
                new KeyValuePair<string, int>("Female", 1)
            };

            // 绑定数据源
            lb_sex.DataSource = data_sex;

            // 指定显示的文本和背后的值
            lb_sex.DisplayMember = "Key";
            lb_sex.ValueMember = "Value";

            readIO();
        }
        public async Task<string> SQSGetDataAsync(int ownOrGuest)
        {
            string tempS = "";
            do
            {
                await Task.Delay(1000);  // Use Task.Delay instead of Thread.Sleep
                tempS = SQSConnect.getMessageSQS(ownOrGuest, roomNumber);    
        //        Console.WriteLine(tempS);
                if (tempS.StartsWith("RD|0|"))
                {
                   // UpdateUIWithStart();  // Call a separate method to update UI
                }
            } while (string.IsNullOrEmpty(tempS));

            return tempS;
        }

        private void lb_sex_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lb_sex.SelectedValue.ToString()=="3"|| lb_sex.SelectedValue.ToString().Length>3)
            {
                lb_detail_1.Enabled = true;
                lb_detail_2.Enabled = true;
            }
            else
            {
                lb_detail_1.Enabled = false;
                lb_detail_2.Enabled = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SQSConnect.sendMessageSQS("EG|"+ownOrGuest+"|"+roomNumber);

            SQSLogic.SQSGetData(ownOrGuest,roomNumber);

            if (Convert.ToInt32(lab_urScore.Text)> Convert.ToInt32(lab_EnemyScore.Text))
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

            this.Close();
        }

        private void btnSurrender_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_readyOrStart_Click(object sender, EventArgs e)
        {

            if (ownOrGuest ==0)
            {
                //check ready 
                if (btn_readyOrStart.Text== "Check Ready!")
                {
                    string tempN = SQSLogic.SQSGetData(ownOrGuest, roomNumber);
                    if (tempN.StartsWith("RD|0|"))
                    {
                        btn_readyOrStart.Text = "Start!";
                        return;
                    }
                }
                else if (btn_readyOrStart.Text == "Start!")
                {
                    SQSConnect.sendMessageSQS("SG|0|" + roomNumber);
                }
            }
            else if (ownOrGuest == 1)
            {
                SQSConnect.sendMessageSQS("RD|1|" + roomNumber);
                btn_readyOrStart.Text = "Start!";
            }

            if (btn_readyOrStart.Text == "Start!")
            {
                string tempM = SQSLogic.SQSGetData(ownOrGuest, roomNumber);

                if (ownOrGuest == 0 && tempM.StartsWith("SG|0|")&& tempM.Length>30)
                {
                    setPic(tempM.Split('|')[3], ownOrGuest);
                    tryTimes = 0;
                    lab_Gaming.Text = "Playing!";

                    //setPic("17#1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24");
                }
                else if (ownOrGuest == 1&& tempM.StartsWith("SG|1|") && tempM.Length > 30)
                {
                    tryTimes = 0;
                    //SG|0|631842?bhr147524529|10#27#30,16,19,29,21,17*11,7,31,15,1,13*32,4,23,10,6,27*3,12,28,24,5,14
                    setPic(tempM.Split('|')[3],ownOrGuest);
                    lab_Gaming.Text = "Playing!";
                }
            }



        }

//==========================================================================
        /// <summary>
        /// 读取txt然后识别detail填写
        /// </summary>
        private void readIO()
        {
            // 使用 ReadAllLines 方法读取文件的所有行
            string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\NameList.txt");

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

        private void setPic(string message_T,int ownOrGuest)
        {
            data_1 = new ArrayList();
            //31#23#10,9,25,18,4,14,28,5,20,6,15,8,24,17,29,13,26,7,23,22,19,31,16,3
            PictureBox[] ownBox = new PictureBox[]
            {
                PB_own1, PB_own2, PB_own3, PB_own4, PB_own5, PB_own6,
                PB_own7, PB_own8, PB_own9, PB_own10, PB_own11, PB_own12,
                PB_own13, PB_own14, PB_own15, PB_own16, PB_own17, PB_own18,
                PB_own19, PB_own20, PB_own21, PB_own22, PB_own23, PB_own24,
            };
            PictureBox[] enemyBox = new PictureBox[]
            {
                PB_enemy1, PB_enemy2, PB_enemy3, PB_enemy4, PB_enemy5, PB_enemy6,
                PB_enemy7, PB_enemy8, PB_enemy9, PB_enemy10, PB_enemy11, PB_enemy12,
                PB_enemy13, PB_enemy14, PB_enemy15, PB_enemy16, PB_enemy17, PB_enemy18,
                PB_enemy19, PB_enemy20, PB_enemy21, PB_enemy22, PB_enemy23, PB_enemy24,
            };
            string[] message_arr = message_T.Split('#');

            string[] data_1T = message_arr[2].Split(',');

            if (ownOrGuest==0)
            {
                ownTarget = Convert.ToInt32(message_arr[0]);
                enemyTarget = Convert.ToInt32(message_arr[1]);
                PicHome.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\" + ownTarget + ".png");
            }
            else
            {
                ownTarget = Convert.ToInt32(message_arr[1]);
                enemyTarget = Convert.ToInt32(message_arr[0]);
                PicHome.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\" + enemyTarget + ".png");
            }


            for (int i = 0; i < data_1T.Length; i++)
            {
                ownBox[i].Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\" + data_1T[i] + ".png");
               // enemyBox[i].Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\" + data_2T[i] + ".png");
               // enemyBox[i].Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\0.png");
                data_1.Add(data_1T[i]);
            }

        }

        private void deletePicEnemy(string keyWord)
        {
            PictureBox[] enemyBox = new PictureBox[]
            {
                PB_enemy1, PB_enemy2, PB_enemy3, PB_enemy4, PB_enemy5, PB_enemy6,
                PB_enemy7, PB_enemy8, PB_enemy9, PB_enemy10, PB_enemy11, PB_enemy12,
                PB_enemy13, PB_enemy14, PB_enemy15, PB_enemy16, PB_enemy17, PB_enemy18,
                PB_enemy19, PB_enemy20, PB_enemy21, PB_enemy22, PB_enemy23, PB_enemy24,
            };

            string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\NameList.txt");
            ArrayList tempList = new ArrayList();

            foreach (string line in lines)
            {
                string[] tempLine = line.Trim().Split('|')[1].Split('-');
                //8|Debby-red_hair-blue_eyes-green_shirt-brown_skin-0

                for (int i = 1; i < tempLine.Length - 1; i++)
                {
                    if (keyWord == tempLine[i])
                    {
                        tempList.Add(line.Trim().Split('|')[0]);
                    }
                }
            }

            ArrayList finalList = new ArrayList();

            foreach (var item in tempList)
            {
                for (int i = 0; i < data_1.Count; i++)
                {
                    if (Convert.ToInt32(data_1[i])==Convert.ToInt32(item))
                    {
                        finalList.Add(i);
                    }
                }
            }

            foreach (var item in finalList)
                {
                    enemyBox[Convert.ToInt32(item)].Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\D.png");
                }

        }

        private void deletePicOwn(string keyWord)
        {
            PictureBox[] ownBox = new PictureBox[]
            {
            PB_own1, PB_own2, PB_own3, PB_own4, PB_own5, PB_own6,
            PB_own7, PB_own8, PB_own9, PB_own10, PB_own11, PB_own12,
            PB_own13, PB_own14, PB_own15, PB_own16, PB_own17, PB_own18,
            PB_own19, PB_own20, PB_own21, PB_own22, PB_own23, PB_own24,
            };

            string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\NameList.txt");
            ArrayList tempList = new ArrayList();
            ArrayList finalList = new ArrayList();
            // 1 | Adam - white_hair - brown_eyes - red_shirt - white_skin - 1

            //check owner charactor has this key or not 
            // if yes ,then kill all without this key
            // if not ,then kill all with this key
            ArrayList ownerDetail = new ArrayList();
            foreach (string line in lines)
            {
                if (ownTarget ==Convert.ToInt32(line.Trim().Split('|')[0]))
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
                if (keyWord == detail)
                {
                    ownerDetailOn=true;
                }
            }

            //find out all charactor has the key word information
            foreach (string line in lines)
            {
                string[] tempLine = line.Trim().Split('|')[1].Split('-');

                for (int i = 1; i < tempLine.Length - 1; i++)
                {
                    if (keyWord == tempLine[i])
                    {
                        tempList.Add(line.Trim().Split('|')[0]);
                    }
                }
            }

            //check the charactor in guess part has in the first arr
            foreach (var item in tempList)
            {
                for (int i = 0; i < data_1.Count; i++)
                {
                    if (ownerDetailOn) {
                        if (Convert.ToInt32(data_1[i]) != Convert.ToInt32(item))
                        {
                            finalList.Add(i);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(data_1[i]) == Convert.ToInt32(item))
                        {
                            finalList.Add(i);
                        }
                    }
                }
            }





            //put them in delete
            foreach (var item in finalList)
            {
                ownBox[Convert.ToInt32(item)].Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\charactor\D.png");
            }

        }

        private void btnGuess_Click(object sender, EventArgs e)
        {
            deletePicOwn(lb_detail_1.Text.ToLower() + "_" + lb_detail_2.Text.ToLower());
            tryTimes += 1;
           // btnGuess.Enabled = false;
        }

        private void PB_own_Click(object sender, EventArgs e)
        {
            PictureBox clickedPic = (PictureBox)sender;

            int tryingWay = Convert.ToInt32(clickedPic.Name.Remove(0, 6));

            if (Convert.ToInt32(data_1[tryingWay])==enemyTarget)
            {
                tryTimes += 1;
                MessageBox.Show("You did it!");
            }


        }


    }
}
