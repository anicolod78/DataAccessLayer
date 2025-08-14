namespace IT.TnDigit.ORM.Interfaces
{
    public interface IControllerSQLText
    {
        /// <summary>
        /// Counts the elements based on the SQL command
        /// </summary>
        /// <param name="comandoSQL">selection SQL command</param>
        /// <returns>number of elements found</returns>
        int ConteggioElementi(string comandoSQL);

        /// <summary>
        /// Reads the elements based on the SQL command and the Generic type
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection</typeparam>
        /// <param name="collection">prototype of the collection</param>
        /// <param name="comandoSQL">selection SQL command</param>
        /// <returns>elements found</returns>
        T LeggiElementiSQL<T>(T collection, string comandoSQL) where T : IFrameworkCollection;

        /// <summary>
        /// Reads the elements based on the SQL command and the Generic type and returns 
        /// the elements count
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection</typeparam>
        /// <param name="collection">prototype of the collection</param>
        /// <param name="comandoSQL">selection SQL command</param>
        /// <param name="risultati">number of elements found</param>
        /// <returns>elements found</returns>
        T LeggiElementiSQL<T>(T collection, string comandoSQL, ref int risultati) where T : IFrameworkCollection;

        /// <summary>
        /// Reads the elements based on the SQL command and the Generic type with pagination
        /// and returns the totale elements count
        /// </summary>
        /// <typeparam name="T">IFrameworkCollection</typeparam>
        /// <param name="collection">prototype of the collection</param>
        /// <param name="comandoSQL">selection SQL command</param>
        /// <param name="pagina">page to get</param>
        /// <param name="elementiPagina">elements per page</param>
        /// <param name="risultati">number of elements found</param>
        /// <returns>elements found</returns>
        T LeggiElementiSQLPaginati<T>(T collection, string comandoSQL, int pagina, int elementiPagina, ref int risultati) where T : IFrameworkCollection;


        T LeggiElementiCommand<T>(T collection, System.Data.IDbCommand cmd) where T : IFrameworkCollection;
    }
}
