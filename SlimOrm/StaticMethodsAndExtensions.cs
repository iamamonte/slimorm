using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace SlimOrm
{
    /// <summary>
    /// Depreciated. Use Extension methods.
    /// </summary>
    public partial class StaticMethods
    {


      
       /// <summary>
       /// Checks an object and converts it to DbNull, if appropriate. Allows for accepting data types such as int?.
       
       /// <param name="x"></param>
       /// <returns></returns>
       public static object CheckDBNull(object x)
        {
            if (x == null)
            {
                return DBNull.Value;
            }
            switch (x.GetType().ToString().ToLower())
            {
                case "system.string":
                    if (((string)x).Length == 0)
                    {
                        return DBNull.Value;
                    }
                    break;

            }
            return x;

        }

       /// <summary>
       /// Converts a database result to int?.
       /// </summary>
       /// <param name="x"></param>
       /// <param name="retVal">Out</param>
       /// <returns></returns>
       public static void DBToInt(object x, out int? retVal)
       {
           int? _retVal = null;
           try { _retVal = int.Parse(x.ToString()); }
           catch { }
           retVal = _retVal;
          

       }

       /// <summary>
       /// Used to convert string or integer values from a SqlDataReader to boolean.
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
        public static bool ConvertToBool(int? value)
        {
            return (value > 0 && value != null) ? true : false;
        }
        /// <summary>
        /// Used to convert string or integer values from a SqlDataReader to boolean.
        /// </summary>
        /// <param name="value">A string value which will be converted. Null values will return false. Empty strings will return false.</param>
        /// <returns></returns>
        public static bool ConvertToBool(string value)
        {
            if (value == null || value == "") { return false; }
            int convertedValue;
            bool doesConvert = int.TryParse(value, out convertedValue);
            if (doesConvert) { return ConvertToBool(convertedValue); }
            return (value.ToLower().Trim() == "true") ? true : false;
        }
       /// <summary>
       /// Converts array to the following xml format:
       /// <para></para>
       /// <para><root><data value={int} /></root></para>
       /// </summary>
       /// <param name="values"></param>
       /// <returns></returns>
        public static object ConvertToGenericXML(int[] values)
        {
            if (values.Length == 0 || values == null)
            {
                return DBNull.Value;
            }

            XDocument retVal = new XDocument();
            retVal.Add(new XElement("root"));
            for (int i = 0; i < values.Length; i++)
            {
                XElement element = new XElement("data", new XAttribute("value", values[i]));
                retVal.Descendants("root").Single().AddFirst(element);

            }
            return retVal.ToString();

        }
        /// <summary>
        /// Converts array to the following xml format:
        /// <para></para>
        /// <para><root><data value={value} /></root></para>
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object ConvertToGenericXML(string[] values)
        {
            if (values.Length == 0 || values == null)
            {
                return DBNull.Value;
            }

            XDocument retVal = new XDocument();
            retVal.Add(new XElement("root"));
            for (int i = 0; i < values.Length; i++)
            {
                XElement element = new XElement("data", new XAttribute("value", values[i]));
                retVal.Descendants("root").Single().AddFirst(element);

            }
            return retVal.ToString();

        }

    }
}

namespace SlimOrm
{
    public static class ExtensionMethods 
    {
        /// <summary>
        /// Removes ',' '.' and '$' and attempts to parse into a long. 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool CheckNumeric(this string x)
        {
            long outVal;
            x = x.Replace(",", "").Replace(".", "").Replace("$", "");
            return long.TryParse(x, out outVal);

        }

        /// <summary>
        /// Checks an object and converts it to DbNull, if appropriate. Allows for accepting data types such as int?.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static object CheckDBNull(this object x)
        {
            if (x == null)
            {
                return DBNull.Value;
            }
            switch (x.GetType().ToString().ToLower())
            {
                case "system.string":
                    if (string.IsNullOrEmpty(((string)x)))
                    {
                        return DBNull.Value;
                    }
                    break;

            }
            return x;

        }

