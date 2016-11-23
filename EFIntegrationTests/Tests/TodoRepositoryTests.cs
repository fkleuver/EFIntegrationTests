using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NodaTime;
using Xunit;
using Xunit.Abstractions;

namespace EFIntegrationTests.Tests
{
    public class TodoRepositoryTests : DbContextTestClass<TodoContext>
    {
        public TodoRepositoryTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Test()
        {
            // Arrange
            var ctx = CreateContext();
            var sut = new TodoRepository(ctx);
            var todo = new Todo {DateCreated = SystemClock.Instance.Now.Ticks, Text = Guid.NewGuid().ToString()};

            // Act
            var saved = await sut.Create(todo);

            // Assert
            saved.Id.Should().NotBe(0);

            // Act
            var result = await sut.GetAsync(query => query.Where(x => x.Id == saved.Id), CancellationToken.None);
            var existing = result.Single();

            // Assert
            existing.ShouldBeEquivalentTo(saved);

            // Act
            var oldText = existing.Text;
            existing.Text = Guid.NewGuid().ToString();
            var success = await sut.Update(existing);

            // Assert
            success.Should().BeTrue();
            result = await sut.GetAsync(query => query.Where(x => x.Id == saved.Id), CancellationToken.None);
            existing = result.Single();
            existing.Text.Should().NotBe(oldText);

            // Act
            success = await sut.Delete(existing);

            // Assert
            success.Should().BeTrue();
            result = await sut.GetAsync(query => query.Where(x => x.Id == saved.Id), CancellationToken.None);
            existing = result.FirstOrDefault();
            existing.Should().BeNull();

        }
    }
}
