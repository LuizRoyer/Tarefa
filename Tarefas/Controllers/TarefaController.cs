using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tarefas.Entities;
using Tarefas.Services;
using Tarefas.UnitOfWorks;

namespace Tarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {      
        [HttpPost("[action]")]
        public IActionResult SaveTarefa([FromBody] Tarefa tarefa,
           [FromServices] IUnitOfWork unitOfWork)
        {
            return new TarefaService().Save(tarefa, unitOfWork);
        }        
        [HttpGet("[action]")]
        public List<Tarefa> SelectTarefa(
         [FromServices] IUnitOfWork unitOfWork)
        {
            return new TarefaService().SelecionarTarefas(unitOfWork);
        }     
        [HttpDelete("[action]/{id}")]
        public IActionResult RemoveTarefa(int id,
        [FromServices] IUnitOfWork unitOfWork)
        {
            return new TarefaService().Remove(id, unitOfWork);
        }
    }
}