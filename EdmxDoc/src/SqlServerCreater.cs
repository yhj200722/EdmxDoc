using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EdmxDoc
{
    /// <summary>
    /// sqlserver default implementation
    /// </summary>
    public class SqlServerCreater : Creater
    {
        private readonly IDbConnection dbConnection = new SqlConnection();

        protected override IDbConnection DbConnection => dbConnection;

        protected override string GetTableDocumentation(string tableName)
        {
            using (IDbCommand dbCommand = DbConnection.CreateCommand())
            {
                dbCommand.CommandText = @"SELECT [value] 
                                                          FROM fn_listextendedproperty (
                                                                'MS_Description', 
                                                                'schema', 'dbo', 
                                                                'table',  @tableName, 
                                                                null, null)";
                dbCommand.Parameters.Add(new SqlParameter("tableName", tableName));
                return dbCommand.ExecuteScalar() as string;
            }
           
        }

        protected override string GetColumnDocumentation(string tableName, string columnName)
        {
            using (IDbCommand dbCommand = DbConnection.CreateCommand())
            {
                dbCommand.CommandText = @"SELECT [value] 
                                                         FROM fn_listextendedproperty (
                                                                'MS_Description', 
                                                                'schema', 'dbo', 
                                                                'table', @tableName, 
                                                                'column', @columnName)";
                dbCommand.Parameters.Add(new SqlParameter("tableName", tableName));
                dbCommand.Parameters.Add(new SqlParameter("columnName", columnName));
                return dbCommand.ExecuteScalar() as string;
            }
        }
    }
}
