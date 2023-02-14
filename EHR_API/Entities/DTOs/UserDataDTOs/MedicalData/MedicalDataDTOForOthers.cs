﻿using System.ComponentModel;

namespace EHR_API.Entities.DTOs.UserDataDTOs
{
    public class MedicalDataDTOForOthers
    {
        public string Id { get; set; }
        public string BloodGroup { get; set; }
        public string FamilyHistory { get; set; }
        public string MedicalHistory { get; set; }
        public string DNAImageResultUrl { get; set; }
    }
}
