
namespace EmployeeManagement.Models;
public class Employee : IDisposable
{
    public int ID { get; set; }
    public string? First { get; set; }
    public string? Last { get; set; }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
