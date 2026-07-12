using System.Text.Json;
using task13;
using Xunit;

namespace task13tests
{
    public class StudentSerializerTests : IDisposable
    {
        private readonly string _tempFile;

        public StudentSerializerTests()
        {
            _tempFile = Path.GetTempFileName();
        }

        public void Dispose()
        {
            if (File.Exists(_tempFile)) File.Delete(_tempFile);
        }

        private Student GetTestStudent()
        {
            return new Student
            {
                FirstName = "Иван",
                LastName = "Петров",
                BirthDate = new DateTime(2000, 5, 15),
                Grades = new()
                {
                    new Subject { Name = "Математика", Grade = 5 },
                    new Subject { Name = "Физика", Grade = 4 }
                }
            };
        }

        [Fact]
        public void Serialize_ShouldIgnoreNullValues()
        {
            var student = new Student
            {
                FirstName = "Иван",
                LastName = "Петров",
                BirthDate = DateTime.Now,
                Grades = null
            };

            var json = StudentSerializer.Serialize(student);
            Assert.DoesNotContain("\"Grades\":", json);
        }

        [Fact]
        public void Serialize_WithManyGrades_WorksCorrectly()
        {
            var student = new Student
            {
                FirstName = "Иван",
                LastName = "Петров",
                BirthDate = new DateTime(2000, 5, 15),
                Grades = Enumerable.Range(1, 100)
                    .Select(i => new Subject { Name = $"Subject{i}", Grade = i % 4 + 2 }).ToList()
            };

            var json = StudentSerializer.Serialize(student);
            var restored = StudentSerializer.Deserialize(json);
            Assert.Equal(100, restored.Grades.Count);
        }

        [Fact]
        public void Serialize_WithEmptyGrades_ShouldSerializeAsEmptyArray()
        {
            var student = new Student
            {
                FirstName = "Иван",
                LastName = "Петров",
                BirthDate = new DateTime(2000, 5, 15),
                Grades = new List<Subject>()
            };

            var json = StudentSerializer.Serialize(student);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var gradesProperty = root.GetProperty("Grades");
            Assert.Equal(JsonValueKind.Array, gradesProperty.ValueKind);
            Assert.Equal(0, gradesProperty.GetArrayLength());
        }

        [Fact]
        public void Serialize_ShouldUseCustomDateFormat()
        {
            var student = GetTestStudent();
            var json = StudentSerializer.Serialize(student);
            Assert.Contains("2000-05-15", json);
        }

        [Fact]
        public void Serialize_ShouldIgnoreTimePart()
        {
            var student = new Student
            {
                FirstName = "Иван",
                LastName = "Петров",
                BirthDate = new DateTime(2000, 5, 15, 14, 30, 0),
                Grades = new List<Subject>()
            };
            var json = StudentSerializer.Serialize(student);
            Assert.Contains("2000-05-15", json);
            Assert.DoesNotContain("T14:30:00", json);
        }

        [Fact]
        public void Deserialize_ShouldRestoreObjectCorrectly()
        {
            var original = GetTestStudent();
            var json = StudentSerializer.Serialize(original);
            var restored = StudentSerializer.Deserialize(json);

            Assert.Equal(original.FirstName, restored.FirstName);
            Assert.Equal(original.LastName, restored.LastName);
            Assert.Equal(original.BirthDate, restored.BirthDate);
            Assert.Equal(original.Grades.Count, restored.Grades.Count);
            Assert.Equal(original.Grades[0].Name, restored.Grades[0].Name);
        }

        [Fact]
        public void Deserialize_InvalidDateFormat_ThrowsJsonException()
        {
            var invalidJson = @"
            {
                ""FirstName"": ""Иван"",
                ""LastName"": ""Петров"",
                ""BirthDate"": ""15-05-2000"",
                ""Grades"": []
            }";

            Assert.Throws<JsonException>(() => StudentSerializer.Deserialize(invalidJson));
        }

        [Fact]
        public void Deserialize_WithExtraFields_ShouldIgnoreThem()
        {
            var json = @"
            {
                ""FirstName"": ""Иван"",
                ""LastName"": ""Петров"",
                ""BirthDate"": ""2000-05-15"",
                ""Grades"": [],
                ""ExtraField"": ""ignored""
            }";

            var student = StudentSerializer.Deserialize(json);
            Assert.Equal("Иван", student.FirstName);
            Assert.Equal("Петров", student.LastName);
        }

        [Fact]
        public void SaveAndLoadFromFile_Roundtrip()
        {
            var original = GetTestStudent();
            StudentSerializer.SaveToFile(original, _tempFile);
            var loaded = StudentSerializer.LoadFromFile(_tempFile);

            Assert.Equal(original.FirstName, loaded.FirstName);
            Assert.Equal(original.LastName, loaded.LastName);
            Assert.Equal(original.BirthDate, loaded.BirthDate);
            Assert.Equal(original.Grades.Count, loaded.Grades.Count);
        }

        [Fact]
        public void LoadFromFile_NonExistentFile_ThrowsFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() =>
                StudentSerializer.LoadFromFile("nonexistent.json"));
        }
    }
}
