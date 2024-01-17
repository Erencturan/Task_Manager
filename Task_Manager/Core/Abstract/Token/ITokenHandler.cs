using Task_Manager.Infrastructure.Models;

namespace Task_Manager.Core.Abstract.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(int minute, User appUser);
    }
}
