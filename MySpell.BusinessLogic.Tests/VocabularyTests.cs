namespace MySpell.BusinessLogic.Tests;

[TestFixture]
public class VocabularyTests
{
	[Test]
	public void BuildTest()
	{
		var words = new string[] { "test", "testability", "testosterone", "bug", "on", "off", };
		
		var result = Vocabulary.BuildVocabulary(words);

		Console.WriteLine("....");
	}

	//[TestCase("testabb")]
	[TestCase("tst")]
	public void GetBestMatchTest(string input)
	{
		var words = new string[] { "test", "testability", "testosterone", "bug", "on", "off", };
		
		var vocabulary = Vocabulary.BuildVocabulary(words);
		var r = vocabulary.GetBestMatch(input,2 );

		Console.WriteLine("....");
	}
}