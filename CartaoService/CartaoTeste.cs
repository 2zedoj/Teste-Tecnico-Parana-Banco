using ClienteService.Domain.Entities.Clients;
using ClienteService.Domain.Entities.Clients.InputCommands;
using ClienteService.Domain.Entities.Clients.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaoService
{
    public class ClienteTests
    {
        [Fact]
        public void CreateClient_ShouldCreateWithValidData()
        {
            var input = new CreatedClientInputCommand
            {
                Name = "John Tester",
                Document = new Document("12345678909"), // CPF válido com dígito correto
                Email = "john.teste@teste.com.br",
                Renda = 5000,
                Score = 700
            };

            var cliente = Client.Create(input);

            Assert.NotNull(cliente);
            Assert.Equal(input.Name, cliente.Name);
            Assert.Equal(input.Document.Value, cliente.Document.Value);
            Assert.Equal(input.Email, cliente.Email);
            Assert.Equal(input.Renda, cliente.Renda);
            Assert.Equal(input.Score, cliente.Score);
        }

        [Fact]
        public void CreateDocument_ShouldThrow_WhenDocumentIsEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
                new Document(string.Empty));
        }

        [Fact]
        public void CreateDocument_ShouldThrow_WhenCpfIsInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
                new Document("11111111111")); // CPF com todos dígitos iguais
        }

        [Fact]
        public void CreateDocument_ShouldCleanMask_WhenCpfHasFormatting()
        {
            var doc = new Document("123.456.789-09");
            Assert.Equal("12345678909", doc.Value); // máscara removida
        }
    }
}
