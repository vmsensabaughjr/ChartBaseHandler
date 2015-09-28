using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChartExample.Models;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace MyChartExample.DAL
{
    class ChartSeriesDAL
    {

        private SqlConnection sqlConnection; 

        public string DBConnxnString { get; set; }

        // =========================================================================================

        public String getKPIStoredProcedureName(String pKPI)
        {
            
            String thisStoredProcedureName;
            thisStoredProcedureName = "";



            try
            {

                using (sqlConnection = new SqlConnection(DBConnxnString))
                {
                    var cmd = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        Connection = sqlConnection,
                        CommandText = "p_VMS_TTL_GetKPIStoredProcedureName"
                    };
                    cmd.CommandTimeout = 300;
                    cmd.Parameters.Add(new SqlParameter("@KPI", SqlDbType.VarChar) { Value = pKPI });
                    sqlConnection.Open();


                    if (thisStoredProcedureName == "")
                        throw new Exception("No stored procedure name found for KPI [" + pKPI + "]!");

                } //    End using

            }
            catch (Exception e)
            {
                MessageBox.Show("Encountered error when retrieving stored procedure name for KPI [" +
                                pKPI + "] -- Error: " + e.Message.ToString());
                Application.Exit();
            }   // End Try/Catch

            return thisStoredProcedureName;
        }




        
        public List<ChartSeries> getChartSeriesList(string pKPI, string pStoredProcedure)
        {
            List<ChartSeries> thisChartSeriesList = new List<ChartSeries>();

            ChartSeries thisChartSeries = new ChartSeries();
            
            Double dWork;
            Boolean ValueIsaNumber;
            Boolean firstPass = true;

            pStoredProcedure = "p_TTL_GetKPIData";
            pKPI = "TotalHoursWorked";

            // Get series data from the database

            try
            {
                using (sqlConnection = new SqlConnection(DBConnxnString))
                {
                    var cmd = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        Connection = sqlConnection,
                        CommandText = pStoredProcedure
                    };
                    cmd.CommandTimeout = 300;
                    sqlConnection.Open();

                    var dRdr = cmd.ExecuteReader();
                    String seriesName = "";
                    String lastSeriesName = "";
                    String elemName = "";
                    Double elemValue = 0.0;

                    // -------------------------------------------------------------------------
                    // Make a series element list object for this chart series
                    List<SeriesElement> SeriesElementList = new List<SeriesElement>();


                    while (dRdr.Read())
                    {
                        // -------------------------------------------------------------------------
                        // Place the database data in local variables for processing.
                        seriesName = dRdr["SeriesName"].ToString();
                        elemName = dRdr["ElemName"].ToString();
                        ValueIsaNumber = Double.TryParse(dRdr["ElementValue"].ToString(), out dWork);
                        if (ValueIsaNumber)
                            elemValue = dWork;
                        else
                        {
                            MessageBox.Show("Could not convert the Value for Series Element [" +
                                            elemName + "] to a valid double!  Zero substituted.");
                            elemValue = 0.0;
                        }
                        // -------------------------------------------------------------------------
                        if (firstPass || seriesName == lastSeriesName) 
                        {
                            
                            if (firstPass) 
                                thisChartSeries.Name = seriesName;
                            

                            // Add the series ElemName and ElemValue to the current SeriesElementList
                            SeriesElement thisSeriesElement = new SeriesElement();
                            thisSeriesElement.Name = elemName;
                            thisSeriesElement.Value = elemValue;
                            //thisSeriesElement.Color = "{something}";
                            //thisSeriesElement.altName = "{something}";
                            SeriesElementList.Add(thisSeriesElement);
                            thisSeriesElement = null;
                            firstPass = false;
                            lastSeriesName = seriesName;
                        }
                        else
                        {
                            // Don't forget to add the current SeriesElementList to the current ChartSeries object.


                            // Make a new chart series object
                            thisChartSeries = new ChartSeries();
                            thisChartSeries.Name = seriesName;



                            // Make a new SeriesElementList

                            // Make a new Series Element object

                            // Add the series ElemName and ElemValue to the new SeriesElementList

                        }
                            // If a SeriesElementList exists, close it out and place it into thisChartSeries.SeriesElementList
                            // There will always be at lease one SeriesElementList created before this while loop is started.
                            //thisChartSeriesList.Add(thisChartSeries);
                            // thisChartSeries.SeriesElementList.Add(SeriesElementList);

                            


                            thisChartSeries.Name = seriesName;


                            //    else




                        thisChartSeries.Name = dRdr["SeriesName"].ToString();
                        // thisSeriesElement.Color = dRdr["Color"].ToString();
                        // thisSeriesElement.altName = dRdr["AltName"].ToString();


                        //SeriesElement thisSeriesElement = new SeriesElement();

                        //thisSeriesElement.Name = dRdr["ElemName"].ToString();
                        //ValueIsaNumber = Double.TryParse(dRdr["ElementValue"].ToString(), out dWork);
                        //if (ValueIsaNumber)
                        //    thisSeriesElement.Value = dWork;
                        //else
                        //{
                        //    MessageBox.Show("Could not convert the Value for Series Element [" +
                        //                    thisSeriesElement.Name + "] to a valid double!  Zero substituted.");
                        //    thisSeriesElement.Value = 0.0;
                        //}
                        //thisChartSeries.SeriesElementList.Add(thisSeriesElement);



                        // Add the Series Element to the list of elements to be returned by this method.
                        thisChartSeriesList.Add(thisChartSeries);

                    } //    End while

                } //    End using

            }
            catch (Exception e)
            {
                MessageBox.Show("Encountered error when retrieving Chart Series data for KPI [" +
                                pKPI + "] -- Error: " + e.Message.ToString());
                thisChartSeriesList = null;
                Application.Exit();
            }   // End Try/Catch
            // -------------------------------------------------------------------------------------
            return thisChartSeriesList;
        }

    }
}
