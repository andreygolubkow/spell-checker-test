namespace MySpell.BusinessLogic;

public interface IVocabularyEntry
{
	string Value { get; }
	
	IEnumerable<IVocabularyEntry> GetChildren();
}