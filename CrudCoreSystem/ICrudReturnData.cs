using System.Data;

namespace CrudCoreSystem
{
    /// <summary>
    /// Interface lưu các return value mặc định cần có.
    /// </summary>
    public interface ICrudReturnData
    {
        int ReturnCode { get; }
        string ReturnMess { get; }
        int ReturnData { get; }
        DataTable ReturnDataTable { get; }
    }
}
