namespace MySpell.BusinessLogic.Services;

public interface IVocabulary
{
	bool IsKnown(string word);
	string[] GetBestMatch(string input, int depth);
}