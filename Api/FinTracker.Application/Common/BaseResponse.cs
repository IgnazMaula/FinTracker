using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Common;

public class BaseResponse<T>
{
    public int Status { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public string LogEventId { get; set; }

    public BaseResponse() { }

    public BaseResponse(string logEventId)
    {
        LogEventId = logEventId;
    }

    public void SetReturnStatus(short code, string message)
    {
        Status = code;
        Message = message;
    }

    public void SetReturnSuccessStatus()
    {
        Status = 200;
        Message = "Success";
    }

    public void SetReturnSuccessStatus(string message)
    {
        Status = 200;
        Message = message;
    }

    public void SetReturnNotFoundStatus()
    {
        Status = 404;
        Message = "Data Not Found";
    }

    public void SetReturnBadRequestStatus(string valMessage)
    {
        Status = 403;
        Message = valMessage;
    }

    public void SetReturnErrorStatus(string errMessage)
    {
        Status = 500;
        Message = errMessage;
    }
}