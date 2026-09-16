namespace MySpell.BusinessLogic.Services;

public interface IVocabulary
{
	string[] GetBestMatch(string input, int depth);
}