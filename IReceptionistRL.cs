using CommonLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public interface IReceptionistRL
    {
        Task<GetAllAppointmentListResponse> GetAllAppointmentList();
        Task<AddPaymentResponse> AddPayment(AddPaymentRequest request);
    }
}
