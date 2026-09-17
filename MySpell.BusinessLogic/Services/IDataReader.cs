namespace MySpell.BusinessLogic.Services;

public interface IDataReader
{
	IEnumerable<string> ReadFile();
}