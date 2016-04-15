using System.Data;

namespace SlimOrm
{
    public interface IDbConnectionService
    {
        IDbConnection CreateConnection();
    }
}
