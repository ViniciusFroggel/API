namespace SistemaBarbearia.Models
{
    public enum StatusAgendamento
    {
        Pendente,    // Criado pelo cliente, aguardando confirmação
        Confirmado,  // Cliente confirmou o código, aguardando aceite do funcionário
        Aceito,      // Funcionário aceitou
        Recusado     // Funcionário recusou
    }
}
