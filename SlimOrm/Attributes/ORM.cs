using System;


namespace SlimOrm
{
    /// <summary>
    /// Used to indicate the field name in a database. Presumes underscore casing if nothing is provdied.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DBName : Attribute
    {
        public string Value { get; set; }

        public DBName(string x)
        {
            Value = x;
        }
        public DBName()
        {
            Value = "";

        }

    }

    /// <summary>
    /// Used for nullable datetimes when you want to default to the database current timestamp
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DateDefault : Attribute
    {

    }

    /// <summary>
    /// Used to indicate the identity property for a table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Identity : Attribute
    {

    }

    /// <summary>
    /// Used to indicate the corresponding table for an class model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableName : Attribute
    {
        public string Value { get; set; }

        public TableName(string x)
        {
            Value = x;
        }
        public TableName()
        {
            Value = "";

        }
    }

}
