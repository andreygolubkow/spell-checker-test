namespace MySpell.BusinessLogic.Services;

public interface IVocabularyEntry
{
	string Value { get; }
	
	IEnumerable<IVocabularyEntry> GetChildren();
}