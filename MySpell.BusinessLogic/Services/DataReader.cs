using System.Text;
using MySpell.BusinessLogic.Models;

namespace MySpell.BusinessLogic.Services;

public class DataReader : IDataReader
{
	private const char Separator = '=';
	private const int BufferSize = 2048;
	private readonly string? _path;
	
	public DataReader(string path)
	{
		_path = path;
	}
	
	public IEnumerable<string> ReadFile()
	{
		if (string.IsNullOrWhiteSpace(_path) || !File.Exists(_path))
		{
			throw new InvalidOperationException("File path wasn't provided or file desn't exist");
		}

		return ReadWords(_path);
	}

	private IEnumerable<string> ReadWords(string path)
	{
		using var reader = new StreamReader(path);
		var state = new ReaderState();
		var buffer = new char[BufferSize];
		int readCount;
		
		while ((readCount = reader.ReadBlock(buffer, 0, buffer.Length)) > 0)
		{
			for (var i = 0; i < readCount; i++)
			{
				var currentChar = buffer[i];
				if (!char.IsWhiteSpace(currentChar))
				{
					state.FullWord.Append(currentChar);
					continue;
				}

				var (word, stopReading) = ProcessToken(state);
				
				if (word is not null)
				{
					yield return word;
				}

				if (stopReading)
				{
					yield break;
				}
			}
		}

		var (lastWord, endReading) = ProcessToken(state);
		
		if (lastWord is not null)
		{
			yield return lastWord;
		}
	}

	private (string? Word, bool StopReading) ProcessToken(ReaderState state)
	{
		if (state.FullWord.Length == 0)
		{
			return (null, false);
		}

		if (IsSeparator(state.FullWord))
		{
			var stopReading = state.SeparatorReached;
			state.SeparatorReached = true;
			state.FullWord.Clear();
			
			return (null, stopReading);
		}

		var word = state.FullWord.ToString();
		state.FullWord.Clear();
		
		return (word, false);
	}

	private bool IsSeparator(StringBuilder letter)
	{
		if (letter.Length == 0)
		{
			return false;
		}

		for (var i = 0; i < letter.Length; i++)
		{
			if (letter[i] != Separator)
			{
				return false;
			}
		}

		return true;
	}
}