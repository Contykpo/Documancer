namespace Documancer.Application.Pipeline.PreProcessors
{
    public sealed class ValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        #region Properties and Fields

        private readonly IReadOnlyCollection<IValidator<TRequest>> _validators;

        #endregion

        #region Constructors

        public ValidationPreProcessor(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators.ToList() ?? throw new ArgumentNullException(nameof(validators));
        }

        #endregion

        #region Methods

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return;

            var validationContext = new ValidationContext<TRequest>(request);

            var failures = await _validators.ValidateAsync(validationContext, cancellationToken);

            if (failures.Any()) throw new ValidationException(failures);
        }

        #endregion
    }
}