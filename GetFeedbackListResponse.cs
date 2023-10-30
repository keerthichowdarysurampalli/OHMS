using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class GetFeedbackListResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; } = "Sucessfully";
        public List<FeedbackInDetails>? data { get; set; } = new List<FeedbackInDetails>();
    }

    public class FeedbackInDetails
    {
        public string? ID { get; set; }
        public string? CreatedDate { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public string? PatientUserID { get; set; }
        public UserDetails? PatientUserDetails { get; set; }
        public string? DoctorUserID { get; set; }
        public UserDetails? DoctorUserDetails { get; set; }
        public string? Feedback { get; set; }
    }
}
