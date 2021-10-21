using Microsoft.Extensions.Logging;
using System;

namespace EcommerceAPI.Model
{
    public abstract class Result
    {
        protected Result()
        {
            ErrorMessage = null;
        }
        public Result(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public string ErrorMessage { get; set; }
        public bool Errored => ErrorMessage != null; 
        public static Success WithOutErrored => new Success();
        public static implicit operator Result(string errorMessage) => new ErrorViewModel(errorMessage);
    }
    public class ErrorViewModel : Result
    {
        public ErrorViewModel(string errorMessage) : base(errorMessage) {}
    }

    public class Success : Result
    {

    }
}

