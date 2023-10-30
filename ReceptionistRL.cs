using AutoMapper;
using ClinicAppointmentBookingSystem.Model;
using CommonLayer;
using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public class ReceptionistRL : IReceptionistRL
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient _mongoConnection;
        private readonly IMongoCollection<UserDetails> _userDetails;
        private readonly IMongoCollection<AppointmentDetails> _appointmentDetails;
        private readonly IMapper _mapper;
        public ReceptionistRL(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            _mongoConnection = new MongoClient(_configuration["MemberRegistrationPortalDatabase:ConnectionString"]);
            var MongoDataBase = _mongoConnection.GetDatabase(_configuration["MemberRegistrationPortalDatabase:DatabaseName"]);
            _userDetails = MongoDataBase.GetCollection<UserDetails>(_configuration["MemberRegistrationPortalDatabase:UserCollectionName"]);
            _appointmentDetails = MongoDataBase.GetCollection<AppointmentDetails>(_configuration["MemberRegistrationPortalDatabase:AppointmentCollectionName"]);

        }

        public async Task<AddPaymentResponse> AddPayment(AddPaymentRequest request)
        {
            AddPaymentResponse response = new AddPaymentResponse();
            try
            {

                var appointmentDetails = _appointmentDetails.Find(x => x.ID==request.ID).FirstOrDefaultAsync().Result;
                if (appointmentDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Appointment Record Not Found";
                    return response;
                }

                appointmentDetails.Price = request.Price;
                var IsUpdate = _appointmentDetails.ReplaceOneAsync(x => x.ID == request.ID, appointmentDetails).Result;
                if (!IsUpdate.IsAcknowledged)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went wrong";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<GetAllAppointmentListResponse> GetAllAppointmentList()
        {
            GetAllAppointmentListResponse response = new GetAllAppointmentListResponse();
            try
            {

                var appointmentDetails = _appointmentDetails.Find(x => true).ToList();
                if (appointmentDetails.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Appointment Record Not Found";
                    return response;
                }
                
                List<AppointmentInDetails> _data = new List<AppointmentInDetails>();
                 appointmentDetails.ForEach(x =>
                {
                    AppointmentInDetails _getData = new AppointmentInDetails();
                    _getData.ID = x.ID;
                    _getData.CreatedDate = x.CreatedDate;
                    _getData.PatientUserID = x.PatientUserID;
                    _getData.patientUserDetails = _userDetails.Find(x1 => x1.Id== x.PatientUserID).FirstOrDefaultAsync().Result;
                    _getData.DoctorUserID = x.DoctorUserID;
                    _getData.doctorUserDetails = _userDetails.Find(x1 => x1.Id == x.DoctorUserID).FirstOrDefaultAsync().Result;
                    _getData.AppointmentDate = x.AppointmentDate;
                    _getData.AppointmentTime = x.AppointmentTime;
                    _getData.PatientDescription = x.PatientDescription;
                    _getData.DoctorDescription = x.DoctorDescription;
                    _getData.Price = x.Price;
                    _getData.IsPayment = x.IsPayment;
                    _getData.Status = x.Status;
                    _getData.IsActive = x.IsActive;
                    _data.Add(_getData);
                });

                response.data = _data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
