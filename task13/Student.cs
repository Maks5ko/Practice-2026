using System.Collections.Generic;

namespace task13
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Subject> Grades { get; set; }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) throw new InvalidOperationException("Имя не может быть пустым.");
            if (string.IsNullOrWhiteSpace(LastName)) throw new InvalidOperationException("Фамилия не может быть пустой.");
            if (BirthDate > DateTime.Now) throw new InvalidOperationException("Дата рождения не может быть в будущем.");
            if (BirthDate < new DateTime(1900, 1, 1)) throw new InvalidOperationException("Дата рождения слишком старая.");
            if (Grades != null)
            {
                foreach (var subject in Grades)
                {
                    if (string.IsNullOrWhiteSpace(subject.Name))
                        throw new InvalidOperationException("Название предмета (Name) не может быть пустым.");

                    if (subject.Grade < 2 || subject.Grade > 5)
                        throw new InvalidOperationException(
                            $"Оценка по предмету '{subject.Name}' должна быть в диапазоне 2-5, получено: {subject.Grade}");
                }
            }
        }
    }
}
