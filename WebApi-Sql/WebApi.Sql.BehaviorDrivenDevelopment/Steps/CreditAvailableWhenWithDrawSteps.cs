using TechTalk.SpecFlow;
using FluentAssertions;

namespace WebApi.Sql.BehaviorDrivenDevelopment.Steps
{
    [Binding]
    public class CreditAvailableWhenWithDrawSteps
    {
        [BeforeScenario]
        public void Before()
        {
            int value = 10;
            ScenarioContext.StepIsPending();
            value.Should().Be(10);
        }

        [AfterScenario]
        public void After()
        {
            int value = 10;
            ScenarioContext.StepIsPending();
            value.Should().Be(10);
        }

        [Given(@"An user account which hasn't balance available")]
        public void AnUserAccountWhicHasntBalanceAvailable()
        {
            int value = 10;
            ScenarioContext.StepIsPending();
            value.Should().Be(10);
        }

        [Given(@"doesn't contain any credit or special credit")]
        public void DoesntContainAnyCreditOrSpecialCredit()
        {
            ScenarioContext.StepIsPending();
        }

        [When(@"the user request a value at ATM")]
        public void TheUserRequestaValueAtATM()
        {
            ScenarioContext.StepIsPending();
        }

        [Then(@"the response should be negative balance unavailable")]
        public void TheresponseShouldBeNegativeBalanceUnavailable()
        {
            ScenarioContext.StepIsPending();
        }

        [Given(@"Give their first credit available to next withdraw")]
        public void GiveTheirFirstCreditAvailableToNextWithdraw()
        {
            ScenarioContext.StepIsPending();
        }

    }
}
