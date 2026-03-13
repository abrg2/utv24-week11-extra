using System.Globalization;
using System.Text.RegularExpressions;

List<Student> students = new List<Student>();
void RegisterStudent()
{
    Student student = new();

    Console.WriteLine("Ange elevens namn: ");
    student.name = Console.ReadLine().Trim();

    while (true)
    {
        Console.WriteLine("Ange elevens email: ");
        string userInput = Console.ReadLine().Trim();
        const string regexPatternEmail = @"^[^@]+@[^@.]+\.[a-z]+$";
        if (Regex.IsMatch(userInput, regexPatternEmail))
        {
            student.email = userInput;
            break;
        }
        else
        {
            Console.WriteLine("Felaktig email adress, försök igen.");
        }
    }

    while (true)
    {
        Console.WriteLine("Ange elevens poäng: ");
        string userInput = Console.ReadLine().Trim();
        bool validScore = double.TryParse(userInput, CultureInfo.InvariantCulture, out student.score);
        if (validScore)
        {
            break;
        }
        else
        {
            Console.WriteLine("Felaktigt värde, försök igen.");
        }
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Eleven har lagts till!");
    Console.ResetColor();

    students.Add(student);
}

double min_score_passed = 0.75;
bool running = true;

while (running)
{
    Console.WriteLine("Meny:");
    Console.WriteLine("  1. Lägg till (registrera) student");
    Console.WriteLine("  2. Visa alla studenter");
    Console.WriteLine("  3. Visa godkända studenter");
    Console.WriteLine("  4. Sök efter student");
    Console.WriteLine("  5. Avsluta");
    Console.WriteLine("");
    Console.WriteLine("Ange ett val: ");

    string userInput = Console.ReadLine();

    switch (userInput)
    {
        case "1":
            RegisterStudent();
            break;
        case "2":
            //ShowStudents();
            break;
        case "3":
            //ShowPassedStudents();
            break;
        case "4":
            //SearchForStudent();
            break;
        case "5":
            running = false;
            break;
        default:
            Console.WriteLine("Felaktigt val.");
            break;
    }
}

Console.WriteLine("Följade elever skapades:");

foreach (Student s in students)
{
    Console.WriteLine(s.name);
    Console.WriteLine(s.email);
    Console.WriteLine(s.score);
}

Console.ReadKey();
public class Student
{
    public string name = "";
    public string email = "";
    public double score = 0.0;
};