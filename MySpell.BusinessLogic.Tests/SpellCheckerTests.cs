namespace MySpell.BusinessLogic.Tests;

[TestFixture]
public class SpellCheckerTests
{
	const string CORRECT_PAIN = "pain";
	const string FEW_CORRECTIONS_WORDS = "main mainly";
	
	// If W is in the dictionary, print it as is.
	[TestCase(CORRECT_PAIN, CORRECT_PAIN)]
	// Otherwise, if W is not in the dictionary,
	// - If no corrections can be found, print “{W?}”.
	[TestCase("rame", "{rame?}")]
	// Ignore any corrections that require two edits / adjacent edits 
	[TestCase("hints", "{hints?}")]
	// If exactly one correction is left, print that word.
	// One remove
	[TestCase($"{CORRECT_PAIN}n", $"{CORRECT_PAIN}")]
	// One insert
	[TestCase($"pai", $"{CORRECT_PAIN}")]
	// If more than one possible correction is left, print the set of corrections as “{W1
	// W2 · · ·}”, in the order they appear in the dictionary.
	[TestCase("mainy", $"{{{FEW_CORRECTIONS_WORDS}}}")]
	public void Process_GeneralCases(string word, string expectedResult)
	{
		var instance = new SpellChecker(null);

		var result = instance.ProcessWord(word);

		Assert.That(result, Is.EqualTo(expectedResult));
	}
}