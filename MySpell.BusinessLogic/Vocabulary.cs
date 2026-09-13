namespace MySpell.BusinessLogic;

public class Vocabulary : IVocabulary
{
	private HashSet<string> _plainWords;
	private IDictionary<string, List<string>> _dictionary;
	
	// TODO: We need here some super-fast hash,
	// to understand the chance of finding word in our cache
	// because probably, if can say we don't have word with two edits at all
	// then we don't have to search it. 
	
	// TODO: Also, we need to think about saving a cache on disk, to don't fill
	// the dictionary for second run
	

	public Vocabulary(HashSet<string> plainWords, IDictionary<string,List<string>> dictionary)
	{
		_plainWords = plainWords;
		_dictionary = dictionary;
	}

	public bool IsKnown(string word)
	{
		return _plainWords.Contains(word);
	}
	
	public string[] GetBestMatch(string input, int depth)
	{
 		// Take left written part
		string leftExactMatch = null;
		
		for (int i = 1; i < input.Length; i++)
		{
			var word = input[0..i];
			if (_dictionary.ContainsKey(word))
			{
				leftExactMatch = word;
			}
			else
			{
				break;
			}
		}
		// TODO: need to cover case, when first letter - wrong, second letter - right
		// Trying to find right word
		
		
		var firstEdit = _dictionary[leftExactMatch];
		// test
		//     a
		//       bility
		//     o
		//       sterone


		return firstEdit.ToArray();
	}

	public static Vocabulary BuildVocabulary(string[] knownWords)
	{
		var plainWords = new HashSet<string>();
		IDictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>()
		{
			[""] = new List<string>()
		};

		for (int i = 0; i < knownWords.Length; i++)
		{
			var word = knownWords[i];
			if (plainWords.Contains(word)) continue;

			for (int j = 1; j < word.Length; j++)
			{
				var key = word[0..j];
				if (dictionary.ContainsKey(key)) continue;
				
				var parent = word[..(j - 1)];
				dictionary[parent].Add(key);
				dictionary[key] = new List<string>();
			}

			plainWords.Add(word);
		}

		return new Vocabulary(plainWords, dictionary);
	}
}