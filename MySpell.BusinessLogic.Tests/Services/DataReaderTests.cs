using MySpell.BusinessLogic.Services;

namespace MySpell.BusinessLogic.Tests.Services;

[TestFixture]
public class DataReaderTests
{
	private const string FEW_LINES_TEXT = """
	                                      test text
	                                      TeSt
	                                      ===
	                                      one two
	                                      three
	                                      ===
	                                      """;
	
	[Test]
	public void ReadFileTest()
	{
		var path = Path.GetTempFileName();
		try
		{
			File.WriteAllText(path,FEW_LINES_TEXT);
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
			
		}
		finally
		{
			File.Delete(path);
		}
	}
}
