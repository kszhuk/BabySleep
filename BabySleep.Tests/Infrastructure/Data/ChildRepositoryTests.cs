using BabySleep.Common.Exceptions.Child;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Repositories;
using BabySleep.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BabySleep.Tests.Infrastructure.Data
{
    [Collection("Non-Parallel Collection")]
    public class ChildRepositoryTests
    {
        [Theory]
        [ClassData(typeof(GetChildrenGenerator))]
        public void AddTest(Child child)
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                Assert.Equal(0, context.Children.Count());

                var repository = new ChildRepository(context);
                repository.Add(child);
                Assert.Equal(1, context.Children.Count());

                var contextChild = context.Children.First();
                Assert.Equal(child.Name, contextChild.Name);
                Assert.Equal(child.BirthDate, contextChild.BirthDate);
                Assert.Equal(child.BirthWeek, contextChild.BirthWeek);
                Assert.Equal(child.Picture, contextChild.Picture);
            }
        }

        [Fact]
        public void AddExceptionTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();
                var child = children[0];

                Assert.Equal(0, context.Children.Count());

                repository.Add(child);
                Assert.Equal(1, context.Children.Count());

                Assert.Throws<ChildAlreadyExistsException>(() => repository.Add(child));
            }
        }

        [Fact]
        public void CountTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                Assert.False(repository.Any());
                Assert.Equal(0, repository.Count());

                for (int i = 0; i < children.Count; i++)
                {
                    repository.Add(children[i]);
                    Assert.True(repository.Any());
                    Assert.Equal(i + 1, repository.Count());
                }

                context.Children.RemoveRange(context.Children.ToArray());
                context.SaveChanges();

                Assert.False(repository.Any());
                Assert.Equal(0, repository.Count());
            }
        }

        [Fact]
        public void DeleteTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                Assert.Equal(0, context.Children.Count());

                for (int i = 0; i < children.Count; i++)
                {
                    repository.Add(children[i]);
                }
                Assert.Equal(children.Count, context.Children.Count());

                var contextChildren = context.Children.ToList();
                for (int i = contextChildren.Count - 1; i > 0; i--)
                {
                    repository.Delete(contextChildren[i].ChildGuid);
                    Assert.Equal(i, repository.Count());
                }
            }
        }

        [Fact]
        public void DeleteLastChildExceptionTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                Assert.Equal(0, context.Children.Count());
                repository.Add(children[0]);
                Assert.Equal(1, context.Children.Count());

                Assert.Throws<DeleteLastChildException>(() => repository.Delete(context.Children.First().ChildGuid));
            }
        }

        [Fact]
        public void DeleteWrongChildTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                repository.Add(children[0]);
                repository.Add(children[1]);
                Assert.Equal(2, context.Children.Count());

                repository.Delete(Guid.NewGuid());
                Assert.Equal(2, context.Children.Count());
            }
        }

        [Fact]
        public void GetTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                for (int i = 0; i < children.Count; i++)
                {
                    repository.Add(children[i]);
                }

                var contextChildren = context.Children.ToList();

                foreach (var contextChild in contextChildren)
                {
                    var child = repository.Get(contextChild.ChildGuid);
                    Assert.Equal(child.Name, contextChild.Name);
                    Assert.Equal(child.BirthDate, contextChild.BirthDate);
                    Assert.Equal(child.BirthWeek, contextChild.BirthWeek);
                    Assert.Equal(child.Picture, contextChild.Picture);
                }
            }
        }

        [Fact]
        public void GetNullTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                for (int i = 0; i < children.Count; i++)
                {
                    repository.Add(children[i]);
                }

                var child = repository.Get(Guid.NewGuid());
                Assert.Null(child);
            }
        }

        [Fact]
        public void GetAllTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                for (int i = 0; i < children.Count; i++)
                {
                    repository.Add(children[i]);
                }

                var contextChildren = context.Children.OrderBy(c => c.Name).ToList();
                var repositoryChildren = repository.GetAll();
                Assert.Equal(contextChildren.Count, repositoryChildren.Count);

                for (int i = 0; i < contextChildren.Count; i++)
                {
                    Assert.Equal(repositoryChildren[i].ChildGuid, contextChildren[i].ChildGuid);
                    Assert.Equal(repositoryChildren[i].Name, contextChildren[i].Name);
                    Assert.Equal(repositoryChildren[i].BirthDate, contextChildren[i].BirthDate);
                    Assert.Equal(repositoryChildren[i].BirthWeek, contextChildren[i].BirthWeek);
                    Assert.Equal(repositoryChildren[i].Picture, contextChildren[i].Picture);
                }
            }
        }

        [Fact]
        public void GetFirstTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                for (int i = 0; i < children.Count; i++)
                {
                    repository.Add(children[i]);
                }

                var contextChild = context.Children.OrderBy(c => c.Name).First();
                var child = repository.GetFirst();
                Assert.Equal(child.Name, contextChild.Name);
                Assert.Equal(child.BirthDate, contextChild.BirthDate);
                Assert.Equal(child.BirthWeek, contextChild.BirthWeek);
                Assert.Equal(child.Picture, contextChild.Picture);
            }
        }

        [Fact]
        public void GetFirstNullTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                var child = repository.GetFirst();
                Assert.Null(child.Name);
                Assert.Equal(Guid.Empty, child.ChildGuid);
            }
        }

        [Fact]
        public void UpdateTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                for (int i = 0; i < children.Count; i++)
                {
                    repository.Add(children[i]);
                }

                var guid = context.Children.First().ChildGuid;
                var name = "test update";
                byte[] picture = new byte[] { 0x30, 0x40 };
                short? birthWeek = 32;
                var birthDate = DateTime.Now.AddDays(-10);

                var updatedChild = ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate);
                repository.Update(updatedChild);

                var contextChild = context.Children.First(c => c.ChildGuid == guid);

                Assert.Equal(updatedChild.Name, contextChild.Name);
                Assert.Equal(updatedChild.BirthDate, contextChild.BirthDate);
                Assert.Equal(updatedChild.BirthWeek, contextChild.BirthWeek);
                Assert.Equal(updatedChild.Picture, contextChild.Picture);
            }
        }

        [Fact]
        public void UpdateChildAlreadyExistsExceptionTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new ChildRepository(context);

                var generator = new GetChildrenGenerator();
                var children = generator.GetTestChildren();

                for (int i = 0; i < children.Count; i++)
                {
                    repository.Add(children[i]);
                }

                var guid = context.Children.First().ChildGuid;
                var name = context.Children.First(c => c.ChildGuid != guid).Name;
                byte[] picture = new byte[] { 0x30, 0x40 };
                short? birthWeek = 32;
                var birthDate = DateTime.Now.AddDays(-10);

                var updatedChild = ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate);
                Assert.Throws<ChildAlreadyExistsException>(() => repository.Update(updatedChild));
            }
        }

        public class GetChildrenGenerator : TheoryData<Child>
        {
            public GetChildrenGenerator()
            {
                foreach (var child in GetTestChildren())
                {
                    this.Add(child);
                }
            }

            public List<Child> GetTestChildren()
            {
                var children = new List<Child>();

                var guid = Guid.NewGuid();
                var name = "test3";
                byte[] picture = null;
                short? birthWeek = null;
                var birthDate = DateTime.Now.AddDays(-1);
                var child = ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate);
                children.Add(child);

                guid = Guid.NewGuid();
                name = "test2";
                picture = new byte[] { 0x20, 0x20 };
                children.Add(ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate));

                guid = Guid.NewGuid();
                birthWeek = 25;
                name = "test1";
                children.Add(ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate));

                guid = Guid.NewGuid();
                birthDate = DateTime.Now.AddMonths(-1).AddDays(-1);
                name = "test4";
                children.Add(ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate));

                return children;
            }
        }
    }
}
