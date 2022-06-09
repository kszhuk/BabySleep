using BabySleep.Application.DTO;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTOAssemblers
{
    public interface IStatisticsDtoAssembler
    {
        StatisticsDto WriteStatisticsDto(IList<Sleep>  sleeps, DateTime startDate, DateTime endDate);
    }
}
