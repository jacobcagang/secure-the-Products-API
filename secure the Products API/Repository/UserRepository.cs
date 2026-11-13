
public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new();

    public Task<User?> GetByUsernameAsync(string username)
    {
        var user = _users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(user);
    }

    public Task<User> AddAsync(User user)
    {
        _users.Add(user);
        return Task.FromResult(user);
    }
}
