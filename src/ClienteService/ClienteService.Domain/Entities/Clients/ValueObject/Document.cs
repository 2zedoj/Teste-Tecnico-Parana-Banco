using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClienteService.Domain.Entities.Clients.ValueObject
{
    public record Document
    {
        public string Value { get; set; } = null!;

        private Document() { }

        public Document(string valeu) 
            => Create(valeu);

        // Método de criação validada
        public void Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("O documento não pode ser vazio.");

            // Remove tudo que não for dígito
            var cleanNumber = Regex.Replace(number, @"\D", "");

            if (cleanNumber.Length == 11)
            {
                if (!IsValidCpf(cleanNumber))
                    throw new ArgumentException("CPF inválido.");
            }
            else if (cleanNumber.Length == 14)
            {
                if (!IsValidCnpj(cleanNumber))
                    throw new ArgumentException("CNPJ inválido.");
            }
            else
            {
                throw new ArgumentException("Documento deve conter 11 dígitos (CPF) ou 14 dígitos (CNPJ).");
            }

            Value = cleanNumber;
        }

        public bool IsCpf => Value.Length == 11;
        public bool IsCnpj => Value.Length == 14;

        public string ToMasked()
        {
            if (IsCpf)
                return Regex.Replace(Value, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");

            if (IsCnpj)
                return Regex.Replace(Value, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");

            return Value;
        }

        public override string ToString() => Value;

        // -----------------------------------------------------
        // Validações de CPF / CNPJ
        // -----------------------------------------------------

        private static bool IsValidCpf(string cpf)
        {
            if (new string(cpf[0], cpf.Length) == cpf) return false;

            var numbers = cpf.Select(c => int.Parse(c.ToString())).ToArray();

            for (int j = 9; j < 11; j++)
            {
                int sum = 0;
                for (int i = 0; i < j; i++)
                    sum += numbers[i] * ((j + 1) - i);

                int remainder = (sum * 10) % 11;
                if (remainder == 10) remainder = 0;

                if (numbers[j] != remainder)
                    return false;
            }

            return true;
        }

        private static bool IsValidCnpj(string cnpj)
        {
            if (new string(cnpj[0], cnpj.Length) == cnpj) return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCnpj = cnpj[..12];
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            var digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}
