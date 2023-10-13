using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ResponseModel<T>
    {
        public bool Ok { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public static ResponseModel<T> EmptySuccess()
        {
            return new ResponseModel<T>
            {
                Data = default(T),
                Ok = true,
                Message = null
            };
        }

        public static ResponseModel<T> Success(T data)
        {
            return new ResponseModel<T>
            {
                Data = data,
                Ok = true,
                Message = null
            };
        }

        public static ResponseModel<T> SuccessMessage(string message, T data = default(T))
        {
            return new ResponseModel<T>
            {
                Ok = true,
                Data = data,
                Message = message
            };
        }

        public static ResponseModel<T> EmptyError()
        {
            return new ResponseModel<T>
            {
                Data = default(T),
                Ok = false,
                Message = null
            };
        }

        public static ResponseModel<T> Error(Dictionary<string, string> errors)
        {
            return new ResponseModel<T>
            {
                Data = default(T),
                Ok = false,
                Message = null
            };
        }

        public static ResponseModel<T> Error(string Message)
        {
            return new ResponseModel<T>
            {
                Data = default(T),
                Ok = false,
                Message = Message
            };
        }

    }
}
