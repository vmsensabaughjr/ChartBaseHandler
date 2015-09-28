using System;
using System.Collections.Generic;

namespace MyChartExample.Models
{
    public class ChartBase
    {

        public string KPIName { get; set; }

        public int ChartWidth { get; set; }
        
        public int ChartHeight { get; set; }

        public Boolean Area3DStyle { get; set; }

        public int Area3DStyleRotation { get; set; }

        public int Area3DStyleInclination { get; set; }

        public string ChartLegendTitle { get; set; }

        //public ChartTitle Title { get; set; }

        public List<ChartSeries> ChartSeriesList { get; set; }

        // =========================================================================================

        private Int32 listController = 0;

        private int counter = 0;

        public ChartSeries getNextChartSeries()
        {
            ChartSeries returnChartSeries = null;

            if (counter < ChartSeriesList.Count) {
                counter++;
                //Get the next Series
                returnChartSeries = ChartSeriesList[counter];
            }else{
                //return null
            }

            return returnChartSeries;
        }

    }
}
