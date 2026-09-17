using System.Text;

namespace MySpell.BusinessLogic.Models;

public class ReaderState
{
	public bool SeparatorReached { get; set; }
	
	public StringBuilder FullWord { get; } = new();
}