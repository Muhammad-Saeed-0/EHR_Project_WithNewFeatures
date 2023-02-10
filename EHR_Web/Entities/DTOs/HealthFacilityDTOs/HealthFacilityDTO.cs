﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EHR_Web.Entities.DTOs.HealthFacilityDTOs
{
    public class HealthFacilityDTO
    {
        [DisplayName("Health Facility No.")]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        [DisplayName("Subordinate To")]
        public string? SubordinateTo { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone3 { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }

        public int GovernorateId { get; set; }
    }
}
