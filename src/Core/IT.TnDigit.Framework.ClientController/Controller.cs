using IT.TnDigit.ORM.DataStorage;
using IT.TnDigit.ORM.Interfaces;
using System;
using System.Collections.Generic;


namespace IT.TnDigit.ORM.ClientController
{
    public class Controller : IDisposable, IController, IControllerSQLText
    {
        private readonly Storage storage;
        private System.Data.IDbTransaction m_tx = null;

        public bool TransactionIsClosed
        {
            get
            {
                return (m_tx == null);
            }
        }

        public Controller(string dataSource, bool useTransaction)
        {
            if (dataSource == null)
                dataSource = string.Empty;

            storage = Storage.ParticularStorage(dataSource);

            if (useTransaction == true)
            {
                this.OpenTransaction();
            }
            else
            {
                m_tx = null;
            }
        }

        public Controller(IDataProvider dataProvider, bool useTransaction)
        {
            if (dataProvider == null)
                storage = Storage.ParticularStorage(string.Empty);
            else
                storage = Storage.ParticularStorage(dataProvider);

            if (useTransaction == true)
            {
                this.OpenTransaction();
            }
            else
            {
                m_tx = null;
            }
        }

        public Controller() : this(null) { }

        public Controller(IDataProvider dataProvider)
        {
            if (dataProvider == null)
                storage = Storage.ParticularStorage(string.Empty);
            else
                storage = Storage.ParticularStorage(dataProvider);

            m_tx = null;
        }


        #region IDisposable Membri di

        public void Dispose()
        {
            if (m_tx != null && m_tx.Connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    this.CloseTransaction(true);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ERRORE IN DISPOSE STORAGE: " + ex.Message);
                    System.Diagnostics.Trace.WriteLine("ERRORE IN DISPOSE STORAGE: " + ex.Message);
                }
            }
            storage.Dispose();
        }

        #endregion

        #region GESTIONE TRANSAZIONI
        public void OpenTransaction()
        {
            if (TransactionIsClosed)
                m_tx = storage.OpenTransaction();
        }

        public void CloseTransaction(bool commit)
        {
            if (TransactionIsClosed == true)
            {
                System.Diagnostics.Debug.WriteLine("Impossibile chiudere la transazione, gi� chiusa.");
                return;
            }
            try
            {
                storage.CloseTransaction(m_tx, commit);
                //se tutto va bene chiudo la transazione
                m_tx = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region LEGGI ELEMENTI

        public T LeggiElementiPaginati<T>(T collection, IFilter filter, string order, int pagina, int elementiPagina, ref int risultati) where T : IFrameworkCollection
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            if (pagina < 0 || elementiPagina <= 0)
            {
                return LeggiElementi<T>(collection, filter, order, ref risultati);
            }
            else
            {
                return (T)storage.GetItems(collection.Prototype(), collection, filter, order, elementiPagina, pagina, ref risultati, m_tx);
            }
        }

        public T LeggiElementi<T>(T collection, IFilter filter, string order, ref int risultati) where T : IFrameworkCollection
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            return (T)storage.GetItems(collection.Prototype(), collection, filter, order, ref risultati, m_tx);
        }

