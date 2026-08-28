namespace nkklesion01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello Nguyễn Khắc Kiên!");

            app.Run();
        }
    }
}
