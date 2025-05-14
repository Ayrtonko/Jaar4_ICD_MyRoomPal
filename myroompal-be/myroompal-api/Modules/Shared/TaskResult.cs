namespace myroompal_api.Modules.Shared
{
    public class TaskResult<T>
    {
        public bool IsSuccessful { get; set; }

        public T Result { get; set; }

        public string Message { get; set; }

        public TaskResult(bool isSuccessful, T result, string message)
        {
            IsSuccessful = isSuccessful;
            Result = result;
            Message = message;
        }

        public static TaskResult<T> Success(T result, string message = null)
        {
            return new TaskResult<T>(true, result, message);
        }

        public static TaskResult<T> Failure(string errorMessage)
        {
            return new TaskResult<T>(false, default(T), errorMessage);
        }
    }
}