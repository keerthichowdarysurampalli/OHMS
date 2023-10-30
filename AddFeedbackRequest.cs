using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class AddFeedbackRequest
    {
        public string? PatientUserID { get; set; }
        public string? DoctorUserID { get; set; }
        public string? Feedback {  get; set; }
    }
}
