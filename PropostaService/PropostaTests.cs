using PropostaService.Domain.Entities.Propostas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService
{
    public class PropostaTests
    {
        [Fact]
        public void CreateProposta_ShouldRefuse_WhenScoreIsLow()
        {
            var proposta = Proposta.Create(Guid.NewGuid(), "John Tester", 50);
            proposta.AnaliseProposta();

            Assert.Equal(0, proposta.MaxCartoes);
            Assert.Equal(0, proposta.Limite);
            Assert.Equal(StatusType.Recusada, proposta.Status); // ← adiciona isso
        }

        [Fact]
        public void CreateProposta_ShouldApproveOneCard_WhenScoreIsMedium()
        {
            var proposta = Proposta.Create(Guid.NewGuid(), "John Tester", 200);
            proposta.AnaliseProposta();

            Assert.Equal(1, proposta.MaxCartoes);
            Assert.Equal(1000, proposta.Limite);
            Assert.Equal(StatusType.Aprovada, proposta.Status); // ← adiciona isso
        }

        [Fact]
        public void CreateProposta_ShouldApproveTwoCards_WhenScoreIsHigh()
        {
            var proposta = Proposta.Create(Guid.NewGuid(), "John Tester", 700);
            proposta.AnaliseProposta();

            Assert.Equal(2, proposta.MaxCartoes);
            Assert.Equal(5000, proposta.Limite);
            Assert.Equal(StatusType.Aprovada, proposta.Status); // ← adiciona isso
        }

        [Fact]
        public void CreateProposta_ShouldRefuse_WhenScoreIsExactly100()
        {
            // testa o limite exato da faixa
            var proposta = Proposta.Create(Guid.NewGuid(), "John Tester", 100);
            proposta.AnaliseProposta();

            Assert.Equal(0, proposta.MaxCartoes);
            Assert.Equal(0, proposta.Limite);
        }

        [Fact]
        public void CreateProposta_ShouldApproveOneCard_WhenScoreIsExactly101()
        {
            // testa o início da segunda faixa
            var proposta = Proposta.Create(Guid.NewGuid(), "John Tester", 101);
            proposta.AnaliseProposta();

            Assert.Equal(1, proposta.MaxCartoes);
            Assert.Equal(1000, proposta.Limite);
        }
    }
}
