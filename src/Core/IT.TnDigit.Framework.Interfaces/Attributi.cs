using System;

namespace IT.TnDigit.ORM.Interfaces
{
    /// <summary>
    /// Attributo che identifica le chiavi primarie per gli oggetti DataTypes
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class PrimaryKey : Attribute
    {
        // Constructor
        public PrimaryKey()
        {
        }

        public static new string ToString()
        {
            return "\t\t[PrimaryKey()]\r\n";
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class MappedField : Attribute
    {
        // Constructor
        public MappedField()
        {
        }

        public static new string ToString()
        {
            return "\t\t[MappedField()]\r\n";
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CalculatedData : Attribute
    {
        // Constructor
        public CalculatedData()
        {
        }

        public static new string ToString()
        {
            return "\t\t[MappedField()]\r\n";
        }
    }

    /// <summary>
    /// Attributo che gestisce le foreign key opzionali
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class NullableForeignKey : Attribute
    {
        // Constructor
        public NullableForeignKey()
        {
        }

        public static new string ToString()
        {
            return "\t\t[NullableForeignKey()]\r\n";
        }
    }

    /// <summary>
    /// Attributo che identifica la dimensione massima dei campi varchar
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class MaxLength : Attribute
    {
        private int max = 1;

        public int Max
        {
            get { return max; }
            set { max = value; }
        }
        // Constructor
        public MaxLength(int value)
        {
            max = value;
        }

        public static string ToString(int value)
        {
            return string.Format("\t\t[MaxLength({0})]\r\n", value.ToString());
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class AllowDateTime : Attribute
    {
        private bool allowTime = true;

        public bool AllowTime
        {
            get { return allowTime; }
            set { allowTime = value; }
        }
        // Constructor
        public AllowDateTime(bool value)
        {
            allowTime = value;
        }

        public static string ToString(bool value)
        {
            return string.Format("\t\t[AllowDateTime({0})]\r\n", value.ToString().ToLower());
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DateCreation : Attribute
    {
        // Constructor
        public DateCreation()
        {
        }

        public static new string ToString()
        {
            return "\t\t[DateCreation()]\r\n";
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DateLastModified : Attribute
    {
        // Constructor
        public DateLastModified()
        {
        }

        public static new string ToString()
        {
            return "\t\t[DateLastModified()]\r\n";
        }
    }


    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ClobField : Attribute
    {
        // Constructor
        public ClobField()
        {
        }

        public static new string ToString()
        {
            return "\t\t[ClobField()]\r\n";
        }
    }
}
