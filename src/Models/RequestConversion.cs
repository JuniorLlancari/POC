using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipoCambio.Models
{
    public class RequestConversion
    {
        public string MonedaOrigen { get; set; }
        public string MonedaDestino { get; set; }
        public decimal Monto { get; set; }
    }

    public class BaseResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public T? Data { get; set; }
    }


    public class ResponseConversion
    {
        public string MonedaOrigen { get; set; }
        public string MonedaDestino { get; set; }
        public decimal Monto { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal MontoConTipoCambio { get; set; }

    }


}
