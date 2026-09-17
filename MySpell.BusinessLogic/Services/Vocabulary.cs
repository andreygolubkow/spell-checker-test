using MySpell.BusinessLogic.Models;

namespace MySpell.BusinessLogic.Services;

public class Vocabulary : IVocabulary
{
	private readonly Dictionary<string, int> _words;
	private readonly Dictionary<string, List<string>> _dictionary;
	
	public Vocabulary(Dictionary<string, int> words, Dictionary<string, List<string>> dictionary)
	{
		_words = words;
		_dictionary = dictionary;
	}

	public bool IsKnown(string word)
	{
		return !string.IsNullOrEmpty(word) && _words.ContainsKey(word.ToLowerInvariant());
	}
	
	public string[] GetBestMatch(string input, int depth)
	{
		if (string.IsNullOrEmpty(input))
		{
			return [];
		}
		
		if (IsKnown(input.ToLowerInvariant()))
		{
			return [input];
		}
		
		var stack = new Stack<Candidate>();
		var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		stack.Push(new Candidate("", 0, 0, CorrectionType.None));
		
		while (stack.TryPop(out var current))
		{
			if (current.Index == input.Length && IsKnown(current.Word))
			{
				if (!result.TryGetValue(current.Word, out var existingEdits) || current.EditsCount < existingEdits)
				{
					result[current.Word] = current.EditsCount;
				}
				continue;
			}
			
			if (!_dictionary.TryGetValue(current.Word, out var children))
			{
				continue;
			}
			
			if (current.Index < input.Length && current.EditsCount < depth && current.CorrectionType == CorrectionType.None)
			{
				stack.Push(new Candidate(current.Word, current.Index + 1, current.EditsCount + 1, CorrectionType.Delete));
			}
			
			foreach (var child in children)
			{
				if (current.Index < input.Length && char.ToLowerInvariant(child[^1]) == char.ToLowerInvariant(input[current.Index]))
				{
					stack.Push(new Candidate(child, current.Index + 1, current.EditsCount, CorrectionType.None));
				}
				
				if (current.EditsCount < depth && current.Index <= input.Length && current.CorrectionType == CorrectionType.None)
				{
					stack.Push(new Candidate(child, current.Index, current.EditsCount + 1, CorrectionType.Insert));
				}
			}
		}

		if (result.Count == 0)
		{
			return [];
		}

		var minEdits = result.Values.Min();
		return result
			.Where(x => x.Value == minEdits)
			.OrderBy(x => _words[x.Key])
			.Select(x => x.Key)
			.ToArray();
	}

	public static Vocabulary BuildVocabulary(string[] knownWords)
	{
		var words = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
		var dictionary = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
		{
			[""] = new List<string>()
		};
		
		for (var i = 0; i < knownWords.Length; i++)
		{
			var word = knownWords[i].Trim().ToLowerInvariant();
			if (!words.TryAdd(word, i))
			{
				// Duplicate in vocabulary, we'll skip this for now.
				continue;
			};
			
			for (int j = 1; j <= word.Length; j++)
			{
				var key = word[0..j];
				if (dictionary.ContainsKey(key)) continue;
				
				var parent = word[..(j - 1)];
				if (!dictionary.ContainsKey(parent))
				{
					dictionary[parent] = new List<string>();
				}
				dictionary[parent].Add(key);
				dictionary[key] = new List<string>();
			}
			
		}

		return new Vocabulary(words, dictionary);
	}
}