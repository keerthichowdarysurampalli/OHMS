using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class SubmitPaymentResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; } = "Payment Done Sucessfully";
    }
}
