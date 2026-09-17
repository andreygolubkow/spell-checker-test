namespace MySpell.BusinessLogic.Services;

public class SpellChecker : ISpellChecker
{
	private readonly IVocabulary _vocabulary;
	private readonly char[] _unsupportedChars = [' '];

	public SpellChecker(IVocabulary vocabulary)
	{
		_vocabulary = vocabulary;
	}

	public string ProcessWord(string word)
	{
		ArgumentNullException.ThrowIfNull(word);

		if (HasUnsupportedChars(word))
		{
			throw new ArgumentException("The word contains unsupported symbols and cannot be checked");
		}

		var lowerCaseWord = word.ToLowerInvariant();

		if (_vocabulary.IsKnown(lowerCaseWord))
		{
			return word;
		}

		var corrections = _vocabulary.GetBestMatch(lowerCaseWord, 2);
		
		if (corrections.Length == 1)
		{
			return corrections[0];
		}

		return corrections.Length == 0 ? $"{{{word}?}}" : $"{{{string.Join(" ", corrections)}}}";
	}
	
	private bool HasUnsupportedChars(string word)
	{
		return _unsupportedChars.Any(word.Contains);
	}
}