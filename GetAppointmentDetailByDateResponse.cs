using ClinicAppointmentBookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class GetAppointmentDetailByDateResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; } = string.Empty;
        public AppointmentDetails? data { get; set; } = new AppointmentDetails();
    }
}
