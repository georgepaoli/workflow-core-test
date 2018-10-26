using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;

namespace workflow_core_test.Workflows
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowHost wfc;

        public WorkflowsController(IWorkflowHost wfc)
        {
            this.wfc = wfc;
        }

        [HttpGet("start-workflow-test")]
        public ActionResult<string> StartWorkflowTest()
        {
            return this.wfc.StartWorkflow(nameof(Workflows.WorkflowSaga)).Result;
        }
    }
}
