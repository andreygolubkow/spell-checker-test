using MySpell.BusinessLogic.Services;

namespace MySpell.CLI;

class Program
{
	static void Main(string[] args)
	{
		var input = args != null && args.Length > 1 ? args[1] : "input.txt";
		
		var reader = new DataReader(input);
		var words = reader.ReadFile().ToList();
		
		var vocabulary = Vocabulary.BuildVocabulary(words);
		var spellChecker = new SpellChecker(vocabulary);

		var isFirst = true;
		foreach (var word in reader.ReadFile())
		{
			if (isFirst)
			{
				isFirst = false;
			}
			else
			{
				Console.Write(' ');
			}
			
			var result = spellChecker.ProcessWord(word);
			
			Console.Write(result);
		}
	}
}