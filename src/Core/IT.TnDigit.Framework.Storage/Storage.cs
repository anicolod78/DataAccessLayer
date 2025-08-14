#region Using directives

using IT.TnDigit.ORM.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion


namespace IT.TnDigit.ORM.DataStorage
{
    public class Storage : IDisposable
    {
        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken,
           uint dwFlags, [System.Runtime.InteropServices.Out] StringBuilder pszPath);


        private IDataProvider provider;
        private string name;

        public string Name
        {
            get { return name; }
        }

        private Assembly GetRelevantAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
                return entryAssembly;

            return FindApplicationAssembly();
        }

        private Assembly FindApplicationAssembly()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Filtra gli assembly di sistema e tool
            var applicationAssemblies = assemblies
                .Where(IsApplicationAssembly)
                .ToList();

            // Cerca assembly con UserSecretsId tra quelli dell'applicazione
            var assemblyWithSecrets = applicationAssemblies
                .FirstOrDefault(HasUserSecrets);

            if (assemblyWithSecrets != null)
                return assemblyWithSecrets;

            // Fallback: primo assembly dell'applicazione
            return applicationAssemblies.FirstOrDefault();
        }

        private bool IsApplicationAssembly(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;

            // Escludi assembly di sistema e tool
            var systemPrefixes = new[]
            {
            "System.",
            "Microsoft.",
            "mscorlib",
            "netstandard",
            "Newtonsoft.",
            "EntityFramework",
            "WebGrease",
            "Antlr3",
            "DotNetOpenAuth",
            "WebMatrix",
            "App_Web_", // Assembly dinamici di ASP.NET
            "App_Code", // Assembly dinamici di ASP.NET
            "Anonymously Hosted DynamicMethods Assembly"
        };

            // Esclusioni specifiche per web tools
            var webToolsExclusions = new[]
            {
            "Microsoft.WebTools.BrowserLink.Runtime",
            "Microsoft.Web.Infrastructure",
            "Microsoft.Owin",
            "Owin"
        };

            return !systemPrefixes.Any(prefix => assemblyName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) &&
                   !webToolsExclusions.Any(exclusion => assemblyName.Equals(exclusion, StringComparison.OrdinalIgnoreCase)) &&
                   !assembly.GlobalAssemblyCache; // Esclude GAC
        }

        private bool HasUserSecrets(Assembly assembly)
        {
            // Verifica se l'assembly ha UserSecretsIdAttribute
            var userSecretsAttr = assembly.GetCustomAttribute<UserSecretsIdAttribute>();
            if (userSecretsAttr != null)
            {
                return true;
            }

            // Alternativa: cerca negli assembly attributes
            var attributes = assembly.GetCustomAttributes(typeof(UserSecretsIdAttribute), false);
            if (attributes.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Inizializzazione del provider di dati sulla base delle informazioni 
        /// contenute nel registro di sistema.
        /// Se nella stringa di connessione ? contenuto il tag [STARTUP] sar?
        /// sostituito con il percorso dell'applicazione
        /// </summary>
        private Storage(string DataSource)
        {

            var c = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();


            if (File.Exists($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json"))
            {
                c.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
            }

            var entryAssembly = GetRelevantAssembly();
            if (entryAssembly != null) {
                c.AddUserSecrets(entryAssembly, optional: true);
                c.AddXmlUserSecrets(entryAssembly);
            }

            IConfigurationRoot configuration = c.Build();


            // Crea la configurazione usando l'assembly di entry
            this.name = DataSource;

            try
            {
                string providerType = "";
                string providerInitString = "";

                #region CONFIG
                try
                {                    
                    providerType = configuration["DataProviders:Type" + DataSource];
                    providerInitString = configuration["DataProviders:InitString" + DataSource];
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                    throw;
                }
                #endregion

                #region ISTANTIATE PROVIDER
                try
                {
                    if (providerInitString.IndexOf("[STARTUP]") > 0)
                    {
                        string startupPth = Directory.GetCurrentDirectory();
                        providerInitString = providerInitString.Replace("[STARTUP]", startupPth);
                    }
                    if (providerInitString.IndexOf("[USERAPPDATA]") > 0)
                    {
                        string userAppDataPth = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                        providerInitString = providerInitString.Replace("[USERAPPDATA]", userAppDataPth);
                    }
                    if (providerInitString.IndexOf("[USERDOCUMENTS]") > 0)
                    {
                        string documentsPth = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                        providerInitString = providerInitString.Replace("[USERDOCUMENTS]", documentsPth);
                    }
                    if (providerInitString.IndexOf("[SHAREDDOCUMENTS]") > 0)
                    {
                        int SIDL_COMMON_DOCUMENTS = 0x002e;
                        StringBuilder sb = new StringBuilder();
                        SHGetFolderPath(IntPtr.Zero, SIDL_COMMON_DOCUMENTS, IntPtr.Zero, 0x0000, sb);
                        string sharedDocumentsPth = sb.ToString();

                        providerInitString = providerInitString.Replace("[SHAREDDOCUMENTS]", sharedDocumentsPth);
                    }
                    this.provider = (IDataProvider)Type.GetType(providerType).GetConstructor(new Type[] { }).Invoke(null);
                    this.provider.Initialize(providerInitString);
                }
                catch (Exception ex)
                {
                    var env = configuration.AsEnumerable().Select(x => $"{x.Key}:{x.Value}").ToArray();

                    throw new Exception($"{ex.Message} - {DataSource}");
                }
                #endregion
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw;
            }
        }

        private Storage(IDataProvider dataProvider)
        {
            this.provider = dataProvider;
        }

        /// <summary>
        /// Ritorna lo storage specifico per la sorgente dati gestendo la cache se necessario
        /// </summary>
        /// <param name="dataSource">provider specifico o null per default</param>
        /// <returns></returns>
        public static Storage ParticularStorage(string dataSource)
        {
            //se non specificato ritorno il provider di default
            if (dataSource == null)
                dataSource = string.Empty;

            return new Storage(dataSource);
        }

        public static Storage ParticularStorage(IDataProvider dataSource)
        {
            return new Storage(dataSource);
        }

        public void Dispose()
        {
            provider.Close();
        }

        #region LETTURA DATI
        public IFrameworkCollection GetItems(SupportCreaElemento prototype, IFrameworkCollection collection, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina, ref int risultati, IDbTransaction tx)
        {
            try
            {
                System.Collections.IEnumerable items = provider.GetItems(prototype, filtro, ordinamento, elementiPerPagina, pagina, ref risultati, tx);
                collection = FillCollection(prototype, collection, items);

                //risultati = provider.CountItems(prototype, filtro, tx);
            }
            catch (Exception ex)
            {
                ((IList)collection).Clear();
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            return collection;
        }

        public IFrameworkCollection GetItems(SupportCreaElemento prototype, IFrameworkCollection collection, IFilter filtro, string ordinamento, ref int risultati, IDbTransaction tx)
        {
            try
            {
                System.Collections.IEnumerable items = provider.GetItems(prototype, filtro, ordinamento, tx);

                collection = FillCollection(prototype, collection, items);
            }
            catch (Exception ex)
            {
                ((IList)collection).Clear();
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            risultati = ((IList)collection).Count;

            return collection;
        }

        public IFrameworkCollection GetItemsFromSQL(SupportCreaElemento prototype, IFrameworkCollection collection, string comandoSQL, ref int risultati, IDbTransaction tx)
        {
            try
            {
                System.Collections.IEnumerable items = provider.GetItemsFromSQL(prototype, comandoSQL, tx);

                collection = FillCollection(prototype, collection, items);
            }
            catch (Exception ex)
            {
                ((IList)collection).Clear();
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            risultati = ((IList)collection).Count;

            return collection;
        }


        public object GetItemsFromCommand(SupportCreaElemento prototype, IFrameworkCollection collection, IDbCommand cmd, ref int risultati, IDbTransaction tx)
        {
            try
            {
                System.Collections.IEnumerable items = provider.GetItemsFromCommand(prototype, cmd, tx);

                collection = FillCollection(prototype, collection, items);
            }
            catch (Exception ex)
            {
                ((IList)collection).Clear();
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            risultati = ((IList)collection).Count;

            return collection;
        }

        private IFrameworkCollection FillCollection(SupportCreaElemento prototype, IFrameworkCollection collection, System.Collections.IEnumerable items)
        {
            return FillCollection(prototype, collection, items, null, "");

        }

        private IFrameworkCollection FillCollection(SupportCreaElemento prototype, IFrameworkCollection collection, System.Collections.IEnumerable items, Dictionary<string, int> itemsOrder, string fieldToOrder)
        {
            if (items != null)
            {
                bool hasRows = true;
                try
                {
                    hasRows = ((System.Data.Common.DbDataReader)items).HasRows;
                }
                catch { }

                if (items is System.Data.Common.DbDataReader && hasRows == false)
                {
                    ((IDataReader)items).Close();
                    return collection;
                }

                if (itemsOrder == null)
                {
                    foreach (IDataRecord item in items)
                    {
                        object obj = prototype.CreaElemento(item);

                        ((IList)collection).Add(obj);
                    }
                }
                else
                {
                    Object[] orderedList = new Object[itemsOrder.Count];
                    int i = 0;

                    foreach (IDataRecord item in items)
                    {
                        object obj = prototype.CreaElemento(item);
                        string key = item[fieldToOrder].ToString();
                        if (itemsOrder.ContainsKey(key))
                        {
                            orderedList[itemsOrder[key]] = obj;
                        }

                        i++;
                    }

                    foreach (object item in orderedList)
                    {
                        ((IList)collection).Add(item);
                    }
                }
                ((IDataReader)items).Close();

            }


            return collection;
        }

        public IFrameworkCollection GetItems(SupportCreaElemento prototype, IFrameworkCollection collection, string comandoSQL, int elementiPerPagina, int pagina, ref int risultati, IDbTransaction tx)
        {
            try
            {
                System.Collections.IEnumerable items = provider.GetItemsFromSQL(prototype, comandoSQL, elementiPerPagina, pagina, ref risultati, tx);
                risultati = provider.CountItemsFromSQL(comandoSQL, tx);
                collection = FillCollection(prototype, collection, items);
            }
            catch (Exception ex)
            {
                ((IList)collection).Clear();
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            return collection;
        }

        public int CountItems(SupportCreaElemento prototype, IFilter filtro, IDbTransaction tx)
        {
            int results = 0;
            try
            {
                results = provider.CountItems(prototype, filtro, tx);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            return results;
        }

        public int CountItemsFromSQL(string comandoSQL, IDbTransaction tx)
        {
            int results = 0;
            try
            {
                results = provider.CountItemsFromSQL(comandoSQL, tx);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            return results;
        }

        public IFrameworkCollection GetItems(SupportCreaElemento prototype, IFrameworkCollection collection, IFilter filtro, string ordinamento, SupportCreaElemento prototypeSearch, string field, string searchField, IDbTransaction tx)
        {
            try
            {
                string[] searchResults = provider.GetItemsField(prototypeSearch, filtro, ordinamento, searchField, tx);

                if (searchResults.Length == 0)
                    return collection;

                IFilter filtroEstrazione = filtro.CreateNewFilter();


                if (searchResults.Length > 1000)
                {
                    int i = 0;
                    while (searchResults.Length >= i)
                    {
                        filtroEstrazione.Add(field, OperatoriFiltro.In, string.Join(",", searchResults, i, 1000));
                        //listFiltroEstrazione.Add(string.Format("{0} in ({1})", field, string.Join(",", searchResults, i, 1000)));
                        i += 1000;
                    }

                    //filtroEstrazione = string.Join(" OR ",listFiltroEstrazione.ToArray());
                }
                else
                {
                    filtroEstrazione.Add(field, OperatoriFiltro.In, string.Join(",", searchResults));
                    //filtroEstrazione = string.Format("{0} in ({1})", field, string.Join(",", searchResults));
                }

                //filtroEstrazione.Add(field, OperatoriFiltro.In, string.Join(",", searchResults));
                //string filtroEstrazione = string.Format("{0} in ({1})", field, string.Join(",", searchResults));

                System.Collections.IEnumerable items = provider.GetItems(prototype, filtroEstrazione, "", tx);

                //create identity order if order by is used
                if (string.IsNullOrEmpty(ordinamento))
                {
                    collection = FillCollection(prototype, collection, items);
                }
                else
                {
                    Dictionary<string, int> itemsOrder = new Dictionary<string, int>();
                    for (int i = 0; i < searchResults.Length; i++)
                    {
                        itemsOrder.Add(searchResults[i], i);
                    }

                    collection = FillCollection(prototype, collection, items, itemsOrder, field);
                }
            }
            catch (Exception ex)
            {
                ((IList)collection).Clear();
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            return collection;
        }

        public IFrameworkCollection GetItems(SupportCreaElemento prototype, IFrameworkCollection collection, IFilter filtro, string ordinamento, SupportCreaElemento prototypeSearch, string field, string searchField, int elementiPerPagina, int pagina, ref int risultati, IDbTransaction tx)
        {
            try
            {
                string[] searchResults = provider.GetItemsField(prototypeSearch, filtro, ordinamento, searchField, elementiPerPagina, pagina, ref risultati, tx);

                if (searchResults.Length == 0)
                    return collection;

                //Split filtri IN max 1000 elementi
                //List<string> listFiltroEstrazione = new List<string>();
                IFilter filtroEstrazione = filtro.CreateNewFilter();

                if (searchResults.Length > 1000)
                {
                    int i = 0;
                    while (searchResults.Length >= i)
                    {
                        filtroEstrazione.Add(field, OperatoriFiltro.In, string.Join(",", searchResults, i, 1000));
                        //listFiltroEstrazione.Add(string.Format("{0} in ({1})", field, string.Join(",", searchResults, i, 1000)));
                        i += 1000;
                    }

                    //filtroEstrazione = string.Join(" OR ",listFiltroEstrazione.ToArray());
                }
                else
                {
                    filtroEstrazione.Add(field, OperatoriFiltro.In, string.Join(",", searchResults));
                    //filtroEstrazione = string.Format("{0} in ({1})", field, string.Join(",", searchResults));
                }

                System.Collections.IEnumerable items = provider.GetItems(prototype, filtroEstrazione, "", tx);

                //create identity order if order by is used
                if (string.IsNullOrEmpty(ordinamento))
                {
                    collection = FillCollection(prototype, collection, items);
                }
                else
                {
                    Dictionary<string, int> itemsOrder = new Dictionary<string, int>();
                    for (int i = 0; i < searchResults.Length; i++)
                    {
                        itemsOrder.Add(searchResults[i], i);
                    }

                    collection = FillCollection(prototype, collection, items, itemsOrder, field);
                }

            }
            catch (Exception ex)
            {
                ((IList)collection).Clear();
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            return collection;
        }

        public string[] GetItemsField(SupportCreaElemento prototype, IFilter filtro, string field, IDbTransaction tx)
        {
            string[] results = new string[0];
            try
            {
                results = provider.GetItemsField(prototype, filtro, field, field, tx);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            return results;
        }



        #endregion

        #region ESECUZIONE PROCEDURE

        public void LaunchStoredProcedure(IFrameworkStoredProcedure proc, IDbTransaction tx)
        {
            object result;
            LaunchStoredProcedure(proc, tx, out result);
        }

        public void LaunchStoredProcedure(IFrameworkStoredProcedure proc, IDbTransaction tx, out object result)
        {
            System.Collections.IEnumerable items = provider.LaunchStoredProcedure(proc, tx, out result);
            if (items != null)
            {
                proc.TableResult = FillCollection(new DataTypes.GenericQueryResult(), new DataTypes.GenericQueryResultCollection(), items);
            }

        }
        #endregion

        #region RECUPERO INFORMAZIONI
        public string GetSQLCommand(SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina)
        {
            return provider.GetSQLCommand(prototype, filtro, ordinamento, elementiPerPagina, pagina);
        }

        public string GetSQLCommand(SupportCreaElemento prototype, IFilter filtro, string ordinamento)
        {
            return provider.GetSQLCommand(prototype, filtro, ordinamento);
        }

        public ConnectionState GetConnectionState()
        {
            return provider.GetConnectionState();
        }
        #endregion

        #region CREAZIONE ELEMENTI

        /// <summary>
        /// Inserimento di un nuovo elemento nello storage
        /// </summary>
        /// <param name="item">elemento da inserire</param>
        /// <param name="utente">utente che esegue l'operazione</param>
        public void InsertItem(IFrameworkObject item, IDbTransaction tx)
        {
            long generatedID = 0;
            this.InsertItem(item, ref generatedID, tx);
        }

        /// <summary>
        /// Inserimento di un nuovo elemento nello storage
        /// </summary>
        /// <param name="item">elemento da inserire</param>
        /// <param name="utente">utente che esegue l'operazione</param>
        public void InsertItem(IFrameworkObject item, ref long generatedID, IDbTransaction tx)
        {
            if (item == null) throw new ArgumentNullException("item");

            // DataItem implementa l'interfaccia IDataRecord usata dal IDataProvider
            DataItem d = new DataItem(item);

            Exception ex = this.provider.InsertItem(d, item.GetType(), tx, ref generatedID);

            if (ex != null)
                throw ex;
        }

        #endregion

        #region AGGIORNAMENTO ELEMENTI

        /// <summary>
        /// Aggiornamento di un elemento nello storage
        /// </summary>
        /// <param name="item">elemento da aggiornare</param>
        /// <param name="utente">utente che esegue l'operazione</param>
        public long UpdateItem(IFrameworkObject item, IDbTransaction tx, IFilter filter = null)
        {
            if (item == null) throw new ArgumentNullException("item");

            // DataItem implementa l'interfaccia IDataRecord usata dal IDataProvider
            DataItem d = new DataItem(item);

            long count;
            Exception ex = this.provider.UpdateItem(d, item.GetType(), tx, out count, filter);

            if (ex != null)
                throw ex;

            return count;
        }

        #endregion

        #region CANCELLAZIONE ELEMENTI

        /// <summary>
        /// Cancellazione di un elemento dallo storage
        /// </summary>
        /// <param name="item">elemento da cancellare</param>
        /// <param name="utente">utente che esegue l'operazione</param>
        public long DeleteItem(IFrameworkObject item, IDbTransaction tx)
        {
            if (item == null) throw new ArgumentNullException("item");

            // DataItem implementa l'interfaccia IDataRecord usata dal IDataProvider
            DataItem d = new DataItem(item);

            long count;
            Exception ex = this.provider.DeleteItem(d, item.GetType(), tx, out count);

            if (ex != null)
                throw ex;

            return count;
        }

        /// <summary>
        /// Cancellazione di elementi dallo storage in base a filtro
        /// </summary>
        /// <param name="item">istanza elemento</param>
        /// <param name="filter">filtro da applicare</param>
        /// <param name="utente">utente che esegue l'operazione</param>
        public long DeleteItem(IFrameworkObject item, IFilter filter, IDbTransaction tx)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (filter == null || filter.ToString() == "") throw new ArgumentNullException("filter");

            long count;
            Exception ex = this.provider.DeleteItem(item.GetType(), filter, tx, out count);

            if (ex != null)
                throw ex;

            return count;
        }

        #endregion

        #region FORMATTAZIONE DATI

        public string DateFilter(string field, object data, bool useTime)
        {
            string value = string.Empty;

            if (useTime)
            {
                value = provider.DateTimeFilter(((DateTime)data).ToString("d",
                    new System.Globalization.CultureInfo("it-IT")));
            }
            else
            {
                value = provider.DateFilter(((DateTime)data).ToString("d",
                    new System.Globalization.CultureInfo("it-IT")));
            }

            return value;
        }

        public string DateFilterTruncateField(string data)
        {
            return this.provider.DateFilterTruncateField(data);
        }

        public string parameterPlaceHolder
        {
            get
            {
                return this.provider.parameterPlaceHolder;
            }
        }


        public string JollyCharacter
        {
            get
            {
                return this.provider.JollyCharacter;
            }
        }
        #endregion

        #region GESTIONE TRANSAZIONI
        public IDbTransaction OpenTransaction()
        {
            return this.provider.OpenTransaction();
        }

        public void CloseTransaction(IDbTransaction tx, bool commit)
        {
            this.provider.CloseTransaction(tx, commit);
        }
        #endregion

        #region GESTIONE BLOB /CLOB

        public byte[] ReadBlob(IFrameworkObjectWithBlob item, string blobField, IDbTransaction tx)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (string.IsNullOrEmpty(blobField)) throw new ArgumentNullException("blobField");

            byte[] result = null;
            try
            {
                result = this.provider.ReadBlob(item, blobField, tx);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(string.Format("Error ReadBlob: item={0}, blobfield={1}\r\n{2}", ((IFrameworkObject)item).TableName(), blobField, ex.Message));
                throw;
            }
            finally
            {
                if (tx == null)
                    provider.Close();
            }

            return result;
        }

        public void WriteBlob(IFrameworkObjectWithBlob item, string blobField, byte[] blobData, IDbTransaction tx)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (string.IsNullOrEmpty(blobField)) throw new ArgumentNullException("blobField");

            Exception ex = this.provider.WriteBlob(item, blobField, blobData, tx);

            if (ex != null)
            {
                System.Diagnostics.Debug.Write(string.Format("Error WriteBlob: item={0}, blobfield={1}, len={2}\r\n{3}", ((IFrameworkObject)item).TableName(), blobField, blobData.Length.ToString(), ex.Message));
                throw ex;
            }

            return;
        }

        public void WriteClob(IFrameworkObjectWithBlob item, string clobField, string clobData, IDbTransaction tx)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (string.IsNullOrEmpty(clobField)) throw new ArgumentNullException("clobField");

            Exception ex = this.provider.WriteClob(item, clobField, clobData, tx);

            if (ex != null)
            {
                System.Diagnostics.Debug.Write(string.Format("Error WriteClob: item={0}, clobfield={1}, len={2}\r\n{3}", ((IFrameworkObject)item).TableName(), clobField, clobData, ex.Message));
                throw ex;
            }

            return;
        }


        #endregion

        public void ExecuteCommand(string comandoSQL, IDbTransaction tx)
        {
            if (string.IsNullOrEmpty(comandoSQL)) throw new ArgumentNullException("comandoSQL");

            this.provider.ExecuteCommand(comandoSQL, tx);
        }

    }
}
