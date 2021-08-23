using TechTalk.SpecFlow.Generator.CodeDom;
using TechTalk.SpecFlow.Generator.UnitTestConverter;
using Xunit;

namespace Xamarin.UITest.SpecFlowPlugin.UnitTests.Decorators
{
    public class XamarinUITestFeatureDecoratorTests
    {
        [Fact]
        public void Decorator_Priority_EqualsNormal()
        {
            // Arrange:
            // Act:
            var decorator = GetDecorator();

            // Assert:
            Assert.Equal(PriorityValues.Normal, decorator.Priority);
        }

        [Fact]
        public void Decorator_RemoveProcessedTags_EqualsTrue()
        {
            // Arrange:
            // Act:
            var decorator = GetDecorator();

            // Assert:
            Assert.True(decorator.RemoveProcessedTags);
        }

        [Fact]
        public void Decorator_ApplyOtherDecoratorsForProcessedTags_EqualsFalse()
        {
            // Arrange:
            // Act:
            var decorator = GetDecorator();

            // Assert:
            Assert.False(decorator.ApplyOtherDecoratorsForProcessedTags);
        }

        [Theory]
        [InlineData("uitest")]
        [InlineData("uitest:ios")]
        [InlineData("uitest:android")]
        public void CanDecorateFrom_ValidTagName_ReturnsTrue(string tagName)
        {
            // Arrange:
            var decorator = GetDecorator();

            // Act:
            bool result = decorator.CanDecorateFrom(tagName, null);

            // Assert:
            Assert.True(result);
        }

        [Theory]
        [InlineData("tag")]
        [InlineData("ios")]
        [InlineData("android")]
        public void CanDecorateFrom_InvalidTagName_ReturnsFalse(string tagName)
        {
            // Arrange:
            var decorator = GetDecorator();

            // Act:
            bool result = decorator.CanDecorateFrom(tagName, null);

            // Assert:
            Assert.False(result);
        }

        private XamarinUITestFeatureDecorator GetDecorator()
            => new XamarinUITestFeatureDecorator(new CodeDomHelper(CodeDomProviderLanguage.CSharp));
    }
}
