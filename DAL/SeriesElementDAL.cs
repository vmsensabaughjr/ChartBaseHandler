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
    class SeriesElementDAL
    {

        public List<SeriesElement> GetListOfSeriesElements(string pKPI, string pSeries, string pConnxnString)
        {
            List<SeriesElement> SeriesElementList = new List<SeriesElement>();

            SeriesElement thisSeriesElement = new SeriesElement();
            SqlConnection sqlConnection;
            Int32 iWork;
            Double dWork;
            Boolean ValueIsaNumber;

            try
            {
                // ==========================================
                // Just some sample settings to put some real data in here for one series.
 
                thisSeriesElement.Name = "DX";
                thisSeriesElement.Value = 45.79;
                thisSeriesElement.Color = "Red";
                thisSeriesElement.altName = "DeXe";
                // Add the Series Element to the list of elements to be returned by this method.
                SeriesElementList.Add(thisSeriesElement);

                using(sqlConnection = new SqlConnection())
                {
                    var cmd = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        Connection = sqlConnection,
                        CommandText = "p_VMS_TTL_Get_ElementsForSeries"
                    };
                    cmd.CommandTimeout = 300;
                    cmd.Parameters.Add(new SqlParameter("@KPI", SqlDbType.VarChar) { Value = pKPI });
                    cmd.Parameters.Add(new SqlParameter("@Series", SqlDbType.VarChar) { Value = pSeries });

                    sqlConnection.Open();

                    var dRdr = cmd.ExecuteReader();

                    // There should be more than one row of data elements returned here.
                    while (dRdr.Read())
                    {
                        thisSeriesElement.Name = dRdr["Series"].ToString();
                        // -------------------------------------------------------------------------
                        // Assuring that the series element Value is numeric.
                        ValueIsaNumber = Double.TryParse(dRdr["ElementValue"].ToString(), out dWork);
                        if (ValueIsaNumber)
                            thisSeriesElement.Value = dWork;
                        else
                        {
                            MessageBox.Show("Could not convert the Value for Series Element [" +
                                            thisSeriesElement.Name + "] to a valid integer!  Zero substituted.");
                            thisSeriesElement.Value = 0;
                        }
                        
                        // -------------------------------------------------------------------------

                        // Add the Series Element to the list of elements to be returned by this method.
                        SeriesElementList.Add(thisSeriesElement);

                    } //    End while

                } //    End using


            }
            catch (Exception e)
            {
                MessageBox.Show("Encountered error when retrieving Series Element data for KPI [" +
                                pKPI + "] -- Error: " + e.Message.ToString());
                SeriesElementList = null;
                Application.Exit();
            } //    End Try/Catch
            
            // -------------------------------------------------------------------------------------            
            return SeriesElementList;
        }


        
    }
}
