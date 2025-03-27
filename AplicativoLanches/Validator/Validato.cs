using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AplicativoLanches.Validator
{
    public class Validato : IValidator
    {
        public string NomeErro { get; set; } = "";
        public string EmailErro { get; set; } = "";
        public string TelefoneErro { get; set; } = "";
        public string SenhaErro { get; set; } = "";

        private string NomeVazioErroMsg = "Por favor informe o seu nome.";
        private string NomeInvalidoErroMsg = "Por favor informe um nome válido.";
        private string EmailVazioErroMsg = "Por favor informe o seu email.";
        private string EmailInvalidooErroMsg = "Por favor informe um email válido.";
        private string TelefoneVazioErroMsg = "Por favor informe o seu telefone.";
        private string TelefoneInvalidoErroMsg = "Por favor informe um telefone válido.";
        private string SenhaVazioErroMsg = "Por favor informe o sua senha.";
        private string SenhaInvalidaErroMsg = "Por favor informe ima senha valida.";

        public Task<bool> Validar(string nome, string email, string telefone, string senha)
        {
            var isNomeValido = ValidarNome(nome);
            var isEmailValido = ValidarEmail(email);
            var isTelefoneValido = ValidarTelefone(telefone);
            var isSenhaValida = ValidarSenha(senha);

            return Task.FromResult(isNomeValido && isEmailValido && isTelefoneValido && isSenhaValida);
        }

        private bool ValidarNome(string nome) {
            if (string.IsNullOrEmpty(nome)) 
            {
                NomeErro = NomeVazioErroMsg;
                return false;
            }
            if(nome.Length < 3) 
            { NomeErro=NomeInvalidoErroMsg;
                return false;
            }
            NomeErro = "";
        return true;
        }

        private bool ValidarEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                EmailErro = EmailVazioErroMsg;
                return false;
            }
            if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                EmailErro=EmailInvalidooErroMsg;
                return false;
            }
            EmailErro = "";
            return true;
        }


        private bool ValidarTelefone(string telefone)
        {
            if (string.IsNullOrEmpty(telefone))
            {
                TelefoneErro = TelefoneVazioErroMsg;
                return false;
            }
            if (telefone.Length < 12)
            {
                TelefoneErro = TelefoneInvalidoErroMsg;
                return false;
            }
            TelefoneErro = "";
            return true;
        }

        private bool ValidarSenha(string senha)
        {


            if (string.IsNullOrEmpty(senha))
            {
                SenhaErro = SenhaVazioErroMsg;
                return false;

            }

            if (senha.Length < 8 || !Regex.IsMatch(senha, @"[a-zA-Z]") || !Regex.IsMatch(senha, @"\d"))
            {
                SenhaErro = SenhaInvalidaErroMsg;
                return false;
            }

            SenhaErro = "";
            return true;
        }



    }
}
