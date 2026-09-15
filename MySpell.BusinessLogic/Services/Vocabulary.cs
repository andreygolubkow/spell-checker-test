using MySpell.BusinessLogic.Models;

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
		if (IsKnown(input))
		{
			return [input];
		}
		
		var stack = new Stack<Candidate>();
		var result = new HashSet<string>();

		stack.Push(new Candidate("", 0, CorrectionType.None));
		
		while (stack.TryPop(out var current))
		{
			if (current.Index == input.Length && IsKnown(current.Word))
			{
				result.Add(current.Word);
				continue;
			}
			
			if (!_dictionary.TryGetValue(current.Word, out var children))
			{
				continue;
			}
			
			foreach (var child in children)
			{
				if (current.Index < input.Length && child[^1] == input[current.Index])
				{
					stack.Push(new Candidate(child, current.Index + 1, CorrectionType.None));
				}
				
				if (current.Index < input.Length && current.CorrectionType != CorrectionType.Insert)
				{
					stack.Push(new Candidate(child, current.Index, CorrectionType.Insert));
				}
			}
		}

		return result.ToArray();
	}

	public static Vocabulary BuildVocabulary(string[] knownWords)
	{
		var plainWords = new HashSet<string>(knownWords);
		IDictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>()
		{
			[""] = new List<string>()
		};

		foreach (var word in plainWords)
		{
			for (int j = 1; j < word.Length; j++)
			{
				var key = j < word.Length ? word[0..j] : word;
				if (dictionary.ContainsKey(key)) continue;
				
				var parent = word[..(j - 1)];
				dictionary[parent].Add(key);
				dictionary[key] = new List<string>();
			}
		}

		return new Vocabulary(plainWords, dictionary);
	}
}