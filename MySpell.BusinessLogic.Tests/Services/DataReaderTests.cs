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
			
			var result = reader.ReadFile().ToArray();
			
			Assert.That(result, Is.EqualTo(new[] { "test", "text", "TeSt", "one", "two", "three" }));
		}
		finally
		{
			File.Delete(path);
		}
	}
}
