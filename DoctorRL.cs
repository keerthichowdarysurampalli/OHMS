using AutoMapper;
using ClinicAppointmentBookingSystem.Model;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer;
using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public class DoctorRL : IDoctorRL
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient _mongoConnection;
        private readonly IMongoCollection<UserDetails> _userDetails;
        private readonly IMongoCollection<AppointmentDetails> _appointmentDetails;
        private readonly IMongoCollection<FeedbackDetails> _feedbackDetails;
        private readonly IMapper _mapper;
        public DoctorRL(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            _mongoConnection = new MongoClient(_configuration["MemberRegistrationPortalDatabase:ConnectionString"]);
            var MongoDataBase = _mongoConnection.GetDatabase(_configuration["MemberRegistrationPortalDatabase:DatabaseName"]);
            _userDetails = MongoDataBase.GetCollection<UserDetails>(_configuration["MemberRegistrationPortalDatabase:UserCollectionName"]);
            _appointmentDetails = MongoDataBase.GetCollection<AppointmentDetails>(_configuration["MemberRegistrationPortalDatabase:AppointmentCollectionName"]);
            _feedbackDetails = MongoDataBase.GetCollection<FeedbackDetails>(_configuration["MemberRegistrationPortalDatabase:FeedbackCollectionName"]);
        }

        public async Task<GetFeedbackListResponse> GetFeedbackList(string UserID)
        {
            GetFeedbackListResponse response = new GetFeedbackListResponse();
            try
            {

                var feedbackDetails = _feedbackDetails.Find(x => x.DoctorUserID.ToLower() == UserID).ToList();
                if (feedbackDetails.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Doctor Record Not Found";
                    return response;
                }

                List<FeedbackInDetails> _data = new List<FeedbackInDetails>();
                feedbackDetails.ForEach(x =>
                {
                    FeedbackInDetails _getData = new FeedbackInDetails();
                    _getData.ID = x.ID;
                    _getData.CreatedDate = x.CreatedDate;
                    _getData.PatientUserID = x.PatientUserID;
                    _getData.PatientUserDetails = _userDetails.Find(x1 => x1.Id == x.PatientUserID).FirstOrDefaultAsync().Result;
                    _getData.DoctorUserID = x.DoctorUserID;
                    _getData.DoctorUserDetails = _userDetails.Find(x1 => x1.Id == x.DoctorUserID).FirstOrDefaultAsync().Result;
                    _getData.Feedback = x.Feedback;
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

        public async Task<GetPatientListResponse> GetPatientList(string UserID)
        {
            GetPatientListResponse response = new GetPatientListResponse();
            try
            {

                var appointmentDetails = _appointmentDetails.Find(x => x.DoctorUserID.ToLower() == UserID).ToList();
                if (appointmentDetails.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Doctor Record Not Found";
                    return response;
                }

                List<AppointmentInDetails> _data = new List<AppointmentInDetails>();
                appointmentDetails.ForEach(x =>
                {
                    AppointmentInDetails _getData = new AppointmentInDetails();
                    _getData.ID = x.ID;
                    _getData.CreatedDate = x.CreatedDate;
                    _getData.PatientUserID = x.PatientUserID;
                    _getData.patientUserDetails = _userDetails.Find(x1 => x1.Id == x.PatientUserID).FirstOrDefaultAsync().Result;
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
                    _getData.SlotTime = x.SlotTime;
                    _getData.Url = x.Url;
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

        public async Task<UpdateAppointmentByDoctorResponse> UpdateAppointmentByDoctor(UpdateAppointmentByDoctorRequest request)
        {
            UpdateAppointmentByDoctorResponse response = new UpdateAppointmentByDoctorResponse();
            try
            {
                Account account = new Account(
                                _configuration["CloudinarySettings:CloudName"],
                                _configuration["CloudinarySettings:ApiKey"],
                                _configuration["CloudinarySettings:ApiSecret"]);

                var path = request.File.OpenReadStream();

                Cloudinary cloudinary = new Cloudinary(account);

                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(request.File.FileName, path),
                };
                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                
                var IsRecord = _appointmentDetails.Find(x => x.ID == request.ID).FirstOrDefaultAsync().Result;
                if (IsRecord == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went wrong";
                    return response;
                }

                IsRecord.AppointmentDate = request.AppointmentDate;
                IsRecord.AppointmentTime = request.AppointmentTime;
                IsRecord.DoctorDescription = request.DoctorDescription;
                IsRecord.Url = uploadResult.Url.ToString();
                IsRecord.Price = request.Price;

                var IsUpdate = _appointmentDetails.ReplaceOneAsync(x => x.ID == request.ID, IsRecord).Result;
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

        public async Task<UpdateAppointmentStatusResponse> UpdateAppointmentStatus(string ID, string Status)
        {
            UpdateAppointmentStatusResponse response = new UpdateAppointmentStatusResponse();
            try
            {

                var IsRecord = _appointmentDetails.Find(x => x.ID == ID).FirstOrDefaultAsync().Result;
                if (IsRecord == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went wrong";
                    return response;
                }

                IsRecord.Status = Status.ToUpper();
                var IsUpdate = _appointmentDetails.ReplaceOneAsync(x => x.ID == ID, IsRecord).Result;
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
    }
}
