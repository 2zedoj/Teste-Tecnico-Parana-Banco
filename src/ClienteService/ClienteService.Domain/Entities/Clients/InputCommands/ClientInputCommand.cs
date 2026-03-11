using ClienteService.Domain.Entities.Clients.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Domain.Entities.Clients.InputCommands
{
    public abstract class BaseClientInputCommand
    {
        public string Name { get; set; } = null!;
        public Document Document { get; set; } = null!;
        public string Email { get; set; } = null!;
        public double Renda { get; set; }
        public int Score { get; set; }
    }

    public class CreatedClientInputCommand : BaseClientInputCommand;
    public class UpdateClientInputCommand : BaseClientInputCommand
    {
        public string Id { get; set; } = null!;
    }
}
