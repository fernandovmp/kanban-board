using FluentAssertions;
using KanbanBoard.WebApi.Configurations;
using KanbanBoard.WebApi.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.Services
{
    [Trait("Category", "PasswordHasher")]
    public class PasswordHasherSerivceTests
    {
        [Fact]
        public void HashShouldBeComposedOfThreePartsSeparatedByDot()
        {
            IOptions<HasherOptions> options = Options.Create(new HasherOptions
            {
                Iterations = 100
            });
            string passwordToHash = "secret";
            var passwordHasher = new PasswordHasherService(options);

            string hash = passwordHasher.Hash(passwordToHash);

            string[] result = hash.Split('.');

            result.Should().HaveCount(3);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void HashFirstPartShouldBeTheNumberOfIterations(int iterations)
        {
            IOptions<HasherOptions> options = Options.Create(new HasherOptions
            {
                Iterations = iterations
            });
            string passwordToHash = "secret";
            var passwordHasher = new PasswordHasherService(options);

            string hash = passwordHasher.Hash(passwordToHash);

            string[] result = hash.Split('.');
            int hashIterations = int.Parse(result[0]);

            hashIterations.Should().Be(iterations);
        }

        [Theory]
        [InlineData("secret", "1000.bb8RbPwW9wqrhZ5BlBNcYg==.bujp0p65QugZ/Jfx/R/rZlomCh4Y/8Qhq0XiPhfAKSM=")]
        [InlineData("password", "1000.gNl25goHzd+vA8JrsXXZOA==.hQOq0NrnxGdDRWJKjN40dFU6G4nHOE9QN3sgvi3ZG90=")]
        [InlineData("password1234", "1000.uy5NG7FwrO6K7jhFvSNaYw==.pYhcWf8tOa6Yh7eyK8E8JnYs7KTjEbHV6nlA+SPAGDI=")]
        [InlineData("$password$", "1000.nJVQphyuoBkxQ6n9ZL0KBA==.iL/c26zw9LEoHrLa6PbgdnmSg1HdDWOahI/sOd4xk4Y=")]
        public void ShouldValidatePassword(string password, string passwordHash)
        {
            IOptions<HasherOptions> options = Options.Create(new HasherOptions
            {
                Iterations = 1000
            });
            var passwordHasher = new PasswordHasherService(options);

            bool validPassword = passwordHasher.VerifyPassword(passwordHash, password);

            validPassword.Should().BeTrue();
        }

        [Theory]
        [InlineData("password", "1000.bb8RbPwW9wqrhZ5BlBNcYg==.bujp0p65QugZ/Jfx/R/rZlomCh4Y/8Qhq0XiPhfAKSM=")]
        [InlineData("password1234", "1000.gNl25goHzd+vA8JrsXXZOA==.hQOq0NrnxGdDRWJKjN40dFU6G4nHOE9QN3sgvi3ZG90=")]
        [InlineData("hash", "1000.uy5NG7FwrO6K7jhFvSNaYw==.pYhcWf8tOa6Yh7eyK8E8JnYs7KTjEbHV6nlA+SPAGDI=")]
        [InlineData("$secret$", "1000.nJVQphyuoBkxQ6n9ZL0KBA==.iL/c26zw9LEoHrLa6PbgdnmSg1HdDWOahI/sOd4xk4Y=")]
        public void ShouldReturnFalseWhenValidateThePassword(string password, string passwordHash)
        {
            IOptions<HasherOptions> options = Options.Create(new HasherOptions
            {
                Iterations = 1000
            });
            var passwordHasher = new PasswordHasherService(options);

            bool validPassword = passwordHasher.VerifyPassword(passwordHash, password);

            validPassword.Should().BeFalse();
        }

        [Theory]
        [InlineData("secret")]
        [InlineData("password")]
        [InlineData("password1234")]
        [InlineData("$password$")]
        [InlineData("hash")]
        [InlineData("$secret$")]
        public void ShouldValidatePasswordInFullCycle(string password)
        {
            IOptions<HasherOptions> options = Options.Create(new HasherOptions
            {
                Iterations = 1000
            });
            var passwordHasher = new PasswordHasherService(options);

            string hash = passwordHasher.Hash(password);
            bool validPassword = passwordHasher.VerifyPassword(hash, password);

            validPassword.Should().BeTrue();
        }
    }
}
