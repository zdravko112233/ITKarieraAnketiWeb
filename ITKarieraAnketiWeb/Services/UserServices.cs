using ITKarieraAnketiWeb.Database;
using ITKarieraAnketiWeb.Repository;
using ITKarieraAnketiWeb.Entities;

namespace ITKarieraAnketiWeb.Services
{
    public class UserServices
    {
        private readonly UserRepository _userRepository;
        public UserServices(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void Register(string username, string password)
        {
            var user = new User
            {
                Name = username,
                Password = password
            };
            _userRepository.AddUser(user);
        }
    }
}
