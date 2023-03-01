﻿using AutoMapper;
using EHR_API.Entities;
using EHR_API.Entities.DTOs.AllergyDTOs;
using EHR_API.Entities.DTOs.MedicationDTOs;
using EHR_API.Entities.DTOs.UserDataDTOs;
using EHR_API.Entities.DTOs.UserDataDTOs.AuthDTOs.Registration;
using EHR_API.Entities.Models;
using EHR_API.Entities.Models.UsersData;
using EHR_API.Extensions;
using EHR_API.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EHR_API.Controllers
{
    [Route("api/AllergyAPI")]
    [ApiController]
    public class AllergyAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly IMainRepository _db;

        public AllergyAPIController(IMainRepository db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }


        //[Authorize]
        [HttpGet("{id}")]
        [ResponseCache(CacheProfileName = SD.ProfileName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetUserAllergies(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(APIResponses.BadRequest("Id is null"));
                }

                var entities = await _db._allergy.GetAllAsync(
                    expression: g => g.RegistrationDataId == id);
                if (entities == null)
                {
                    return BadRequest(APIResponses.BadRequest($"No objects with Id = {id} "));
                }

                _response.Result = _mapper.Map<List<AllergyDTO>>(entities);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return APIResponses.InternalServerError(ex);
            }
        }

        //[Authorize]
        [HttpGet("GetAllergy")]
        [ResponseCache(CacheProfileName = SD.ProfileName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllergy([FromQuery(Name = "id")] int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest(APIResponses.BadRequest("Id is less than 1"));
                }

                var entity = await _db._allergy.GetAsync(
                    expression: g => g.Id == id);

                if (entity == null)
                {
                    return BadRequest(APIResponses.BadRequest($"No object with Id = {id} "));
                }

                _response.Result = _mapper.Map<AllergyDTO>(entity);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return APIResponses.InternalServerError(ex);
            }
        }

        //[Authorize]
        [HttpPost("CreateAllergy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateAllergy([FromBody] AllergyCreateDTO entityCreateDTO)
        {
            try
            {
                if (entityCreateDTO == null)
                {
                    return BadRequest(APIResponses.BadRequest("No data has been sent"));
                }

                if (await _db._authentication.GetAsync(expression: e => e.Id == entityCreateDTO.RegistrationDataId) == null)
                {
                    return BadRequest(APIResponses.BadRequest("User is not exists"));
                }

                var entity = _mapper.Map<Allergy>(entityCreateDTO);
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;
                await _db._allergy.CreateAsync(entity);

                if (entityCreateDTO.AllergyDrugs != null)
                {
                    await _db._allergyDrug.CreateRangeAsync(
                        _mapper.Map<List<AllergyDrug>>(entityCreateDTO.AllergyDrugs.ToList()));
                }

                _response.Result = _mapper.Map<AllergyDTO>(entity);
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return APIResponses.InternalServerError(ex);
            }
        }


        //[Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> DeleteAllergy(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest(APIResponses.BadRequest("Id less than 1"));
                }

                var removedEntity = await _db._allergy.GetAsync(expression: g => g.Id == id);
                if (removedEntity == null)
                {
                    return NotFound(APIResponses.NotFound($"No object with Id = {id} "));
                }

                await _db._allergy.DeleteAsync(removedEntity);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "The object has been deleted";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return APIResponses.InternalServerError(ex);
            }
        }

        //[Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> UpdateAllergy(int id, [FromBody] AllergyUpdateDTO entityUpdateDTO)
        {
            try
            {
                if (entityUpdateDTO == null)
                {
                    return BadRequest(APIResponses.BadRequest("No data has been sent"));
                }

                if (id != entityUpdateDTO.Id)
                {
                    return BadRequest(APIResponses.BadRequest("Id is not equal to the Id of the object"));
                }

                if (await _db._allergy.GetAsync(expression: g => g.Id == id) == null)
                {
                    return NotFound(APIResponses.NotFound($"No object with Id = {id} "));
                }

                if (await _db._allergy.GetAsync(expression: e => e.RegistrationDataId == entityUpdateDTO.RegistrationDataId) == null)
                {
                    return BadRequest(APIResponses.BadRequest("User is not exists"));
                }

                var entity = _mapper.Map<Allergy>(entityUpdateDTO);
                entity.UpdatedAt = DateTime.Now;
                await _db._allergy.UpdateAsync(entity);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<AllergyDTO>(entity);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return APIResponses.InternalServerError(ex);
            }
        }
    }
}
