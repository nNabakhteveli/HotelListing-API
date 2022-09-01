using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HotelListing.Models;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Services;

namespace HotelListing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountController> _logger;
    private readonly IAuthManager _authManager;
    private readonly IMapper _mapper;

    public AccountController(UserManager<ApiUser> userManager, ILogger<AccountController> logger, IMapper mapper, IUnitOfWork unitOfWork, IAuthManager authManager)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _authManager = authManager;
    }

    [HttpGet]
    public async Task<IList<ApiUser>> GetUsers()
    {
        return await _unitOfWork.Users.GetAll();
    }
    
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userDTO)
    {
        _logger.LogInformation($"Registration attempt for {userDTO.Email}");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var user = _mapper.Map<ApiUser>(userDTO);
            user.UserName = userDTO.Email;
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRolesAsync(user, userDTO.Roles);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, $"Something went wrong in the {this.GetType().Name}");
            return Problem($"Something went wrong in the {this.GetType().Name}", statusCode: 500);
        }
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userDTO) 
    {
        _logger.LogInformation($"Login attempt for {userDTO.Email}");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (!await _authManager.ValidateUser(userDTO))
            {
                return Unauthorized();
            }

            return Accepted(new { Token = await _authManager.CreateToken() });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Something went wrong in the {this.GetType().Name}");
            return Problem($"Something went wrong in the {this.GetType().Name}", statusCode: 500);
        }
    }
}