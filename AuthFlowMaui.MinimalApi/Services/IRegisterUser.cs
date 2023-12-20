using AuthFlowMaui.Shared.Dtos;

namespace AuthFlowMaui.MinimalApi.Services
{
    public interface IRegisterUser
    {
        Task<IResult> Register( RegisterUserDto keycloakRegisterUserDto, CancellationToken cancellationToken);
    }
}