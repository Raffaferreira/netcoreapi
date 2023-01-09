Feature: Users gets credit when they request withdraw 
	In order to make a withdraw
	As a user of bank
	I want to be able to withdraw even with negative balance

Background: An user that has an bank account

@creditavailable 
Scenario: Verify withdraw available
	Given : An user account which hasn't balance available
	And : doesn't contain any credit or special credit
	When : the user request a value at ATM
	Then the response should be negative balance unavailable
	But : Give their first credit available to next withdraw
 

@creditnotvailable
Scenario: Add two numbers
	Add two numbers with the calculator
	Given I have entered <First> into the calculator
	And I have entered <Second> into the calculator 
	When I press add
	Then the result should be <Result> on the screen
	Examples:
	| First | Second | Result |
	| 50    | 70     | 120    |
	| 30    | 40     | 70     |
	| 60    | 30     | 90     |
