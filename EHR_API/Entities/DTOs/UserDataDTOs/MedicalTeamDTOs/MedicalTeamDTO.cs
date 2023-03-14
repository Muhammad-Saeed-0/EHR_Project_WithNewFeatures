﻿using EHR_API.Entities.DTOs.UserDataDTOs.AuthDTOs.Registration;
using EHR_API.Entities.DTOs.GovernorateDTOs;
using System.ComponentModel;
using EHR_API.Entities.Models.UsersData;
using EHR_API.Entities.Models;

namespace EHR_API.Entities.DTOs.UserDataDTOs.MedicalTeamDTOs
{
    public class MedicalTeamDTO
    {
        public string Id { get; set; }
        public string MedicalSpecialization { get; set; }
        public string Degree { get; set; }
        [DisplayName("Created at")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Updated at")]
        public DateTime UpdatedAt { get; set; }
    }
}
