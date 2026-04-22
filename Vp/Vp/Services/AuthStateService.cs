using Vp.Interfaces;
using Vp.Models;

namespace Vp.Services;

public sealed class AuthStateService(DataStore store) : IAuthStateService
{
    public Person? CurrentUser { get; private set; }
    public event Action? AuthStateChanged;

    public bool Login(string email, string password)
    {
        var user = store.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        if (user is null) return false;
        CurrentUser = user;
        AuthStateChanged?.Invoke();
        return true;
    }

    public void Logout()
    {
        CurrentUser = null;
        AuthStateChanged?.Invoke();
    }
}
