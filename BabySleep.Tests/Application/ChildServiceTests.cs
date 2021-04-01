using BabySleep.Application.DTO;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Interfaces;
using BabySleep.Application.Services;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using BabySleep.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Application
{
    [Collection("ChildService")]
    public class ChildServiceTests
    {
        [Theory]
        [ClassData(typeof(GetFirstDataGenerator))]
        public void GetFirstChildTest(Child child, ChildDto childDto)
        {
            var mock = new Mock<IChildRepository>();
            mock.Setup(repo => repo.GetFirst()).Returns(child);

            var assembler = new ChildDtoAssembler();
            var childService = new ChildService(mock.Object, assembler);
            Assert.True(JsonComparer.JsonCompare(childDto, childService.GetFirstChild()));
            Assert.True(JsonComparer.JsonCompare(childDto, assembler.WriteChildDto(child)));
        }

        [Theory]
        [ClassData(typeof(GetFirstDataGenerator))]
        public void GetChildTest(Child child, ChildDto childDto)
        {
            var mock = new Mock<IChildRepository>();
            mock.Setup(repo => repo.Get(childDto.ChildGuid)).Returns(child);

            var assembler = new ChildDtoAssembler();
            var childService = new ChildService(mock.Object, assembler);
            Assert.True(JsonComparer.JsonCompare(childDto, childService.GetChild(childDto.ChildGuid)));
            Assert.True(JsonComparer.JsonCompare(childDto, assembler.WriteChildDto(child)));
        }

        [Fact]
        public void GetChildrenTest()
        {
            var dataGenerator = new GetFirstDataGenerator();
            var testChildren = dataGenerator.GetTestChildren();

            var children = new List<Child>();
            var childrenDto = new List<ChildDto>();

            foreach (var child in testChildren)
            {
                childrenDto.Add(child.Item2);
                if (child.Item1 == null)
                {
                    children.Add(new Child());
                    continue;
                }
                children.Add(child.Item1);
            }

            var mock = new Mock<IChildRepository>();
            mock.Setup(repo => repo.GetAll()).Returns(children);

            var assembler = new ChildDtoAssembler();
            var childService = new ChildService(mock.Object, assembler);

            var serviceChildren = childService.GetChildren();
            var assemblerChildren = assembler.WriteChildrenDto(children);

            Assert.Equal(childrenDto.Count, serviceChildren.Count);
            Assert.Equal(childrenDto.Count, assemblerChildren.Count);

            foreach (var childDto in childrenDto)
            {
                Assert.True(JsonComparer.JsonCompare(childDto, serviceChildren.First(c => c.ChildGuid == childDto.ChildGuid)));
                Assert.True(JsonComparer.JsonCompare(childDto, assemblerChildren.First(c => c.ChildGuid == childDto.ChildGuid)));
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(250)]
        public void GetChildrenCountTest(int count)
        {
            var mock = new Mock<IChildRepository>();
            mock.Setup(repo => repo.Count()).Returns(count);

            var childService = new ChildService(mock.Object, new ChildDtoAssembler());
            Assert.Equal(count, childService.GetChildrenCount());
        }

        [Theory]
        [ClassData(typeof(GetFirstDataGenerator))]
        public void DeleteChildTest(Child child, ChildDto childDto)
        {
            if (child == null)
            {
                return;
            }

            var childGuid = child.ChildGuid;

            var mock = new Mock<IChildRepository>();
            mock.Setup(repo => repo.Delete(childGuid));

            var childService = new ChildService(mock.Object, new ChildDtoAssembler());
            childService.DeleteChild(childGuid);

            mock.Verify(repo => repo.Delete(childGuid), Times.Once());
        }

        [Theory]
        [ClassData(typeof(GetFirstDataGenerator))]
        public void SaveChildTest(Child child, ChildDto childDto)
        {
            if (childDto.Name == null)
            {
                childDto.Name = "";
            }

            var mock = new Mock<IChildRepository>();

            if (child == null)
            {
                mock.Setup(repo => repo.Add(child));
            }
            else
            {
                mock.Setup(repo => repo.Update(child));
            }

            var childService = new ChildService(mock.Object, new ChildDtoAssembler());
            childService.SaveChild(childDto);

            if (child == null)
            {
                mock.Verify(repo => repo.Add(It.Is<Child>(s => s.ChildGuid == childDto.ChildGuid)), Times.Once());
            }
            else
            {
                mock.Verify(repo => repo.Update(It.Is<Child>(s => s.ChildGuid == childDto.ChildGuid)), Times.Once());
            }
        }

        public class GetFirstDataGenerator : TheoryData<Child, ChildDto>
        {
            public GetFirstDataGenerator()
            {
                var children = GetTestChildren();
                foreach (var child in children)
                {
                    this.Add(child.Item1, child.Item2);
                }
            }

            public List<Tuple<Child, ChildDto>> GetTestChildren()
            {
                var children = new List<Tuple<Child, ChildDto>>();

                children.Add(Tuple.Create<Child, ChildDto>(null,
                    ChildHelper.FillChildDto(Guid.Empty, null, null, null, new DateTime(), new Child().Age)
                ));

                var guid = Guid.NewGuid();
                var name = "test1";
                byte[] picture = null;
                short? birthWeek = null;
                var birthDate = DateTime.Now.AddDays(-1);
                var age = "Newborn";
                children.Add(Tuple.Create(
                    ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate),
                    ChildHelper.FillChildDto(guid, name, picture, birthWeek, birthDate, age))
                );

                guid = Guid.NewGuid();
                picture = new byte[] { 0x20, 0x20 };
                children.Add(Tuple.Create(
                    ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate),
                    ChildHelper.FillChildDto(guid, name, picture, birthWeek, birthDate, age))
                );

                guid = Guid.NewGuid();
                birthWeek = 25;
                children.Add(Tuple.Create(
                    ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate),
                    ChildHelper.FillChildDto(guid, name, picture, birthWeek, birthDate, age))
                );

                guid = Guid.NewGuid();
                birthDate = DateTime.Now.AddMonths(-1).AddDays(-1);
                age = "1 month(s)";
                children.Add(Tuple.Create(
                    ChildHelper.FillChild(guid, name, picture, birthWeek, birthDate),
                    ChildHelper.FillChildDto(guid, name, picture, birthWeek, birthDate, age))
                );

                return children;
            }
        }
    }
}
