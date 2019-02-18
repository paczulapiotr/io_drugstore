using Drugstore.Core;
using Drugstore.Data;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Mapper;
using Drugstore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Tests.UseCases
{
    public class AdminTests
    {
        private readonly DbContextOptions<DrugstoreDbContext> options;
        private AdminUseCase useCase;
        private DrugstoreDbContext context;
        public AdminTests()
        {
            options = new DbContextOptionsBuilder<DrugstoreDbContext>()
               .UseInMemoryDatabase(databaseName: "Drugstore_Admin").Options;

            var userStoreMock = new Mock<IUserStore<SystemUser>>();
            var userManagerMock = new Mock<UserManager<SystemUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                    new Mock<IRoleStore<IdentityRole>>().Object,
                    new IRoleValidator<IdentityRole> [0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var passwordHasherMock = new Mock<IPasswordHasher<SystemUser>>();
            passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<SystemUser>(), It.IsAny<string>()))
                .Returns("password");
            userManagerMock.Object.PasswordHasher = passwordHasherMock.Object;

            userManagerMock.Setup(u => u.CreateAsync(It.IsAny<SystemUser>()))
                .Returns(Task<IdentityResult>.FromResult(new IdentityResult()));

            userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<SystemUser>(), It.IsAny<string>()))
                .Returns(Task<IdentityResult>.FromResult(new IdentityResult()));

            userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<SystemUser>(new SystemUser()));

            userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<SystemUser>()))
                .Returns(Task.FromResult<IList<string>>(new string [] { "Admin" }));

            userManagerMock.Setup(u => u.DeleteAsync(It.IsAny<SystemUser>()))
                .Returns(Task.FromResult<IdentityResult>(new IdentityResult()));

            context = new DrugstoreDbContext(options);

            useCase = new AdminUseCase(userManagerMock.Object, roleManagerMock.Object, context);
        }

        [SetUp]
        public void SetUp()
        {
            MapperDependencyResolver.Resolve();
        }

        [TearDown]
        public void TearDown()
        {
            AutoMapper.Mapper.Reset();
        }



        [Order(1)]
        [Test]
        public void Should_Add_User()
        {
            // given
            var department = new Department { Name = "Oddział Testowy" };
            context.Departments.Add(department);
            context.SaveChanges();
            var newUser = new UserViewModel
            {
                DepartmentID = department.ID,
                Email = "testowy@email.com",
                ConfirmPassword = "haslo123",
                Password = "haslo123",
                Role = UserRoleTypes.Admin,
                FirstName = "Testowy",
                SecondName = "User",
                PhoneNumber = "555666777",
                UserName = "testowyuser"
            };
            var expectedUserCount = context.Admins.Count() + 1;

            // when
            useCase.AddNewUser(newUser);

            // then
            Assert.AreEqual(expectedUserCount, context.Admins.Count());
            Assert.IsNotNull(context.Admins.FirstOrDefault(a => a.FirstName == newUser.FirstName));
        }




        [Order(2)]
        [Test]
        public void Should_Delete_User()
        {
            // given
            var expectedResultAdmins = context.Admins.Count() - 1;
            var admin = context.Admins.First();

            // when
            useCase.DeleteUser(admin.SystemUser.Id);

            // then
            Assert.AreEqual(expectedResultAdmins, context.Admins.Count());
        }



        [Order(3)]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
