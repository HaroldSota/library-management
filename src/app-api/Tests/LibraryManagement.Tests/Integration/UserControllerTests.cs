using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryManagement.Application;
using LibraryManagement.Application.Model.UserModel;
using LibraryManagement.Tests.Infrastructure;
using Xunit;

namespace LibraryManagement.Tests.Integration
{
    [CollectionDefinition("UserControllerTests", DisableParallelization = true)]
    public sealed class UserControllerTests : ControllerTestBase
    {
        public UserControllerTests()
            : base(new TestServerFixture(), "/api/User/")
        {
            TestDbContext.Seed();
        }

        [Fact]
        public async Task Authenticate_Correct_Credentials()
        {
            //Act
            var response = await PostAsync("Authenticate",
                new AuthenticateRequest
                {
                     Username = "admin@gmail.com",
                     Password = "1234567890"
                });

            //Assert
            var result = response.GetObject<AuthenticateResponse>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);


            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.FirstName.Should().Be("Admin");
            result.LastName.Should().Be("Admin");
            result.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Negative_Authenticate_Wrong_Credentials()
        {
            //Act
            var response = await PostAsync("authenticate",
                new AuthenticateRequest
                {
                    Username = "admin@gmail.com",
                    Password = "admin"
                });

            //Assert
            var result = response.GetObject<MessageResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result.Message.Should().Be("Username and/or password not correct!");
        }

        [Fact]
        public async Task Negative_Authenticate_Empty_Username()
        {
            //Act
            var response = await PostAsync("authenticate",
                new AuthenticateRequest
                {
                    Username = "",
                    Password = "password"
                });

            //Assert
            var result = response.GetObject<MessageResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result.Message.Should().Be("Username can not be empty");
        }

        [Fact]
        public async Task Negative_Authenticate_Empty_Password()
        {
            //Act
            var response = await PostAsync("authenticate",
                new AuthenticateRequest
                {
                    Username = "admin@gmail.com",
                    Password = ""
                });

            //Assert
            var result = response.GetObject<MessageResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result.Message.Should().Be("Password can not be empty");
        }

        [Fact]
        public async Task Negative_Authenticate_User_Not_Found()
        {
            //Act
            var response = await PostAsync("authenticate",
                new AuthenticateRequest
                {
                    Username = "admin2@gmail.com",
                    Password = "admin"
                });

            //Assert
            var result = response.GetObject<MessageResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Should().NotBeNull();
            result.Message.Should().Be("User not found!");
        }
    }
}