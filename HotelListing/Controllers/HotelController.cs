using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelListing.IRepository;
using HotelListing.Data;
using HotelListing.Models;

namespace HotelListing.Controllers;

[Route("api/[controller]")]
public class HotelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HotelController> _logger;
    private readonly IMapper _mapper;

    public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotels([FromQuery] RequestParams requestParams)
    {
        var hotels = await _unitOfWork.Hotels.GetAll(requestParams);
        var results = _mapper.Map<IList<HotelDTO>>(hotels);

        return Ok(results);
    }

    [HttpGet("{id:int}", Name = "GetHotel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string>() { "Country" });
        var result = _mapper.Map<HotelDTO>(hotel);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateHotel(int id, [FromBody] CreateHotelDTO hotelDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST request attempt in {this.GetType().Name}");
            return BadRequest(ModelState);
        }

        var hotel = _mapper.Map<Hotel>(hotelDTO);

        await _unitOfWork.Hotels.Insert(hotel);
        await _unitOfWork.Save();

        return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO updateHotelDto)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT request attempt in {this.GetType().Name}");
            return BadRequest();
        }

        var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);

        if (hotel == null)
        {
            _logger.LogError($"Invalid PUT request attempt in {this.GetType().Name}");
            return BadRequest("Submited data is invalid");
        }

        _mapper.Map(updateHotelDto, hotel);
        _unitOfWork.Hotels.Update(hotel);

        await _unitOfWork.Save();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE request attempt in {this.GetType().Name}");
            return BadRequest();
        }

        var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);

        if (hotel == null)
        {
            _logger.LogError($"Invalid DELETE request attempt in {this.GetType().Name}");
            return BadRequest("Submitted data is invalid.");
        }

        await _unitOfWork.Hotels.Delete(id);
        await _unitOfWork.Save();

        return NoContent();
    }
}