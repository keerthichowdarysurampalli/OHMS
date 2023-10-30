using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class UpdateAppointmentByDoctorRequest
    {
        public string? ID { get; set; }
        public string? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public string? DoctorDescription { get; set; }
        public string? Price { get; set; }
        public IFormFile? File {  get; set; } 
    }
}
