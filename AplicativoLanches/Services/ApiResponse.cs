using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicativoLanches.Services
{
    public class ApiResponse<T>
    {public T? Data { get; set; }
        public string?  ErrorMessage { get; set; }
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    }
}
