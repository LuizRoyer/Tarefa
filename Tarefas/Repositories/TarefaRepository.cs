using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Tarefas.Entities;
using Tarefas.IRepositories;

namespace Tarefas.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        SqlConnection _conn;
        SqlTransaction _trans;
        public TarefaRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _conn = connection;
            _trans = transaction;
        }
        public void Add(Tarefa obj)
        {
            string sqlInsert = @"INSERT INTO dbo.Tarefa
                                    (TITULO , DESCRICAO, STATUS, DATACRIACAO)
                                VALUES
                                    (@titulo, @descricao, @status, CURRENT_TIMESTAMP)";

            SqlCommand cmd = new SqlCommand(sqlInsert, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@titulo", obj.Titulo));
            cmd.Parameters.Add(new SqlParameter("@descricao", obj.Descricao));
            cmd.Parameters.Add(new SqlParameter("@status", obj.Status));
         
            cmd.ExecuteNonQuery();
        }

        public Tarefa Get(int id, string titulo)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT  Id
                                      ,Titulo
                                      ,Status
                                      ,Descricao                                     
                                  FROM dbo.Tarefa
                                        WHERE 1=1");

            if (!string.IsNullOrWhiteSpace(titulo))
            {
                sqlSelect.Append(" AND TITULO= @titulo");
            }
            else
                sqlSelect.Append(" AND ID = @id");

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            if (!string.IsNullOrWhiteSpace(titulo))
                cmd.Parameters.Add(new SqlParameter("@titulo", titulo));
            else
                cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.Transaction = _trans;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Tarefa tarefa = new Tarefa();
                while (reader.Read())
                {
                    tarefa = PopularObjeto(reader);
                }
                return tarefa;
            }
        }
        public List<Tarefa> GetAll()
        {
            string sqlSelect = @"SELECT  Id
                                      ,Titulo
                                      ,Status
                                      ,Descricao                                     
                                  FROM dbo.Tarefa
                                        WHERE 1=1
                                            AND STATUS <> 3
                                   ORDER BY ID DESC";
            SqlCommand cmd = new SqlCommand(sqlSelect, _conn);
            cmd.Transaction = _trans;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                List<Tarefa> tarefas = new List<Tarefa>();
                while (reader.Read())
                {
                    tarefas.Add(PopularObjeto(reader));
                }

                return tarefas;
            }
        }
        /// <summary>
        /// Para manter o historico Não farei um Delete no banco mas sim , Alterarei o status para nao aparecer na consulta;
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            string sqlRemove = @" UPDATE dbo.Tarefa 
                                        SET STATUS = 3 
                                           ,DATAEXCLUSAO = CURRENT_TIMESTAMP       
                                    WHERE ID = @id";
            SqlCommand cmd = new SqlCommand(sqlRemove, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            cmd.ExecuteNonQuery();
        }

        public void Update(Tarefa obj)
        {
            StringBuilder sqlUpdate = new StringBuilder();
            sqlUpdate.Append(@" UPDATE dbo.Tarefa 
                                    SET TITULO =@titulo
                                        ,DESCRICAO =@descricao
                                        ,STATUS = @status");
            if (obj.Status == 2)
                sqlUpdate.Append("      ,DATAEDICAO = CURRENT_TIMESTAMP");
            if (obj.Status == 4)
                sqlUpdate.Append("      ,DATACONCLUSAo = CURRENT_TIMESTAMP");

            sqlUpdate.Append(" WHERE ID= @id ");

            SqlCommand cmd = new SqlCommand(sqlUpdate.ToString(), _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));
            cmd.Parameters.Add(new SqlParameter("@titulo", obj.Titulo));
            cmd.Parameters.Add(new SqlParameter("@descricao", obj.Descricao));
            cmd.Parameters.Add(new SqlParameter("@status", obj.Status));
           
            cmd.ExecuteNonQuery();
        }

        private Tarefa PopularObjeto(SqlDataReader reader)
        {
            return new Tarefa
            {
                Id = Convert.ToInt32(reader["ID"].ToString()),
                Titulo = reader["TITULO"].ToString(),
                Status = Convert.ToInt32(reader["STATUS"].ToString()),
                Descricao = reader["DESCRICAO"].ToString()
            };           
        }
    }
}
