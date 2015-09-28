using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChartExample.Models;
using MyChartExample.DAL;


namespace MyChartExample.Handlers
{
    class SeriesElementHandler
    {
        public List<SeriesElement > BuildSeriesElementList(string pKPI, string pConnxnString)
        {
            List<SeriesElement> thisSeriesElementList = new List<SeriesElement>();

            // Instantiate a DAL for access to the database.
            SeriesElementDAL seriesElementDAL = new SeriesElementDAL();

            // Use the DAL to go get series element data for the pKPI chart being built.
            List<SeriesElement> seriesElementList = seriesElementDAL.GetListOfSeriesElements(pKPI, "", pConnxnString);

            return thisSeriesElementList;

        }
    }
}
