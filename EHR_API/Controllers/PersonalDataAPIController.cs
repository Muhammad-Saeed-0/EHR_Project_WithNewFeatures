﻿using AutoMapper;
using EHR_API.Entities;
using EHR_API.Entities.DTOs.UserDataDTOs.PersonalDataDTOs;
using EHR_API.Entities.Models.UsersData;
using EHR_API.Extensions;
using EHR_API.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EHR_API.Controllers
{
    //[Authorize]
    [Route("api/PersonalDataAPI")]
    [ApiController]
    public class PersonalDataAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly IMainRepository _db;

        public PersonalDataAPIController(IMainRepository db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }

        [HttpPost("CreateUserPersonalData")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateUserPersonalData([FromForm] PersonalDataCreateDTO entityCreateDTO)
        {
            try
            {
                if (entityCreateDTO == null)
                {
                    return BadRequest(APIResponses.BadRequest("No data has been sent"));
                }

                if (await _db._governorate.GetAsync(expression: e => e.Id == entityCreateDTO.GovernorateId) == null)
                {
                    return BadRequest(APIResponses.BadRequest("Governorate is not exists"));
                }

                if (await _db._personal.GetAsync(expression: g => g.Id.ToLower() == entityCreateDTO.Id.ToLower()) != null)
                {
                    return BadRequest(APIResponses.BadRequest("The object is already exists"));
                }

                var entity = _mapper.Map<PersonalData>(entityCreateDTO);
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;
                await _db._personal.CreateAsync(entity);

                _response.Result = _mapper.Map<PersonalDataDTO>(entity);
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return APIResponses.InternalServerError(ex);
            }
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> DeletePersonalData(string userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest(APIResponses.BadRequest("Id is null"));
                }

                var removedEntity = await _db._personal.GetAsync(expression: g => g.Id == userId);
                if (removedEntity == null)
                {
                    return NotFound(APIResponses.NotFound($"No object with Id = {userId} "));
                }

                await _db._personal.DeleteAsync(removedEntity);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "The object has been deleted";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return APIResponses.InternalServerError(ex);
            }
        }


        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> UpdateUserPersonalData(string userId, [FromForm] PersonalDataUpdateDTO entityUpdateDTO)
        {
            try
            {
                if (entityUpdateDTO == null)
                {
                    return BadRequest(APIResponses.BadRequest("No data has been sent"));
                }

                if (userId != entityUpdateDTO.Id)
                {
                    return BadRequest(APIResponses.BadRequest("Id is not equal to the Id of the object"));
                }

                var oldEntity = await _db._personal.GetAsync(expression: g => g.Id == userId);
                if (oldEntity == null)
                {
                    return NotFound(APIResponses.NotFound($"No object with Id = {userId} "));
                }

                var entity = _mapper.Map<PersonalData>(entityUpdateDTO);
                entity.UpdatedAt = DateTime.Now;
                await _db._personal.UpdateAsync(entity, oldEntity);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<PersonalDataDTO>(entity);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return APIResponses.InternalServerError(ex);
            }
        }
    }
}
