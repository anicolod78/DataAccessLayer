using System;
using System.Data;
using System.Reflection;


namespace IT.TnDigit.ORM.Interfaces
{
    /// <summary>
    /// Incapsula un qualsiasi oggetto in una interfaccia IDataRecord
    /// in modo che possa essere utilizzata da un IDataProvider
    /// </summary>
    public class DataItem : IDataRecord
    {
        private object item;


        private Type itemType;
        private PropertyInfo[] itemProperties;

        public DataItem(object item)
        {
            if (item == null) throw new ArgumentNullException("item");

            this.item = item;
            this.itemType = item.GetType();
            this.itemProperties = this.itemType.GetProperties();
        }

        public object GetItem
        {
            get { return item; }
        }

        #region IDataRecord Members

        public int FieldCount
        {
            get { return this.itemProperties.Length; }
        }

        public string GetName(int i)
        {
            return this.itemProperties[i].Name;
        }

        public int GetOrdinal(string name)
        {
            int propCount = this.itemProperties.Length;

            for (int idx = 0; idx < propCount; idx++)
            {
                if (this.itemProperties[idx].Name == name)
                    return idx;
            }

            return -1;
        }

        public object GetValue(int i)
        {
            return this.itemProperties[i].GetValue(this.item, null);
        }

        public string GetString(int i)
        {
            object val = this.GetValue(i);
            if (val == null || val == DBNull.Value)
                return String.Empty;

            return val.ToString();
        }

        public bool IsDBNull(int i)
        {
            object val = this.GetValue(i);
            if (val == null || val == DBNull.Value)
                return true;

            return false;
        }

        public object this[string name]
        {
            get { return this.GetValue(this.GetOrdinal(name)); }
        }

        public object this[int i]
        {
            get { return this.GetValue(i); }
        }

        public Type GetFieldType(int i)
        {
            return this.itemProperties[i].PropertyType;
        }


        #region Not Supported Members

        bool IDataRecord.GetBoolean(int i)
        {
            throw new NotSupportedException();
        }

        byte IDataRecord.GetByte(int i)
        {
            throw new NotSupportedException();
        }

        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        char IDataRecord.GetChar(int i)
        {
            throw new NotSupportedException();
        }

        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        IDataReader IDataRecord.GetData(int i)
        {
            throw new NotSupportedException();
        }

        string IDataRecord.GetDataTypeName(int i)
        {
            throw new NotSupportedException();
        }

        DateTime IDataRecord.GetDateTime(int i)
        {
            throw new NotSupportedException();
        }

        decimal IDataRecord.GetDecimal(int i)
        {
            throw new NotSupportedException();
        }

        double IDataRecord.GetDouble(int i)
        {
            throw new NotSupportedException();
        }

        float IDataRecord.GetFloat(int i)
        {
            throw new NotSupportedException();
        }

        Guid IDataRecord.GetGuid(int i)
        {
            throw new NotSupportedException();
        }

        short IDataRecord.GetInt16(int i)
        {
            throw new NotSupportedException();
        }

        int IDataRecord.GetInt32(int i)
        {
            throw new NotSupportedException();
        }

        long IDataRecord.GetInt64(int i)
        {
            throw new NotSupportedException();
        }

        int IDataRecord.GetValues(object[] values)
        {
            throw new NotSupportedException();
        }

        #endregion Not Supported Members

        #endregion IDataRecord Members

    }
}
