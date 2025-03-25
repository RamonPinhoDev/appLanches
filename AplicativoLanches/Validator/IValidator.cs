using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicativoLanches.Validator
{
    public interface IValidator
    {
      public  string NomeErro { get; set; }
        public string EmailErro { get; set; }
        public string TelefoneErro { get; set; }
        public string SenhaErro { get; set; }
        public Task<bool> Validar(string nome , string email, string telefone, string senha);
    }
}
