using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FastMapper.Adapters;

namespace FastMapper.Tests
{
    #region Test Objects

    public enum Departments
    {
        Finance = 1,
        IT = 2,
        Sales = 3
    }

    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Department { get; set; }
    }

    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Departments Department { get; set; }
    }

    #endregion

    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void MapEnum()
        {
            TypeAdapterConfig<Employee, EmployeeDTO>
                .NewConfig()
                .MapFrom(dest => dest.Department, src => (Departments)src.Department);

            var employee = new Employee() { Id = Guid.NewGuid(), Name = "Timuçin", Surname = "KIVANÇ", Department = (int)Departments.IT  };

            var dto = ClassAdapter<Employee, EmployeeDTO>.Adapt(employee);

            Assert.IsNotNull(dto);
          
            Assert.IsTrue(dto.Id == employee.Id &&
                dto.Name == employee.Name &&
                dto.Department == Departments.IT);
       
        }
    }
}
