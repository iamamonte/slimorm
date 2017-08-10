using System;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;

namespace SlimOrm
{
    /// <summary>
    /// A class that assists in reading data from a database and giving values to an object based on the rows in the database query result.
    /// </summary>
    public partial class SqlDataAssigner
    {
        /// <summary>
        /// Iterates through the public fields in an object and gives the field a value.
        /// <para>Searches for a row in the System.Data.SqlClient.SqlDataReader result with a name that corresponds to the Field name.</para>
        /// <para>Classes will be skipped.</para>
        /// <para>Automatically created BackingFields will be skipped.</para>
        /// <para>Conversions to boolean use an extension from SlimOrm.Extensions namespace.</para>
        /// </summary>
        /// <param name="targetObject">The object whose members will be iterated through, searching for public fields.</param>
        /// <param name="reader">The System.Data.SqlClient.SqlDataReader to match against.</param>
        public static void AssignFieldValuesFromReader(object targetObject, DbDataReader reader)
        {
            if (reader.IsClosed)
            {
                while (reader.Read())
                {
                    foreach (FieldInfo field in targetObject.GetType().GetFields())
                    {
                        SetFieldValue(field, targetObject, reader);
                    }
                }
            }
            else
            {
                while (reader.Read())
                {
                    foreach (FieldInfo field in targetObject.GetType().GetFields())
                    {
                        SetFieldValue(field, targetObject, reader);
                    }
                }
            }

        }

        /// <summary>
        /// Iterates through the public fields in an object and gives the field a value.
        /// <para>Searches for a row in the System.Data.SqlClient.SqlDataReader result with a name that corresponds to the Field name.</para>
        /// <para>Classes will be skipped.</para>
        /// <para>Automatically created BackingFields will be skipped.</para>
        /// <para>Conversions to boolean use an extension from SlimOrm.Extensions namespace.</para>
        /// </summary>
        /// <param name="targetObject">The object whose members will be iterated through, searching for public fields.</param>
        /// <param name="reader">The System.Data.SqlClient.SqlDataReader to match against.</param>
        public static void AssignPublicFieldValuesFromReader(object targetObject, DbDataReader reader)
        {
            if (reader.IsClosed)
            {
                while (reader.Read())
                {
                    foreach (FieldInfo field in targetObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        SetFieldValue(field, targetObject, reader);
                    }
                }
            }
            else
            {
                while (reader.Read())
                {
                    foreach (FieldInfo field in targetObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        SetFieldValue(field, targetObject, reader);
                    }
                }
            }

        }

        /// <summary>
        /// Iterates through the Private fields in an object and gives the field a value.
        /// <para>Searches for a row in the System.Data.SqlClient.SqlDataReader result with a name that corresponds to the Field name.</para>
        /// <para>Classes will be skipped.</para>
        /// <para>Automatically created BackingFields will be skipped.</para>
        /// <para>Conversions to boolean use an extension from SlimOrm.Extensions namespace.</para>
        /// </summary>
        /// <param name="targetObject">The object whose members will be iterated through, searching for private fields.</param>
        /// <param name="reader">The System.Data.SqlClient.SqlDataReader to match against.</param>
        public static void AssignPrivateFieldValuesFromReader(object targetObject, DbDataReader reader)
        {
            if (reader.IsClosed)
            {
                while (reader.Read())
                {
                    foreach (FieldInfo field in targetObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        SetFieldValue(field, targetObject, reader);
                    }
                }
            }
            else
            {
                while (reader.Read())
                {
                    foreach (FieldInfo field in targetObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        SetFieldValue(field, targetObject, reader);
                    }
                }
            }

        }
       
