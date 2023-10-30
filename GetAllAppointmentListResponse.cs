using ClinicAppointmentBookingSystem.Model;
using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class GetAllAppointmentListResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; } = "Sucessfully";
        public List<AppointmentInDetails>? data { get; set; }
    }

    public class AppointmentInDetails
    {
        public string? ID { get; set; }
        public string? CreatedDate { get; set; } 
        public string? PatientUserID { get; set; }
        public UserDetails? patientUserDetails { get; set; } = new UserDetails();
        public string? DoctorUserID { get; set; }
        public UserDetails? doctorUserDetails { get; set; } = new UserDetails();
        public string? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public string? PatientDescription { get; set; } = string.Empty;
        public string? DoctorDescription { get; set; } = string.Empty;
        public string? Price { get; set; }
        public string? SlotTime { get; set; }
        public bool IsPayment { get; set; } 
        public string? Status { get; set; } 
        public bool IsActive { get; set; } 
        public string? Url { get; set; }
    }
}
