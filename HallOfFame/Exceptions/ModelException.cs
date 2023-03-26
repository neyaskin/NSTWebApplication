using System.ComponentModel.DataAnnotations;

namespace HallOfFame.Exceptions;

public class ModelException : Exception
{
    public List<ValidationResult> Errors { get; set; }
    public ModelException(List<ValidationResult> validationResults)
    {
        Errors = validationResults;
    }
}