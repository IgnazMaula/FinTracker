using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Shared.Model;

public class FileUploadModel
{
    public MultipartFormDataContent? Content { get; set; }
}
