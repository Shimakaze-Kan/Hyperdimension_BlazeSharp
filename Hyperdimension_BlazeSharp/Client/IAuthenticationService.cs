using Hyperdimension_BlazeSharp.Shared.Dto;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client
{
    public interface IAuthenticationService
    {
        Task<UserAuthResult> Login(UserAuthRequest userAuthRequest);
        Task Logout();
    }
}