        /// <summary>
        /// Iterates through the object's properties, searching for Properties with an Attribute of DBName. 
        /// <para>Requires the property have a DBName attribute.</para>
        /// <para>If the DBName attribute does not provide a value, an extension is used to convert the property to underscore casing. </para>
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="reader"></param>
        public static void AssignPropertiesFromReader(object targetObject, DbDataReader reader)
        {
            string fieldName = "";
            DBName currentAttribute;
            while (reader.Read())
            {
                foreach (PropertyInfo property in targetObject.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(DBName)).Count() > 0))
                {
                    currentAttribute = (DBName)property.GetCustomAttribute(typeof(DBName));
                    fieldName = (currentAttribute.Value == "") ? property.Name.ToUnderScoreCase() : currentAttribute.Value;
                    SetPropertyValue(property, targetObject, reader, fieldName);
                    //property.SetValue(targetObject, Convert.ChangeType(reader[fieldName].ToString(), property.PropertyType));

                }
            }

        }

        /// <summary>
        /// Iterates through the object's properties, searching for Properties with an Attribute of DBName. 
        /// <para>Ensure the SqlDataReader is open.</para>
        /// <para>Requires the property have a DBName attribute.</para>
        /// <para>If the DBName attribute does not provide a value, an extension is used to convert the property to underscore casing. </para>
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="reader"></param>
        public static void AssignProperties(object targetObject, DbDataReader reader)
        {
            string fieldName = "";
            DBName currentAttribute;
            
                foreach (PropertyInfo property in targetObject.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(DBName)).Count() > 0))
                {
                    currentAttribute = (DBName)property.GetCustomAttribute(typeof(DBName));
                    fieldName = (currentAttribute.Value == "") ? property.Name.ToUnderScoreCase() : currentAttribute.Value;
                    SetPropertyValue(property, targetObject, reader, fieldName);
                    //property.SetValue(targetObject, Convert.ChangeType(reader[fieldName].ToString(), property.PropertyType));

                }
            

        }

        private static void SetFieldValue(FieldInfo field, object targetObject, DbDataReader reader) 
        {
            if (!(field.FieldType.IsClass && field.FieldType != typeof(String)))
            {
                if (field.Name.IndexOf("BackingField") >= 0) { return; }
                if (field.FieldType.FullName == "System.Boolean")
                {
                    field.SetValue(targetObject, reader[field.Name].ToString().ConvertToBool());

                }
                else
                {
                    //if(!string.IsNullOrEmpty(reader[field.Name].ToString()))
                    field.SetValue(targetObject, Convert.ChangeType(reader[field.Name].ToString(), field.FieldType));
                }
            }
        }

        /*
        private  static void SetPropertyValueUsingDBName(PropertyInfo property, object targetObject, SqlDataReader reader) 
        {
            DBName currentAttribute;
            currentAttribute = (DBName)property.GetCustomAttribute(typeof(DBName));
           string fieldName = (currentAttribute.Value == "") ? property.Name.ToUnderScoreCase() : currentAttribute.Value;
           SetPropertyValue(property, targetObject, reader, fieldName);
        }
        */
        private static void SetPropertyValue(PropertyInfo field, object targetObject, DbDataReader reader, string dbName)
        {
            var propertyType = field.PropertyType.ToString().ToLower();
            if (reader[dbName].ToString() == "") 
            {
                return;
            }
            
            //binary
            if (field.PropertyType.FullName == "System.Byte[]")
            {
                field.SetValue(targetObject, (byte[])reader[dbName]);
                return;
            }

            if (!(field.PropertyType.IsClass && field.PropertyType != typeof(String)))
            {
                if (field.Name.IndexOf("BackingField") >= 0) { return; }

               

                //boolean
                if (field.PropertyType.FullName == "System.Boolean")
                {
                    field.SetValue(targetObject, reader[dbName].ToString().ConvertToBool());
                    return;
                }

                //enum
                if (field.PropertyType.BaseType == typeof(Enum))
                {
                    int _value = (int)Convert.ChangeType(reader[dbName].ToString(), typeof(int));

                    field.SetValue(targetObject, _value);
                    return;
                }
                else
                {
                   //nullable
                    if (propertyType.Contains("nullable"))
                    { 
                        //If no value is found on a nullable type.
                        if (string.IsNullOrEmpty(reader[dbName].ToString()))
                            return;

                        field.SetValue(targetObject, Convert.ChangeType(reader[dbName].ToString(), Nullable.GetUnderlyingType(field.PropertyType)));
                        return;
                    }

                    //all else
                    field.SetValue(targetObject, Convert.ChangeType(reader[dbName].ToString(), field.PropertyType));
                }
            }
        }

        /// <summary>
        /// Adds parameters to a SqlCommand based on the properties in a class who declare the DBName attribute.
        /// </summary>
        /// <param name="providingObject"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static SqlCommand SetParametersFromDbNameProperties(object providingObject, SqlCommand command)
        {
            string fieldName = "";
            DBName currentAttribute;
            foreach (PropertyInfo property in providingObject.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(DBName)).Count() > 0))
            {

                currentAttribute = (DBName)property.GetCustomAttribute(typeof(DBName));
                fieldName = (currentAttribute.Value == "") ? property.Name.ToUnderScoreCase() : currentAttribute.Value;
                object value = property.GetValue(providingObject);

                


                //if (property.PropertyType.Name.ToLower().Contains("nullable"))
                //    if (value == null)
                //    {
                //        command.Parameters.AddWithValue("@" + fieldName, DBNull.Value);
                //        continue;
                //    }

                if (value == null)
                {

                    if (!property.PropertyType.FullName.Contains("DateTime"))
                    {
                        command.Parameters.AddWithValue("@" + fieldName, DBNull.Value);

                    }
                    else
                    {
                        object dateDefault = (object)property.GetCustomAttribute(typeof(DateDefault));

                        //default of current time 
                        if(dateDefault != null)
                        command.Parameters.AddWithValue("@" + fieldName, DateTime.Now);
                    }

                }
                else
                    command.Parameters.AddWithValue("@" + fieldName, value);
            }
            return command;
        }


        public static SqlCommand SetParametersFromObject(object providingObject, SqlCommand command)
        {
            string fieldName = "";
            DBName currentAttribute;
            foreach (PropertyInfo property in providingObject.GetType().GetProperties())
            {

                fieldName = property.Name.ToUnderScoreCase();
                object value = property.GetValue(providingObject);
                if (property.PropertyType.Name.ToLower().Contains("nullable"))
                    if (value == null)
                    {
                        command.Parameters.AddWithValue("@" + fieldName, DBNull.Value);
                        continue;
                    }
                command.Parameters.AddWithValue("@" + fieldName, value);
            }
            return command;
        }


    }

   
}
