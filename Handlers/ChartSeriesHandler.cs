using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChartExample.Models;
using MyChartExample.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace MyChartExample.Handlers
{
    class ChartSeriesHandler
    {
        // This will handle 1 to many Series Objects needed for the chart being built.

        internal ChartSeries MakeASeriesObj(ChartDataSetRow pDataRow)
        {
            ChartSeries thisChartSeries = new ChartSeries();

            String seriesName = "";

            seriesName = pDataRow.SeriesName;

            thisChartSeries.Name = seriesName;

            // thisChartSeries.




            return thisChartSeries;
        }

        public List<ChartSeries> getChartData(string pKPI, string pDBConnxnString)
        {
            // Data will be place in a list of series objects.
            List<ChartSeries> thisChartSeriesList = new List<ChartSeries>();

            String sWork;
            String KPIStoredProcedure;

            try
            {
                // Create the Chart Series DAL
                ChartSeriesDAL chartSeriesDAL = new ChartSeriesDAL();
                chartSeriesDAL.DBConnxnString = pDBConnxnString;

                // Based on the KPI passed in the application will chose which stored procedure to use (and series to request)?
                // select [stored proc name] from KPI_StoredProc WHERE KPI = pKPI
                // sWork = chartSeriesDAL.getKPIStoredProcedureName(pKPI);
                KPIStoredProcedure = "";  //  sWork;
                // We will not be getting the stored proc name here.

                // Use the stored proc based on the KPI to get the chart data from the database.
                thisChartSeriesList = chartSeriesDAL.getChartSeriesList(pKPI, KPIStoredProcedure);
            }
            catch (Exception e)
            {
                MessageBox.Show("Encountered error when building the Chart Series List for KPI [" +
                                pKPI + "] -- Error: " + e.Message.ToString());
                thisChartSeriesList = null;
                Application.Exit();
            }
            return thisChartSeriesList;
        }

        
    }
}
