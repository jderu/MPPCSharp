namespace MPPCSharp.domain {
    public class Destination:Entity<int> {
        public string Name { get; set; }

        public Destination(int id, string name) : base(id) { Name = name; }
    }
}