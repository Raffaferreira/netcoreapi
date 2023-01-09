using TechTalk.SpecFlow;

namespace WebApi.Sql.BehaviorDrivenDevelopment.Steps
{
    [Binding]
    public class CreditAvailableWhenWithDrawSteps
    {
        [Given(@"An user account which hasn't balance available")]
        public void AnUserAccountWhicHasntBalanceAvailable()
        {
            ScenarioContext.StepIsPending();
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
