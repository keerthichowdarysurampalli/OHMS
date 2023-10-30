using Amazon.Runtime.Internal;
using AutoMapper;
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
    public class AuthenticationRL : IAuthenticationRL
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient _mongoConnection;
        private readonly IMongoCollection<UserDetails> _userDetails;
        private readonly IMapper _mapper;
        public AuthenticationRL(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            _mongoConnection = new MongoClient(_configuration["MemberRegistrationPortalDatabase:ConnectionString"]);
            var MongoDataBase = _mongoConnection.GetDatabase(_configuration["MemberRegistrationPortalDatabase:DatabaseName"]);
            _userDetails = MongoDataBase.GetCollection<UserDetails>(_configuration["MemberRegistrationPortalDatabase:UserCollectionName"]);

        }

        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            SignInResponse response = new SignInResponse();
            response.IsSuccess = true;
            response.Message = "LogIn Successfully.";

            try
            {
                var IsUserExist = await _userDetails
                    .Find(x => x.EmailID.ToLower().Equals(request.EmailId.ToLower()) && x.Password == request.Password)
                    .FirstOrDefaultAsync();

                if (IsUserExist == null)
                {
                    response.IsSuccess = false;
                    response.Message = "User Not Exist.";
                    return response;
                }

                response.data = new UserDetails();
                response.data = IsUserExist;
                response.data.Password = string.Empty;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public async Task<SignUpResponse> SignUp(SignUpRequest request)
        {
            SignUpResponse response = new SignUpResponse();
            response.IsSuccess = true;
            response.Message = "Registration Successfully.";

            try
            {
                var IsUserExist = await _userDetails
                    .Find(x => x.EmailID.ToLower().Equals(request.EmailID.ToLower()))
                    .FirstOrDefaultAsync();

                if (IsUserExist != null)
                {
                    response.IsSuccess = false;
                    response.Message = "User already exist.";
                    return response;
                }

               
                string Age = await CalculateAge(Convert.ToDateTime(request.DateOfBirth));

                UserDetails userDetails = new UserDetails();
                userDetails = _mapper.Map<UserDetails>(request);
                userDetails.Age = Convert.ToInt32(Age.Split('-')[0]);
                
                await _userDetails.InsertOneAsync(userDetails);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        private async Task<string> CalculateAge(DateTime Dob)
        {
            try
            {

                DateTime Now = DateTime.Now;
                int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
                DateTime PastYearDate = Dob.AddYears(Years);
                int Months = 0;
                for (int i = 1; i <= 12; i++)
                {
                    if (PastYearDate.AddMonths(i) == Now)
                    {
                        Months = i;
                        break;
                    }
                    else if (PastYearDate.AddMonths(i) >= Now)
                    {
                        Months = i - 1;
                        break;
                    }
                }
                int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
                int Hours = Now.Subtract(PastYearDate).Hours;
                int Minutes = Now.Subtract(PastYearDate).Minutes;
                int Seconds = Now.Subtract(PastYearDate).Seconds;
                return Years + "-" + Months + "-" + Days;


            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}
