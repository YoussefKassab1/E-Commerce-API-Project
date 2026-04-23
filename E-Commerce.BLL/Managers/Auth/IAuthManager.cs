using E_Commerce.Common;
using E_Commerce.DAL.Data.Models;

namespace E_Commerce.BLL
{
    public interface IAuthManager
    {
        Task<GeneralResult> RegisterAsync(RegisterDto registerDto);

        Task<GeneralResult<TokenDto>> LoginAsync(UserLoginDto userLoginDto);
    }
}
