using System.Globalization;
using System.Text.RegularExpressions;

/*
 * Constants
 */

const uint MIN_SCORE_EXCELLENT = 90;
const uint MIN_SCORE_GOOD = 70;
const uint MIN_SCORE_PASSED = 50;

/*
 * Global variables
 */

List<Student> students = new();

/*
 * Program entry point
 */

main();

/*
 * Functions
 */

void main()
{
    bool running = true;

    while (running)
    {
        Console.WriteLine("Main menu:");
        Console.WriteLine("  1. Register a student");
        Console.WriteLine("  2. Show all students");
        Console.WriteLine("  3. Show passed students");
        Console.WriteLine("  4. Search for student");
        Console.WriteLine("  5. Delete a student");
        Console.WriteLine("  6. Exit program");
        Console.WriteLine();
        Console.Write("Enter a choice: ");

        string userInput = Console.ReadLine().Trim();

        switch (userInput)
        {
            case "1":
                RegisterStudent();
                break;
            case "2":
                ShowAllStudents();
                break;
            case "3":
                ShowPassedStudents();
                break;
            case "4":
                SearchForStudent();
                break;
            case "5":
                DeleteStudent();
                break;
            case "6":
                running = false;
                break;
            default:
                WriteLineError("Invalid menu option. Try again.");
                break;
        }
    }
}

string GetStudentScoreCategory(uint score)
{
    if (score >= MIN_SCORE_EXCELLENT)
        return "Excellent";
    else if (score >= MIN_SCORE_GOOD)
        return "Good";
    else if (score >= MIN_SCORE_PASSED)
        return "Pass";
    else
        return "Fail";
}

bool IsEmailRegistered(string email)
{
    foreach (Student student in students)
    {
        if (student.email.ToLower() == email.ToLower())
        {
            return true;
        }
    }
    return false;
}

void WriteLineWarning(string str)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(str);
    Console.ResetColor();
}

void WriteLineSuccess(string str)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(str);
    Console.ResetColor();
}
void WriteLineError(string str)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(str);
    Console.ResetColor();
}

void RegisterStudent()
{
    Student student = new();

    Console.WriteLine("--- Register Student ---");

    Console.Write("* Name: ");
    student.name = Console.ReadLine().Trim();

    while (true)
    {
        Console.Write("* Email: ");
        student.email = Console.ReadLine().Trim();

        // the regex pattern means:
        // 1. 1 or more non-space/@ chars
        // 2. at-symbol (@)
        // 3. 1 or more non-space/@ chars
        // 4. period (.)
        // 5. 1 or more non-space/@ chars
        const string regexPatternEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (Regex.IsMatch(student.email, regexPatternEmail))
        {
            if (!IsEmailRegistered(student.email))
                break;
            else
                WriteLineError("Email address is already registered!");
        }
        else
            WriteLineError("Invalid email address. Try again.");
    }

    while (true)
    {
        Console.Write("* Score: ");
        string userInput = Console.ReadLine().Trim();

        bool scoreIsUint = uint.TryParse(userInput, CultureInfo.InvariantCulture, out student.score);

        if (scoreIsUint)
        {
            if (student.score >= 0 && student.score <= 100)
                break;
            else
                WriteLineError("Invalid score. Value must be 0 - 100. Try again.");
        }
        else
            WriteLineError("Invalid score. Value must be a non-negative integer. Try again.");
    }

    student.category = GetStudentScoreCategory(student.score);

    students.Add(student);

    WriteLineSuccess("Student has been added!");
}

void ShowAllStudents()
{
    ShowStudents(false);
}

void ShowPassedStudents()
{
    ShowStudents(true);
}

