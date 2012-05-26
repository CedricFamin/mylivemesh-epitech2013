using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLiveMesh.Utils
{
    public class WebResult
    {
        public ErrorCodeList ErrorCode { get; set; }

        public enum ErrorCodeList
        {
            SUCCESS = 0x0,
            USER_NOT_FOUND,
            INFORMATION_REQUIRED,
            USER_ALREADY_EXIST,
            CANNOT_CREATE_DIRECTORY,
            DIRECTORY_NOT_FOUND,
            CANNOT_DELETE_DIRECTORY,
            CANNOT_RENAME_DIRECTORY

        }

        public WebResult()
        {
            ErrorCode = ErrorCodeList.SUCCESS;
        }

        public WebResult(ErrorCodeList error)
        {
            ErrorCode = error;
        }
    }

    public class WebResult<T> : WebResult
    {
        public T Value { get; set; }

        public WebResult() 
            : base()
        {
        }

        public WebResult(ErrorCodeList error)
            : base(error)
        {
        }

        public WebResult(T value)
            : base()
        {
            Value = value;
        }

        public WebResult(T value, ErrorCodeList error)
            : base(error)
        {
            Value = value;
        }
    }
}