using task13;
using Xunit;

namespace task13tests
{
    public class StudentValidationTests
    {
        [Fact]
        public void Deserialize_EmptyFirstName_ThrowsInvalidOperationException()
        {
            var invalidJson = @"
            {
                ""FirstName"": """",
                ""LastName"": ""Петров"",
                ""BirthDate"": ""2000-05-15"",
                ""Grades"": []
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentSerializer.Deserialize(invalidJson));
        }

        [Fact]
        public void Deserialize_EmptyLastName_ThrowsInvalidOperationException()
        {
            var invalidJson = @"
            {
                ""FirstName"": ""Иван"",
                ""LastName"": """",
                ""BirthDate"": ""2000-05-15"",
                ""Grades"": []
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentSerializer.Deserialize(invalidJson));
        }

        [Fact]
        public void Deserialize_MissingFirstName_ThrowsInvalidOperationException()
        {
            var invalidJson = @"
            {
                ""LastName"": ""Петров"",
                ""BirthDate"": ""2000-05-15"",
                ""Grades"": []
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentSerializer.Deserialize(invalidJson));
        }

        [Fact]
        public void Deserialize_FutureBirthDate_ThrowsInvalidOperationException()
        {
            var invalidJson = @"
            {
                ""FirstName"": ""Иван"",
                ""LastName"": ""Петров"",
                ""BirthDate"": ""2030-05-15"",
                ""Grades"": []
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentSerializer.Deserialize(invalidJson));
        }

        [Fact]
        public void Deserialize_TooOldBirthDate_ThrowsInvalidOperationException()
        {
            var invalidJson = @"
            {
                ""FirstName"": ""Иван"",
                ""LastName"": ""Петров"",
                ""BirthDate"": ""1890-01-01"",
                ""Grades"": []
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentSerializer.Deserialize(invalidJson));
        }

        [Fact]
        public void Deserialize_SubjectWithEmptyName_ThrowsInvalidOperationException()
        {
            var invalidJson = @"
            {
                ""FirstName"": ""Иван"",
                ""LastName"": ""Петров"",
                ""BirthDate"": ""2000-05-15"",
                ""Grades"": [
                    { ""Name"": """", ""Grade"": 4 }
                ]
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentSerializer.Deserialize(invalidJson));
        }

        [Fact]
        public void Deserialize_SubjectWithInvalidGrade_ThrowsInvalidOperationException()
        {
            var invalidJson = @"
            {
                ""FirstName"": ""Иван"",
                ""LastName"": ""Петров"",
                ""BirthDate"": ""2000-05-15"",
                ""Grades"": [
                    { ""Name"": ""Математика"", ""Grade"": 6 }
                ]
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentSerializer.Deserialize(invalidJson));
        }

        [Fact]
        public void Deserialize_WithNullGrades_ShouldPassValidation()
        {
            var json = @"
            {
                ""FirstName"": ""Иван"",
                ""LastName"": ""Петров"",
                ""BirthDate"": ""2000-05-15"",
                ""Grades"": null
            }";

            var student = StudentSerializer.Deserialize(json);
            Assert.Null(student.Grades);
        }
    }
}
