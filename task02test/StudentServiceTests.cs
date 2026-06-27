using Xunit;
using task02;
namespace task02tests
{
    public class StudentServiceTests
    {
        private List<Student> _testStudents;
        private StudentService _service;

        public StudentServiceTests()
        {
            _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } },
            new() { Name = "Павел", Faculty = "Экономика", Grades = new List<int> {3, 3 , 4} }
        };
            _service = new StudentService(_testStudents);
        }

        [Fact]
        public void GetStudentsByFaculty_ReturnsCorrectStudents()
        {
            var result = _service.GetStudentsByFaculty("ФИТ").ToList();
            Assert.Equal(2, result.Count);
            Assert.True(result.All(s => s.Faculty == "ФИТ"));
        }
        [Fact]
        public void GetStudentsByFaculty_ReturnsEmpty()
        {
            var emptyService = new StudentService(new List<Student>());
            var result = emptyService.GetStudentsByFaculty("ФИТ");
            Assert.Empty(result);
        }
        [Fact]
        public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
        {
            var result = _service.GetStudentsWithMinAverageGrade(4.4).ToList();
            Assert.Equal(2, result.Count);
            Assert.Contains(result, s => s.Name == "Иван");
            Assert.Contains(result, s => s.Name == "Петр");
            Assert.DoesNotContain(result, s => s.Name == "Анна");
            Assert.DoesNotContain(result, s => s.Name == "Павел");
        }
        [Fact]
        public void GetStudentsWithMinAverageGrade_ReturnsAllWithGrades()
        {
            var result = _service.GetStudentsWithMinAverageGrade(0).ToList();
            Assert.Equal(4, result.Count);
        }
        [Fact]
        public void GetStudentsWithMinAverageGrade_ReturnsEmpty()
        {
            var emptyService = new StudentService(new List<Student>());
            var result = emptyService.GetStudentsWithMinAverageGrade(4.0);
            Assert.Empty(result);
        }
        [Fact]
        public void GetStudentsOrderedByName_ReturnsCorrectStudents()
        {
            var result = _service.GetStudentsOrderedByName().ToList();
            var expectedNames = _testStudents.OrderBy(s => s.Name).Select(s => s.Name).ToList();
            Assert.Equal(expectedNames, result.Select(s => s.Name).ToList());
            Assert.Equal("Анна", result.First().Name);
            Assert.Equal("Петр", result.Last().Name);
        }
        [Fact]
        public void GetStudentsOrderedByName_ReturnsEmpty()
        {
            var emptyService = new StudentService(new List<Student>());
            var result = emptyService.GetStudentsOrderedByName();
            Assert.Empty(result);
        }
        [Fact]
        public void GroupStudentsByFaculty_ReturnsCorrectGroups()
        {
            var lookup = _service.GroupStudentsByFaculty();
            Assert.Equal(2, lookup.Count);

            var fitStudents = lookup["ФИТ"].ToList();
            Assert.Equal(2, fitStudents.Count);
            Assert.All(fitStudents, s => Assert.Equal("ФИТ", s.Faculty));

            var econStudents = lookup["Экономика"].ToList();
            Assert.Equal(2, econStudents.Count);
            Assert.All(econStudents, s => Assert.Equal("Экономика", s.Faculty));
        }

        [Fact]
        public void GroupStudentsByFaculty_ReturnsEmpty()
        {
            var emptyService = new StudentService(new List<Student>());
            var lookup = emptyService.GroupStudentsByFaculty();
            Assert.Empty(lookup);
        }
        [Fact]
        public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
        {
            var result = _service.GetFacultyWithHighestAverageGrade();
            Assert.Equal("Экономика", result);
        }
        [Fact]
        public void GetFacultyWithHighestAverageGrade_WhenFacultyWithEmptyGrades()
        {
            var students = _testStudents.ToList();
            students.Add(new Student { Name = "Владимир", Faculty = "ФИТ", Grades = new List<int>() });
            var service = new StudentService(students);
            var result = service.GetFacultyWithHighestAverageGrade();
            Assert.Equal("Экономика", result);
        }
        [Fact]
        public void GetFacultyWithHighestAverageGrade_ReturnsNull()
        {
            var emptyService = new StudentService(new List<Student>());
            var result = emptyService.GetFacultyWithHighestAverageGrade();
            Assert.Null(result);
        }
    }
}
