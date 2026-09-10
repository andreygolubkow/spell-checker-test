namespace MySpell.BusinessLogic;

public interface IVocabulary
{
	string[] GetBestMatchEntry(string input, int depth);
}