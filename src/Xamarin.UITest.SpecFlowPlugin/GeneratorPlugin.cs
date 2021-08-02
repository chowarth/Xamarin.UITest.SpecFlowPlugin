using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestConverter;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.UnitTestProvider;

[assembly: GeneratorPlugin(typeof(Xamarin.UITest.SpecFlowPlugin.GeneratorPlugin))]

namespace Xamarin.UITest.SpecFlowPlugin
{
    public class GeneratorPlugin : IGeneratorPlugin
    {
        public void Initialize(GeneratorPluginEvents generatorPluginEvents, GeneratorPluginParameters generatorPluginParameters, UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            generatorPluginEvents.CustomizeDependencies += CustomizeDependencies;
        }

        private void CustomizeDependencies(object sender, CustomizeDependenciesEventArgs args)
        {
            args.ObjectContainer.RegisterTypeAs<XamarinUITestGeneratorProvider, IUnitTestGeneratorProvider>();
            args.ObjectContainer.RegisterTypeAs<XamarinUITestFeatureDecorator, ITestClassTagDecorator>(Constants.UITEST_CATEGORY);
        }
    }
}
