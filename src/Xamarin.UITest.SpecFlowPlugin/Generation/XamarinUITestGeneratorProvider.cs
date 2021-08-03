using System;
using System.CodeDom;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.CodeDom;
using TechTalk.SpecFlow.Generator.UnitTestProvider;

namespace Xamarin.UITest.SpecFlowPlugin
{
    public class XamarinUITestGeneratorProvider : NUnit3TestGeneratorProvider, IUnitTestGeneratorProvider
    {
        public XamarinUITestGeneratorProvider(CodeDomHelper codeDomHelper)
            : base(codeDomHelper)
        {
        }

        public override void FinalizeTestClass(TestClassGenerationContext generationContext)
        {
            // Declare platform field
            DeclareField(generationContext.TestClass, typeof(Platform), Constants.PLATFORM_FIELD);

            // Create constructor
            AddConstructor(generationContext.TestClass);

            // Add AppManager.Start call in Setup:
            AddMethodStatement(generationContext.TestInitializeMethod,
                Constants.APPMANAGER,
                Constants.APPMANAGER_START,
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), Constants.PLATFORM_FIELD));

            // Add AppManager.Shutdown call in TearDown:
            AddMethodStatement(generationContext.TestCleanupMethod,
                Constants.APPMANAGER,
                Constants.APPMANAGER_SHUTDOWN);

            base.FinalizeTestClass(generationContext);
        }

        private void DeclareField(CodeTypeDeclaration classDeclaration, Type type, string value)
        {
            classDeclaration.Members.Add(new CodeMemberField(type, value));
        }

        private void AddConstructor(CodeTypeDeclaration classDeclaration)
        {
            CodeConstructor constructor = new CodeConstructor()
            {
                Attributes = MemberAttributes.Public
            };

            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Platform), Constants.PLATFORM_PARAMETER));

            // Assign constructor parameter to field
            constructor.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), Constants.PLATFORM_FIELD),
                new CodeVariableReferenceExpression(Constants.PLATFORM_PARAMETER)));

            classDeclaration.Members.Add(constructor);
        }

        private void AddMethodStatement(CodeMemberMethod method, string typeName, string methodName, params CodeExpression[] parameters)
        {
            method.Statements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(typeName),
                methodName,
                parameters));
        }
    }
}
