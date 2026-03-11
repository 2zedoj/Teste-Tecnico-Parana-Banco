using ClienteService.Domain.Abstraction;
using ClienteService.Domain.Entities.Clients.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClienteService.Application.Features.Clients
{
    public abstract class BaseClientDto
    {
        [Required(
                ErrorMessageResourceType = typeof(Resources.ExceptionResources.ExceptionResources),
                ErrorMessageResourceName = "exception_required_name")]
        public string Name { get; set; } = null!;
        [Required(
                ErrorMessageResourceType = typeof(Resources.ExceptionResources.ExceptionResources),
                ErrorMessageResourceName = "exception_required_document")]
        public string Document { get; set; } = null!;
        [Required(
            ErrorMessageResourceType = typeof(Resources.ExceptionResources.ExceptionResources),
            ErrorMessageResourceName = "exception_required_email")]
        public string Email { get; set; } = null!;
        [Required(
            ErrorMessageResourceType = typeof(Resources.ExceptionResources.ExceptionResources),
            ErrorMessageResourceName = "exception_required_renda")]
        public double? Renda { get; set; }
        [Required(
            ErrorMessageResourceType = typeof(Resources.ExceptionResources.ExceptionResources),
            ErrorMessageResourceName = "exception_required_score")]
        public int? Score { get; set; }
    }

    public class CreatedClientDto : BaseClientDto
    {
        public Result<ClientResponse>? Validate()
        {
            if (!IsCpfCnpjValido())
                return Result<ClientResponse>.
                        Failed(400, Resources.ExceptionResources.ExceptionResources.exception_validate_document);

            return null;
        }
        private bool IsCpfCnpjValido()
        {
            if (string.IsNullOrWhiteSpace(Document))
                return false;

            Document = Regex.Replace(Document, "[^0-9]", "");

            if (Document.Length == 11)
                return ValidarCpf(Document);

            if (Document.Length == 14)
                return ValidarCnpj(Document);

            return false;
        }

        public static bool ValidarCpf(string cpf)
        {
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public static bool ValidarCnpj(string cnpj)
        {
            if (cnpj.Length != 14 || cnpj.Distinct().Count() == 1)
                return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
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

    public class UpdateClientDto : BaseClientDto
    {
        public string Id { get; set; } = null!;
    }
}
