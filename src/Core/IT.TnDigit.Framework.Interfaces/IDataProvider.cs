#region Using directives

using System;
using System.Collections;
using System.Data;
#endregion

namespace IT.TnDigit.ORM.Interfaces
{

    public interface IDataProvider
    {
        string JollyCharacter { get; }
        string parameterPlaceHolder { get; }

        /// <summary>
        /// Inizializzazione del provider
        /// </summary>
        /// <param name="initString">parametri di connessione</param>
        void Initialize(string initString);

        /// <summary>
        /// Chiusura del provider (termina la connessione)
        /// </summary>
        void Close();

        IDbConnection CreateConnection();

        #region LETTURA DATI
        System.Collections.IEnumerable GetItems(SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina, ref int risultati, IDbTransaction tx);

        System.Collections.IEnumerable GetItems(SupportCreaElemento prototype, IFilter filtro, string ordinamento, IDbTransaction tx);

        System.Collections.IEnumerable GetItemsFromSQL(SupportCreaElemento prototype, string comandoSQL, IDbTransaction tx);

        System.Collections.IEnumerable GetItemsFromSQL(SupportCreaElemento prototype, string comandoSQL, int elementiPerPagina, int pagina, ref int risultati, IDbTransaction tx);

        System.Collections.IEnumerable GetItemsFromCommand(SupportCreaElemento prototype, IDbCommand cmd, IDbTransaction tx);

        int CountItems(SupportCreaElemento prototype, IFilter filtro, IDbTransaction tx);

        int CountItemsFromSQL(string comandoSQL, IDbTransaction tx);

        string[] GetItemsField(SupportCreaElemento prototype, IFilter filtro, string ordinamento, string field, IDbTransaction tx);

        string[] GetItemsField(SupportCreaElemento prototype, IFilter filtro, string ordinamento, string field, int elementiPerPagina, int pagina, ref int risultati, IDbTransaction tx);

        #endregion

        #region STORED PROCEDURES
        IEnumerable LaunchStoredProcedure(IFrameworkStoredProcedure proc, IDbTransaction tx, out object result);
        #endregion

        #region INFO STATO E COMANDI
        string GetSQLCommand(SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina);

        string GetSQLCommand(SupportCreaElemento prototype, IFilter filtro, string ordinamento);

        ConnectionState GetConnectionState();
        #endregion

        #region TRANSAZIONI
        /// <summary>
        /// Apre una transazione sulla connessione attiva
        /// </summary>
        /// <returns>transazione aperta</returns>
        IDbTransaction OpenTransaction();

        /// <summary>
        /// Termina la transazione con un commit o un rollback
        /// </summary>
        /// <param name="tx">transazione</param>
        /// <param name="commit">se true esegue commit, altrimenti rollback</param>
        /// <returns>eccezione contenente eventuali errori</returns>
        void CloseTransaction(IDbTransaction tx, bool commit);
        #endregion

        #region FORMATTAZIONE DATI
        /// <summary>
        /// Formatta correttamente le date per i comandi SQL
        /// </summary>
        /// <param name="data">data da formattare</param>
        /// <returns>data formattata</returns>
        string DateFilter(string data);

        string DateTimeFilter(string data);

        string DateFilterTruncateField(string campo);
        #endregion

        #region GESTIONE OPERAZIONI STANDARD

        Exception InsertItem(IDataRecord d, Type type, IDbTransaction tx, ref long generatedID);

        Exception UpdateItem(IDataRecord d, Type type, IDbTransaction tx, out long recordCount, IFilter filter = null);

        Exception DeleteItem(IDataRecord d, Type type, IDbTransaction tx, out long recordCount);

        Exception DeleteItem(Type type, IFilter filtro, IDbTransaction tx, out long recordCount);
        #endregion

        #region GESTIONE BLOB / CLOB

        byte[] ReadBlob(IFrameworkObjectWithBlob item, string blobField, IDbTransaction tx);

        Exception WriteBlob(IFrameworkObjectWithBlob item, string blobField, byte[] blobData, IDbTransaction tx);

        Exception WriteClob(IFrameworkObjectWithBlob item, string blobField, string blobData, IDbTransaction tx);

        #endregion

        void ExecuteCommand(string comandoSQL, IDbTransaction tx);


    }
}
