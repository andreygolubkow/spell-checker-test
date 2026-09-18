using MySpell.BusinessLogic.Services;

namespace MySpell.BusinessLogic.Tests.Services;

[TestFixture]
public class DataReaderTests
{
	private const string FewLinesText = """
	                                      test text
	                                      TeSt
	                                      ===
	                                      one two
	                                      three
	                                      ===
	                                      """;
	
	private const string ZeroLineText = """
	                                      test text
	                                      TeSt
	                                      ===
	                                      ===
	                                      """;
	
	private const string UnfinishedText = """
	                                      test text
	                                      TeSt
	                                      
	                                      """;
	
	private const string BrokenText = "=============";
	
	private const string EmptyText = "";
	
	[Test]
	public void ReadFileTest()
	{
		WithTempFile((path) =>
		{
			var reader = new DataReader(path);
			var vocabulary = reader.ReadFile().ToArray();
			var inputText = reader.ReadFile().ToArray();
			
			Assert.Multiple(() =>
			{
				Assert.That(vocabulary, 
					Is.EqualTo(new[] { "test", "text", "TeSt" }), "Vocabulary is wrong");
				Assert.That(inputText, 
					Is.EqualTo(new[] { "one", "two", "three" }), "Input sentences are wrong");
			});
		}, FewLinesText);
	}

	[TestCase(UnfinishedText)]
	[TestCase(ZeroLineText)]
	public void ReadFileOnlyVocabularyTest(string textPayload)
	{
		WithTempFile((path) =>
		{
			var reader = new DataReader(path);
			var vocabulary = reader.ReadFile().ToArray();
			var inputText = reader.ReadFile().ToArray();
			
			Assert.Multiple(() =>
			{
				Assert.That(vocabulary, 
					Is.EqualTo(new[] { "test", "text", "TeSt" }), "Vocabulary is wrong");
				Assert.That(inputText, Is.Empty);
			});
		}, textPayload);
	}
	
	[TestCase(BrokenText)]
	[TestCase(EmptyText)]
	public void ReadFileHandlesBrokenFileCorrectlyTest(string textPayload)
	{
		WithTempFile((path) =>
		{
			var reader = new DataReader(path);
			var vocabulary = reader.ReadFile().ToArray();
			
			Assert.That(vocabulary, Is.Not.Null);
		}, UnfinishedText);
	}

	private void WithTempFile(Action<string> test, string filePayload)
	{
		var path = Path.GetTempFileName();
		try
		{
			File.WriteAllText(path,filePayload);
			test(path);
		}
		finally
		{
			File.Delete(path);
		}
	}
}
