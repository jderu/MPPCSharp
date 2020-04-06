using System;

namespace model {
    [Serializable]public class Trip : Entity<int> {
        public DateTime Departure { get; set; }

        public int FreeSeats { get; set; }

        public int DestinationId { get; set; }

        public Trip(int id, int destinationId, DateTime departure, int freeSeats) : base(id) {
            DestinationId = destinationId;
            Departure = departure;
            FreeSeats = freeSeats;
        }

        protected bool Equals(Trip other) { return Departure.Equals(other.Departure) && DestinationId == other.DestinationId; }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Trip) obj);
        }

        public override int GetHashCode() { return HashCode.Combine(Departure, DestinationId); }
    }
}