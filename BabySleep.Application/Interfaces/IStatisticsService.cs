using BabySleep.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Interfaces
{
    public interface IStatisticsService
    {
        StatisticsDto GetStatistics(Guid childGuid, DateTime startDate, DateTime endDate);
    }
}
