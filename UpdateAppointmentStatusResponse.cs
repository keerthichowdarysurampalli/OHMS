using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class UpdateAppointmentStatusResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; } = "Update Appointment Status Sucessfully";
    }
}
