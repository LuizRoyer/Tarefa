namespace Tarefas.Entities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        /// <summary>
        ///  1= Criação , 
        ///  2 = Edição ,
        ///  3 = Remoção
        ///  4= Concluido.
        /// </summary>
        public int Status { get; set; }      
    }
}
