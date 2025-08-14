using IT.TnDigit.ORM.DataStorage;
using IT.TnDigit.ORM.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IT.TnDigit.ORM.ClientController
{
    /// <summary>
    /// Creates a SQL Filter
    /// </summary>
    public class SQLFilter : IDisposable, IFilter
    {


        private SortedList entries = new SortedList();
        private ArrayList listaFiltri = new ArrayList();
        private bool filtriInOr = false;
        private Storage refStorage = null;
        private List<QueryParameter> parameters = new List<QueryParameter>();
        private string SQLappended = "";

        public List<QueryParameter> Parameters
        {
            get { return parameters; }
        }

        public SQLFilter()
        {
            refStorage = DataStorage.Storage.ParticularStorage(string.Empty);
        }

        public SQLFilter(string storage)
        {
            refStorage = DataStorage.Storage.ParticularStorage(storage);
        }

        public SQLFilter(IDataProvider storage)
        {
            refStorage = DataStorage.Storage.ParticularStorage(storage);
        }

        public SQLFilter(bool forzaFiltriInOr, string storage)
        {
            this.filtriInOr = forzaFiltriInOr;
            refStorage = DataStorage.Storage.ParticularStorage(storage);
        }

        public SQLFilter(bool forzaFiltriInOr, IDataProvider storage)
        {
            this.filtriInOr = forzaFiltriInOr;
            refStorage = DataStorage.Storage.ParticularStorage(storage);
        }

        private object LimitatoriTesto(object valore)
        {
            if (valore.GetType().Name != "String")
            {
                return valore;
            }
            else
            {
                return string.Format("'{0}'", valore);
            }
        }

        private string FuzzificatoreTesto(object testo, int score)
        {
            if (score < 1 || score > 80)
                throw new Exception("Il valore di score impostato � fuori dai limiti consentiti (1-80)");

            StringBuilder sb = new StringBuilder();
            string[] arr = testo.ToString().Split(Convert.ToChar(" "));

            sb.Append(string.Format("%{0}% or ", testo.ToString()));

            for (int i = 0; i < arr.Length - 1; i++)
            {
                sb.Append(string.Format("fuzzy({0},{1},50,weight) or ", arr[i], score.ToString()));
            }
            sb.Append(string.Format("fuzzy({0},{1},50,weight)", arr[arr.Length - 1], score.ToString()));

            return sb.ToString();
        }


        public String[] FiltriApplicati()
        {
            return (String[])listaFiltri.ToArray(typeof(System.String));

        }

        /// <summary>
        /// Aggiunta di un filtro SQL
        /// </summary>
        /// <param name="key">chiave per gestione operatori logici</param>
        /// <param name="filter">filtro SQL da applicare</param>
        public void Add(string key, string filter)
        {
            if (entries.ContainsKey(key.ToUpper()))
            {
                entries[key.ToUpper()] = entries[key.ToUpper()].ToString() + " OR " + filter;
            }
            else
            {
                entries.Add(key.ToUpper(), filter);
            }
        }

        /// <summary>
        /// Aggiunta di un filtro di tipo a avanzato (es. Fuzzy contains)
        /// </summary>
        /// <param name="campo">campo da filtrare</param>
        /// <param name="operatore">operatore di filtro</param>
        /// <param name="valore">valore per cui filtrare</param>
        /// <param name="score">ampiezza del filtro (valore da 1 a 80)</param>
        public void Add(string campo, OperatoriFiltroAvanzati operatore, object valore, int score)
        {
            string key = campo;
            string filtro = "";

            if (valore == null)
            {
                return;
            }
            else
            {
                if (valore.GetType().Name != "String")
                {
                    return;
                }
                else
                {
                    valore = valore.ToString().Replace("'", "''").Trim();
                    if (operatore == OperatoriFiltroAvanzati.FuzzyContain)
                        filtro = string.Format(TranslateOperatoriFiltro.Get(operatore), campo, FuzzificatoreTesto(valore, score));
                }
            }

            listaFiltri.Add(string.Format("<b>{0}</b>: {1}", key, valore));

            this.Add(key, filtro);
        }

        public void Add(string campo, OperatoriFiltroMultipli operatore, object valore1, object valore2)
        {
            this.Add(campo, operatore, valore1, valore2, false);
        }

        /// <summary>
        /// Aggiunta di un filtro di tipo a valori multipli (es. Between)
        /// </summary>
        /// <param name="campo">campo da filtrare</param>
        /// <param name="operatore">operatore di filtro</param>
        /// <param name="valore1">valore per cui filtrare</param>
        /// <param name="valore2">valore per cui filtrare</param>
        public void Add(string campo, OperatoriFiltroMultipli operatore, object valore1, object valore2, bool useTime)
        {
            string key = campo;
            //Creazione nome parametro
            string nomeParametro1 = refStorage.parameterPlaceHolder + parameters.Count.ToString();
            string nomeParametro2 = refStorage.parameterPlaceHolder + (parameters.Count + 1).ToString();

            string filtro = "";
            listaFiltri.Add(string.Format("<b>{0}</b>: Tra {1} e {2}", key, valore1, valore2));

            if (valore1 == null || valore2 == null)
            {
                return;
            }
            else
            {
                QueryParameter par = new QueryParameter();
                //Il valore viene registrato nel parametro
                par.Value = valore1;
                par.ParameterName = nomeParametro1;
                parameters.Add(par);

                par = new QueryParameter();
                //Il valore viene registrato nel parametro
                par.Value = valore2;
                par.ParameterName = nomeParametro2;
                par.useTime = useTime;
                parameters.Add(par);

                if (useTime || !(valore1 is DateTime))
                {
                    filtro = string.Format(TranslateOperatoriFiltro.Get(operatore),
                        campo, nomeParametro1, nomeParametro2);
                }
                else
                {
                    filtro = string.Format(TranslateOperatoriFiltro.GetTruncDate(operatore),
                        campo, nomeParametro1, nomeParametro2);
                }
            }

            this.Add(key, filtro);
        }


        /// <summary>
        /// Aggiunta di un filtro su di un campo (in caso di AND sullo stesso campo)
        /// </summary>
        /// <param name="campo">campo da filtrare</param>
        /// <param name="operatore">operatore di filtro</param>
        /// <param name="valore">valore o SQL nidificato per cui filtrare</param>
        /// <param name="distintivo">suffisso distintivo del campo per oerare AND al posto di OR</param>
        public void Add(string campo, OperatoriFiltro operatore, object valore, string distintivo)
        {
            string key = campo;
            string filtro = "";
            listaFiltri.Add(string.Format("<b>{0}</b>: {1}", key, valore));

            if (operatore == OperatoriFiltro.In
                || operatore == OperatoriFiltro.NotIn
                || operatore == OperatoriFiltro.Contains)
            {
                //operatore non parametrico
                filtro = string.Format(TranslateOperatoriFiltro.Get(operatore), campo, valore);
            }
            else
            {
                //Null value non parametrico
                if (valore == null)
                {
                    switch (operatore)
                    {
                        case OperatoriFiltro.Uguale:
                            filtro = string.Format("{0} IS NULL", campo);
                            break;
                        case OperatoriFiltro.Diverso:
                        default:
                            filtro = string.Format("{0} IS NOT NULL", campo);
                            break;
                    }
                }
                else
                {
                    string nomeParametro = refStorage.parameterPlaceHolder + parameters.Count.ToString();

                    switch (operatore)
                    {
                        case OperatoriFiltro.Contiene:
                            valore = refStorage.JollyCharacter + valore.ToString() + refStorage.JollyCharacter;
                            break;
                        case OperatoriFiltro.IniziaCon:
                            valore = valore.ToString() + refStorage.JollyCharacter;
                            break;
                        case OperatoriFiltro.FinisceCon:
                            valore = refStorage.JollyCharacter + valore.ToString();
                            break;
                        default:
                            break;
                    }

                    #region Creazione where parametrica e parametri

                    QueryParameter par = new QueryParameter();
                    //Il valore viene registrato nel parametro (dbnull se necessario)
                    par.Value = valore;
                    par.ParameterName = nomeParametro;
                    parameters.Add(par);

                    //creo il filtro come CAMPO op :PPARAMETRO
                    filtro = string.Format(TranslateOperatoriFiltro.Get(operatore),
                        campo, nomeParametro);


                    #endregion
                }
            }

            this.Add(key + distintivo, filtro);

        }

        /// <summary>
        /// Aggiunta di un filtro su di un campo
        /// </summary>
        /// <param name="campo">campo da filtrare</param>
        /// <param name="operatore">operatore di filtro</param>
        /// <param name="valore">valore o SQL nidificato per cui filtrare</param>
        public void Add(string campo, OperatoriFiltro operatore, object valore)
        {
            this.Add(campo, operatore, valore, "");
        }


        public void Add(string campo, OperatoriFiltro operatore, DateTime? valore, bool useTime)
        {
            if (operatore != OperatoriFiltro.Diverso
                && operatore != OperatoriFiltro.Maggiore
                && operatore != OperatoriFiltro.MaggioreUguale
                && operatore != OperatoriFiltro.Minore
                && operatore != OperatoriFiltro.MinoreUguale
                && operatore != OperatoriFiltro.Uguale)
            {
                throw new Exception("Operatore non valido per un campo date: " + campo + " " + TranslateOperatoriFiltro.Get(operatore) + ".");
            }

            string key = campo;
            string filtro = "";
            listaFiltri.Add(string.Format("<b>{0}</b>: {1}", key, valore));


            //Null value non parametrico
            if (valore.HasValue == false)
            {
                switch (operatore)
                {
                    case OperatoriFiltro.Uguale:
                        filtro = string.Format("{0} IS NULL", campo);
                        break;
                    case OperatoriFiltro.Diverso:
                    default:
                        filtro = string.Format("{0} IS NOT NULL", campo);
                        break;
                }
            }
            else
            {
                string nomeParametro = refStorage.parameterPlaceHolder + parameters.Count.ToString();

                #region Creazione where parametrica e parametri

                QueryParameter par = new QueryParameter();
                //Il valore viene registrato nel parametro (dbnull se necessario)
                par.Value = valore.Value;
                par.ParameterName = nomeParametro;
                par.useTime = useTime;
                parameters.Add(par);

                //creo il filtro come CAMPO op :PPARAMETRO
                if (useTime)
                {
                    filtro = string.Format(TranslateOperatoriFiltro.Get(operatore),
                       campo, nomeParametro);
                }
                else
                {
                    filtro = string.Format(TranslateOperatoriFiltro.GetTruncDate(operatore),
                        campo, nomeParametro);
                }

                #endregion
            }

            this.Add(key, filtro);
        }


        public void Add(string campo, OperatoriContainsMultipli operatore, string[] valore)
        {
            string key = campo;
            string filtro = "";
            string paramlist = "";

            if (valore == null || valore.Length == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < valore.Length; i++)
                {
                    //gestisco gli apici nelle stringhe
                    valore[i] = valore[i].ToString().Replace("'", "''").Trim();
                }

                switch (operatore)
                {
                    case OperatoriContainsMultipli.ContainsOR:
                        paramlist = "'%" + String.Join("% OR %", valore) + "%'";
                        break;
                    case OperatoriContainsMultipli.ContainsAND:
                        paramlist = "'%" + String.Join("% AND %", valore) + "%'";
                        break;
                }

                paramlist = paramlist.Replace("%", refStorage.JollyCharacter);
                filtro = string.Format(TranslateOperatoriFiltro.Get(operatore), campo, paramlist);
            }

            listaFiltri.Add(string.Format("<b>{0}</b>: {1}", key, paramlist));

            this.Add(key, filtro);
        }

        /// <summary>
        /// Ritorna una string contenente il filtro SQL (es. (ID=10) AND (STATE=1) )
        /// </summary>
        /// <returns>stringa del filtro</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DictionaryEntry entry in entries)
            {
                if (sb.Length > 0)
                {
                    if (filtriInOr == true)
                        sb.Append(" OR ");
                    else
                        sb.Append(" AND ");
                }
                sb.Append(String.Format("({0})", entry.Value));
            }

            sb.Append(SQLappended);

            return sb.ToString();
        }



        public string ToNonParametricString()
        {
            string s = this.ToString();

            for (int i = this.parameters.Count - 1; i >= 0; i--)
            {
                QueryParameter p = this.parameters[i];

                string val = "";
                val = p.Value.ToString();

                if (p.Value is string)
                    val = LimitatoriTesto(p.Value).ToString();

                if (p.Value is DateTime)
                    val = refStorage.DateFilter("X", p.Value, p.useTime);

                s = s.Replace(p.ParameterName, val);
            }


            return s;
        }



        public void Reset()
        {
            entries.Clear();
            listaFiltri.Clear();
            parameters.Clear();
        }

        public IFilter CreateNewFilter()
        {
            return new SQLFilter(this.refStorage.Name);
        }

        /// <summary>
        /// Default destructor
        /// </summary>
        ~SQLFilter()
        {
            Dispose();
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        #region IFilter Members

        public void AppendSQL(string SQL)
        {
            SQLappended = " " + SQLappended + SQL;
        }

        #endregion


    }
}
