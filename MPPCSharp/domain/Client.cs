namespace MPPCSharp.domain {
    public class Client:Entity<int> {
        public string Name { get; set; }

        public Client(int id, string name) : base(id) { Name = name; }
    }
}