namespace IT.TnDigit.ORM.Interfaces
{
    public interface IController
    {
        System.Data.ConnectionState GetConnectionState();
        System.Data.ConnectionState GetConnectionStateNoTx();

        void OpenTransaction();
        void CloseTransaction(bool commit);
        bool TransactionIsClosed { get; }

        /// <summary>
        /// Deletes all the entities of the prototype's type which fit the filter
        /// </summary>
        /// <param name="st">elements prototype</param>
        /// <param name="filter">sql filter</param>
        long CancellaEntita(IFrameworkObject st, IFilter filter);

        /// <summary>
        /// Deletes the entity based on the value of the primary keys set in the object
        /// </summary>
        /// <param name="st">element to delete</param>
        bool CancellaEntita(IFrameworkObject st);

        /// <summary>
        /// Counts the elements which fit the filter 
        /// </summary>
        /// <param name="collection">prototype collection</param>
        /// <param name="filter">sql filter</param>
        /// <returns>number of elements found</returns>
        int ConteggioElementi(IFrameworkCollection collection, IFilter filter);

        /// <summary>
        /// Returns the SQL text command composed starting from the parameters
        /// </summary>
        /// <param name="collection">prototype collection</param>
        /// <param name="filter">sql filter</param>
        /// <param name="order">order by</param>
        /// <param name="pagina">page to get</param>
        /// <param name="elementiPagina">elements per page</param>
        /// <returns>sql string</returns>
        string GetSQLCommand(IFrameworkCollection collection, IFilter filter, string order, int pagina, int elementiPagina);

        /// <summary>
        /// Returns the SQL text command composed starting from the parameters
        /// </summary>
        /// <param name="collection">prototype collection</param>
        /// <param name="filter">sql filter</param>
        /// <param name="order">order by</param>
        /// <returns>sql string</returns>
        string GetSQLCommand(IFrameworkCollection collection, IFilter filter, string order);

        /// <summary>
        /// Create a new record with the data of the element
        /// </summary>
        /// <param name="st">element to create</param>
        /// <returns>if autonumbered id, returns the new id generated</returns>
        long InserisciEntita(IFrameworkObject st);

        /// <summary>
        /// Update the record of the element with the value altered
        /// </summary>
        /// <param name="st">element to update</param>
        long ModificaEntita(IFrameworkObject st, IFilter filter = null);

        /// <summary>
        /// Returns the first element fitting the search parameters
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection</typeparam>
        /// <param name="collection">prototype collection</param>
        /// <param name="filter">sql filter</param>
        /// <returns>elements found</returns>
        T GetElemento<T>(IFrameworkCollection collection, IFilter filter) where T : IFrameworkObject;

        /// <summary>
        /// Reads the elements based on the parameters and the Generic type and returns 
        /// the elements count
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection</typeparam>
        /// <param name="collection">prototype of the collection</param>
        /// <param name="filter">sql filter</param>
        /// <param name="order">order by</param>
        /// <param name="risultati">number of elements found</param>
        /// <returns>elements found</returns>
        T LeggiElementi<T>(T collection, IFilter filter, string order, ref int risultati) where T : IFrameworkCollection;

        /// <summary>
        /// Reads the elements based on the parameters and the Generic type
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection</typeparam>
        /// <param name="collection">prototype of the collection</param>
        /// <param name="filter">sql filter</param>
        /// <param name="order">order by</param>
        /// <returns>elements found</returns>
        T LeggiElementi<T>(T collection, IFilter filter, string order) where T : IFrameworkCollection;


        /// <summary>
        /// Reads the elements based on the parameters and the Generic type and returns
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection</typeparam>
        /// <param name="collection">prototype of the collection</param>
        /// <param name="filter">sql filter</param>
        /// <param name="order">order by</param>
        /// <param name="pagina">page to get</param>
        /// <param name="elementiPagina">elements per page</param>
        /// <param name="risultati">total number of elements found</param>
        /// <returns>elements found</returns>
        T LeggiElementiPaginati<T>(T collection, IFilter filter, string order, int pagina, int elementiPagina, ref int risultati) where T : IFrameworkCollection;

        /// <summary>
        /// Reads the elements based on the parameters and the Generic type and returns an array
        /// containing the value of the specified field for each element found
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection</typeparam>
        /// <param name="collection">prototype of the collection</param>
        /// <param name="filtro">sql filter</param>
        /// <param name="field">field to retrieve</param>
        /// <returns>values array</returns>
        string[] GetItemsField<T>(T collection, IFilter filter, string field) where T : IFrameworkCollection;

        /// <summary>
        /// Reads the elements which are in the result set of another search
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection type to return</typeparam>
        /// <typeparam name="K">IFrameworkCollection type to search in</typeparam>
        /// <param name="collection">prototype of the collection to retrun</param>
        /// <param name="filter">sql filter</param>
        /// <param name="order">order by</param>
        /// <param name="searchCollection">prototype of the collection to search in</param>
        /// <param name="field">field of T</param>
        /// <param name="searchField">field of K</param>
        /// <returns>elements found</returns>
        T LeggiElementi<T, K>(T collection, IFilter filter, string order, K searchCollection, string field, string searchField)
            where T : IFrameworkCollection
            where K : IFrameworkCollection;

        /// <summary>
        /// Reads the elements paginated which are in the result set of another search
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection type to return</typeparam>
        /// <typeparam name="K">IFrameworkCollection type to search in</typeparam>
        /// <param name="collection">prototype of the collection to retrun</param>
        /// <param name="filter">sql filter</param>
        /// <param name="order">order by</param>
        /// <param name="searchCollection">prototype of the collection to search in</param>
        /// <param name="field">field of T</param>
        /// <param name="searchField">field of K</param>
        /// <param name="pagina">page to get</param>
        /// <param name="elementiPagina">elements per page</param>
        /// <param name="risultati">total number of elements found</param>
        /// <returns>elements found</returns>
        T LeggiElementiPaginati<T, K>(T collection, IFilter filter, string order, K searchCollection, string field, string searchField, int pagina, int elementiPagina, ref int risultati)
            where T : IFrameworkCollection
            where K : IFrameworkCollection;



        /// <summary>
        /// Executes the Function or Stored Procedure and eventually fills the output params
        /// </summary>
        /// <param name="proc">procedure to execute</param>
        object LaunchStoredProcedure(IFrameworkStoredProcedure proc);

        /// <summary>
        /// Return the binary content of the BLOB field
        /// </summary>
        /// <param name="item">element to retrieve the BLOB value for</param>
        /// <param name="blobField">blob field name</param>
        /// <returns>binary</returns>
        byte[] ReadBlob(IFrameworkObjectWithBlob item, string blobField);

        /// <summary>
        /// Writes the binary in the BLOB field
        /// </summary>
        /// <param name="item">element to update the BLOB value for</param>
        /// <param name="blobField">blob field name</param>
        /// <param name="blobData">binary to write</param>
        void WriteBlob(IFrameworkObjectWithBlob item, string blobField, byte[] blobData);

        /// <summary>
        /// Writes the binary in the CLOB field
        /// </summary>
        /// <param name="item">element to update the CLOB value for</param>
        /// <param name="clobField">clob field name</param>
        /// <param name="clobData">long string to write</param>
        void WriteClob(IFrameworkObjectWithBlob item, string clobField, string clobData);
    }
}
