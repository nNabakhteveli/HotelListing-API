using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;

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
    public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
    {
        var countries = await _unitOfWork.Countries.GetAll(requestParams);
        var results = _mapper.Map<IList<CountryDTO>>(countries);

        return Ok(results);
    }

    [HttpGet("{id:int}", Name = "GetCountry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountry(int id)
    {
        var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string>() { "Hotels" });
        var result = _mapper.Map<CountryDTO>(country);

        return Ok(result);
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

        var country = _mapper.Map<Country>(createCountryDto);

        await _unitOfWork.Countries.Insert(country);
        await _unitOfWork.Save();

        return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
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
}