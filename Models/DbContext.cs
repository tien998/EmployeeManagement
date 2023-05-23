using System.Data;
using System.Diagnostics;
using System.Text;
using EmployeeManagement.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeManagement.DbConnection;

public class EmployDb
{
    static string str = "data source= 127.0.0.1,1433; initial catalog=Manager; uid=sa; pwd=Password123; TrustServerCertificate=true";
    internal static SqlConnection? connection;
    internal static SqlDataAdapter? adapter;
    internal static DataSet? dataSet;
    internal static DataTable? empTable;
    public static bool rowNull { get; set; }

    static void InitADO_Object()
    {
        connection = new SqlConnection(str);
        adapter = new SqlDataAdapter();
        dataSet = new DataSet();
    }
    static void MappingAndFill()
    {
        adapter!.TableMappings.Add("Employee", "EmployeeT");
        adapter!.Fill(dataSet!, "Employee");
        empTable = dataSet!.Tables["EmployeeT"];
    }
    public static async Task<List<Employee>> Get()
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand("Select * from Employee", connection);
        MappingAndFill();
        connection!.Open();
        empTable!.WriteXml("Xml/EmpTable.xml");
        List<Employee> employees = new List<Employee>();

        // Sử dụng vòng lặp để truyền giá trị từ 'DataRowCollection' sang 'List<Employee>'
        foreach (DataRow r in empTable!.Rows)
        {
            // Bảng Employee trên CSDL có: NvID, FirstName, LastName
            var em = new Employee
            {
                ID = (int)r["NvID"],
                First = r["FirstName"].ToString(),
                Last = r["LastName"].ToString(),
            };

            employees.Add(em);
        }
        connection.Close();
        Dispose.ADO_Objects();
        await Task.CompletedTask;
        return employees;
    }
    public static async Task<Employee> Get(int id)
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand("Select * from Employee where NvID = @nvID", connection);
        var nvID = adapter!.SelectCommand.Parameters.Add("nvID", SqlDbType.Int);
        nvID.Value = id;
        MappingAndFill();
        adapter.Fill(dataSet!, "Employee");
        dataSet!.WriteXml("Xml/dataSet.xml");

        var e = new Employee();
        rowNull = false;
        if (empTable!.Rows.Count != 0)
        {
            var r = empTable!.Rows[0];
            e.ID = (int)r!["NvID"];
            e.First = r["Firstname"].ToString()!;
            e.Last = r["Lastname"].ToString()!;
        }
        else
        {
            rowNull = true;
        }

        Dispose.ADO_Objects();
        await Task.CompletedTask;
        return e;
    }
    public static void Create(Employee e)
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand("Select Firstname, Lastname from Employee", connection);
        MappingAndFill();
        adapter!.InsertCommand = new SqlCommand("Insert into Employee(Firstname, Lastname) values (@first, @last)", connection);

        var first = adapter!.InsertCommand.Parameters.AddWithValue("first", e.First);
        var last = adapter!.InsertCommand.Parameters.AddWithValue("last", e.Last);
        var nRow = empTable!.Rows.Add();
        Dispose.ADO_Objects();
        adapter.Update(dataSet!, "Employee");
    }
    public static void Update(Employee e)
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand($"Select * from Employee Where NvId={e.ID}", connection);
        MappingAndFill();

        adapter!.UpdateCommand = new SqlCommand("Update Employee Set FirstName=@FirstName, LastName=@LastName Where NvID=@NvID", connection);
        adapter!.UpdateCommand.Parameters.AddRange(
            new SqlParameter[]{
                new SqlParameter("FirstName", SqlDbType.NVarChar, 255, "FirstName"),
                new SqlParameter("LastName", SqlDbType.NVarChar, 255, "LastName"),
                new SqlParameter("NvID", SqlDbType.Int) { SourceColumn = "nvid"}
            }
        );
        DataRow r = empTable!.Rows[0];
        r[1] = e.First;
        r[2] = e.Last;
        adapter!.Update(dataSet!, "Employee");

        Dispose.ADO_Objects();
    }
    public static void Delete(int id)
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand($"Select NvId from Employee Where nvId={id}", connection);
        MappingAndFill();
        adapter!.DeleteCommand = new SqlCommand($"Delete Employee Where nvId={id}",connection);
        empTable!.Rows[0].Delete();
        adapter.Update(dataSet!, "Employee");
        Dispose.ADO_Objects();
    }

    // static string UpdateCmdBuilder()
    // {
    //     StringBuilder cmd = new StringBuilder("Update Employee Set ");
    //     List<string> colNames = new List<string>();
    //     List<string> cmdParams = new List<string>();
    //     // empTable!.PrimaryKey = new DataColumn[] { empTable.Columns[0]};
    //     foreach (DataColumn col in empTable!.Columns)
    //     {
    //         if (!col.Unique)
    //         {
    //             colNames.Add(col.ColumnName);
    //             cmdParams.Add($"@{col.ColumnName}");
    //         }
    //     }
    //     for (int i = 1; i < colNames.Count(); i++)
    //     {
    //         cmd.Append(colNames[i] + "=" + cmdParams[i] + " ");
    //     }
    //     cmd.Append($"Where {colNames[0]}={cmdParams[0]}");

    //     return cmd.ToString();
    // }

}
public class Dispose
{
    public static void ADO_Objects()
    {
        Debug.WriteLine("Tien hanh dispose: SqlConnection, SqlDataAdapter, DataSet, DataTable");
        EmployDb.connection!.Dispose();
        EmployDb.adapter!.Dispose();
        EmployDb.dataSet!.Dispose();
        EmployDb.empTable!.Dispose();
    }
}