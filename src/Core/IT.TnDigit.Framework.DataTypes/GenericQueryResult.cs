using IT.TnDigit.ORM.Interfaces;
using System;
using System.Collections.Generic;

namespace IT.TnDigit.ORM.DataTypes
{
    public class GenericQueryResult : SupportCreaElemento, IFrameworkObject
    {
        public Dictionary<string, object> Properties = new Dictionary<string, object>();
        public List<string> PropertyNames = new List<string>();

        public override object CreaElemento(System.Data.IDataRecord dati)
        {
            GenericQueryResult item = new GenericQueryResult();

            for (int i = 0; i < dati.FieldCount; i++)
            {
                string name = dati.GetName(i);
                item.PropertyNames.Add(name);
                if (dati[name] != DBNull.Value)
                {
                    item.Properties.Add(name, dati[name]);
                }
                else
                {
                    item.Properties.Add(name, null);
                }
            }
            return item;
        }

        public string TableName()
        {
            return "";
        }

        public string FieldsList()
        {
            return "*";
        }

        public string SequenceName()
        {
            return null;
        }

        public List<string> PropertiesChanged
        {
            get { return PropertyNames; }
        }

        private List<BaseRelation> m_DataRelationships = new List<BaseRelation>();

        public List<BaseRelation> DataRelationships
        {
            get
            {
                return m_DataRelationships;
            }
        }
    }

    public class GenericQueryResultCollection : List<GenericQueryResult>, IFrameworkCollection
    {
        public SupportCreaElemento Prototype()
        {
            return new GenericQueryResult();
        }
    }

}
