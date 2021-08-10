# Xamarin.UITest.SpecFlowPlugin

Unofficial SpecFlow plugin for `Xamarin.UITest`. I decided to create this plugin to remove the need for boiler-plate code, [outlined here](https://github.com/xamarin-automation-service/uitest-specflow-example), when creating a new feature. This plugin will remove the need to:

- Have a `BaseTestFixture` class that is used to create and start `Xamarin.UITest`
- Manually creating a partial test fixture with the appropriate `NUnit.TestFixtureAttribute` for each desired platform

This is achieved by taking advantage of [SpecFlow feature tags](https://docs.specflow.org/projects/specflow/en/latest/Gherkin/Gherkin-Reference.html#tags) and replacing the default `NUnit.TestFixtureAttribute` on the generated test fixture.

## Getting Started

1. The first thing to do is to install the SpecFlow extension for Visual Studio:
    - [Windows](https://marketplace.visualstudio.com/items?itemName=TechTalkSpecFlowTeam.SpecFlowForVisualStudio)
    - [Mac](https://github.com/straighteight/SpecFlow-VS-Mac-Integration)
1. Create a new **Xamarin.UITest Cross-Platform Test Project**
1. Install the `SpecFlow.NUnit` NuGet package
1. Now you can install the `Xamarin.UITest.SpecFlowPlugin` package
1. Create an `AppManager` class at the **root** of your UI test project. This is going to be the bridge between the SpecFlow generated test fixture and `Xamarin.UITest`
    - Add the following two methods:

        ```csharp
        public static void Start(Xamarin.UITest.Platform platform)
        {
            // Configure Xamarin.UITest e.g.
            if(platform == Platform.Android)
                ConfigureApp.Android.StartApp()
            else
                ConfigureApp.iOS.StartApp();
        }
        
        public static void Shutdown()
        {
            // Run any shutdown logic required between scenarios
        }
        ```

1. Create a new feature file using the SpecFlow file template
1. Add a supported tag above the feature title. This is how `Xamarin.UITest.SpecFlowPlugin` will pick up on which platforms to add as test fixture attributes. The following tags are supported:

    - `@uitest` will add both `Platform.iOS` and `Platform.Android` as test fixtures
    - `@uitest:ios` will add `Platform.iOS` as a test fixture attribute
    - `@uitest:android` will add `Platform.Android` as a test fixture attribute
    - Any other tags will be treated as normal SpecFlow feature tags

    Example:

    ```text
    @uitest
    Feature: UITest category fixture generation 
        Simple calculator for adding two numbers

    Scenario: Add two numbers
        Given the first number is 50
        And the second number is 70
        When the two numbers are added
        Then the result should be 120
    ```
