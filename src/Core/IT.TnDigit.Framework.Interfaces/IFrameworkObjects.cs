using System.Collections.Generic;

namespace IT.TnDigit.ORM.Interfaces
{
    public interface IFrameworkObject
    {
        string TableName();
        string FieldsList();

        string SequenceName();


        List<string> PropertiesChanged
        {
            get;
        }

        List<BaseRelation> DataRelationships
        {
            get;
        }

    }

    public interface IFrameworkView
    {
        string ViewSQL();
    }

    public interface IFrameworkObjectExtension
    {
        string AggiunteSelect();
    }
}
