using System.Collections.Generic;
using Tarefas.Entities;

namespace Tarefas.IRepositories
{
    public interface ITarefaRepository
    {
        void Add(Tarefa obj);
        void Remove(int id);
        void Update(Tarefa obj);
        List<Tarefa> GetAll();
        Tarefa Get(int id, string titulo);      
    }
}
