namespace SistemaBarbearia.Models;

public class Agendamento
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int ServicoId { get; set; }
    public DateTime DataHora { get; set; }

    public Cliente? Cliente { get; set; }
    public Servico? Servico { get; set; }

    public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
    public bool ConfirmadoCliente { get; set; } = false;
}
