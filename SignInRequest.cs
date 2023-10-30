using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class SignInRequest
    {
        [Required]
        public string? EmailId { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
