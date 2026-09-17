using MySpell.BusinessLogic.Services;

namespace MySpell.CLI;

class Program
{
	static void Main(string[] args)
	{
		var input = args != null && args.Length > 1 ? args[1] : "input.txt";
		
		var reader = new DataReader(input);
		var words = new List<string>();

		foreach (var w in reader.ReadFile())
		{
			words.Add(w);	
		}
		
		



	}
}