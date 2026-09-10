namespace MySpell.BusinessLogic;

public interface IVocabulary
{
	string[] GetBestMatch(string input, int depth);
}