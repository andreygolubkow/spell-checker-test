using MySpell.BusinessLogic.Services;

namespace MySpell.BusinessLogic;

public class WordsNormaliser : IWordsNormaliser
{
	public string Normalise(string word)
	{
		return word.ToLowerInvariant();
	}
}