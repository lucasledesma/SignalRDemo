using System;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace ZeebeClientExample.Handlers
{
    class Handler3
    {
        public static void HandleJob(IJobClient jobClient, IJob job)
        {
            // business logic
            var jobKey = job.Key;
            Console.WriteLine("Handling job: " + job);

            jobClient.NewCompleteJobCommand(jobKey)
                .Variables("{\"foo\":2}")
                .Send()
                .GetAwaiter()
                .GetResult();
        }
    }

}