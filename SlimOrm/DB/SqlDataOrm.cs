using SlimOrm.Attributes;
using SlimOrm.DB;
using SlimOrm.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;


namespace SlimOrm
{
    /// <summary>
    /// Lightweight MS SQL ORM
    /// </summary>
    public class SqlDataOrm : ISqlDataOrm
    {
        #region Fields
        private readonly IDbConnectionService _dbConn;

        public IDbConnectionService DbConnectionService
        {
            get
            {
                return _dbConn;
            }
        }

        #endregion

        #region Ctor
        public SqlDataOrm(IDbConnectionService dbConn)
        {
            _dbConn = dbConn;
        }

        #endregion

        #region Private Methods
        private static string CreateDeleteStatement(DataBase obj)
        {
            StringBuilder sb = new StringBuilder();
            TableName tableName = obj.GetType().GetCustomAttribute<TableName>();
            if (tableName == null)
                throw new Exception("No table name provided.");
            sb.AppendFormat("Delete from {0}", tableName.Value);

            //get properties
            string fieldName = "";
            DBName currentAttribute;

            string identity = "";

            foreach (PropertyInfo property in obj.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(DBName)).Count() > 0))
            {

                currentAttribute = (DBName)property.GetCustomAttribute(typeof(DBName));
                fieldName = (currentAttribute.Value == "") ? property.Name.ToUnderScoreCase() : currentAttribute.Value;

                //identity check
                if (property.GetCustomAttribute(typeof(Identity)) != null)
                {
                    identity = fieldName;
                    break;
                }
            }

            //ensure there was an identity
            if (string.IsNullOrEmpty(identity))
                throw new Exception("No identity column was specified.");

            //append identity where clause
            sb.AppendFormat(" where {0} = @{0}", identity);

