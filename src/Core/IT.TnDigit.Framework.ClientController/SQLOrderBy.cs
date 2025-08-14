using System;
using System.Collections.Generic;
using System.Text;

namespace IT.TnDigit.ORM.ClientController
{
    public enum Ordine
    {
        Ascendente,
        Discendente
    }


    public class SQLOrderBy : IDisposable
    {
        private List<Ordinatore> entries = new List<Ordinatore>();

        public SQLOrderBy()
        {
        }

        public SQLOrderBy(string nomeCampo, Ordine ordinamento)
        {
            entries.Add(new Ordinatore(nomeCampo, ordinamento));
        }

        public void Add(Ordinatore ordine)
        {
            entries.Add(ordine);
        }

        public void Add(string nomeCampo, Ordine ordinamento)
        {
            entries.Add(new Ordinatore(nomeCampo, ordinamento));
        }

        private string TestoOrdinamento(Ordine ord)
        {
            switch (ord)
            {
                case Ordine.Ascendente:
                    return "";
                case Ordine.Discendente:
                    return "DESC";
                default:
                    return "";
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Ordinatore ord in entries)
            {
                sb.AppendFormat(",{0} {1}", ord.NomeCampo, TestoOrdinamento(ord.Ordinamento));
            }
            if (sb.ToString().Trim().Length > 0)
                sb.Remove(0, 1);

            return sb.ToString();
        }

        public void Reset()
        {
            entries.Clear();
        }

        /// <summary>
        /// Default destructor
        /// </summary>
        ~SQLOrderBy()
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
    }

    public class Ordinatore
    {
        private string m_nomeCampo = string.Empty;
        private Ordine m_ordinamento = Ordine.Ascendente;

        public string NomeCampo
        {
            get { return m_nomeCampo; }
        }

        public Ordine Ordinamento
        {
            get { return m_ordinamento; }
        }

        public Ordinatore(string nomeCampo, Ordine ordinamento)
        {
            m_nomeCampo = nomeCampo;
            m_ordinamento = ordinamento;
        }
    }
}
