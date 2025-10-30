using Microsoft.EntityFrameworkCore;
using SistemaBarbearia.Models;
using System.Collections.Generic;

namespace SistemaBarbearia.Data
{
    public class BarbeariaContext : DbContext
    {
        public BarbeariaContext(DbContextOptions<BarbeariaContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
    }
}
