// Overload.Api/Services/DevCurrentUser.cs
namespace Overload.Api.Services;

public class DevCurrentUser : ICurrentUser
{
    public Guid Id { get; } = Guid.Parse("11111111-1111-1111-1111-111111111111");
}
