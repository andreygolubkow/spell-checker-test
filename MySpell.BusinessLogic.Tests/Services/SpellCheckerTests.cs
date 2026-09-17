using Moq;
using MySpell.BusinessLogic.Services;

namespace MySpell.BusinessLogic.Tests.Services;

[TestFixture]
public class SpellCheckerTests
{
	[Test]
	public void ProcessKnownWordSaveCaseTest()
	{
		var vocabulary = new Mock<IVocabulary>();
		vocabulary.Setup(x => x.IsKnown(It.IsAny<string>()))
			.Returns(true);
			
		var spellChecker = new SpellChecker(vocabulary.Object);
		
		var result = spellChecker.ProcessWord("Test");

		Assert.That(result, Is.EqualTo("Test"));
	}

	[Test]
	public void ProcessUnknownWordTest()
	{
		var vocabulary = new Mock<IVocabulary>();
		vocabulary.Setup(x => x.IsKnown(It.IsAny<string>()))
			.Returns(false);
			
		var spellChecker = new SpellChecker(vocabulary.Object);
		
		var result = spellChecker.ProcessWord("test");

		Assert.That(result, Is.EqualTo("{test?}"));
	}

	[Test]
	public void ProcessWordWithCorrectionTest()
	{
		var vocabulary = new Mock<IVocabulary>();
		vocabulary.Setup(x => x.IsKnown(It.IsAny<string>()))
			.Returns(false);
		vocabulary.Setup(x => x.GetBestMatch(It.IsAny<string>(), It.IsAny<int>()))
			.Returns(["corrected"]);
		
		var spellChecker = new SpellChecker(vocabulary.Object);
		
		var result = spellChecker.ProcessWord("test");

		Assert.That(result, Is.EqualTo("corrected"));
	}
	
	[Test]
	public void ProcessUnsupportedCharsTest()
	{
		var vocabulary = Mock.Of<IVocabulary>();
		var spellChecker = new SpellChecker(vocabulary);
		
		Assert.That(
			() => spellChecker.ProcessWord("two words"), 
			Throws.TypeOf<ArgumentException>()
			);
	}
}