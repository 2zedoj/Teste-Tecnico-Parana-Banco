using ClienteService.Domain.Abstraction;
using ClienteService.Domain.Common.Events;
using ClienteService.Domain.Entities.Clients.InputCommands;
using ClienteService.Domain.Entities.Clients.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteService.Domain.Entities.Clients
{
    public sealed class Client : AggregateRoot
    {
        private Client(
            string name, 
            Document document, 
            string email, 
            double renda, 
            int score)
        {
            Name = name;
            Document = document;
            Email = email;
            Renda = renda;
            Score = score;
        }

        protected Client() { }

        public string Name { get; private set; } = null!;
        public Document Document { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public double Renda { get; private set; }
        public int Score { get; private set; }

        public static Client Create(CreatedClientInputCommand input)
        {
            var client = new Client(
                input.Name,
                input.Document,
                input.Email,
                input.Renda,
                input.Score);

            return client;
        }

        public void Update(UpdateClientInputCommand input)
        {
            Name = input.Name;
            Email = input.Email;
        }

        public void MarkAsCreated()
            => AddDomainEvent(new CreateclientEventhandler(this));
    }
}
