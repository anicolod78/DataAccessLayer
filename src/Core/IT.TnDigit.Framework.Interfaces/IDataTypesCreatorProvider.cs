using System;
using System.Collections.Generic;
using System.Data;

namespace IT.TnDigit.ORM.Interfaces
{
    public interface IDataTypesCreatorProvider
    {
        Exception CheckConnection();

        DataTable GetTables(String owner);

        DataTable GetViews(String owner);

        DataTable GetProcedures(String owner);

        string WriteTableObject(Object itm, String model, String baseClass, String owner);

        string WriteViewObject(Object itm, String model, String baseClass);

        string WriteProcedureObject(Object itm, String model, String baseClass, String paramsDefaultValue, String owner);

        Dictionary<string, object> DatabaseParams();
    }
}
