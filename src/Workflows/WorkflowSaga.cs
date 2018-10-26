using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace workflow_core_test.Workflows
{
    public class WorkflowSaga : IWorkflow
    {
        public string Id => this.GetType().Name;
        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith(context =>
                {
                    Console.WriteLine("Starting workflow...");
                    return ExecutionResult.Next();
                })
                .Saga(saga => saga
                    .StartWith<Task1>()
                        .CompensateWith<UndoTask1>(t => t.Input(s => s.Id, d => this.Id))
                    .Then<Task2>()
                        .CompensateWith<UndoTask2>()
                    .Then<Task3>()
                        .CompensateWith<UndoTask3>()
                    .Then<Task4>()
                        .CompensateWith<UndoTask4>()
                )
                //.OnError(WorkflowCore.Models.WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(5))
                .Then(context => Console.WriteLine("End"));
        }

        public class Task1 : StepBody
        {
            private readonly ILogger<Task1> logger;

            public Task1(ILogger<Task1> logger)
            {
                Console.WriteLine("construtor com di");
                this.logger = logger;
            }

            public Task1(){
                Console.WriteLine("construtor sem di");
            }

            public override ExecutionResult Run(IStepExecutionContext context)
            {
                this.logger.LogInformation("Log test");

                Console.WriteLine("Doing Task 1");
                return ExecutionResult.Next();
            }
        }

        public class Task2 : StepBody
        {
            public override ExecutionResult Run(IStepExecutionContext context)
            {
                Console.WriteLine("Doing Task 2");
                return ExecutionResult.Next();
            }
        }

        public class Task3 : StepBody
        {
            public override ExecutionResult Run(IStepExecutionContext context)
            {
                Console.WriteLine("Doing Task 3");
                return ExecutionResult.Next();
            }
        }

        public class Task4 : StepBody
        {
            public override ExecutionResult Run(IStepExecutionContext context)
            {
                Console.WriteLine("Throw exception in Task 4");

                throw new Exception("Test compensation saga..");
            }
        }

        public class UndoTask1 : StepBody
        {
            public string Id { get; set; }

            public override ExecutionResult Run(IStepExecutionContext context)
            {
                Console.WriteLine("Undoing Task 1. Id=" + Id);
                return ExecutionResult.Next();
            }
        }

        public class UndoTask2 : StepBody
        {
            public override ExecutionResult Run(IStepExecutionContext context)
            {
                Console.WriteLine("Undoing Task 2");
                return ExecutionResult.Next();
            }
        }

        public class UndoTask3 : StepBody
        {
            public override ExecutionResult Run(IStepExecutionContext context)
            {
                Console.WriteLine("Undoing Task 3");
                return ExecutionResult.Next();
            }
        }

        public class UndoTask4 : StepBody
        {
            public override ExecutionResult Run(IStepExecutionContext context)
            {
                Console.WriteLine("Undoing Task 4");
                return ExecutionResult.Next();
            }
        }
    }
}
