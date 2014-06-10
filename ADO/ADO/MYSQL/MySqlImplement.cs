using ADO.Interfaces;
using ADO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.MYSQL
{
    public class MySqlImplement : IADO
    {
        public IDbConnection CurrentConnection { get; private set; }

        public MySqlImplement()
        {
            CurrentConnection = new MySql.Data.MySqlClient.MySqlConnection();
        }

        public MySqlImplement(string ConnectionName)
        {
            CurrentConnection = new MySql.Data.MySqlClient.MySqlConnection
                (System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);
        }

        public void SetConnection(string ConnectionString)
        {
            CurrentConnection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
        }

        public ADOModelResponse ExecuteQuery(ADOModelRequest QueryParams)
        {
            ADOModelResponse oRetorno = new ADOModelResponse();

            //create mysql command
            MySql.Data.MySqlClient.MySqlCommand CurrentCommand = new MySql.Data.MySqlClient.MySqlCommand();

            CurrentCommand.Connection = (MySql.Data.MySqlClient.MySqlConnection)CurrentConnection;
            CurrentCommand.CommandText = QueryParams.CommandText;

            QueryParams.Parameters.All(param =>
            {
                CurrentCommand.Parameters.Add((MySql.Data.MySqlClient.MySqlParameter)param);
                return true;
            });

            CurrentCommand.CommandType = QueryParams.CommandType;

            //validate method to execute
            switch (QueryParams.CommandExecutionType)
            {
                case enumCommandExecutionType.NonQuery:
                    oRetorno.NonQueryResult = CurrentCommand.ExecuteNonQuery();
                    break;
                case enumCommandExecutionType.Scalar:
                    oRetorno.ScalarResult = CurrentCommand.ExecuteScalar();
                    break;
                case enumCommandExecutionType.DataTable:

                    oRetorno.DataTableResult = new DataTable();
                    MySql.Data.MySqlClient.MySqlDataAdapter dat = new MySql.Data.MySqlClient.MySqlDataAdapter(CurrentCommand);
                    oRetorno.NonQueryResult = dat.Fill(oRetorno.DataTableResult);

                    break;
                case enumCommandExecutionType.DataSet:

                    oRetorno.DataSetResult = new DataSet();
                    MySql.Data.MySqlClient.MySqlDataAdapter dads = new MySql.Data.MySqlClient.MySqlDataAdapter(CurrentCommand);
                    oRetorno.NonQueryResult = dads.Fill(oRetorno.DataTableResult);

                    break;
                default:
                    break;
            }

            return oRetorno;
        }

        public IDbDataParameter CreateTypedParameter()
        {
            return new MySql.Data.MySqlClient.MySqlParameter();
        }
    }
}
