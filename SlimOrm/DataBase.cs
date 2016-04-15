using System.Data.Common;

namespace SlimOrm
{
    public class DataBase
    {
        public DataBase() { }
        public DataBase(DbDataReader reader)
        {
            SqlDataAssigner.AssignProperties(this, reader);
        }
       
    }
}
