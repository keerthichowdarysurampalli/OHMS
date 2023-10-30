using CommonLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public interface IDoctorRL
    {
        Task<GetPatientListResponse> GetPatientList(string UserID);
        Task<UpdateAppointmentStatusResponse> UpdateAppointmentStatus(string UserID, string Status);
        Task<UpdateAppointmentByDoctorResponse> UpdateAppointmentByDoctor(UpdateAppointmentByDoctorRequest request);
        Task<GetFeedbackListResponse> GetFeedbackList(string UserID);
    }
}
