namespace MySpell.BusinessLogic;

public class SpellChecker : ISpellChecker
{
	private readonly char[] _unsupportedChars = [' '];
	
	public string ProcessWord(string word)
	{
		if (HasUnsupportedChars(word))
		{
			throw new ArgumentException("The word contains unsupported symbols and cannot be checked");
		}

		// TODO
		
		return word;
	}
	
	private bool HasUnsupportedChars(string word)
	{
		foreach (var character in _unsupportedChars)
		{
			if (word.Contains(character))
			{
				return true;
			}
		}
		return false;
	}
}