void ShowStudents(bool showOnlyPassed)
{
    if (students.Count < 1)
    {
        WriteLineWarning("No students have been registered yet.");
        return;
    }

    int studentIndex = 0;
    uint scoreTotalAll = 0;
    uint scoreTotalPassed = 0;
    uint numStudentsPassed = 0;
    Student highestScoreStudent = students[0];

    foreach (Student student in students)
    {
        if (!showOnlyPassed || student.score >= MIN_SCORE_PASSED)
        {
            Console.WriteLine($"Student:");
            Console.WriteLine($"  Index: {studentIndex}");
            Console.WriteLine($"  Name: {student.name}");
            Console.WriteLine($"  Email: {student.email}");
            Console.WriteLine($"  Score: {student.score}");
            Console.WriteLine($"  Category: {student.category}");
            Console.WriteLine(); // blank line separator
        }

        if (student.score > highestScoreStudent.score)
            highestScoreStudent = student;

        if (student.score >= MIN_SCORE_PASSED)
        {
            scoreTotalPassed += student.score;
            numStudentsPassed++;
        }

        scoreTotalAll += student.score;
        studentIndex++;
    }

    if (showOnlyPassed && numStudentsPassed < 1)
    {
        WriteLineWarning("No students have passed yet.");
        return;
    }

    Console.WriteLine("Statistics:");
    Console.WriteLine($"  Total students registered : {students.Count}");
    Console.WriteLine($"      Total students passed : {numStudentsPassed}");
    Console.WriteLine($"        Average score (all) : {scoreTotalAll/students.Count}");
    Console.WriteLine($"     Average score (passed) : {(numStudentsPassed > 0 ? scoreTotalPassed/numStudentsPassed : 0)}");

    if (!showOnlyPassed || highestScoreStudent.score >= MIN_SCORE_PASSED)
    {
        Console.Write($"              Highest score : ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{highestScoreStudent.score} ({highestScoreStudent.name} <{highestScoreStudent.email}>)");
        Console.ResetColor();
    }
}

int FindStudent(string searchMethod, string searchString)
{
    int studentIndex = 0;

    foreach (Student student in students)
    {
        if (searchMethod == "name" && student.name.ToLower() == searchString.ToLower())
            return studentIndex;
        else if (searchMethod == "email" && student.email.ToLower() == searchString.ToLower())
            return studentIndex;
        studentIndex++;
    }

    return -1;
}

void SearchForStudent()
{
    if (students.Count < 1)
    {
        WriteLineWarning("No students have been registered yet.");
        return;
    }

    bool running = true;

    while (running)
    {
        Console.WriteLine("Search For Student - menu:");
        Console.WriteLine("  1. Search by name (case-insensitive)");
        Console.WriteLine("  2. Search by email (case-insensitive)");
        Console.WriteLine("  3. Exit this menu");
        Console.WriteLine();
        Console.Write("Enter a choice: ");

        string userInput = Console.ReadLine().Trim();

        switch (userInput)
        {
            case "1": // fall through
            case "2":
                string searchMethod = userInput == "2" ? "email" : "name";

                Console.Write($"Enter {searchMethod} to search for: ");
                string searchString = Console.ReadLine().Trim();

                int studentIndex = FindStudent(searchMethod, searchString);

                if (studentIndex != -1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Found student via {searchMethod}!");
                    Console.ResetColor();
                    Console.WriteLine($"  Index: {studentIndex}");
                    Console.WriteLine($"  Name: {students[studentIndex].name}");
                    Console.WriteLine($"  Email: {students[studentIndex].email}");
                    Console.WriteLine($"  Score: {students[studentIndex].score}");
                    Console.WriteLine($"  Category: {students[studentIndex].category}");
                    Console.WriteLine(); // blank line separator
                }

                break;

            case "3":
                running = false;
                break;

            default:
                WriteLineError("Invalid menu option! Try again.");
                break;
        }
    }
}

void DeleteStudent()
{
    if (students.Count < 1)
    {
        WriteLineWarning("No students have been registered yet.");
        return;
    }

    while (true)
    {
        Console.Write("Enter index (0 or higher) of student to delete or \"exit\" to go back to main menu: ");

        string userInput = Console.ReadLine().Trim();

        if (userInput == "exit")
        {
            return;
        }

        uint studentIndex = 0;
        bool isValidUint = uint.TryParse(userInput, out studentIndex);

        if (isValidUint)
        {
            if (studentIndex < students.Count)
            {
                Student student = students[(int)studentIndex];

                Console.WriteLine("Student:");
                Console.WriteLine($"  Name: {student.name}");
                Console.WriteLine($"  Email: {student.email}");
                Console.WriteLine($"  Score: {student.score}");
                Console.WriteLine($"  Category: {student.category}");

                Console.WriteLine(); // blank line separator

                Console.WriteLine("Are you sure you want to delete this student? [y/N]: ");

                string confirmation = Console.ReadLine();

                if (confirmation.ToUpper() == "Y")
                {
                    students.RemoveAt((int)studentIndex);
                    WriteLineSuccess("Student has been removed.");
                }

                return;
            }
            else
                WriteLineError($"Index out of range. Value can be at most {students.Count - 1}");
        }
        else
            WriteLineError("Invalid index. The number must be a non-negative integer. Try again.");
    }
}

/*
 * Structs
 */

public class Student
{
    public string name = "";
    public string email = "";
    public uint score = 0;
    public string category = "";
}