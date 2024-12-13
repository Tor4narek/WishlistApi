namespace Models
{

    public class Wishlist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }

        public string
            PresentsNumber { get; set; }

        // Параметризованный конструктор, который ожидает Dapper
        public Wishlist(string id, string name, string description, string ownerId, string presentsNumber)
        {
            Id = id;
            Name = name;
            Description = description;
            OwnerId = ownerId;
            PresentsNumber = presentsNumber;
        }

        // Пустой конструктор для случаев, когда он необходим
        public Wishlist()
        {
        }
    }
}