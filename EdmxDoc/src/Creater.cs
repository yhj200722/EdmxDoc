using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EdmxDoc
{
    /// <summary>modify edmx file,add table and column comment</summary>
    public abstract class Creater : IDisposable
    {

        /// <summary>input file（absolute path）</summary>
        protected string InputFileName { get; set; }

        /// <summary>output file（absolute path）</summary>
        protected string OutputFileName { get; set; }

        /// <summary>Xml NameSpace</summary>
        protected string XmlNameSpace { get; set; }


        /// <summary>database connection</summary>
        protected abstract IDbConnection DbConnection { get; }

        /// <summary>
        /// concrete creater
        /// </summary>
        public static Creater TargetCreater { get; set; }

        public void Dispose()
        {
            DbConnection.Dispose();
        }

        /// <summary>
        /// generate edmx documentation summary from database
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="inputFileName"></param>
        /// <param name="outputFileName"></param>
        /// <param name="xmlNameSpace"></param>
        public static void Execute(string connectionString, string inputFileName, string outputFileName = null, string xmlNameSpace = "http://schemas.microsoft.com/ado/2009/11/edm")
        {
            if (null == TargetCreater)
            {
                TargetCreater = new SqlServerCreater();
            }
            
            TargetCreater.DbConnection.ConnectionString = connectionString;
            TargetCreater.InputFileName = inputFileName;
            TargetCreater.OutputFileName = outputFileName ?? inputFileName;
            TargetCreater.XmlNameSpace = xmlNameSpace;

            using (TargetCreater)
            {
                TargetCreater.CreateDocumentation();
            }
        }

        /// <summary>create summary text</summary>
        protected virtual void CreateDocumentation()
        {
            DbConnection.Open();
            var doc = XDocument.Load(InputFileName);
            var entityTypeElements = doc.Descendants("{" + XmlNameSpace + "}EntityType");
            foreach (var entityTypeElement in entityTypeElements)
            {
                var tableName = entityTypeElement.Attribute("Name").Value;
                var propertyElements = entityTypeElement.Descendants("{" + XmlNameSpace + "}Property");
                AddNodeDocumentation(entityTypeElement, GetTableDocumentation(tableName));
                foreach (var propertyElement in propertyElements)
                {
                    var columnName = propertyElement.Attribute("Name").Value;
                    AddNodeDocumentation(propertyElement, GetColumnDocumentation(tableName, columnName));
                }
            }
            if (File.Exists(OutputFileName))
            {
                File.Delete(OutputFileName);
            }
            doc.Save(OutputFileName);
        }

        /// <summary>
        /// add documentation summary to specific xml node
        /// </summary>
        /// <param name="element">entityTypeElement</param>
        /// <param name="documentation">comment from database</param>
        protected virtual void AddNodeDocumentation(XElement element, string documentation)
        {
            if (string.IsNullOrEmpty(documentation))
            {
                return;
            }
            element.Descendants("{" + XmlNameSpace + "}Documentation").Remove();
            element.AddFirst(new XElement("{" + XmlNameSpace + "}Documentation", new XElement("{" + XmlNameSpace + "}Summary", documentation)));
        }

        /// <summary>get table comment</summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        protected abstract string GetTableDocumentation(string tableName);

        /// <summary>
        /// get table's column comment
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="columnName">columnName</param>
        /// <returns></returns>
        protected abstract string GetColumnDocumentation(string tableName, string columnName);
    }
}
