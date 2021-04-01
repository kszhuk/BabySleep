using BabySleep.Common.Exceptions.Child;
using BabySleep.Common.Interfaces;
using BabySleep.Domain.Models;
using BabySleep.EfData;
using BabySleep.EfData.Interfaces;
using BabySleep.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabySleep.Infrastructure.Data.Repositories
{
    public class ChildRepository : IChildRepository
    {
        private readonly IApplicationContext context;

        public ChildRepository(IApplicationContext context)
        {
            this.context = context;
        }

        public void Add(Child child)
        {
            var childrenName = context.Children.AsNoTracking().Select(c => c.Name.ToUpper()).ToList();
            if (childrenName.Contains(child.Name.ToUpper()))
            {
                throw new ChildAlreadyExistsException();
            }

            var efChild = new EfData.Models.Child()
            {
                Name = child.Name,
                BirthDate = child.BirthDate,
                BirthWeek = child.BirthWeek,
                Picture = child.Picture,
                ChildGuid = Guid.NewGuid()
            };

            context.Children.Add(efChild);
            context.SaveChanges();
        }

        public bool Any()
        {
            return context.Children.Any();
        }

        public int Count()
        {
            return context.Children.Count();
        }

        public void Delete(Guid childGuid)
        {
            if(context.Children.Count() == 1)
            {
                throw new DeleteLastChildException();
            }

            var child = context.Children.FirstOrDefault(c => c.ChildGuid == childGuid);

            if (child != null)
            {
                context.Children.Remove(child);
                context.SaveChanges();
            }
        }

        public Child Get(Guid childGuid)
        {
            return ConvertToDomain(context.Children.AsNoTracking().FirstOrDefault(c => c.ChildGuid == childGuid));
        }

        public IList<Child> GetAll()
        {
            return context.Children.AsNoTracking().OrderBy(c => c.Name).ToList().Select(c => ConvertToDomain(c)).ToList();
        }

        public Child GetFirst()
        {
            var children = context.Children.AsNoTracking().OrderBy(c => c.Name).ToList();

            if (!children.Any())
            {
                return new Child();
            }

            return ConvertToDomain(children.First());
        }

        public void Update(Child child)
        {
            var childrenName = context.Children.AsNoTracking().Where(c => c.ChildGuid != child.ChildGuid).Select(c => c.Name.ToUpper()).ToList();
            if (childrenName.Contains(child.Name.ToUpper()))
            {
                throw new ChildAlreadyExistsException();
            }

            var efChild = context.Children.FirstOrDefault(c => c.ChildGuid == child.ChildGuid);

            if (efChild != null)
            {
                efChild.Name = child.Name;
                efChild.BirthDate = child.BirthDate;
                efChild.BirthWeek = child.BirthWeek;
                efChild.Picture = child.Picture;

                context.SaveChanges();
            }
        }

        private Child ConvertToDomain(EfData.Models.Child child)
        {
            if (child == null)
            {
                return null;
            }

            return new Child(child.ChildGuid, child.BirthDate, child.BirthWeek,
                child.Name, child.Picture);
        }
    }
}