        /// <summary>
        /// Takes a DB value and converts it to an int? 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static void DBToInt(this object x, out int? retVal)
        {
            int? _retVal = null;
            try { _retVal = int.Parse(x.ToString()); }
            catch { retVal = 0;  }
            retVal = _retVal;


        }

        /// <summary>
        /// Takes a DB value and converts it to an int? 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int? DBToInt(this object x)
        {
            int? _retVal = null;
            try { _retVal = int.Parse(x.ToString()); }
            catch { return 0; }
            return _retVal;


        }

        /// <summary>
        /// Takes a DB value and converts it to an long? 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static long? DBToLong(this object x)
        {
            long? _retVal = null;
            try { _retVal = long.Parse(x.ToString()); }
            catch {  }
            return _retVal;


        }

        /// <summary>
        /// Takes a DB value and converts it to an int? 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DateTime? DBToDateTime(this object x)
        {
            DateTime? _retVal = null;
            try { _retVal = DateTime.Parse(x.ToString()); }
            catch { }
            return _retVal;


        }

        /// <summary>
        /// Used to convert string or integer values from a SqlDataReader to boolean.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConvertToBool(this int? value)
        {
            return (value > 0 && value != null) ? true : false;
        }

        private static bool _convertToBool(int? value) 
        {
            return (value > 0 && value != null) ? true : false;
        }
        
        /// <summary>
        /// Used to convert string or integer values from a SqlDataReader to boolean.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConvertToBool(this string value)
        {
            int convertedValue;
            bool doesConvert = int.TryParse(value, out convertedValue);
            if (doesConvert) { return _convertToBool(convertedValue); }
            return (value.ToLower().Trim() == "true") ? true : false;
        }
        
        /// <summary>
        /// Converts array to the following xml format:
        /// <para></para>
        /// <para><root><data value={int} /></root></para>
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object ConvertToGenericXML(this int[] values)
        {
            if (values == null)
                return DBNull.Value;
            if (values.Length == 0)
            {
                return DBNull.Value;
            }

            XDocument retVal = new XDocument();
            retVal.Add(new XElement("root"));
            for (int i = 0; i < values.Length; i++)
            {
                XElement element = new XElement("data", new XAttribute("value", values[i]));
                retVal.Descendants("root").Single().AddFirst(element);

            }
            return retVal.ToString();

        }

        /// <summary>
        /// Converts array to the following xml format:
        /// <para></para>
        /// <para><root><data value={int} /></root></para>
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object ConvertToGenericXML(this IEnumerable<long> values)
        {
            if (values == null)
                return DBNull.Value;

            long[] valuesAsArray = values.ToArray();
            if (valuesAsArray.Length == 0)
            {
                return DBNull.Value;
            }

            XDocument retVal = new XDocument();
            retVal.Add(new XElement("root"));
            for (int i = 0; i < valuesAsArray.Length; i++)
            {
                XElement element = new XElement("data", new XAttribute("value", valuesAsArray[i]));
                retVal.Descendants("root").Single().AddFirst(element);

            }
            return retVal.ToString();

        }
        
        /// <summary>
        /// Converts array to the following xml format:
        /// <para></para>
        /// <para><root><data value={value} /></root></para>
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object ConvertToGenericXML(this IEnumerable<string> values)
        {
            if (values == null)
                return DBNull.Value;    
            if (values.Count() == 0 || values == null)
            {
                return DBNull.Value;
            }
            string[] _values = values.ToArray();
            XDocument retVal = new XDocument();
            retVal.Add(new XElement("root"));
            for (int i = 0; i < _values.Length; i++)
            {
                XElement element = new XElement("data", new XAttribute("value", _values[i]));
                retVal.Descendants("root").Single().AddFirst(element);

            }
            return retVal.ToString();

        }

        public static string ToBestGuessDBString(this string x)
        {
            return x.Replace(" ", "_").ToLower();

        }

        public static bool IsUpperCaseCharacter(this char x) 
        {
            //Char code 65 = 90 inclusive is A - Z upper case
            int charAsNum = (int)x;
            
            return charAsNum >= 65 && charAsNum <= 90;
            
        
        }

