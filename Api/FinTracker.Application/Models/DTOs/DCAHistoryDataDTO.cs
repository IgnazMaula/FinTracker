using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Models.DTOs;

public class DCAHistoryDataDTO
{
    public DateTime Date { get; set; }
    public double Volume { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Open { get; set; }
    public double Close { get; set; }
    public double PreviousClose { get; set; }
    public double Price { get; set; }

}
