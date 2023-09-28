using CP3405Game.DATA;
using System.IO;
using System.Media;
using System.Threading;

namespace CP3405Game.Business
{
    public class SQSLogic
    {
        /// <summary>
        /// Get SQS Data Without check start word
        /// </summary>
        /// <param name="ownOrGuset">0 - owner , 1- player</param>
        /// <param name="roomNumber"> room number</param>
        /// <returns>The message from SQS</returns>
        public static string SQSGetData(int ownOrGuset,string roomNumber) {
         
            string tempS = "";
            do
            {
                tempS= SQSConnect.getMessageSQS(ownOrGuset,roomNumber);
                Thread.Sleep(1000);
            } while (tempS.Length==0);
        
            return tempS;   
        }


        /// <summary>
        ///   Get SQS Data With check start word
        /// </summary>
        /// <param name="ownOrGuset">0 - owner , 1- player</param>
        /// <param name="roomNumber">room number</param>
        /// <param name="startWith">The key word with message start word</param>
        /// <returns>The message from SQS</returns>
        public static string SQSGetData(int ownOrGuset, string roomNumber,string startWith)
        {
            string tempS = "";
            do
            {
                tempS = SQSConnect.getMessageSQSWithStart(ownOrGuset, roomNumber, startWith);
                if (tempS.Length == 0)
                {
                    Thread.Sleep(3000);
                }
            } while (tempS.Length == 0);

            return tempS;
        }

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="soundName">sound name</param>
        /// <param name="cardSet">in which card set</param>
        public static void buttonSound(string soundName,int cardSet)
        {
            SoundPlayer player = new SoundPlayer(Directory.GetCurrentDirectory() + @"\Sound\"+ cardSet + "\\"+soundName + ".wav");
            player.Play(); // 异步播放音乐
        }
    }
}
