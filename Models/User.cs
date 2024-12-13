namespace Models
{
    public class User
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string PasswordHash { get; init; }

        // Добавляем конструктор без параметров
        public User()
        {
        }

        // Конструктор с параметрами
        public User(string id, string name, string email, string passwordHash)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}