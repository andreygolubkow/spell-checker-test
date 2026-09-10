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
}