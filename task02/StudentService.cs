using System;
using System.Collections.Generic;
namespace task02
{
    public class StudentService
    {
        private readonly List<Student> _students;

        public StudentService(List<Student> students) => _students = students;

        // 1. Возвращает студентов указанного факультета
        public IEnumerable<Student> GetStudentsByFaculty(string faculty)
            => from s in _students where s.Faculty == faculty select s;

        // 2. Возвращает студентов со средним баллом >= minAverageGrade
        public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
            => from s in _students where s.Grades.Count > 0 && s.Grades.Average() >= minAverageGrade select s;

        // 3. Возвращает студентов, отсортированных по имени (A-Z)
        public IEnumerable<Student> GetStudentsOrderedByName()
            => from s in _students orderby s.Name select s;

        // 4. Группировка по факультету
        public ILookup<string, Student> GroupStudentsByFaculty()
            => _students.ToLookup(s => s.Faculty);

        // 5. Находит факультет с максимальным средним баллом
        public string? GetFacultyWithHighestAverageGrade()
        {
            if (_students.Count == 0) return null;
            var facultyAvg = from s in _students
                             where s.Grades.Count > 0
                             group s by s.Faculty into g
                             select new
                             {
                                 faculty = g.Key,
                                 AvgGrade = g.Average(s => s.Grades.Average())
                             };
            return (from f in facultyAvg
                    orderby f.AvgGrade descending
                    select f.faculty).FirstOrDefault();
        }
    }
}
