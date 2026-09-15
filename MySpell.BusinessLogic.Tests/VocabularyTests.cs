namespace MySpell.BusinessLogic.Tests;

[TestFixture]
public class VocabularyTests
{
	[Test]
	public void BuildTest()
	{
		var words = new string[] { "test", "testability", "testosterone", "bug", "on", "off", };
		
		var result = Vocabulary.BuildVocabulary(words);

		Assert.Multiple(() =>
		{
			foreach (var s in words)
			{
				Assert.That(result.IsKnown(s), $"{s} word should be known as it's used to build vocabulary.");
			}
		});
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