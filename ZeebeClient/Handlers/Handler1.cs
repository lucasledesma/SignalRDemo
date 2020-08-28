using System;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace ZeebeClientExample.Handlers
{
    class Handler1
    {
        public static void HandleJob(IJobClient jobClient, IJob job)
        {
            // business logic
            var jobKey = job.Key;
            Console.WriteLine("Handling job: " + job);

            //            if (jobKey % 3 == 0)
            //            {
            jobClient.NewCompleteJobCommand(jobKey)
                .Variables("{\"foo\":2}")
                .Send()
                .GetAwaiter()
                .GetResult();
            //}
            // else if (jobKey % 2 == 0)
            // {
            //     jobClient.NewFailCommand(jobKey)
            //         .Retries(job.Retries - 1)
            //         .ErrorMessage("Example fail")
            //         .Send()
            //         .GetAwaiter()
            //         .GetResult();
            // }
            // else
            // {
            //     // auto completion
            // }
        }
    }

}