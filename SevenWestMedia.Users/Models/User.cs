namespace SevenWestMediaTechInterview.Models
{
    /// <summary>
    /// A user.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }

        public string FullName => $"{GivenName} {Surname}";
    }
}