        public static string ToProperCase(this string x) 
        {
            List<int> capitalSpots = new List<int>() {0 };

            string underscoreReplaced = x.Replace("_", " ");
            int startIndex = 0;
            int foundIndex = 0;
            do 
            {
                foundIndex = underscoreReplaced.IndexOf(" ", startIndex);
                if (foundIndex != -1) 
                {
                    startIndex = foundIndex + 1;
                    capitalSpots.Add(startIndex);
                }
            } while (foundIndex != -1);
            char[] stringAsChar = underscoreReplaced.ToCharArray();
            foreach (int i in capitalSpots) 
            {
                int charAsInt = (int)stringAsChar[i];
                if(charAsInt >= 97 && charAsInt <= 122) stringAsChar[i] = (char)((int)stringAsChar[i] - 32);
            }
            return new string(stringAsChar);
            
        
        }

        public static string ToUnderScoreCase(this string x) 
        {
            char previousChar = ',';
             char currentChar;
             int iterations = 0;
             int charValue = 0;
            List<char> newWordAsChars = new List<char>() { };
            foreach (char y in x.ToCharArray())
            {
                
                currentChar = y;
                //Underscore check.
                if (currentChar == '_') 
                {
                    newWordAsChars.Add(currentChar);
                    previousChar = currentChar;
                    iterations++;
                    continue;
                
                }
                //Legal character check.
                try
                {
                    charValue = (int)currentChar;
                    if (!((charValue >= 65 && currentChar <= 90) || (currentChar >= 97 && currentChar <= 122)))
                    {
                        continue;

                    }
                }
                catch { continue; }
                //First character check.
                if (iterations == 0) { previousChar = y; }
             
                //Equal casing check.
                if (previousChar.IsUpperCaseCharacter() == currentChar.IsUpperCaseCharacter()) 
                {
                    newWordAsChars.Add(currentChar);
                    previousChar = currentChar;
                    iterations++;
                    continue;
                }
                //Upper to Lower case check, and skip.
                if (previousChar.IsUpperCaseCharacter() && !currentChar.IsUpperCaseCharacter()) 
                {
                    iterations++;
                    previousChar = currentChar;
                    newWordAsChars.Add(currentChar);
                    continue;
                
                }

                iterations++;
                newWordAsChars.Add('_');
                previousChar = currentChar;
                newWordAsChars.Add(currentChar);
              
               
                 
                
            }
            string retVal = new String(newWordAsChars.ToArray()).ToLower();
            retVal = retVal.Replace("__", "_");
            return retVal;
        }

        /// <summary>
        /// Replaces greater than and less than signs with square brackets.
        /// </summary>
        /// <param name="x"></param>
        /// <returns> </returns>
        public static string ReplaceBrackets(this string x) { return x.Replace("<", "[").Replace(">", "]").Replace("&lt;", "[").Replace("&gt;", "]"); }

        public static string ToInsideOfDiv(this string x) 
        {
            return x.Replace(Environment.NewLine, "<br/>").Replace("\n\n", "<br/>");
        
        }

        /// <summary>
        /// Depreciated.
        /// <para>
        /// Attempts to manually convert problem characters for xml.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ManuallyDeserialize(this string x) 
        {
            return x.Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&");
        }

        /// <summary>
        /// Depreciated.
        /// <para>Attempts to manually convert problem characters for xml.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ManuallySerialize(this string x) 
        {
            return x.Replace("\"", "&quot;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;");
        
        }

       

        
    }

    public static class XMLExtensions 
    {
        public static IEnumerable<XElement> HasMatches(this IEnumerable<XElement> source, Func<XElement,  bool> predicate) 
        {
            var result = source.Where(predicate);
            try { if (result.Count() == 0) { return null; } }
            catch (NullReferenceException) { return null; }
            
            return result;
        
        }
    
    }

}


namespace SlimOrm
{
    public static class BestDBGuess
    {
        public static string Get(string x) 
        {
            return x.Replace(" ", "_").ToLower();
        
        }
        
    }


}