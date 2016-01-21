using System;
using VaultSharp.Infrastructure.Validation;
using Xunit;

namespace VaultSharp.UnitTests.Infrastructure.Validation
{
    public class CheckerTests
    {
        [Fact]
        public void NotNullShouldThrowForObjects()
        {
            var paramName = "client";

            IVaultClient client = null;
            var exception = Assert.Throws<ArgumentNullException>(() => Checker.NotNull(client, paramName));

            Assert.Equal(paramName, exception.ParamName);
        }

        [Fact]
        public void NotNullShouldThrowForStrings()
        {
            var paramName = "someString";

            var exception = Assert.Throws<ArgumentNullException>(() => Checker.NotNull(null, paramName));
            Assert.Equal(paramName, exception.ParamName);

            var argumentException = Assert.Throws<ArgumentException>(() => Checker.NotNull(string.Empty, paramName));
            Assert.Equal(paramName, argumentException.ParamName);

            argumentException = Assert.Throws<ArgumentException>(() => Checker.NotNull("  ", paramName));
            Assert.Equal(paramName, argumentException.ParamName);
        }
    }
}