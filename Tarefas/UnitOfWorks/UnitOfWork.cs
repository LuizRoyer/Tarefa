using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using Tarefas.IRepositories;
using Tarefas.Repositories;

namespace Tarefas.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        ITarefaRepository tarefaRepository;
        SqlConnection _sqlConn;
        SqlTransaction _sqlTrans;

       public UnitOfWork()
        {
            _sqlConn = new SqlConnection(ConnectionSql());
            _sqlConn.Open();
            _sqlTrans = _sqlConn.BeginTransaction();
        }
        
        public ITarefaRepository TarefaRepository()
        {
            if (tarefaRepository == null)
                tarefaRepository = new TarefaRepository(_sqlConn, _sqlTrans);
            return tarefaRepository;
        }

        public void Commit()
        {
            _sqlTrans.Commit();
            _sqlConn.Close();
        }
        public void Rollback()
        {
            try
            {
                _sqlTrans.Rollback();
                _sqlConn.Close();
            }
            catch
            { }
        }
        /// <summary>
        /// Metodo para Obter a Conecção no Appsettings
        /// </summary>
        /// <returns></returns>
        private string ConnectionSql()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            return configuration.GetConnectionString("DefaultConnection");
        }

    }
}
