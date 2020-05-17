using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Tarefas.Entities;
using Tarefas.UnitOfWorks;

namespace Tarefas.Services
{
    public class TarefaService
    {
        public IActionResult Save(Tarefa tarefa,
            [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                ValidarParametros(tarefa);
                Tarefa exis = unitOfWork.TarefaRepository().Get(tarefa.Id, string.Empty);
                if (exis.Id > 0)
                {
                    if (ValidarAlteracao(exis, tarefa))
                        unitOfWork.TarefaRepository().Update(tarefa);
                }
                else
                {
                    unitOfWork.TarefaRepository().Add(tarefa);
                }

                unitOfWork.Commit();

                return new OkObjectResult("Salvo com Sucesso");
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
        }

        public IActionResult Remove(int id,
            [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                if (id < 0)
                    return new BadRequestObjectResult("Identificador de Tarefa Inválido");

                if (unitOfWork.TarefaRepository().Get(id, string.Empty).Id < 1)
                    return new BadRequestObjectResult("Identificador de Tarefa não Encontrado");

                unitOfWork.TarefaRepository().Remove(id);
                unitOfWork.Commit();
                return new OkObjectResult("Tarefa Excluida com Sucesso");
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
        }
        public List<Tarefa> SelecionarTarefas([FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.TarefaRepository().GetAll();

            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }

        private void ValidarParametros(Tarefa tarefa)
        {
            if (string.IsNullOrWhiteSpace(tarefa.Titulo))
                throw new ArgumentException("Campo Titulo é Obrigatorio");

            if (tarefa.Status < 1)
                throw new ArgumentException("Campo Status Inválido");
        }

        private bool ValidarAlteracao(Tarefa exis, Tarefa tarefa)
        {
            if (exis.Titulo.ToUpper() != tarefa.Titulo.ToUpper())
                return true;
            if (exis.Descricao.ToUpper() != tarefa.Descricao.ToUpper())
                return true;
            if (exis.Status != tarefa.Status)
                return true;

            return false;
        }
    }
}
