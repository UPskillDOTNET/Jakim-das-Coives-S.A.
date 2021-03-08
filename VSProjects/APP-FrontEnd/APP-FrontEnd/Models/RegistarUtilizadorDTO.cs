using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class RegistarUtilizadorDTO
    {
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string Nif { get; set; }
        public string NomeUtilizador { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailUtilizador { get; set; }
        [DataType(DataType.Password)]
        public string PasswordUtilizador { get; set; }
        public int MetodoId { get; set; }

        // Dados Cartão de Crédito
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Numero do cartão de crédito inválido")]
        public string Numero { get; set; }
        public string NomeCartao { get; set; }

        [RegularExpression(@"^(0[1-9]|1[0-2])[/]\d{2}$", ErrorMessage = "Data de Validade do cartão de crédito inválido")] 
        public string DataValidade { get; set; }

        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV do cartão de crédito inválido")]
        public string Cvv { get; set; }

        // Dados Debito Direto
        [RegularExpression(@"^[P][T][5][0]\d{21}$", ErrorMessage = "IBAN inválido")]
        public string Iban { get; set; }
        public string NomeDebitoDireto { get; set; }
        public string Rua { get; set; }

        [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "Código postal inválido")]
        public string CodigoPostal { get; set; }
        public string Freguesia { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]

        public DateTime DataSubscricao { get; set; }

        // Dados PayPal
        [DataType(DataType.EmailAddress)]
        public string EmailPayPal { get; set; }

        [DataType(DataType.Password)]
        public string PasswordPayPal { get; set; }
    }
}
