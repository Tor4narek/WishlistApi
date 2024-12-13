using System.Data.Common;

namespace Models
{

    public class Present
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string WishlistId { get; init; }
        public bool IsReserved { get; init; }
        public string ReserverId { get; init; }

        public Present(string Id, string Name, string Description, string WishlistId, bool IsReserved,
            string ReserverId)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.WishlistId = WishlistId;
            this.IsReserved = IsReserved;
            this.ReserverId = ReserverId;

        }

        public Present()
        {

        }
    }
}