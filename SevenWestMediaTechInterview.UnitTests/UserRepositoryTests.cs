using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SevenWestMediaTechInterview.Client;
using SevenWestMediaTechInterview.Client.Dto;
using SevenWestMediaTechInterview.Configuration;
using SevenWestMediaTechInterview.Data;

namespace SevenWestMediaTechInterview.UnitTests
{
    public class UserRepositoryTests
    {
        private List<User> _users = new List<User>()
        {
            new User() {Id = 1, Age = 21, Gender = "M", GivenName = "First", Surname = "Last"},
            new User() {Id = 2, Age = 21, Gender = "F", GivenName = "Second", Surname = "Last"},
            new User() {Id = 3, Age = 21, Gender = "M", GivenName = "Third", Surname = "Last"},
        };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetUser_UserIdExists_ReturnsUser()
        {
            Mock<IUserHttpClient> userHttpClient = new Mock<IUserHttpClient>();
            userHttpClient.Setup(client => client.GetUsers()).ReturnsAsync(_users);

            var repository = new UserRepository(userHttpClient.Object, new Mapper(new MapperConfiguration(m => m.AddProfile(typeof(UsersAutoMapperProfile)))));

            var targetId = 1;
            var user = await repository.GetUser(targetId);

            Assert.IsTrue(user.Id == targetId);
        }

        [Test]
        public async Task GetUser_UserIdDoesNotExist_ReturnsNull()
        {
            Mock<IUserHttpClient> userHttpClient = new Mock<IUserHttpClient>();
            userHttpClient.Setup(client => client.GetUsers()).ReturnsAsync(_users);

            var repository = new UserRepository(userHttpClient.Object, new Mapper(new MapperConfiguration(m => m.AddProfile(typeof(UsersAutoMapperProfile)))));

            var targetId = 0;
            var user = await repository.GetUser(targetId);

            Assert.IsNull(user);
        }

    }
}