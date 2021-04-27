using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SevenWestMediaTechInterview.Data;
using SevenWestMediaTechInterview.Models;

namespace SevenWestMediaTechInterview.UnitTests
{
    public class UserServiceTests
    {
        private List<User> _users = new List<User>()
        {
            new User() {Id = 1, Age = 21, Gender = "M", GivenName = "First", Surname = "Last"},
            new User() {Id = 2, Age = 21, Gender = "F", GivenName = "Second", Surname = "Last"},
            new User() {Id = 3, Age = 21, Gender = "M", GivenName = "Third", Surname = "Last"},
            new User() {Id = 4, Age = 23, Gender = "M", GivenName = "Fourth", Surname = "Last"},
        };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetUsers_GivenSearchPredicate_FindsMatchingUsers()
        {
            Mock<IUserRepository> repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetUsers()).ReturnsAsync(_users);
            var userService = new UserService(repository.Object);

            var results = await userService.GetUsers(user => user.Id == 1);

            Assert.IsTrue(results.Count() == 1);
            Assert.AreSame(results.First(), _users.First());
        }


        [Test]
        public async Task GetUserGenderCountsByAge_GivenMixedAges_CorrectSums()
        {
            Mock<IUserRepository> repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetUsers()).ReturnsAsync(_users);
            var userService = new UserService(repository.Object);

            var results = await userService.GetUserGenderCountsByAge();

            Assert.IsTrue(results.Count() == 2);
            Assert.IsTrue(results[21]["M"] == 2 && results[21]["F"] == 1);
            Assert.IsTrue(results[23]["M"] == 1);
        }

        

    }
}