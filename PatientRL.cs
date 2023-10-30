using Amazon.Runtime.Internal;
using AutoMapper;
using ClinicAppointmentBookingSystem.Model;
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
    public class PatientRL : IPatientRL
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient _mongoConnection;
        private readonly IMongoCollection<UserDetails> _userDetails;
        private readonly IMongoCollection<AppointmentDetails> _appointmentDetails;
        private readonly IMongoCollection<FeedbackDetails> _feedbackDetails;
        private readonly IMapper _mapper;
        public PatientRL(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            _mongoConnection = new MongoClient(_configuration["MemberRegistrationPortalDatabase:ConnectionString"]);
            var MongoDataBase = _mongoConnection.GetDatabase(_configuration["MemberRegistrationPortalDatabase:DatabaseName"]);
            _userDetails = MongoDataBase.GetCollection<UserDetails>(_configuration["MemberRegistrationPortalDatabase:UserCollectionName"]);
            _appointmentDetails = MongoDataBase.GetCollection<AppointmentDetails>(_configuration["MemberRegistrationPortalDatabase:AppointmentCollectionName"]);
            _feedbackDetails = MongoDataBase.GetCollection<FeedbackDetails>(_configuration["MemberRegistrationPortalDatabase:FeedbackCollectionName"]);
        }

        public async Task<AddAppointmentResponse> AddAppointment(AddAppointmentRequest request)
        {
            AddAppointmentResponse response = new AddAppointmentResponse();
            try
            {
                // Add Appointment Details
                AppointmentDetails userDetails = new AppointmentDetails();
                userDetails = _mapper.Map<AppointmentDetails>(request);
                userDetails.Status = "BOOKED";


                await _appointmentDetails.InsertOneAsync(userDetails);

                var DoctorUserDetails = await BookUserSlot(request);

                var updateDoctorUserDetails = _appointmentDetails
                    .Find(x => x.DoctorUserID == request.DoctorUserID && x.AppointmentDate == request.AppointmentDate).ToList();

                foreach (var item in updateDoctorUserDetails)
                {
                    item.Slot1 = DoctorUserDetails.Slot1;
                    item.Slot2 = DoctorUserDetails.Slot2;
                    item.Slot3 = DoctorUserDetails.Slot3;
                    item.Slot4 = DoctorUserDetails.Slot4;
                    item.Slot5 = DoctorUserDetails.Slot5;
                    item.Slot6 = DoctorUserDetails.Slot6;
                    item.Slot7 = DoctorUserDetails.Slot7;
                    item.Slot8 = DoctorUserDetails.Slot8;
                    item.Slot9 = DoctorUserDetails.Slot9;
                    item.Slot10 = DoctorUserDetails.Slot10;
                    item.Slot11 = DoctorUserDetails.Slot11;
                    item.Slot12 = DoctorUserDetails.Slot12;
                    item.Slot13 = DoctorUserDetails.Slot13;
                    item.Slot14 = DoctorUserDetails.Slot14;

                    var _data = await _appointmentDetails.ReplaceOneAsync(x => x.ID == item.ID, item);
                    if (!_data.IsAcknowledged)
                    {
                        response.IsSuccess = false;
                        response.Message = "Something went wrong At Doctor User Details";
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<AppointmentDetails> BookUserSlot(AddAppointmentRequest request)
        {
            var DoctorUserDetails = _appointmentDetails
                .Find(x => x.DoctorUserID == request.DoctorUserID && x.AppointmentDate == request.AppointmentDate).FirstOrDefaultAsync().Result;

            if (request.SlotNumber.ToUpper() == "SLOT1") { DoctorUserDetails.Slot1 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT2") { DoctorUserDetails.Slot2 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT3") { DoctorUserDetails.Slot3 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT4") { DoctorUserDetails.Slot4 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT5") { DoctorUserDetails.Slot5 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT6") { DoctorUserDetails.Slot6 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT7") { DoctorUserDetails.Slot7 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT8") { DoctorUserDetails.Slot8 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT9") { DoctorUserDetails.Slot9 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT10") { DoctorUserDetails.Slot10 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT11") { DoctorUserDetails.Slot11 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT12") { DoctorUserDetails.Slot12 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT13") { DoctorUserDetails.Slot13 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT14") { DoctorUserDetails.Slot14 = true; }

            return DoctorUserDetails;
        }

        public async Task<AddFeedbackResponse> AddFeedback(AddFeedbackRequest request)
        {
            AddFeedbackResponse response = new AddFeedbackResponse();
            try
            {
                FeedbackDetails userDetails = new FeedbackDetails();
                userDetails = _mapper.Map<FeedbackDetails>(request);
                await _feedbackDetails.InsertOneAsync(userDetails);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response; throw new NotImplementedException();
        }

        public async Task<DeleteAppointmentResponse> DeleteAppointment(string Id)
        {
            DeleteAppointmentResponse response = new DeleteAppointmentResponse();
            try
            {
                var IsRecord = _appointmentDetails.Find(x => x.ID == Id).FirstOrDefaultAsync().Result;
                if (IsRecord == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went wrong";
                }

                IsRecord.Status = "CANCELLED";
                var IsUpdate = _appointmentDetails.ReplaceOneAsync(x => x.ID == Id, IsRecord).Result;
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

        public async Task<GetAllDoctorListResponse> GetAllDoctorList()
        {
            GetAllDoctorListResponse response = new GetAllDoctorListResponse();
            try
            {

                response.data = _userDetails.Find(x => x.Role.ToLower() == "doctor").ToList();
                if (response.data.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Doctor Record Not Found";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<GetAppointmentResponse> GetAppointment(string UserID)
        {
            GetAppointmentResponse response = new GetAppointmentResponse();
            try
            {
                response.data = _appointmentDetails.Find(x => x.PatientUserID == UserID).SortByDescending(x => x.ID).ToList();
                if (response.data.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Record Not Found";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<SubmitPaymentResponse> SubmitPayment(string ID)
        {
            SubmitPaymentResponse response = new SubmitPaymentResponse();
            try
            {
                var _appointmentExist = _appointmentDetails
                   .Find(x => x.ID == ID).FirstOrDefaultAsync().Result;
                if (_appointmentExist == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Appointment Record Not Present";
                    return response;
                }

                _appointmentExist.IsPayment = true;
                var IsUpdate = _appointmentDetails.ReplaceOneAsync(x => x.ID == ID, _appointmentExist).Result;
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

        public async Task<UpdateAppointmentResponse> UpdateAppointment(UpdateAppointmentRequest request)
        {
            UpdateAppointmentResponse response = new UpdateAppointmentResponse();
            try
            {
                var _appointmentExist = _appointmentDetails
                   .Find(x => x.ID == request.ID).FirstOrDefaultAsync().Result;
                if (_appointmentExist == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Appointment Record Not Present";
                    return response;
                }

                _appointmentExist.AppointmentDate = request.AppointmentDate;
                _appointmentExist.AppointmentTime = request.AppointmentTime;
                _appointmentExist.PatientDescription = request.PatientDescription;
                _appointmentExist.SlotNumber = request.SlotNumber;
                _appointmentExist.SlotTime = request.SlotTime;

                var IsUpdateAppointmentDetails = _appointmentDetails.ReplaceOneAsync(x => x.ID == request.ID, _appointmentExist).Result;
                if (!IsUpdateAppointmentDetails.IsAcknowledged)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went wrong";
                }

                if (request.OldSlotNumber.ToUpper() != request.SlotNumber.ToUpper())
                {
                    var DoctorUserDetails = await UpdateUserSlot(request);

                    var DoctorAppointmentDetails = _appointmentDetails
                        .Find(x => x.DoctorUserID == request.DoctorUserID && x.AppointmentDate == request.AppointmentDate).ToList();

                    foreach ( var item in DoctorAppointmentDetails ) 
                    {
                        item.Slot1 = DoctorUserDetails.Slot1;
                        item.Slot2 = DoctorUserDetails.Slot2;
                        item.Slot3 = DoctorUserDetails.Slot3;
                        item.Slot4 = DoctorUserDetails.Slot4;
                        item.Slot5 = DoctorUserDetails.Slot5;
                        item.Slot6 = DoctorUserDetails.Slot6;
                        item.Slot7 = DoctorUserDetails.Slot7;
                        item.Slot8 = DoctorUserDetails.Slot8;
                        item.Slot9 = DoctorUserDetails.Slot9;
                        item.Slot10 = DoctorUserDetails.Slot10;
                        item.Slot11 = DoctorUserDetails.Slot11;
                        item.Slot12 = DoctorUserDetails.Slot12;
                        item.Slot13 = DoctorUserDetails.Slot13;
                        item.Slot14 = DoctorUserDetails.Slot14;

                        var IsUpdate = _appointmentDetails.ReplaceOneAsync(x => x.ID == item.ID, item).Result;
                        if (!IsUpdate.IsAcknowledged)
                        {
                            response.IsSuccess = false;
                            response.Message = "Something went wrong";
                        }
                    }   
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<AppointmentDetails> UpdateUserSlot(UpdateAppointmentRequest request)
        {
            //Remove Previes Slot

            var DoctorUserDetails = _appointmentDetails
                .Find(x => x.DoctorUserID == request.DoctorUserID && x.AppointmentDate == request.AppointmentDate)
                .FirstOrDefaultAsync().Result;

            if (request.OldSlotNumber.ToUpper() == "SLOT1") { DoctorUserDetails.Slot1 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT2") { DoctorUserDetails.Slot2 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT3") { DoctorUserDetails.Slot3 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT4") { DoctorUserDetails.Slot4 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT5") { DoctorUserDetails.Slot5 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT6") { DoctorUserDetails.Slot6 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT7") { DoctorUserDetails.Slot7 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT8") { DoctorUserDetails.Slot8 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT9") { DoctorUserDetails.Slot9 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT10") { DoctorUserDetails.Slot10 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT11") { DoctorUserDetails.Slot11 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT12") { DoctorUserDetails.Slot12 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT13") { DoctorUserDetails.Slot13 = false; }
            else
                if (request.OldSlotNumber.ToUpper() == "SLOT14") { DoctorUserDetails.Slot14 = false; }

            //Update New Slot

            if (request.SlotNumber.ToUpper() == "SLOT1") { DoctorUserDetails.Slot1 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT2") { DoctorUserDetails.Slot2 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT3") { DoctorUserDetails.Slot3 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT4") { DoctorUserDetails.Slot4 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT5") { DoctorUserDetails.Slot5 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT6") { DoctorUserDetails.Slot6 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT7") { DoctorUserDetails.Slot7 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT8") { DoctorUserDetails.Slot8 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT9") { DoctorUserDetails.Slot9 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT10") { DoctorUserDetails.Slot10 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT11") { DoctorUserDetails.Slot11 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT12") { DoctorUserDetails.Slot12 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT13") { DoctorUserDetails.Slot13 = true; }
            else
                if (request.SlotNumber.ToUpper() == "SLOT14") { DoctorUserDetails.Slot14 = true; }

            return DoctorUserDetails;
        }

        public async Task<GetAppointmentDetailByDateResponse> GetAppointmentDetailByDate(string DoctorUserID, string AppointmentDate)
        {
            GetAppointmentDetailByDateResponse response = new GetAppointmentDetailByDateResponse();
            try
            {
                var AppointmentDetails = await _appointmentDetails
                    .Find(x => x.DoctorUserID == DoctorUserID && x.AppointmentDate == AppointmentDate)
                    .FirstOrDefaultAsync();
                if (AppointmentDetails == null)
                {
                    response.IsSuccess = false;
                    response.data = null;
                }

                response.data = AppointmentDetails;
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
