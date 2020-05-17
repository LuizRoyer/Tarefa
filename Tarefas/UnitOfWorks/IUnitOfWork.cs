using Tarefas.IRepositories;

namespace Tarefas.UnitOfWorks
{
    public interface IUnitOfWork
    {
        ITarefaRepository TarefaRepository();
        void Commit();
        void Rollback();
    }
}
