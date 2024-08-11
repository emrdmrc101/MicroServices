using Shared.Objects.Identity;

namespace Shared.Interfaces;

public interface IUserClaimsService
{
    IdentityContext UserContext { get;  }
    void SetUserClaims(IdentityContext userClaims);
}