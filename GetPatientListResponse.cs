using ClinicAppointmentBookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class GetPatientListResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; } = "Sucessfully";
        public List<AppointmentInDetails>? data { get; set; } = new List<AppointmentInDetails>();
    }
}
