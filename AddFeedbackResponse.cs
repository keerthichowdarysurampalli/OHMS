using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class AddFeedbackResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; } = "Send Feedback Sucessfully";
    }
}
