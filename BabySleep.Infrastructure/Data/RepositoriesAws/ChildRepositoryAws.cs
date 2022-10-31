using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using BabySleep.Infrastructure.Helpers;
using BabySleep.Infrastructure.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BabySleep.Infrastructure.Data.RepositoriesAws
{
    public class ChildRepositoryAws : IChildRepository
    {
        private AwsHelper awsHelper;
        public ChildRepositoryAws()
        {
            awsHelper = new AwsHelper();
        }

        public void Add(Child child)
        {
            throw new NotImplementedException();
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid childGuid)
        {
            throw new NotImplementedException();
        }

        public Child Get(Guid childGuid)
        {
            throw new NotImplementedException();
        }

        public IList<Child> GetAll()
        {
            return new List<Child>();
        }

        public IList<Child> GetAll(Guid userGuid)
        {
            try
            {
                var request = new GetChildrenRequest() { userGuid = userGuid.ToString() };
                var jsonRequest = JsonConvert.SerializeObject(request);
                var jsonResponse = awsHelper.GetLambdaResponse(AwsFunctionsEnum.GetChildren, jsonRequest);
                var jsonChildren = JsonConvert.DeserializeObject<List<AWS.Common.Models.Child>>(jsonResponse);

                var result = new List<Child>();

                foreach(var child in jsonChildren)
                {
                    result.Add(new Child(child.ChildGuid, child.BirthDate, null, child.Name, null));
                }

                return result;

            }
            catch 
            {
                return new List<Child>();
            };
        }

        public Child GetFirst()
        {
            throw new NotImplementedException();
        }

        public void Update(Child child)
        {
            throw new NotImplementedException();
        }
    }
}
