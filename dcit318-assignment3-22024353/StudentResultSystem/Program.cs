using System;
using System.Collections.Generic;
using System.IO;

namespace StudentResultSystem
{
    // Student class
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Score { get; set; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80)
                return "A";
            else if (Score >= 70)
                return "B";
            else if (Score >= 60)
                return "C";
            else if (Score >= 50)
                return "D";
            else
                return "F";
        }
    }

    // Custom exception for invalid score
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message)
            : base(message)
        {
        }
    }

    // Custom exception for missing fields
    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message)
            : base(message)
        {
        }
    }

    // Student result processor
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string filePath)
        {
            List<Student> students = new List<Student>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = line.Split(',');

                    if (fields.Length < 3)
                    {
                        throw new MissingFieldException(
                            "A student record is missing required fields.");
                    }

                    if (!int.TryParse(fields[0].Trim(), out int id))
                    {
                        throw new InvalidScoreFormatException(
                            "Invalid student ID format.");
                    }

                    string fullName = fields[1].Trim();

                    if (string.IsNullOrWhiteSpace(fullName))
                    {
                        throw new MissingFieldException(
                            "Student name is missing.");
                    }

                    if (!int.TryParse(fields[2].Trim(), out int score))
                    {
                        throw new InvalidScoreFormatException(
                            $"Invalid score format for {fullName}.");
                    }

                    if (score < 0 || score > 100)
                    {
                        throw new InvalidScoreFormatException(
                            $"Score for {fullName} must be between 0 and 100.");
                    }

                    students.Add(
                        new Student(id, fullName, score));
                }
            }

            return students;
        }

        public void WriteReportToFile(
            List<Student> students,
            string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("===== STUDENT RESULT REPORT =====");
                writer.WriteLine();

                foreach (Student student in students)
                {
                    writer.WriteLine(
                        $"{student.FullName} " +
                        $"(ID: {student.Id}): " +
                        $"Score = {student.Score}, " +
                        $"Grade = {student.GetGrade()}");
                }
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            string inputFile = "students.txt";
            string outputFile = "student_report.txt";

            try
            {
                // Create sample student data
                File.WriteAllLines(
                    inputFile,
                    new string[]
                    {
                        "1,Ama Mensah,85",
                        "2,Kwame Asante,72",
                        "3,Abena Owusu,65",
                        "4,Kofi Mensah,54",
                        "5,Akosua Boateng,45"
                    });

                StudentResultProcessor processor =
                    new StudentResultProcessor();

                List<Student> students =
                    processor.ReadStudentsFromFile(inputFile);

                processor.WriteReportToFile(
                    students,
                    outputFile);

                foreach (Student student in students)
                {
                    Console.WriteLine(
                        $"{student.FullName} " +
                        $"(ID: {student.Id}): " +
                        $"Score = {student.Score}, " +
                        $"Grade = {student.GetGrade()}");
                }

                Console.WriteLine();
                Console.WriteLine(
                    "Student report created successfully.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(
                    $"File Error: {ex.Message}");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine(
                    $"Score Error: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine(
                    $"Missing Field Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error: {ex.Message}");
            }
        }
    }
}