﻿using Amazon.SQS.Model;
using Amazon.SQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Amazon.SQS;
using Amazon;

namespace CP3405Game.DATA
{
   public  class SQSConnect
    {
        private static string inputUrl = "https://sqs.us-west-2.amazonaws.com/494931322127/input.fifo";
        private static string outputUrl = "https://sqs.us-west-2.amazonaws.com/494931322127/Output.fifo";

        public static string MessageIDNow = "";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string getMessageSQS()
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
                QueueUrl = outputUrl,
                MaxNumberOfMessages = 10 // 指定接收的消息数量
            };
            var receiveResponse = sqsClient.ReceiveMessage(receiveRequest);
            if (receiveResponse.Messages.Count > 0)
            {
                // 处理接收到的消息
                foreach (var message in receiveResponse.Messages)
                {
                    if (message.MessageId == MessageIDNow)
                    {
                        returnInfor = message.Body;
                        // 删除已处理的消息
                        var deleteRequest = new DeleteMessageRequest
                        {
                            QueueUrl = outputUrl,
                            ReceiptHandle = message.ReceiptHandle
                        };
                        sqsClient.DeleteMessage(deleteRequest);
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
        /// 
        /// </summary>
        /// <param name="Message"></param>
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
                MessageBody = Message,
                MessageGroupId = MessageIDNow,
                MessageDeduplicationId = MessageIDNow
            };
            // 发送消息
            var response = await sqsClient.SendMessageAsync(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
            return result+random.Next(int.MinValue, int.MaxValue);
        }
    }
}
