using MySpell.BusinessLogic.Services;

namespace MySpell.BusinessLogic.Tests.Services;

[TestFixture]
public class VocabularyTests
{
	private string[] _sharedWords = ["test", "CaSe"];
	
	[Test]
	public void BuildTest()
	{
		var result = Vocabulary.BuildVocabulary(_sharedWords);

		Assert.Multiple(() =>
		{
			foreach (var s in _sharedWords)
			{
				Assert.That(result.IsKnown(s), $"{s} word should be known as it's used to build vocabulary.");
			}
		});
	}
	
	[TestCase("test", "test", "Exact match")]
	[TestCase("tEsT", "tEsT", "Exact match case same case output")]
	[TestCase("case", "case", "Exact match case insensitive vocabulary side")]
	[TestCase("tST", "test", "One insert different case word")]
	[TestCase("tst", "test", "One middle insert case")]
	[TestCase("est", "test", "First insert case")]
	[TestCase("tes", "test", "Last insert case")]
	[TestCase("teest", "test", "One middle delete case")]
	[TestCase("xtest", "test", "First delete case")]
	[TestCase("testy", "test", "Last delete case")]
	[TestCase("tset", "test", "One delete and one insert case")]
	[TestCase("taesat", "test", "Two deletes, but not near")]
	[TestCase("ts", "test", "Two inserts, but not near")]
	public void GetBestMatchTest(string input, string candidate, string description)
	{
		var vocabulary = Vocabulary.BuildVocabulary(_sharedWords);
		var result = vocabulary.GetBestMatch(input,2 );

		Assert.Multiple(() =>
		{
			Assert.That(result.Length == 1, "Unexpected words found in result");
			Assert.That(result.Contains(candidate), "The correction doesn't contain right correction");
		});
	}

	[Test]
	public void BestCorrectionWinsTest()
	{
		string[] words = ["taste", "test"];
		
		var vocabulary = Vocabulary.BuildVocabulary(words);
		
		var result = vocabulary.GetBestMatch("tst",2 ); 
		
		Assert.Multiple(() =>
		{
			Assert.That(result.Length == 1, "Unexpected words found in result");
			Assert.That(result.Contains("test"));
		});
	}
	
	[Test]
	public void FewBestCorrectionsTest()
	{
		string[] words = ["tost", "test", "toset"];
		
		var vocabulary = Vocabulary.BuildVocabulary(words);
		
		var result = vocabulary.GetBestMatch("tst",2 ); 
		
		Assert.Multiple(() =>
		{
			Assert.That(result.Length == 2, "Unexpected words found in result");
			Assert.That(result.Contains("test"));
			Assert.That(result.Contains("tost"));
		});
	}

	[TestCase("tzst")]
	[TestCase("aatest")]
	[TestCase("testaa")]
	[TestCase("st")]
	[TestCase("te")]
	public void NearestCorrectionsAreNotAllowedTest(string text)
	{
		var vocabulary = Vocabulary.BuildVocabulary(_sharedWords);
		
		var result = vocabulary.GetBestMatch(text, 2);
		
		Assert.That(result.Length == 0);
	}
}