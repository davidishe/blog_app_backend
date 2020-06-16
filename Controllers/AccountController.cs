using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyAppBack.Dtos;
using MyAppBack.Errors;
using MyAppBack.Extensionss;
using MyAppBack.Identity;
using MyAppBack.Services.TokenService;

namespace MyAppBack.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    // 
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
    {
      _signInManager = signInManager;
      _tokenService = tokenService;
      _mapper = mapper;
      _userManager = userManager;
    }

    [HttpGet]
    [Authorize]
    [Route("current")]
    public async Task<ActionResult<UserToReturnDto>> GetCurrentUser()
    {
      var user = await _userManager.FindByClaimsCurrentUser(HttpContext.User);
      return new UserToReturnDto
      {
        Email = user.Email,
        Token = _tokenService.CreateToken(user),
        DisplayName = user.DisplayName
      };
    }

    [HttpGet]
    [Route("checkmail")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
      return await _userManager.FindByEmailAsync(email) != null;
    }


    [HttpGet]
    [Authorize]
    [Route("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {
      var user = await _userManager.FindByClaimsPrincipleUserWithAddressAsync(HttpContext.User);
      return _mapper.Map<Address, AddressDto>(user.Address);
    }

    [HttpPut]
    [Authorize]
    [Route("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
    {
      var user = await _userManager.FindByClaimsPrincipleUserWithAddressAsync(HttpContext.User);
      user.Address = _mapper.Map<AddressDto, Address>(address);
      var result = await _userManager.UpdateAsync(user);
      if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
      return BadRequest("Problem when updating the user");
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<ActionResult<UserToReturnDto>> Login(UserToLoginDto loginDto)
    {
      var user = await _userManager.FindByEmailAsync(loginDto.Email);
      if (user == null) return Unauthorized(new ApiResponse(401));

      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
      if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

      return new UserToReturnDto
      {
        Email = user.Email,
        Token = _tokenService.CreateToken(user),
        DisplayName = user.DisplayName
      };
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<ActionResult<UserToReturnDto>> Register(UserToRegisterDto registerDto)
    {
      if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
      {
        return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Пользователь уже существует" } });
      }

      var user = new AppUser
      {
        DisplayName = registerDto.DisplayName,
        Email = registerDto.Email,
        UserName = registerDto.Email
      };

      var result = await _userManager.CreateAsync(user, registerDto.Password);
      if (!result.Succeeded) return BadRequest(new ApiResponse(400));

      return new UserToReturnDto
      {
        Email = user.Email,
        Token = _tokenService.CreateToken(user),
        DisplayName = user.DisplayName
      };

    }

  }
}