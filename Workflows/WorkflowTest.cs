using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace workflow_core_test.Workflows
{
    public class WorkflowTest : IWorkflow
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
                        .CompensateWith<UndoTask1>(t=>t.Input(s=>s.Name, d=> "teste"))
                    .Then<Task2>()
                        .CompensateWith<UndoTask2>()
                    .Then<Task3>()
                        .CompensateWith<UndoTask3>()
                    .Then<Task4>()
                        .CompensateWith<UndoTask4>()
                )
                .OnError(WorkflowCore.Models.WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(5))
                .Then(context => Console.WriteLine("End"));
        }

        public class Task1 : StepBody
        {
            public override ExecutionResult Run(IStepExecutionContext context)
            {
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
            public string Name{get;set;}
            public override ExecutionResult Run(IStepExecutionContext context)
            {
                Console.WriteLine("Undoing Task 1 "+Name);
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
