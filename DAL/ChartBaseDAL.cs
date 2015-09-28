using System;
using System.Collections.Generic;
using MyChartExample.Models;
using System.Data;
using System.Data.SqlClient;



namespace MyChartExample.DAL
{
    public class ChartBaseDAL
    {

        private SqlConnection sqlConnection;

        public string DBConnxnString { get; set; }

        // =========================================================================================

        public List<ChartDataSetRow> getChartData(string pKPI)
        {

            ChartDataSetRow thisChartDataSetRow; //  = new ChartDataSetRow();
            List<ChartDataSetRow> ChartDataSetRowList = new List<ChartDataSetRow>();
            Double dWork;
            Boolean ValueIsaNumber;
            String seriesName = "";
            String elementName = "";
            Double elementValue = 0.0;

            try
            {
                using (sqlConnection = new SqlConnection(DBConnxnString))
                {
                    var cmd = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        Connection = sqlConnection,
                        CommandText = "p_TTL_GetKPIData"
                    };
                    cmd.CommandTimeout = 300;
                    cmd.Parameters.Add(new SqlParameter("@KPI", SqlDbType.VarChar, 120) { Value = pKPI });

                    sqlConnection.Open();

                    var dr = cmd.ExecuteReader();
                    
                    // Place the data returned into a dataset object.
                    while (dr.Read())
                    {
                        thisChartDataSetRow = new ChartDataSetRow();
                        
                        // -------------------------------------------------------------------------
                        // Place the dataset data into local variables before putting into 
                        // chart data row object.
                        seriesName = dr["SeriesName"].ToString();
                        elementName = dr["ElemName"].ToString();

                        ValueIsaNumber = Double.TryParse(dr["ElemValue"].ToString(), out dWork);
                        if (ValueIsaNumber)
                            elementValue = dWork;
                        else
                        {
                            //MessageBox.Show("Could not convert the Value for Series Element [" +
                            //                elementName + "] to a valid double!  Zero substituted.");
                            elementValue = 0.0;
                        }
                        
                        // -------------------------------------------------------------------------
                        // Place data from local variables into chart data row object.
                        thisChartDataSetRow.SeriesName = seriesName;
                        thisChartDataSetRow.ElemName = elementName;
                        thisChartDataSetRow.ElemValue = elementValue;

                        // ----------------------------
                        ChartDataSetRowList.Add(thisChartDataSetRow);

                    } //    End while

                } //    End using
            }
            catch (Exception e)
            {
                int passwordPosition = 0;
                int length = 0;
                string DBConnxnInfo = "";
                string errMsg = "";

                passwordPosition = DBConnxnString.IndexOf("Password");
                length = passwordPosition;
                DBConnxnInfo = DBConnxnString.Substring(0, length);

                //MessageBox.Show("Encountered error when retrieving Chart data for KPI [" +
                //                pKPI + "] " +
                //                "-- SQL Server connection string [" + DBConnxnInfo + "] " +
                //                "-- Error: " + e.Message.ToString());

                ChartDataSetRowList = null;
                errMsg = "Encountered error when retrieving Chart data for KPI [" +
                         pKPI + "] " +
                         "-- SQL Server connection string [" + DBConnxnInfo + "] " +
                         "-- Error: " + e.Message.ToString();
                throw new Exception(errMsg);
 
            } //    End try/catch

            return ChartDataSetRowList;

        }  //   End getChartData

    }
}
