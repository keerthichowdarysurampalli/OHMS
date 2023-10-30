using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class SignInResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public UserDetails? data { get; set; }
        public string? Token { get; set; }
    }
}
