using CP3405Game.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CP3405Game.Business
{
    public class SQSLogic
    {
        public static string SQSGetData(int ownOrGuset,string roomNumber) {
         
            string tempS = "";
            do
            {
                tempS= SQSConnect.getMessageSQS(ownOrGuset,roomNumber);
                Thread.Sleep(1000);
            } while (tempS.Length==0);
        
            return tempS;   
        }


    }
}
