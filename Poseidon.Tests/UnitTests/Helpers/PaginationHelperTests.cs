using Poseidon.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Poseidon.Tests.UnitTests.Helpers
{
    public class PaginationHelperTests
    {
        [Fact]
        public void Paginate_ReturnsCorrectNumberOfItemsForFirstPage()
        {
            // Arrange
            var data = Enumerable.Range(1, 100).AsQueryable();
            var page = 1;
            var pageSize = 10;

            // Act
            var result = PaginationHelper.Paginate(data, page, pageSize);

            // Assert
            Assert.Equal(pageSize, result.Count());
            Assert.Equal(Enumerable.Range(1, pageSize), result);
        }

        [Fact]
        public void Paginate_ReturnsCorrectItemsForSecondPage()
        {
            // Arrange
            var data = Enumerable.Range(1, 100).AsQueryable();
            var page = 2;
            var pageSize = 10;

            // Act
            var result = PaginationHelper.Paginate(data, page, pageSize);

            // Assert
            Assert.Equal(pageSize, result.Count());
            Assert.Equal(Enumerable.Range(11, pageSize), result);
        }

        [Fact]
        public void Paginate_ReturnsEmptyWhenPageExceedsDataSize()
        {
            // Arrange
            var data = Enumerable.Range(1, 10).AsQueryable();
            var page = 3;
            var pageSize = 10;

            // Act
            var result = PaginationHelper.Paginate(data, page, pageSize);

            // Assert
            Assert.Empty(result);
        }
    }
}
