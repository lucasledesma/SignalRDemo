using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using ZeebeClientExample.Handlers;

namespace ZeebeClientExample.Main
{
    internal class Program
    {
        private static readonly string DemoProcessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "signalrdemo.bpmn");
        private static readonly string ZeebeUrl = "localhost:26500";
        private static readonly string WorkflowInstanceVariables = "{\"a\":\"123\"}";
        private static readonly string WorkerName = Environment.MachineName;
        private static readonly long WorkCount = 1L;

        public static async Task Main(string[] args)
        {
            // create zeebe client
            var client = ZeebeClient.Builder()
                .UseLoggerFactory(new NLogLoggerFactory())
                .UseGatewayAddress(ZeebeUrl)
                .UsePlainText()
                .Build();

            // deploy
            var deployResponse = await client.NewDeployCommand()
                .AddResourceFile(DemoProcessPath)
                .Send();

            // create workflow instance
            var workflowKey = deployResponse.Workflows[0].WorkflowKey;

            for (var i = 0; i < WorkCount; i++)
            {
                await client
                    .NewCreateWorkflowInstanceCommand()
                    .WorkflowKey(workflowKey)
                    .Variables(WorkflowInstanceVariables)
                    .Send();
            }

            // open job worker
            using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
            {

                client.NewWorker()
                      .JobType("State1")
                      .Handler(Handler1.HandleJob)
                      .MaxJobsActive(4)
                      .Name(WorkerName + "1")
                      .AutoCompletion()
                      .PollInterval(TimeSpan.FromSeconds(1))
                      .Timeout(TimeSpan.FromSeconds(10))
                      .Open();

                client.NewWorker()
                      .JobType("State2")
                      .Handler(Handler2.HandleJob)
                      .MaxJobsActive(4)
                      .Name(WorkerName + "2")
                      .AutoCompletion()
                      .PollInterval(TimeSpan.FromSeconds(1))
                      .Timeout(TimeSpan.FromSeconds(10))
                      .Open();


                client.NewWorker()
                      .JobType("State3")
                      .Handler(Handler3.HandleJob)
                      .MaxJobsActive(4)
                      .Name(WorkerName + "3")
                      .AutoCompletion()
                      .PollInterval(TimeSpan.FromSeconds(1))
                      .Timeout(TimeSpan.FromSeconds(10))
                      .Open();


                // blocks main thread, so that worker can run
                signal.WaitOne();
            }
        }

    }
}