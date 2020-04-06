using model;

namespace persistence {
    public interface IClientRepository : ICrudRepository<int, Client> {
        public Client FindByName(string name);
    }
}