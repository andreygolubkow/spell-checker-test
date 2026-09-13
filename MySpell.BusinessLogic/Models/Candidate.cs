namespace MySpell.BusinessLogic.Models;

public record Candidate(
	string Word,
	int Index,
	CorrectionType CorrectionType
);