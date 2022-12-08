namespace DataCreator.ConApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var numberOfCourses = 0;
            var numberOfTeachers = 0;

            var random = new Random(DateTime.Now.Millisecond);


            var teacherFaker = new Bogus.Faker<Teacher>().RuleFor(t => t.FirstName, f => f.Name.FirstName())
                                                    .RuleFor(t => t.LastName, f => f.Name.LastName())
                                                    .RuleFor(t => t.Title, f => f.Name.Prefix())
                                                    .RuleFor(t => t.Id, f => ++numberOfTeachers);

            var teachers = teacherFaker.Generate(10);

            var courseFaker = new Bogus.Faker<Course>().RuleFor(c => c.Designation, f => string.Join(" ", f.Lorem.Words(5)))
                                                       .RuleFor(c => c.TeacherId, f => f.PickRandom(teachers).Id)
                                                       .RuleFor(c => c.Id, f => ++numberOfCourses);

            var courses = courseFaker.Generate(100);

            //foreach (var item in teachers)
            //{
            //    Console.WriteLine(item.Id + " " + item.LastName);
            //}

            //foreach (var item in courses)
            //{
            //    Console.WriteLine(item.Id  + " " + item.Designation + " " + item.TeacherId);
            //}

            File.WriteAllLines(@"Teachers.csv", teachers.Select(t => t.ToString()));
            File.WriteAllLines(@"Courses.csv", courses.Select(c => c.ToString()));


        }
    }
}