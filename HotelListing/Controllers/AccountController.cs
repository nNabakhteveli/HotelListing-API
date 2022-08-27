using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HotelListing.Models.User;
using HotelListing.Data;
using HotelListing.IRepository;

namespace HotelListing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountController> _logger;
    private readonly IMapper _mapper;

    public AccountController(UserManager<ApiUser> userManager, ILogger<AccountController> logger, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, $"Something went wrong in the {this.GetType().Name}");
            return Problem($"Something went wrong in the {this.GetType().Name}", statusCode: 500);
        }
    }
}