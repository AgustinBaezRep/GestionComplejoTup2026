using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface IAuthService
    {
        AuthResponse? SignUp(SignUpRequest request);
        AuthResponse? SignIn(SignInRequest request);
    }
}
