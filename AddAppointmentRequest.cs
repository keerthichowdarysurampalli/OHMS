using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class AddAppointmentRequest
    {
        public string? PatientUserID { get; set; }
        public string? DoctorUserID { get; set; }
        public string? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public string? SlotNumber { get; set; }
        public string? SlotTime { get; set; }
        public string? PatientDescription { get; set; }
    }
}
