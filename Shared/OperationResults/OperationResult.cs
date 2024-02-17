using System.ComponentModel.DataAnnotations;

namespace Shared.OperationResults
{
    public class OperationResult<TEnumResult, TResult>
    {
        public TResult Result { get; set; }
        public TEnumResult EnumResult { get; set; }
        public ICollection<ValidationResult> ValidationResults { get; set; }
        public string ErrorMessages
        {
            get
            {
                if (ValidationResults != null)
                {
                    return string.Join(", ", ValidationResults);
                }
                return string.Empty;
            }
        }
        public void AddError(string errorMessage)
        {
            if (ValidationResults is null)
            {
                ValidationResults = new List<ValidationResult>();
            }
            ValidationResults.Add(new ValidationResult(errorMessage));
        }
    }
}

