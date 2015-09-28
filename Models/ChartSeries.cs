using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;


namespace MyChartExample.Models
{
    public class ChartSeries
    {
        public string Name { get; set; }

        public SeriesChartType ChartType { get; set; }

        public string ToolTipFormat { get; set; }

        public List<SeriesElement> SeriesElementList { get; set; }

        public Double SeriesValue { get; set; }

        public ChartSeries()
        {
            List<SeriesElement> SeriesElementList = new List<SeriesElement>();
        }


        public String[] NameArray
        {
            get
            {
                Int32 iMax = SeriesElementList.Count;

                String[] thisNameArray = new String[iMax];

                Int32 indx = 0;

                foreach(SeriesElement thisSeriesElement in  SeriesElementList  )
                {
                    thisNameArray[indx] = thisSeriesElement.Name;
                    indx = indx + 1;
                }
                return thisNameArray;
            }
        }


        public Double[] ValueArray
        {
            get
            {
                Int32 iMax = SeriesElementList.Count;

                Double[] thisValueArray = new Double[iMax];

                Int32 indx = 0;

                foreach (SeriesElement thisSeriesElement in SeriesElementList)
                {
                    thisValueArray[indx] = thisSeriesElement.Value;
                    indx = indx + 1;
                }
                return thisValueArray;
            }
        }

    }

}