        public T LeggiElementi<T>(T collection, IFilter filter, string order) where T : IFrameworkCollection
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            int risultati = 0;
            return this.LeggiElementi<T>(collection, filter, order, ref risultati);
        }

        public int ConteggioElementi(IFrameworkCollection collection, IFilter filter)
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            return storage.CountItems(collection.Prototype(), filter, m_tx);
        }

        public int ConteggioElementi(string comandoSQL)
        {
            return storage.CountItemsFromSQL(comandoSQL, m_tx);
        }

        public T GetElemento<T>(IFrameworkCollection collection, IFilter filter)
            where T : IFrameworkObject
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            collection = LeggiElementi<IFrameworkCollection>(collection, filter, string.Empty);
            if (collection.Count == 0)
                return default(T);

            return (T)collection[0];
        }

        /// <summary>
        /// Compila le liste degli oggetti relazionati all'elemento corrente
        /// </summary>
        /// <param name="item"></param>
        public void FillRelatedData(IFrameworkObject item)
        {
            foreach (var relation in item.DataRelationships)
            {
                if (relation is RelatedRecord)
                {
                    var rel = relation as RelatedRecord;
                    var data = this.LeggiElementi(rel.collTemplate, rel.filter, "");
                    if (data.Count == 0)
                        rel.data = data.Prototype() as IFrameworkObject;
                    else
                        rel.data = (IFrameworkObject)data[0];

                }

                if (relation is RelatedCollection)
                {
                    var rel = relation as RelatedCollection;
                    var data = this.LeggiElementi(rel.collTemplate, rel.filter, rel.order);
                    rel.data = data;
                }
            }
        }


        #endregion

        #region LEGGI DA COMANDO SQL
        public T LeggiElementiSQLPaginati<T>(T collection, string comandoSQL, int pagina, int elementiPagina, ref int risultati) where T : IFrameworkCollection
        {
            return (T)storage.GetItems(collection.Prototype(), collection, comandoSQL, elementiPagina, pagina, ref risultati, m_tx);
        }

        public T LeggiElementiSQL<T>(T collection, string comandoSQL, ref int risultati) where T : IFrameworkCollection
        {
            return (T)storage.GetItemsFromSQL(collection.Prototype(), collection, comandoSQL, ref risultati, m_tx);
        }

        public T LeggiElementiSQL<T>(T collection, string comandoSQL) where T : IFrameworkCollection
        {
            int risultati = 0;
            return this.LeggiElementiSQL(collection, comandoSQL, ref risultati);
        }

        public T LeggiElementiCommand<T>(T collection, System.Data.IDbCommand cmd) where T : IFrameworkCollection
        {
            int risultati = 0;
            return (T)storage.GetItemsFromCommand(collection.Prototype(), collection, cmd, ref risultati, m_tx);
        }

        #endregion

        #region INFORMAZIONI
        public string GetSQLCommand(IFrameworkCollection collection, IFilter filter, string order, int pagina, int elementiPagina)
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            return storage.GetSQLCommand(collection.Prototype(), filter, order, elementiPagina, pagina);
        }

        public string GetSQLCommand(IFrameworkCollection collection, IFilter filter, string order)
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            return storage.GetSQLCommand(collection.Prototype(), filter, order);
        }

        public System.Data.ConnectionState GetConnectionStateNoTx()
        {
            return storage.GetConnectionState();
        }

        public System.Data.ConnectionState GetConnectionState()
        {
            return storage.GetConnectionState();
        }
        #endregion

        #region GESTIONE OPERAZIONI STANDARD

        public virtual long InserisciEntita(IFrameworkObject st)
        {
            long generatedID = 0;

            if (m_tx == null)
            {
                System.Data.IDbTransaction tx = storage.OpenTransaction();
                try
                {
                    storage.InsertItem(st, ref generatedID, tx);
                }
                catch (Exception ex)
                {
                    storage.CloseTransaction(tx, false);
                    throw ex;
                }

                storage.CloseTransaction(tx, true);
            }
            else
            {
                storage.InsertItem(st, ref generatedID, m_tx);
            }


            return generatedID;
        }

        //private IFrameworkObject GetEntityByPk(IFrameworkObject st)
        //{
        //    DataItem item = new DataItem(st);
        //    int fieldCount = item.FieldCount;
        //    System.Reflection.PropertyInfo[] props = st.GetType().GetProperties();

        //    SQLFilter sf = new SQLFilter();

        //    for (int idx = 0; idx < fieldCount; idx++)
        //    {
        //        if (this.IsPrimaryKey(props[idx]) == true)
        //        {
        //            sf.Add(props[idx].Name, OperatoriFiltro.Uguale, item.GetValue(idx));
        //        }
        //    }

        //    //trovare il modo di leggere l'oggetto da db, settare i valori e fare update
        //    //return this.l
        //}

        protected bool IsPrimaryKey(System.Reflection.PropertyInfo prop)
        {
            Object[] attributes = prop.GetCustomAttributes(true);
            foreach (Object att in attributes)
            {
                if ((Attribute)att is PrimaryKey)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual long ModificaEntita(IFrameworkObject st, IFilter filter = null)
        {
            long updatedRecords = 0;
            if (m_tx == null)
            {
                System.Data.IDbTransaction tx = storage.OpenTransaction();

                try
                {
                    var clobs = new List<KeyValuePair<string, string>>();
                    if (st is IFrameworkObjectWithBlob)
                    {
                        //TODO: clear clob values
                        var props = st.GetType().GetProperties();
                        foreach (var prop in props)
                        {
                            if (prop.CanWrite)
                            {
                                Object[] attributes = prop.GetCustomAttributes(true);
                                foreach (Object att in attributes)
                                {
                                    if ((Attribute)att is ClobField)
                                    {
                                        st.PropertiesChanged.Remove(prop.Name.ToLower());
                                        object val = prop.GetValue(st, null);

                                        if (val != null)
                                        {
                                            clobs.Add(new KeyValuePair<string, string>(prop.Name, string.Copy(val.ToString())));
                                            prop.SetValue(st, null, null);
                                        }
                                    }
                                }

                            }
                        }
                    }

                    updatedRecords = storage.UpdateItem(st, tx, filter);

                    if (st is IFrameworkObjectWithBlob)
                    {
                        //TODO: save clobs
                        foreach (var item in clobs)
                        {
                            this.WriteClob((IFrameworkObjectWithBlob)st, item.Key, item.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    storage.CloseTransaction(tx, false);
                    throw ex;
                }

                storage.CloseTransaction(tx, true);
            }
            else
            {
                updatedRecords = storage.UpdateItem(st, m_tx);
            }

            return updatedRecords;
        }

        public virtual bool CancellaEntita(IFrameworkObject st)
        {
            long deletedRows = 0;
            if (m_tx == null)
            {
                System.Data.IDbTransaction tx = storage.OpenTransaction();

                try
                {
                    deletedRows = storage.DeleteItem(st, tx);
                }
                catch (Exception ex)
                {
                    storage.CloseTransaction(tx, false);
                    throw ex;
                }

                storage.CloseTransaction(tx, true);
            }
            else
            {
                deletedRows = storage.DeleteItem(st, m_tx);
            }
            return deletedRows > 0;
        }

        public virtual long CancellaEntita(IFrameworkObject st, IFilter filter)
        {
            long deletedRows = 0;
            if (filter == null) filter = new SQLFilter(storage.Name);
            if (m_tx == null)
            {
                System.Data.IDbTransaction tx = storage.OpenTransaction();

                try
                {
                    deletedRows = storage.DeleteItem(st, filter, tx);
                }
                catch (Exception ex)
                {
                    storage.CloseTransaction(tx, false);
                    throw ex;
                }

                storage.CloseTransaction(tx, true);
            }
            else
            {
                deletedRows = storage.DeleteItem(st, filter, m_tx);
            }
            return deletedRows;
        }

        public object LaunchStoredProcedure(IFrameworkStoredProcedure proc)
        {
            object result = null;
            try
            {
                if (m_tx == null)
                {
                    System.Data.IDbTransaction tx = storage.OpenTransaction();

                    try
                    {
                        storage.LaunchStoredProcedure(proc, tx, out result);
                    }
                    catch (Exception ex)
                    {
                        storage.CloseTransaction(tx, false);
                        throw ex;
                    }

                    storage.CloseTransaction(tx, true);
                }
                else
                {
                    storage.LaunchStoredProcedure(proc, m_tx, out result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion

        #region GESTIONE BLOB / CLOB

        public byte[] ReadBlob(IFrameworkObjectWithBlob item, string blobField)
        {
            return storage.ReadBlob(item, blobField, m_tx);
        }

        public void WriteBlob(IFrameworkObjectWithBlob item, string blobField, byte[] blobData)
        {
            if (m_tx == null)
            {
                System.Data.IDbTransaction tx = storage.OpenTransaction();

                try
                {
                    storage.WriteBlob(item, blobField, blobData, tx);
                }
                catch (Exception ex)
                {
                    storage.CloseTransaction(tx, false);
                    throw ex;
                }

                storage.CloseTransaction(tx, true);
            }
            else
            {
                storage.WriteBlob(item, blobField, blobData, m_tx);
            }
        }

        public void WriteClob(IFrameworkObjectWithBlob item, string clobField, string clobData)
        {
            if (m_tx == null)
            {
                System.Data.IDbTransaction tx = storage.OpenTransaction();

                try
                {
                    storage.WriteClob(item, clobField, clobData, tx);
                }
                catch (Exception ex)
                {
                    storage.CloseTransaction(tx, false);
                    throw ex;
                }

                storage.CloseTransaction(tx, true);
            }
            else
            {
                storage.WriteClob(item, clobField, clobData, m_tx);
            }
        }

        #endregion

        #region LEGGI ELEMENTI DA RICERCA

        public T LeggiElementi<T, K>(T collection, IFilter filter, string order, K searchCollection, string field, string searchField)
            where T : IFrameworkCollection
            where K : IFrameworkCollection
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            return (T)storage.GetItems(collection.Prototype(), collection, filter, order, searchCollection.Prototype(), field, searchField, m_tx);
        }


        public T LeggiElementiPaginati<T, K>(T collection, IFilter filter, string order, K searchCollection, string field, string searchField, int pagina, int elementiPagina, ref int risultati)
            where T : IFrameworkCollection
            where K : IFrameworkCollection
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            if (pagina < 0 || elementiPagina <= 0)
            {
                return LeggiElementi<T, K>(collection, filter, order, searchCollection, field, searchField);
            }
            else
            {
                return (T)storage.GetItems(collection.Prototype(), collection, filter, order, searchCollection.Prototype(), field, searchField, elementiPagina, pagina, ref risultati, m_tx);
            }
        }

        public string[] GetItemsField<T>(T collection, IFilter filter, string field) where T : IFrameworkCollection
        {
            if (filter == null) filter = new SQLFilter(storage.Name);
            return storage.GetItemsField(collection.Prototype(), filter, field, m_tx);
        }

        #endregion

        public void ExecuteCommand(string comandoSQL)
        {
            if (m_tx == null)
            {
                System.Data.IDbTransaction tx = storage.OpenTransaction();

                try
                {
                    storage.ExecuteCommand(comandoSQL, tx);
                }
                catch (Exception ex)
                {
                    storage.CloseTransaction(tx, false);
                    throw ex;
                }

                storage.CloseTransaction(tx, true);
            }
            else
            {
                storage.ExecuteCommand(comandoSQL, m_tx);
            }
        }
    }
}
