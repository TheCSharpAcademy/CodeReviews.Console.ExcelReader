using Microsoft.Extensions.Configuration;

namespace ExcelReader.kwm0304.Config;

public class AppConfiguration
{
  public static IConfigurationRoot Configuration { get; private set; }
  static AppConfiguration()
  {
    Configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
  }
  public static string GetConnectionString(string name)
  {
    return Configuration.GetConnectionString(name) ?? "Configuration is not being acknowledged";
  }
}