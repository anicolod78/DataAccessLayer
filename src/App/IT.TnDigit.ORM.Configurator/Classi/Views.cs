using System;
using System.IO;

namespace IT.TnDigit.ORM.Configurator.Classi
{
    abstract class Views
    {
        static public Boolean WriteObject(object itm)
        {
            string content = GetModel();

            var p = DBStructure.ProviderFactory();
            content = p.WriteViewObject(
                itm,
                content,
                Params.Instance.Domain.BaseClassViews);

            return WriteFile(itm.ToString() + ".cs", content);
        }

        private static string GetModel()
        {
            StreamReader sr = new StreamReader(
                Path.Combine(System.Windows.Forms.Application.StartupPath, "Schemi\\Schema_Viste.txt"));
            string model = sr.ReadToEnd();
            sr.Close();
            //string model = global::IT.InfoTn.Framework.DataTypesCreator.Properties.Resources.Schema_Viste;
            model = model.Replace("/*NAMESPACE*/", Params.Instance.Domain.NameSpace); ;
            return model;
        }

        private static Boolean WriteFile(string filename, string content)
        {
            string path = Path.Combine(Params.Instance.Domain.FilePath, "Viste");
            //predispongo la directory
            if (System.IO.Directory.Exists(path) == false)
                System.IO.Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, filename);

            if (System.IO.File.Exists(filePath))
            {
                //check read-only
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                if (fileInfo.IsReadOnly == true)
                    return false;
                //fileInfo.IsReadOnly = false;
            }

            //scrivo la classe
            StreamWriter cls = new StreamWriter(filePath);
            cls.Write(content);
            cls.Flush();
            cls.Close();
            return true;
        }

    }
}