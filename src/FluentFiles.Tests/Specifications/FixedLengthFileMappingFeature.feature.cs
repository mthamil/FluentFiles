// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace FluentFiles.Tests.Specifications
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class FixedLengthFileMappingFeatureFeature : Xunit.IClassFixture<FixedLengthFileMappingFeatureFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "FixedLengthFileMappingFeature.feature"
#line hidden
        
        public FixedLengthFileMappingFeatureFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FixedLengthFileMappingFeature", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void SetFixture(FixedLengthFileMappingFeatureFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Write fixed-length file", Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "FixedLengthFileMappingFeature")]
        [Xunit.TraitAttribute("Description", "Write fixed-length file")]
        public virtual void WriteFixed_LengthFile()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Write fixed-length file", new string[] {
                        "ignore"});
#line 4
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Length",
                        "PaddingChar",
                        "Padding",
                        "NullValue"});
            table1.AddRow(new string[] {
                        "Id",
                        "5",
                        "0",
                        "Left",
                        ""});
            table1.AddRow(new string[] {
                        "Description",
                        "25",
                        "<space>",
                        "Right",
                        ""});
            table1.AddRow(new string[] {
                        "NullableInt",
                        "5",
                        "0",
                        "Left",
                        "=Null"});
#line 5
 testRunner.Given("I have specification for \'TestObject\' fixed-length type", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Description",
                        "NullableInt"});
            table2.AddRow(new string[] {
                        "1",
                        "Description 1",
                        "00003"});
            table2.AddRow(new string[] {
                        "2",
                        "Description 2",
                        "00003"});
            table2.AddRow(new string[] {
                        "3",
                        "Description 3",
                        "00003"});
            table2.AddRow(new string[] {
                        "4",
                        "Description 4",
                        "00003"});
            table2.AddRow(new string[] {
                        "5",
                        "Description 5",
                        "=Null"});
            table2.AddRow(new string[] {
                        "6",
                        "Description 6",
                        "00003"});
            table2.AddRow(new string[] {
                        "7",
                        "Description 7",
                        "00003"});
            table2.AddRow(new string[] {
                        "8",
                        "Description 8",
                        "00003"});
            table2.AddRow(new string[] {
                        "9",
                        "Description 9",
                        "00003"});
            table2.AddRow(new string[] {
                        "10",
                        "Description 10",
                        "=Null"});
#line 10
 testRunner.And("I have several entities", ((string)(null)), table2, "And ");
#line 22
 testRunner.When("I convert entities to the fixed-length format", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 23
 testRunner.Then("the result should be", @"00001Description 1            00003
00002Description 2            00003
00003Description 3            00003
00004Description 4            00003
00005Description 5            =Null
00006Description 6            00003
00007Description 7            00003
00008Description 8            00003
00009Description 9            00003
00010Description 10           =Null
", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                FixedLengthFileMappingFeatureFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                FixedLengthFileMappingFeatureFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
