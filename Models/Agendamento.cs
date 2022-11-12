namespace ProjetoEscola_API.Models
{
    // my DBO
    public class Agendamento
    {
        public int id {get; set;}

        public string? email {get; set;}

        public string? data {get; set;}

        public string? horario {get; set;}

        public string? pet {get; set;}
        public string? servico {get; set;}

        public string? especie {get; set;}

        public Boolean? aprovado {get; set;}
        
    }
}