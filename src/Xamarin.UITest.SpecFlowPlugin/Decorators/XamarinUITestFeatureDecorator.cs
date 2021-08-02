using System;
using System.CodeDom;
using System.Linq;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.CodeDom;
using TechTalk.SpecFlow.Generator.UnitTestConverter;

namespace Xamarin.UITest.SpecFlowPlugin
{
    class XamarinUITestFeatureDecorator : ITestClassTagDecorator
    {
        private readonly CodeDomHelper _codeDomHelper;

        public int Priority
            => PriorityValues.Normal;

        public bool RemoveProcessedTags
            => true;

        public bool ApplyOtherDecoratorsForProcessedTags
            => false;

        public XamarinUITestFeatureDecorator(CodeDomHelper codeDomHelper)
        {
            _codeDomHelper = codeDomHelper;
        }

        public bool CanDecorateFrom(string tagName, TestClassGenerationContext generationContext)
        {
            return tagName.StartsWith(Constants.UITEST_CATEGORY, StringComparison.OrdinalIgnoreCase);
        }

        public void DecorateFrom(string tagName, TestClassGenerationContext generationContext)
        {
            CodeTypeDeclaration testClass = generationContext.TestClass;

            // Remove the default NUnit TestFixture attribute
            CodeAttributeDeclaration defaultAttribute = testClass.CustomAttributes.OfType<CodeAttributeDeclaration>()
                .FirstOrDefault(attr => attr.Name == Constants.TESTFIXTURE_ATTR);

            if (defaultAttribute != null)
                testClass.CustomAttributes.Remove(defaultAttribute);

            // Parse uitest tags into TestFixture attributes with parameters
            string[] tagParts = tagName.Split(new[] { Constants.UITEST_TAG_DELIMETER }, StringSplitOptions.RemoveEmptyEntries);

            string iOS = $"{Platform.iOS}";
            string Android = $"{Platform.Android}";

            // Just a uitest tag adds both iOS and Android
            if (tagParts.Length == 1)
            {
                AddTestFixtureAttributeWithPlatform(testClass, iOS);
                AddTestFixtureAttributeWithPlatform(testClass, Android);
            }
            else
            {
                string[] platforms = tagParts[1]?.Split(new[] { Constants.PLATFORM_DELIMETER }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var platform in platforms ?? Enumerable.Empty<string>())
                {
                    if (platform.Equals(iOS, StringComparison.OrdinalIgnoreCase))
                    {
                        AddTestFixtureAttributeWithPlatform(testClass, iOS);
                    }
                    else if (platform.Equals(Android, StringComparison.OrdinalIgnoreCase))
                    {
                        AddTestFixtureAttributeWithPlatform(testClass, Android);
                    }
                }
            }
        }

        private void AddTestFixtureAttributeWithPlatform(CodeTypeDeclaration codeTypeMember, string platform)
        {
            _codeDomHelper.AddAttribute(codeTypeMember, Constants.TESTFIXTURE_ATTR, new CodeAttributeArgument()
            {
                Value = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(Platform)), platform)
            });
        }
    }
}