            return sb.ToString();
        }

        private static string CreateUpdateStatement(DataBase obj)
        {
            StringBuilder sb = new StringBuilder();
            TableName tableName = obj.GetType().GetCustomAttribute<TableName>();
            if (tableName == null)
                throw new Exception("No table name provided.");
            sb.AppendFormat("Update {0} set", tableName.Value);

            //get properties
            string fieldName = "";
            DBName currentAttribute;

            string identity = "";

            foreach (PropertyInfo property in obj.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(DBName)).Count() > 0))
            {

                currentAttribute = (DBName)property.GetCustomAttribute(typeof(DBName));
                fieldName = (currentAttribute.Value == "") ? property.Name.ToUnderScoreCase() : currentAttribute.Value;

                //identity check
                if (property.GetCustomAttribute(typeof(Identity)) != null)
                {
                    identity = fieldName;
                    continue;
                }

                //otherwise add to statement
                sb.AppendFormat(" {0} = @{0},", fieldName);
            }

            //ensure there was an identity
            if (string.IsNullOrEmpty(identity))
                throw new Exception("No identity column was specified.");

            //remove trailing comma
            sb.Remove(sb.Length - 1, 1);
            //append identity where clause
            sb.AppendFormat(" where {0} = @{0}; Select * from {1} where {0} = @{0}", identity, tableName.Value);

            return sb.ToString();
        }

        private static string CreateInsertStatement(DataBase obj)
        {
            StringBuilder sb = new StringBuilder();
            TableName tableName = obj.GetType().GetCustomAttribute<TableName>();
            if (tableName == null)
                throw new Exception("No table name provided.");
            sb.AppendFormat("Insert into {0} ( ", tableName.Value);

            List<string> fields = new List<string>();
            string fieldName = "";
            DBName currentAttribute;
            string identity = "";

            foreach (PropertyInfo property in obj.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(DBName)).Count() > 0))
            {

                currentAttribute = (DBName)property.GetCustomAttribute(typeof(DBName));
                fieldName = (currentAttribute.Value == "") ? property.Name.ToUnderScoreCase() : currentAttribute.Value;

                //identity check
                if (property.GetCustomAttribute(typeof(Identity)) != null)
                {
                    identity = fieldName;
                    continue;
                }

                //otherwise add to statement
                sb.AppendFormat("{0},", fieldName);
                fields.Add(fieldName);

            }

            //ensure there was an identity
            if (string.IsNullOrEmpty(identity))
                throw new Exception("No identity column was specified.");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") values (");

            foreach (var s in fields)
                sb.AppendFormat("@{0},", s);
            //remove trailing comma
            sb.Remove(sb.Length - 1, 1);
            sb.AppendFormat("); select * from {0} where {1} = @@IDENTITY", tableName.Value, identity);
            return sb.ToString();
        }

        private static string CreateSelectStatement(DataBase obj)
        {
            StringBuilder sb = new StringBuilder();
            TableName tableName = obj.GetType().GetCustomAttribute<TableName>();
            if (tableName == null)
                throw new Exception("No table name provided.");
            sb.AppendFormat("Select * from {0}", tableName.Value);

            //get properties
            string fieldName = "";
            DBName currentAttribute;

            string identity = "";

            foreach (PropertyInfo property in obj.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(DBName)).Count() > 0))
            {

                currentAttribute = (DBName)property.GetCustomAttribute(typeof(DBName));
                fieldName = (currentAttribute.Value == "") ? property.Name.ToUnderScoreCase() : currentAttribute.Value;

                //identity check
                if (property.GetCustomAttribute(typeof(Identity)) != null)
                {
                    identity = fieldName;
                    break;
                }
            }

            //ensure there was an identity
            if (string.IsNullOrEmpty(identity))
                throw new Exception("No identity column was specified.");

            //append identity where clause
            sb.AppendFormat(" where {0} = @{0}", identity);

            return sb.ToString();
        }


        #endregion

        #region Public Methods
        public T Create<T>(T obj) where T : DataBase
        {
            using (SqlConnection conn = (SqlConnection)_dbConn.CreateConnection())
            {
                SqlCommand insertStatment = new SqlCommand(CreateInsertStatement(obj), conn);
                SqlDataAssigner.SetParametersFromDbNameProperties(obj, insertStatment);
                conn.Open();
                SqlDataReader reader = insertStatment.ExecuteReader();
                T retVal = null;
                while(reader.Read())
                 retVal = Activator.CreateInstance(typeof(T), args: reader) as T;
                return retVal;
            }
        }

        public int Purge<T>(T obj) where T : DataBase
        {
            using (SqlConnection conn = (SqlConnection)_dbConn.CreateConnection())
            {
                SqlCommand updateCommand = new SqlCommand(CreateDeleteStatement(obj), conn);
                SqlDataAssigner.SetParametersFromDbNameProperties(obj, updateCommand);
                conn.Open();
                return updateCommand.ExecuteNonQuery();
            }
        }

        public T Retrive<T>(T obj) where T : DataBase
        {
            using (SqlConnection conn = (SqlConnection)_dbConn.CreateConnection())
            {
                SqlCommand updateCommand = new SqlCommand(CreateSelectStatement(obj), conn);
                SqlDataAssigner.SetParametersFromDbNameProperties(obj, updateCommand);
                conn.Open();
                SqlDataReader reader = updateCommand.ExecuteReader();
                T retVal = null;
                while (reader.Read())
                    retVal = Activator.CreateInstance(typeof(T), args: reader) as T;
                return retVal;
            }
        }

        public T Update<T>(T obj) where T : DataBase
        {
            using (SqlConnection conn = (SqlConnection)_dbConn.CreateConnection())
            {
                SqlCommand updateCommand = new SqlCommand(CreateUpdateStatement(obj), conn);
                SqlDataAssigner.SetParametersFromDbNameProperties(obj, updateCommand);
                conn.Open();
                SqlDataReader reader = updateCommand.ExecuteReader();
                T retVal = null;
                while (reader.Read())
                    retVal = Activator.CreateInstance(typeof(T), args: reader) as T;
                return retVal;


            }
        }

        public List<T> GetWithQuery<T>(string query, object paramObject) where T : class
        {
            List<T> retVal = null;
            using (SqlConnection conn = (SqlConnection)_dbConn.CreateConnection())
            {
                SqlCommand selectCommand = new SqlCommand(query, conn);
                if(paramObject!=null)
                SqlDataAssigner.SetParametersFromObject(paramObject, selectCommand);
                conn.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                if (reader.HasRows)
                    retVal = new List<T>();
                while (reader.Read())
                    retVal.Add(Activator.CreateInstance(typeof(T), args: reader) as T);
                return retVal;
            }
        }
        #endregion

    }
}
