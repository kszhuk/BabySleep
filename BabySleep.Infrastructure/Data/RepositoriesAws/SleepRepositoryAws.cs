using AutoMapper;
using BabySleep.Common.Enums;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using BabySleep.Infrastructure.Helpers;
using BabySleep.Infrastructure.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabySleep.Infrastructure.Data.RepositoriesAws
{
    public class SleepRepositoryAws : ISleepRepository
    {
        private AwsHelper awsHelper;
        public SleepRepositoryAws()
        {
            awsHelper = new AwsHelper();
        }

        public void Add(Sleep sleep)
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Sleep, AWS.Common.Models.Sleep>()
                    .ForMember(dest => dest.SleepPlace, act => act.MapFrom(src => (short)src.SleepPlace))
                    .ForMember(dest => dest.StartTime, act => act.MapFrom(src => src.SleepTime.StartTime))
                    .ForMember(dest => dest.EndTime, act => act.MapFrom(src => src.SleepTime.EndTime))
                    .ForMember(dest => dest.Quality, act => act.MapFrom(src => src.CustomerInfo.Quality)));
            var mapper = new Mapper(config);
            var inputSleep = mapper.Map<AWS.Common.Models.Sleep>(sleep);

            var request = new AddSleepRequest() { sleep = JsonConvert.SerializeObject(inputSleep) };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var jsonResponse = awsHelper.GetLambdaResponse(AwsFunctionsEnum.AddSleep, jsonRequest);
        }

        public void Delete(Guid sleepGuid)
        {
            var request = new DeleteRequest() { sleepGuid = sleepGuid };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var jsonResponse = awsHelper.GetLambdaResponse(AwsFunctionsEnum.DeleteSleep, jsonRequest);
        }

        public Sleep Get(Guid sleepGuid)
        {
            try
            {
                var request = new GetSleepRequest() { sleepGuid = sleepGuid.ToString() };
                var jsonRequest = JsonConvert.SerializeObject(request);
                var jsonResponse = awsHelper.GetLambdaResponse(AwsFunctionsEnum.GetSleep, jsonRequest);
                var jsonSleep = JsonConvert.DeserializeObject<AWS.Common.Models.Sleep>(jsonResponse);

                var sleep = new Sleep(jsonSleep.SleepGuid, jsonSleep.ChildGuid, (SleepPlace)jsonSleep.SleepPlace, 
                    jsonSleep.StartTime, jsonSleep.EndTime,
                    jsonSleep.FeedingCount, jsonSleep.FallAsleepTime, jsonSleep.AwakeningCount, jsonSleep.Quality, string.Empty);

                return sleep;

            }
            catch
            {
                return new Sleep();
            };
        }

        public IList<Sleep> Take(Guid childGuid, DateTime currentDate)
        {
            try
            {
                var request = new GetSleepsRequest() { childGuid = childGuid.ToString(), currentDate = currentDate.ToString() };
                var jsonRequest = JsonConvert.SerializeObject(request);
                var jsonResponse = awsHelper.GetLambdaResponse(AwsFunctionsEnum.GetSleeps, jsonRequest);
                var jsonSleeps = JsonConvert.DeserializeObject<List<AWS.Common.Models.Sleep>>(jsonResponse).OrderBy(s => s.StartTime);

                var result = new List<Sleep>();

                foreach (var sleep in jsonSleeps)
                {
                    result.Add(new Sleep(sleep.SleepGuid, sleep.ChildGuid, (SleepPlace)sleep.SleepPlace, sleep.StartTime, sleep.EndTime,
                        sleep.FeedingCount, sleep.FallAsleepTime, sleep.AwakeningCount, sleep.Quality, String.Empty));
                }

                return result;

            }
            catch
            {
                return new List<Sleep>();
            };
        }

        public IList<Sleep> Take(Guid childGuid, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void Update(Sleep sleep)
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Sleep, AWS.Common.Models.Sleep>()
                    .ForMember(dest => dest.SleepPlace, act => act.MapFrom(src => (short)src.SleepPlace))
                    .ForMember(dest => dest.StartTime, act => act.MapFrom(src => src.SleepTime.StartTime))
                    .ForMember(dest => dest.EndTime, act => act.MapFrom(src => src.SleepTime.EndTime))
                    .ForMember(dest => dest.Quality, act => act.MapFrom(src => src.CustomerInfo.Quality)));
            var mapper = new Mapper(config);
            var inputSleep = mapper.Map<AWS.Common.Models.Sleep>(sleep);

            var request = new AddSleepRequest() { sleep = JsonConvert.SerializeObject(inputSleep) };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var jsonResponse = awsHelper.GetLambdaResponse(AwsFunctionsEnum.UpdateSleep, jsonRequest);
        }
    }
}
