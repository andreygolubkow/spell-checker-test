using System.Text;
using MySpell.BusinessLogic.Models;

namespace MySpell.BusinessLogic.Services;

public class DataReader : IDataReader
{
	private const char Separator = '=';
	private const int BufferSize = 2048;
	private readonly string? _path;
	private readonly List<string[]> _sections = [];
	private bool _sectionsLoaded;
	private int _nextSectionIndex;
	
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

		LoadSections();

		if (_nextSectionIndex >= _sections.Count)
		{
			yield break;
		}

		foreach (var word in _sections[_nextSectionIndex])
		{
			yield return word;
		}

		_nextSectionIndex++;
	}

	private void LoadSections()
	{
		if (_sectionsLoaded) return;

		_sectionsLoaded = true;
		
		var currentSection = new List<string>();
		var state = new ReaderState();
		using var reader = new StreamReader(_path!);
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

				var separatorReached = ProcessToken(state, currentSection);
				
				if (!separatorReached) continue;

				if (currentSection.Count <= 0) continue;
				
				_sections.Add(currentSection.ToArray());
				currentSection = new List<string>();
			}
		}

		var endSeparatorReached = ProcessToken(state, currentSection);
		if (endSeparatorReached && currentSection.Count > 0)
		{
			_sections.Add(currentSection.ToArray());
			return;
		}

		if (currentSection.Count > 0)
		{
			_sections.Add(currentSection.ToArray());
		}
	}

	private bool ProcessToken(ReaderState state, List<string> currentSection)
	{
		var separatorReached = false;
		if (state.FullWord.Length == 0)
		{
			return separatorReached;
		}

		if (IsSeparator(state.FullWord))
		{
			separatorReached = true;
			state.FullWord.Clear();
			return separatorReached;
		}

		currentSection.Add(state.FullWord.ToString());
		state.FullWord.Clear();

		return separatorReached;
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