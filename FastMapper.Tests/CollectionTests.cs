﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using FastMapper.Adapters;

namespace FastMapper.Tests
{
    #region Test Objects

    public class XProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class YProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum Projects
    {
        A = 1,
        B = 2,
        C = 3
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Projects Project { get; set; }

        public int[] X { get; set; }
        public List<int> Y { get; set; }        
        public ArrayList Z { get; set; }
        public ICollection<Guid> Ids { get; set; }
        public Nullable<Guid> CityId { get; set; }
        public byte[] Picture { get; set; }
        public List<string> Countries { get; set; }
        public ICollection<string> XX { get; set; }
        public List<int> YY { get; set; }
        public IList<int> ZZ { get; set; }
        public Departments[] RelatedDepartments { get; set; }
        public ICollection<XProject> Projects { get; set; }
    }

    public class PersonDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Projects Project { get; set; }
        public List<int> X { get; set; }
        public int[] Y { get; set; }
        public ICollection<Guid> Z { get; set; }
        public ArrayList Ids { get; set; }
        public Nullable<Guid> CityId { get; set; }
        public byte[] Picture { get; set; }
        public ICollection<string> Countries { get; set; }
        public IEnumerable<string> XX { get; set; }
        public IList<int> YY { get; set; }
        public List<int> ZZ { get; set; }
        public Departments[] RelatedDepartments { get; set; }
        public List<YProject> Projects { get; set; }

    }

    #endregion

    [TestClass]
    public class CollectionTests
    {
        [TestMethod]
        public void MapCollectionProperty()
        {
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Timuçin",
                Surname = "KIVANÇ",
                Project = Projects.A,
                X = new int[] { 1, 2, 3, 4 },
                Y = new List<int>() { 5, 6, 7 },
                Z = new ArrayList((ICollection)(new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() })),
                Ids = new List<Guid>() { Guid.Empty, Guid.NewGuid() },
                CityId = Guid.NewGuid(),
                Picture = new byte[] { 0, 1, 2 },
                Countries = new List<string> { "Turkey", "Germany" },
                XX = new List<string> { "Nederland", "USA" },
                YY = new List<int> { 22, 33 },
                ZZ = new List<int> { 44, 55 },
                RelatedDepartments = new Departments[] { Departments.IT, Departments.Finance }
            };

            var dto = ClassAdapter<Person, PersonDTO>.Adapt(person);
            Assert.IsNotNull(dto);
            Assert.IsTrue(dto.Id == person.Id && 
                dto.Name == person.Name &&
                dto.Project == person.Project);

            Assert.IsNotNull(dto.X);
            Assert.IsTrue(dto.X.Count == 4 && dto.X[0] == 1 && dto.X[1] == 2 && dto.X[2] == 3 && dto.X[3] == 4);

            Assert.IsNotNull(dto.Y);
            Assert.IsTrue(dto.Y.Length == 3 && dto.Y[0] == 5 && dto.Y[1] == 6 && dto.Y[2] == 7);

            Assert.IsNotNull(dto.Z);
            Assert.IsTrue(dto.Z.Count == 2 && dto.Z.Contains((Guid)person.Z[0]) && dto.Z.Contains((Guid)person.Z[1]));

            Assert.IsNotNull(dto.Ids);
            Assert.IsTrue(dto.Ids.Count == 2);

            Assert.IsTrue(dto.CityId == person.CityId);

            Assert.IsNotNull(dto.Picture);
            Assert.IsTrue(dto.Picture.Length == 3 && dto.Picture[0] == 0 && dto.Picture[1] == 1 && dto.Picture[2] == 2);

            Assert.IsNotNull(dto.Countries);
            Assert.IsTrue(dto.Countries.Count == 2 && dto.Countries.First() == "Turkey" && dto.Countries.Last() == "Germany");

            Assert.IsNotNull(dto.XX);
            Assert.IsTrue(dto.XX.Count() == 2 && dto.XX.First() == "Nederland" && dto.XX.Last() == "USA");

            Assert.IsNotNull(dto.YY);
            Assert.IsTrue(dto.YY.Count == 2 && dto.YY.First() == 22 && dto.YY.Last() == 33);

            Assert.IsNotNull(dto.ZZ);
            Assert.IsTrue(dto.ZZ.Count == 2 && dto.ZZ.First() == 44 && dto.ZZ.Last() == 55);

            Assert.IsNotNull(dto.RelatedDepartments);
            Assert.IsTrue(dto.RelatedDepartments.Length == 2 && dto.RelatedDepartments[0] == Departments.IT && dto.RelatedDepartments[1] == Departments.Finance);           
        }

        [TestMethod]
        public void MapCollection()
        {
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Timuçin",
                Surname = "KIVANÇ",
                Project = Projects.A,
                X = new int[] { 1, 2, 3, 4 },
                Y = new List<int>() { 5, 6, 7 },
                Z = new ArrayList((ICollection)(new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() })),
                Ids = new List<Guid>() { Guid.Empty, Guid.NewGuid() },
                CityId = Guid.NewGuid(),
                Picture = new byte[] { 0, 1, 2 },
                Countries = new List<string> { "Turkey", "Germany" },
                XX = new List<string> { "Nederland", "USA" },
                YY = new List<int> { 22, 33 },
                ZZ = new List<int> { 44, 55 },
                RelatedDepartments = new Departments[] { Departments.IT, Departments.Finance },
                Projects = new List<XProject>() { new XProject { Id = 1, Name = "Project X" } }
            };

            var persons = new List<Person>() { person };

            var dtos = (Person[])CollectionAdapter<List<Person>, Person, Person[]>.Adapt(persons);

            Assert.IsNotNull(dtos);
            Assert.IsTrue(dtos.Length == 1);
            Assert.IsTrue(dtos.First().Id == person.Id &&
                dtos.First().Name == "Timuçin" &&
                dtos.First().Surname == "KIVANÇ");

            Assert.IsNotNull(dtos[0].Projects);

            Assert.IsTrue(dtos[0].Projects.First().Id == 1 && dtos[0].Projects.First().Name == "Project X");
        }
    }
}
