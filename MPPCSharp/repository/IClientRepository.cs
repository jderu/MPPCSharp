using MPPCSharp.domain;

namespace MPPCSharp.repository {
    public interface IClientRepository : ICrudRepository<int, Client> {
        public Client FindByName(string name);
    }
}