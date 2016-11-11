#r "Microsoft.Azure.WebJobs.ServiceBus"
using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings.Runtime;

public static void Run(string myEventHubMessage, Binder outBinder, TraceWriter log)
{
    log.Info($"C# Event Hub trigger received a message: {myEventHubMessage}");
    var queueName = "otherq";

    var attributes = new Attribute[]
    {
        new ServiceBusAttribute(queueName),
        new ServiceBusAccountAttribute("AzureWebJobsServiceBus")
    };

    var collector = outBinder.BindAsync<IAsyncCollector<string>>(attributes).Result;
    collector.AddAsync(myEventHubMessage).Wait();

    log.Info($"Function posted to queue {queueName} using a Binder");
}
