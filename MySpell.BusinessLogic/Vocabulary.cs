namespace MySpell.BusinessLogic;

public class Vocabulary : IVocabulary
{
	private HashSet<string> _plainWords;
	private IDictionary<string, List<string>> _dictionary;

	public Vocabulary(HashSet<string> plainWords, IDictionary<string,List<string>> dictionary)
	{
		_plainWords = plainWords;
		_dictionary = dictionary;
	}
	
	public string[] GetBestMatchEntry()
	{
		throw new NotImplementedException();
	}

	public static Vocabulary BuildVocabulary(string[] knownWords)
	{
		var plainWords = new HashSet<string>();
		IDictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

		for (int i = 0; i < knownWords.Length; i++)
		{
			var word = knownWords[i];
			if (plainWords.Contains(word)) continue;

			for (int j = 0; j < word.Length; j++)
			{
				var key = word[0..j];
				if (dictionary.ContainsKey(key)) continue;

				if (j == 0)
				{
					dictionary[$"{word[0]}"] = new List<string>();
					continue;
				}
				
				var parent = word[0..(j - 1)];
				dictionary[parent].Add(key);
				dictionary[key] = new List<string>();
			}
		}

		return new Vocabulary(plainWords, dictionary);
	}
}