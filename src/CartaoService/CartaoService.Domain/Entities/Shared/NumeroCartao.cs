using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaoService.Domain.Entities.Shared
{
    public sealed class NumeroCartao
    {
        public string Valor { get; set; } = null!;

        private NumeroCartao() { }
        public NumeroCartao(string valor)
            => Valor = valor;

        public static NumeroCartao Gerar()
        {
            var random = new Random();

            // gera 15 dígitos aleatórios
            var digits = new int[16];
            for (int i = 0; i < 15; i++)
                digits[i] = random.Next(0, 10);

            // calcula o dígito verificador pelo algoritmo Luhn
            digits[15] = CalcularDigitoVerificador(digits);

            return new NumeroCartao(string.Concat(digits));
        }

        public static bool Validar(string numero)
        {
            if (numero.Length != 16 || !numero.All(char.IsDigit))
                return false;

            var digits = numero.Select(c => c - '0').ToArray();
            var checkDigit = digits[^1];
            var calculado = CalcularDigitoVerificador(digits[..^1]);
            return checkDigit == calculado;
        }

        private static int CalcularDigitoVerificador(int[] digits)
        {
            // Luhn: dobra os dígitos em posições pares (da direita)
            var soma = 0;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                var d = digits[i];
                if ((digits.Length - i) % 2 == 0) // posição par da direita
                {
                    d *= 2;
                    if (d > 9) d -= 9;
                }
                soma += d;
            }
            return (10 - (soma % 10)) % 10;
        }

        public override string ToString() => Valor;
    }
}
