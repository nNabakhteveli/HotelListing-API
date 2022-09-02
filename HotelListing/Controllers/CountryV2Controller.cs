using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;

namespace HotelListing.Controllers;

// =========== This controller is just for testing versioning ===========
[ApiVersion("2.0")]
[ApiController]
[Route("api/{v:apiVersion}/country")]
public class CountryV2Controller : ControllerBase
{
    private readonly DatabaseContext _context;

    public CountryV2Controller(DatabaseContext databaseContext)
    {
        _context = databaseContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountries() => Ok(_context.Countries);
}