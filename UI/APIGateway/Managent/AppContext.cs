using Microsoft.EntityFrameworkCore;

namespace APIGateway.Managent
{
  public class AppContext : DbContext
  {
    public AppContext() { }
    public AppContext(DbContextOptions<AppContext> options) : base(options) { }
  }
}
