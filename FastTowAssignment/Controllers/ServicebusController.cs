using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using FastTowAssignment.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.ServiceBus.Core;

namespace FastTowAssignment.Controllers
{
    public class ServicebusController : Controller
    {
        const string ServiceBusConnectionString = "Endpoint=sb://fasttow.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=I/Qmn9/bgDNzJQxNZ3rYd0iSj+5gNQsI0ZIPhteQuGI="; 
        const string QueueName = "fasttow";


        public async Task<ActionResult> Index()
        {
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(QueueName);
            ViewBag.MessageCount = queue.MessageCount;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ClientInquiry clientInquiry)
        {
            QueueClient queue = new QueueClient(ServiceBusConnectionString, QueueName);
            if (ModelState.IsValid)
            {
                var clientInquiryJSON = JsonConvert.SerializeObject(clientInquiry);
                var message = new Message(Encoding.UTF8.GetBytes(clientInquiryJSON))
                {
                    MessageId = Guid.NewGuid().ToString(),
                    ContentType = "application/json"
                };
                await queue.SendAsync(message);
                return RedirectToAction("Index", "servicebus", new { });
            }
            return View(clientInquiry);

        }

        private static async Task CreateQueueFunctionAsync()
        {
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            bool queueExists = await managementClient.QueueExistsAsync(QueueName);
            if (!queueExists)
            {
                QueueDescription qd = new QueueDescription(QueueName);
                qd.MaxSizeInMB = 1024;
                qd.MaxDeliveryCount = 3;
                await managementClient.CreateQueueAsync(qd);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ReceivedMessage()
        {
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(QueueName);
            List<ClientInquiry> messages = new List<ClientInquiry>();
            List<long> sequence = new List<long>();
            MessageReceiver messageReceiver = new MessageReceiver(ServiceBusConnectionString,
           QueueName);
            for (int i = 0; i < queue.MessageCount; i++)
            {
                Message message = await messageReceiver.PeekAsync();
                ClientInquiry result = JsonConvert.DeserializeObject<ClientInquiry>(Encoding.UTF8.GetString(message.Body));
                sequence.Add(message.SystemProperties.SequenceNumber);
                messages.Add(result);
            }
            ViewBag.sequence = sequence;
            ViewBag.messages = messages;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Call(long sequence)
        {
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(QueueName);
            MessageReceiver messageReceiver = new MessageReceiver(ServiceBusConnectionString,
           QueueName);
            ClientInquiry result = null;
            for (int i = 0; i < queue.MessageCount; i++)
            {
                Message message = await messageReceiver.ReceiveAsync();
                string token = message.SystemProperties.LockToken;
                if (message.SystemProperties.SequenceNumber == sequence)
                {
                    result = JsonConvert.DeserializeObject<ClientInquiry>(Encoding.UTF8.GetString(message.Body));
                    await messageReceiver.CompleteAsync(token);
                    break;
                }
            }
            return RedirectToAction("ReceivedMessage", "servicebus");
        }

        public static void Initialize()
        {
            CreateQueueFunctionAsync().GetAwaiter().GetResult();
        }
    }
}
