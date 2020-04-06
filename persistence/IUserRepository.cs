using model;

namespace persistence {
    public interface IUserRepository : ICrudRepository<int, User> {
        public User FindByUsername(string username);
    }
}