using MySpell.BusinessLogic.Models;
using MySpell.BusinessLogic.Services;

namespace MySpell.BusinessLogic;

public class Vocabulary : IVocabulary
{
	private readonly HashSet<string> _plainWords;
	private readonly IDictionary<string, List<string>> _dictionary;
	
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
		var result = new Dictionary<string, int>();

		stack.Push(new Candidate("", 0, 0, CorrectionType.None));
		
		while (stack.TryPop(out var current))
		{
			if (current.Index == input.Length && IsKnown(current.Word))
			{
				if (!result.TryGetValue(current.Word, out var existingEdits) || 
					current.EditsCount < existingEdits)
				{
					result[current.Word] = current.EditsCount;
				}
				continue;
			}
			
			if (!_dictionary.TryGetValue(current.Word, out var children))
			{
				continue;
			}
			
			if (current.Index < input.Length && current.EditsCount < depth 
			                                 && current.CorrectionType == CorrectionType.None)
			{
				stack.Push(new Candidate(current.Word, current.Index + 1, current.EditsCount + 1, CorrectionType.Delete));
			}
			
			foreach (var child in children)
			{
				if (current.Index < input.Length && child[^1] == input[current.Index])
				{
					stack.Push(new Candidate(child, current.Index + 1, current.EditsCount, CorrectionType.None));
				}
				
				if (current.EditsCount < depth && current.Index <= input.Length 
				                               && current.CorrectionType == CorrectionType.None)
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
			.Select(x => x.Key)
			.ToArray();
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
			for (int j = 1; j <= word.Length; j++)
			{
				var key = word[0..j];
				if (dictionary.ContainsKey(key)) continue;
				
				var parent = word[..(j - 1)];
				dictionary[parent].Add(key);
				dictionary[key] = new List<string>();
			}
		}

		return new Vocabulary(plainWords, dictionary);
	}
}