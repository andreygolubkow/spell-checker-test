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
	
	[TestCase("test", "test", "Exact match")]
	[TestCase("tst", "test", "One insert case")]
	[TestCase("teest", "test", "One delete case")]
	public void GetBestMatchTest(string input, string candidate, string description)
	{
		var words = new string[] { "test", "testability", "testosterone", "bug", "on", "off", };
		
		var vocabulary = Vocabulary.BuildVocabulary(words);
		var r = vocabulary.GetBestMatch(input,2 );

		Assert.That(r.Contains(candidate));
	}
}