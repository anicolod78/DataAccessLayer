using IT.TnDigit.ORM.Interfaces;
using System;
using System.Collections.Generic;


namespace IT.TnDigit.ORM.DataTypes
{

    public abstract class FrameworkObject : SupportCreaElemento, IFrameworkObject
    {
        private List<string> m_PropertiesChanged = new List<string>();

        public List<string> PropertiesChanged
        {
            get { return m_PropertiesChanged; }
        }

        /// <summary>
        /// Ritorna il nome della tabella o vista cui fa riferimento l'oggetto
        /// </summary>
        /// <returns>nome tabella/vista</returns>
        public virtual string TableName()
        {
            return "";
        }

        public virtual string FieldsList()
        {
            return "*";
        }

        public void SetPropertyChanged(string PropertyName)
        {
            if (m_PropertiesChanged.Contains(PropertyName.ToLower()) == false)
            {
                m_PropertiesChanged.Add(PropertyName.ToLower());
            }
        }

        public virtual bool PropertyChanged(string PropertyName)
        {
            return (m_PropertiesChanged.Contains(PropertyName.ToLower()));
        }

        protected void ResetPropertiesChanged()
        {
            m_PropertiesChanged.Clear();
        }

        /// <summary>
        /// Serializzazione dell'elemento
        /// </summary>
        /// <param name="writer">writer binario</param>
        /// <param name="value">valore da serializzare</param>
        protected void WriteSerialization(System.IO.BinaryWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write("#NULL#");
                return;
            }

            writer.Write(value.ToString());
            return;
        }

        /// <summary>
        /// Deserializzazione dell'elemento
        /// </summary>
        /// <param name="reader">reader binario</param>
        /// <returns>valore deserializzato</returns>
        protected static string ReadSerialization(System.IO.BinaryReader reader)
        {
            string value = reader.ReadString();

            if (value == "#NULL#")
            {
                return null;
            }

            return value;
        }

        /// <summary>
        /// conversione da data a decimal (yyyyMMdd)
        /// </summary>
        /// <param name="data">data da convertire</param>
        /// <returns>valore convertito</returns>
        public static decimal Date2Decimal(DateTime? data)
        {
            if (data == null)
                return 0;
            string anno = data.Value.Year.ToString();
            string mese = data.Value.Month.ToString();
            if (data.Value.Month < 10)
                mese = "0" + mese;
            string giorno = data.Value.Day.ToString();
            if (data.Value.Day < 10)
                giorno = "0" + giorno;


            return (decimal)long.Parse(string.Format("{0}{1}{2}", anno, mese, giorno));
        }

        /// <summary>
        /// conversione da data a decimal (yyyyMMdd)
        /// </summary>
        /// <param name="data">data da convertire in formato testo</param>
        /// <returns>valore convertito</returns>
        public static decimal Date2Decimal(String data)
        {
            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(data);
                return Date2Decimal(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Conversione da numero a data
        /// </summary>
        /// <param name="data">numero in formato yyyyMMdd</param>
        /// <returns>data convertita</returns>
        public static DateTime? Decimal2Date(decimal? data)
        {
            if (data == null)
                return null;

            if (data == 0)
                return DateTime.MinValue;

            try
            {
                string s = data.ToString();
                int anno = int.Parse(s.Substring(0, 4));
                int mese = int.Parse(s.Substring(4, 2));
                int giorno = int.Parse(s.Substring(6, 2));

                return new DateTime(anno, mese, giorno);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Conversione da numero a data in formato testo
        /// </summary>
        /// <param name="data">numero in formato yyyyMMdd</param>
        /// <returns>data convertita in formato ShortDate</returns>
        public static String Decimal2String(decimal? data)
        {
            if (data == 0 || data == null)
                return "";

            try
            {
                string s = data.ToString();
                int anno = int.Parse(s.Substring(0, 4));
                int mese = int.Parse(s.Substring(4, 2));
                int giorno = int.Parse(s.Substring(6, 2));

                DateTime d = new DateTime(anno, mese, giorno);

                return d.ToShortDateString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                return "";
            }
        }

        #region IFrameworkObject Membri di


        public virtual string SequenceName()
        {
            return string.Empty; ;
        }

        #endregion

        private List<BaseRelation> m_DataRelationships = new List<BaseRelation>();

        public List<BaseRelation> DataRelationships
        {
            get
            {
                return m_DataRelationships;
            }
        }
    }

    public class FrameworkObjectWithAction<T> where T : FrameworkObject
    {
        public T Object { get; set; }
        public Actions Action { get; set; }
    }

}

