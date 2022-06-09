using BabySleep.Application.DTO;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Interfaces;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ISleepRepository sleepRepository;
        private readonly IStatisticsDtoAssembler statisticsDtoAssembler;

        public StatisticsService(ISleepRepository sleepRepository, IStatisticsDtoAssembler statisticsDtoAssembler)
        {
            this.statisticsDtoAssembler = statisticsDtoAssembler;
            this.sleepRepository = sleepRepository;
        }


        public StatisticsDto GetStatistics(Guid childGuid, DateTime startDate, DateTime endDate)
        {
            return statisticsDtoAssembler.WriteStatisticsDto(sleepRepository.Take(childGuid, startDate, endDate), startDate, endDate);
        }
    }
}
