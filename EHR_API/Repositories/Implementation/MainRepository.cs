﻿using AutoMapper;
using EHR_API.Entities;
using EHR_API.Entities.Models.UsersData;
using EHR_API.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;

namespace EHR_API.Repositories.Implementation
{
    public class MainRepository : IMainRepository
    {
        public MainRepository(ApplicationDbContext  db, IMapper mapper, UserManager<RegistrationData> userManager, IConfiguration configuration)
        {
            _governorate = new GovernorateRepository(db);
            _healthFacility = new HealthFacilityRepository(db);
            _authentication = new AuthenticationRepository(mapper, userManager, configuration);
            _personal = new PersonalDataRepository(db);
            _patient = new PatientRepository(db);
            _medicalTeam = new MedicalTeamRepository(db);
            _medicalData = new MedicalDataRepository(db);
        }

        public IGovernorateRepository _governorate { get; private set; }
        public IHealthFacilityRepository _healthFacility { get; private set; }
        public IAuthenticationRepository _authentication { get; private set; }
        public IPersonalDataRepository _personal { get; private set; }
        public IPatientRepository _patient { get; private set; }
        public IMedicalTeamRepository _medicalTeam { get; private set; }
        public IMedicalDataRepository _medicalData { get; private set; }
    }
}
