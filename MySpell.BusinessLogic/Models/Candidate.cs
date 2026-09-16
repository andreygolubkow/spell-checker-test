namespace MySpell.BusinessLogic.Models;

public record Candidate(
	string Word,
	int Index,
	int EditsCount,
	CorrectionType CorrectionType
);