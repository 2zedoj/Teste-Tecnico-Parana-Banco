using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Domain.Entities.Propostas.InputCommands
{
    public abstract class BasePropostaInputCommand
    {
        public Guid ClientId { get; set; }
        public int Score { get; set; }
    }

    public class CreatedPropostaInputCommand : BasePropostaInputCommand;
}
