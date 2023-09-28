using Amazon.SQS.Model;
using Amazon.SQS;
using System;
using System.Linq;
using Amazon;

namespace CP3405Game.DATA
{
   public  class SQSConnect
    {
        /// <summary>
        /// SQS input url
        /// </summary>
        private static string inputUrl = "https://sqs.us-west-2.amazonaws.com/494931322127/input.fifo";

        /// <summary>
        /// SQS owner output url
        /// </summary>
        private static string outputUrl = "https://sqs.us-west-2.amazonaws.com/494931322127/Output.fifo";

        /// <summary>
        /// SQS player output url
        /// </summary>
        private static string output2Url = "https://sqs.us-west-2.amazonaws.com/494931322127/output2.fifo";

        /// <summary>
        ///  Message ID for message before get room ID
        /// </summary>
        public static string MessageIDNow = "";

        /// <summary>
        /// total number of message get in one time.
        /// </summary>
        private static int messageNumb =5;

        /// <summary>
        /// Get Message from SQS without check start 
        /// </summary>
        /// <param name="ownerOrGuest">0 - owner , 1- player</param>
        /// <param name="roomNumb">room number </param>
        /// <returns>will be the message from sqs</returns>
        public static string getMessageSQS(int ownerOrGuest,string roomNumb)
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "AKIAXGPBUUUHYHVGHEHL");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "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");

            var accessKey = "AKIAXGPBUUUHYHVGHEHL";
            var secretKey = "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";
            var region = RegionEndpoint.USWest2;
            var sqsClient = new AmazonSQSClient(accessKey, secretKey, region);
            string returnInfor = "";

               var receiveRequest = new ReceiveMessageRequest
            {
                QueueUrl = ownerOrGuest == 0 ? outputUrl : output2Url,
                MaxNumberOfMessages = messageNumb // 指定接收的消息数量
               };
            var  receiveResponse = sqsClient.ReceiveMessage(receiveRequest);

            if (receiveResponse.Messages.Count > 0)
            {
                // 处理接收到的消息
                foreach (var message in receiveResponse.Messages)
                {
                    Console.WriteLine(message.Body);
                    if (message.Body.Contains('?'))
                    {
                        if (message.Body.Split('?')[1] == MessageIDNow)
                        {
                            returnInfor = message.Body.Split('?')[0];
                            //ER | 1 | 983146 | Y ? rwz1412270116
                            //RD | 0 | 107577 ? fpp472367967
                            if (returnInfor.Split('|')[1]== ownerOrGuest.ToString())
                            {
                                // 删除已处理的消息
                                var deleteRequest = new DeleteMessageRequest
                                {
                                    QueueUrl = ownerOrGuest == 0 ? outputUrl : output2Url,
                                    ReceiptHandle = message.ReceiptHandle
                                };
                               sqsClient.DeleteMessage(deleteRequest);
                            }
                        }
                        else if (message.Body.Split('?')[0].Split('|')[2] == roomNumb && message.Body.Split('?')[0].Split('|')[1] == ownerOrGuest.ToString())
                        {
                            returnInfor = message.Body.Split('?')[0];
                            // 删除已处理的消息
                            var deleteRequest = new DeleteMessageRequest
                            {
                                QueueUrl = ownerOrGuest == 0 ? outputUrl : output2Url,
                                ReceiptHandle = message.ReceiptHandle
                            };
                            sqsClient.DeleteMessage(deleteRequest);
                        }
                    }
                    else
                    {
                        if (message.Body.Split('|')[2] == roomNumb)
                        {
                            returnInfor = message.Body.Split('?')[0];
                            if (returnInfor.Split('|')[1] == ownerOrGuest.ToString())
                            {
                                // 删除已处理的消息
                                var deleteRequest = new DeleteMessageRequest
                                {
                                    QueueUrl = ownerOrGuest == 0 ? outputUrl : output2Url,
                                    ReceiptHandle = message.ReceiptHandle
                                };
                                sqsClient.DeleteMessage(deleteRequest);
                            }
                        }
                    }

                }
                
            }
            else
            {
                Console.WriteLine("No messages available in the queue.");
            }
            return returnInfor;
        }

        /// <summary>
        /// Get Message from SQS with check start 
        /// </summary>
        /// <param name="ownerOrGuest">0 - owner , 1- player</param>
        /// <param name="roomNumb">room number</param>
        /// <param name="startWith">the message start with</param>
        /// <returns>will be the message from sqs</returns>
        public static string getMessageSQSWithStart(int ownerOrGuest, string roomNumb,string startWith)
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "AKIAXGPBUUUHYHVGHEHL");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "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");

            var accessKey = "AKIAXGPBUUUHYHVGHEHL";
            var secretKey = "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";
            var region = RegionEndpoint.USWest2;
            var sqsClient = new AmazonSQSClient(accessKey, secretKey, region);
            string returnInfor = "";

            var receiveRequest = new ReceiveMessageRequest
            {
                QueueUrl = ownerOrGuest == 0 ? outputUrl : output2Url,
                MaxNumberOfMessages = messageNumb,// 指定接收的消息数量
                WaitTimeSeconds=20
            };
            var receiveResponse = sqsClient.ReceiveMessage(receiveRequest);

            if (receiveResponse.Messages.Count > 0)
            {
                // 处理接收到的消息
                foreach (var message in receiveResponse.Messages)
                {
                    Console.WriteLine(message.Body);
                    if (message.Body.Contains('?'))
                    {
                        if (message.Body.Split('?')[1] == MessageIDNow && message.Body.Split('?')[0].Split('|')[1] == ownerOrGuest.ToString() && message.Body.Trim().StartsWith(startWith))
                        {
                            returnInfor = message.Body.Split('?')[0];
                            //ER | 1 | 983146 | Y ? rwz1412270116
                            //RD | 0 | 107577 ? fpp472367967
                            if (returnInfor.Split('|')[1] == ownerOrGuest.ToString())
                            {
                                // 删除已处理的消息
                                var deleteRequest = new DeleteMessageRequest
                                {
                                    QueueUrl =ownerOrGuest==0?outputUrl: output2Url,
                                    ReceiptHandle = message.ReceiptHandle
                                };
                                sqsClient.DeleteMessage(deleteRequest);
                            }
                        }
                        else if (message.Body.Split('?')[0].Split('|')[2] == roomNumb && message.Body.Split('?')[0].Split('|')[1] == ownerOrGuest.ToString())
                        {
                            returnInfor = message.Body.Split('?')[0];
                            // 删除已处理的消息
                            var deleteRequest = new DeleteMessageRequest
                            {
                                QueueUrl = ownerOrGuest == 0 ? outputUrl : output2Url,
                                ReceiptHandle = message.ReceiptHandle
                            };
                            sqsClient.DeleteMessage(deleteRequest);
                        }
                    }
                    else
                    {
                        if (message.Body.Split('|')[2] == roomNumb && message.Body.Split('?')[0].Split('|')[1] == ownerOrGuest.ToString() && message.Body.Trim().StartsWith(startWith))
                        {
                            returnInfor = message.Body.Split('?')[0];
                            if (returnInfor.Split('|')[1] == ownerOrGuest.ToString())
                            {
                                // 删除已处理的消息
                                var deleteRequest = new DeleteMessageRequest
                                {
                                    QueueUrl = ownerOrGuest == 0 ? outputUrl : output2Url,
                                    ReceiptHandle = message.ReceiptHandle
                                };
                                sqsClient.DeleteMessage(deleteRequest);
                            }
                        }
                    }

                }
            }
            else
            {
                Console.WriteLine("No messages available in the queue.");
            }
            return returnInfor;
        }

        /// <summary>
        ///  Send message to SQS
        /// </summary>
        /// <param name="Message">message </param>
        public static async void sendMessageSQS(string Message)
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "AKIAXGPBUUUHYHVGHEHL");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "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");


            // 配置 Amazon SQS 客户端
            var sqsClient = new AmazonSQSClient(RegionEndpoint.USWest2);

             MessageIDNow = random();

            // 设置发送消息请求
            var request = new SendMessageRequest
            {
                QueueUrl = inputUrl ,
                MessageBody = Message+"?"+ MessageIDNow ,
                MessageGroupId = MessageIDNow,
                MessageDeduplicationId = MessageIDNow
            };
            // 发送消息
            var response = await sqsClient.SendMessageAsync(request);
        }

        /// <summary>
        ///  get random message id ,with 3 letter and other 6 number 
        /// </summary>
        /// <returns>random message id</returns>
        private static string random()
        {
            var random = new Random();

            Random random1 = new Random();
            char[] letters = new char[3];
            for (int i = 0; i < 3; i++)
            {
                letters[i] = (char)('a' + random1.Next(0, 26)); 
            }
            string result = new string(letters);
            return result+Math.Abs(random.Next(int.MinValue, int.MaxValue));
        }
    }
}
