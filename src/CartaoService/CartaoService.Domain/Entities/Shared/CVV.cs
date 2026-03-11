using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaoService.Domain.Entities.Shared
{
    public record CVV
    {
        public string Valor { get; set; } = null!;

        private CVV() { }

        public CVV(string valor) 
            => Valor = valor;

        public static CVV Gerar()
        {
            var valor = new Random().Next(100, 999).ToString();
            return new CVV(valor);
        }

        public override string ToString() => Valor;
    }
}
