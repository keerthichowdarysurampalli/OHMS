using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public static class RepositoryLayerDependenciesExtension
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationRL, AuthenticationRL>();
            services.AddScoped<IPatientRL, PatientRL>();
            services.AddScoped<IDoctorRL, DoctorRL>();
            services.AddScoped<IReceptionistRL, ReceptionistRL>();
            services.AddScoped<IPharmacistRL, PharmacistRL>();
        }
    }
}
