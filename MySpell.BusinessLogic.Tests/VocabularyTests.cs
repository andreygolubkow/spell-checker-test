namespace MySpell.BusinessLogic.Tests;

[TestFixture]
public class VocabularyTests
{
	private string[] _sharedDictionary = ["test", "testability", "testosterone", "bug", "on", "off"];
	
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
	[TestCase("tst", "test", "One middle insert case")]
	[TestCase("est", "test", "First insert case")]
	[TestCase("tes", "test", "Last insert case")]
	[TestCase("teest", "test", "One middle delete case")]
	[TestCase("xtest", "test", "First delete case")]
	[TestCase("testy", "test", "Last delete case")]
	[TestCase("tset", "test", "One delete and one insert case")]
	public void GetBestMatchTest(string input, string candidate, string description)
	{
		var vocabulary = Vocabulary.BuildVocabulary(_sharedDictionary);
		var result = vocabulary.GetBestMatch(input,2 );

		Assert.Multiple(() =>
		{
			Assert.That(result.Length == 1, "Unexpected words found in result");
			Assert.That(result.Contains(candidate), "The correction doesn't contain right correction");
		});
	}
}