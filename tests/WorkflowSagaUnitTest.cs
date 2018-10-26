using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using workflow_core_test.Workflows;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Testing;
using Xunit;

public class WorkflowTestCustom : WorkflowTest<WorkflowSaga, object>
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole());
        services.AddTransient<WorkflowSaga.Task1>();
        services.AddWorkflow();
    }
}

public class WorkflowSagaUnitTest : WorkflowTestCustom
{
    public WorkflowSagaUnitTest()
    {
        Setup();
    }

    [Fact]
    public void WorkflowTenantInclusao()
    {
        // Arrange
        var payload = new { Teste = "" };

        // Act
        var workflowId = StartWorkflow(payload);
        WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

        // Assert
        GetStatus(workflowId).Should().Be(WorkflowStatus.Complete);
        UnhandledStepErrors.Count.Should().Be(0);
    }
}