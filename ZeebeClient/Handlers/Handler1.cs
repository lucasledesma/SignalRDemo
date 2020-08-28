using System;
using System.Threading;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Newtonsoft.Json;


namespace ZeebeClientExample.Handlers
{
    class Handler1
    {

        class WorkflowData
        {
            public string workflowid ;
            public string connectionid;
            public string stepDone;
        }
        public async void HandleJob(IJobClient jobClient, IJob job)
        {
            // business logic
            var jobKey = job.Key;
            Console.WriteLine("Handling job: " + job);

            Thread.Sleep(3000);

            var connection = new HubConnectionBuilder()
                           .WithUrl("https://localhost:5001/signalrhub", (opts) =>
                               {
                                   opts.HttpMessageHandlerFactory = (message) =>
                                   {
                                       if (message is HttpClientHandler clientHandler)
                                           // bypass SSL certificate
                                           clientHandler.ServerCertificateCustomValidationCallback +=
                                           (sender, certificate, chain, sslPolicyErrors) => { return true; };
                                       return message;
                                   };
                               })
                           .Build();

            await connection.StartAsync();

            WorkflowData variables = JsonConvert.DeserializeObject<WorkflowData>(job.Variables);

            await connection.InvokeCoreAsync("WorkflowStepDone", typeof(void), new object[] { job.WorkflowInstanceKey.ToString(), variables.connectionid, job.Type.ToString() });

            jobClient.NewCompleteJobCommand(jobKey)
                .Variables("")
                .Send()
                .GetAwaiter()
                .GetResult();
        }
    }

}