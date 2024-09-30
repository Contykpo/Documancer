namespace Documancer.Application.Common.Models
{
    public class Result : IResult
    {
        #region Propertes

        public string ErrorMessage => string.Join(", ", Errors ?? new string[] { });

        public bool Succeeded { get; init; }

        public string[] Errors { get; init; }

        #endregion

        #region Constructors

        internal Result()
        {
            Errors = new string[] { };
        }

        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        #endregion

        #region Methods

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Task<Result> SuccessAsync()
        {
            return Task.FromResult(new Result(true, Array.Empty<string>()));
        }

        public static Result Failure(params string[] errors)
        {
            return new Result(false, errors);
        }

        public static Task<Result> FailureAsync(params string[] errors)
        {
            return Task.FromResult(new Result(false, errors));
        }

        #endregion
    }

    public class Result<T> : Result, IResult<T>
    {
        #region Properties

        public T? Data { get; set; }

        #endregion

        #region Methods

        public new static Result<T> Failure(params string[] errors)
        {
            return new Result<T> { Succeeded = false, Errors = errors.ToArray() };
        }

        public new static async Task<Result<T>> FailureAsync(params string[] errors)
        {
            return await Task.FromResult(Failure(errors));
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data };
        }

        public static async Task<Result<T>> SuccessAsync(T data)
        {
            return await Task.FromResult(Success(data));
        }

        #endregion
    }
}