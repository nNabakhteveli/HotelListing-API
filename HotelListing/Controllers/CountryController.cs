using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CountryController> _logger;
    private readonly IMapper _mapper;

    public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountries()
    {
        try
        {
            var countries = await _unitOfWork.Countries.GetAll();
            var results = _mapper.Map<IList<CountryDTO>>(countries);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {this.GetType().Name} method...");
            return StatusCode(500, "Internal Server Error.");
        }
    }

    [HttpGet("{id:int}", Name = "GetCountry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountry(int id)
    {
        try
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string>() { "Hotels" });
            var result = _mapper.Map<CountryDTO>(country);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {this.GetType().Name} method...");
            return StatusCode(500, "Internal Server Error.");
        }
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO createCountryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            var country = _mapper.Map<Country>(createCountryDto);

            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError($"Something went wrong in {this.GetType().Name}");
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO updateCountryDto)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT request attempt in {this.GetType().Name}");
            return BadRequest();
        }

        try
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);

            if (country == null)
            {
                _logger.LogError($"Invalid PUT request attempt in {this.GetType().Name}");
                return BadRequest("Submited data is invalid");
            }

            _mapper.Map(updateCountryDto, country);
            _unitOfWork.Countries.Update(country);

            await _unitOfWork.Save();

            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError($"Something went wrong in {this.GetType().Name}");
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE request attempt in {this.GetType().Name}");
        }

        try
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);

            if (country == null)
            {
                _logger.LogError($"Invalid DELETE request attempt in {this.GetType().Name}");
                return BadRequest("Submitted data is invalid.");
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();

            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine(e);
            _logger.LogError($"Something went wrong in {this.GetType().Name}");
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }
}