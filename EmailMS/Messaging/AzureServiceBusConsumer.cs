
using Azure.Messaging.ServiceBus;
using EmailMS.Dtos;
using EmailMS.Services;
using MailKit;
using Newtonsoft.Json;
using System.Text;

namespace EmailMS.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {

        private readonly string _connstring;

        private readonly string _queuname;
        private readonly string _stackname;

        private readonly IConfiguration _configuration;
        private readonly ServiceBusProcessor _emailprocessor;
        private readonly ServiceBusProcessor _paymentemailprocessor;
        private readonly EmailService _emailservice;


        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

            _connstring = _configuration.GetValue<string>("Azureconnectionstring");
            _queuname = _configuration.GetValue<string>("QueueandTopics:registerqueue");
            _stackname = _configuration.GetValue<string>("paymentverification:paymentmade");


            var client = new ServiceBusClient(_connstring);

            _emailprocessor = client.CreateProcessor(_queuname);
            _paymentemailprocessor = client.CreateProcessor(_stackname, "EmailService");
            _emailservice = new EmailService(configuration);



        }
        public async Task Start()
        {
            
            _paymentemailprocessor.ProcessMessageAsync += OnPaymentVerification;
            _paymentemailprocessor.ProcessErrorAsync += ErrorHandler;
            await _paymentemailprocessor.StartProcessingAsync();
            _emailprocessor.ProcessMessageAsync += OnRegisterUser;
            _emailprocessor.ProcessErrorAsync += ErrorHandler;
            await _emailprocessor.StartProcessingAsync();
        }

        private async Task OnPaymentVerification(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            var user = JsonConvert.DeserializeObject<UserMessageDTO>(body);

            try
            {
                // Sending the Mail
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<h1> Hello " + user.Name + "</h1>");
                stringBuilder.AppendLine("<br/> Your Order has been Recieved");

                stringBuilder.Append("<br/>");
                stringBuilder.Append('\n');

                await _emailservice.SendMail(user, stringBuilder.ToString());

                // When Done
                await args.CompleteMessageAsync(args.Message); // we are done delete the message from the Stack

            }
            catch (Exception ex)
            {

                // send mail to admin
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }

        private async Task OnRegisterUser(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            var user = JsonConvert.DeserializeObject<UserMessageDTO>(body);

            try
            {
                // Sending the Mail

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<h1> Hello " + user.Name + "</h1>");
                stringBuilder.AppendLine("<br/> Welcome to Jumia ");

                stringBuilder.Append("<br/>");
                stringBuilder.Append('\n');
                stringBuilder.Append("<p>You can start your first adventure!!</p>");

                await _emailservice.SendMail(user,stringBuilder.ToString());

                // When Done
                await args.CompleteMessageAsync(args.Message); // we are done delete the message from the Queue

            }
            catch (Exception ex)
            {

                // send mail to admin
                Console.WriteLine(ex.Message);
                throw;


            }
        }

        public async  Task Stop()
        {
            await _emailprocessor.StopProcessingAsync();
            await _paymentemailprocessor.StopProcessingAsync();
            await _paymentemailprocessor.StartProcessingAsync();
            await _emailprocessor.DisposeAsync();
        }
    }
}
