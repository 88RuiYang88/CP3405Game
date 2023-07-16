using CP3405Game.DATA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CP3405Game.GUI
{
    public partial class GamePage : Form
    {
        private string roomNumber;

        private int ownOrGuest;

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

        private void GamePage_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(Directory.GetCurrentDirectory());
            //PicHome.Image = Image.FromFile(Directory.GetCurrentDirectory()+ @"\charactor\1.png");
            this.Text = "Room Number : "+ roomNumber;
            lab_RoomNumb.Text = roomNumber;

            if (ownOrGuest==0)
            {
                btn_readyOrStart.Text = "Start!";
                btn_readyOrStart.Enabled = false;
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


        private void lb_sex_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lb_sex.SelectedValue.ToString()=="3"|| lb_sex.SelectedValue.ToString().Length>3)
            {
                lb_detail_1.Enabled = false;
                lb_detail_2.Enabled = false;
            }
            else
            {
                lb_detail_1.Enabled = true;
                lb_detail_2.Enabled = true;
            }
        }

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

                for (int i = 1; i < tempLine.Length-1; i++)
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            SQSConnect.sendMessageSQS("EG|"+ownOrGuest+"|"+roomNumber);

            SQSConnect.getMessageSQS();

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

    }
}
