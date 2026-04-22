using Vp.Models;

namespace Vp.Interfaces;

public interface IAuthStateService
{
    Person? CurrentUser { get; }
    event Action? AuthStateChanged;
    bool Login(string email, string password);
    void Logout();
}
