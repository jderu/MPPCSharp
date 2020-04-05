using MPPCSharp.domain;

namespace MPPCSharp.repository {
    public interface IUserRepository : ICrudRepository<int, User> {
        public User FindByUsername(string username);
    }
}