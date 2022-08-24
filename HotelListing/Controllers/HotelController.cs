using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models.Hotel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetHotels()
    {
        try
        {
            var hotels = await _unitOfWork.Hotels.GetAll();
            var results = _mapper.Map<IList<HotelDTO>>(hotels);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {this.GetType().Name} method...");
            return StatusCode(500, "Internal Server Error.");
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotel(int id)
    {
        try
        {
            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string>() { "Country" });
            var result = _mapper.Map<HotelDTO>(hotel);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {this.GetType().Name} method...");
            return StatusCode(500, "Internal Server Error.");
        }
    }
}