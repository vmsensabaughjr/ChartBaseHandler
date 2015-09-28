using System;
using System.Collections.Generic;
using MyChartExample.Models;
using MyChartExample.DAL;

namespace MyChartExample.Handlers
{
    public class ChartBaseHandler
    {
        private ChartBase mMyChartBase;

        public ChartBase MyChartBase {
            get
            {
                return mMyChartBase;
            }
        
        }
       
        public void BuildChartDataForKPI(string pKPI, string pDBConnxnString)
        {

            // Create a DAL for retrieving data from the database.
            ChartBaseDAL cbaseDAL = new ChartBaseDAL();
            cbaseDAL.DBConnxnString = pDBConnxnString;

            // Use the DAL to go get data for the pKPI being built and place the data in a ChartBase object 
            // which is on the ArgoUML.
            // ChartBase cbase = cbaseDAL.getNewChartData(pKPI, pDBConnxnString );

            List<ChartDataSetRow> chartDataSetRowList = cbaseDAL.getChartData(pKPI);

            // Take the data and place it into the Chart Model.
            ChartBase cbase = new ChartBase();

            if (chartDataSetRowList.Count < 1)
                cbase = null;
            else
            {
                // continue
                // Build the process to iterate over the data and make series objects with series element sub objects.

                List<ChartSeries> ChartSeriesList = new List<ChartSeries>();

                ChartSeriesList = BuildChartSeriesList(chartDataSetRowList);

                cbase.ChartSeriesList = ChartSeriesList;

                cbase.KPIName = pKPI;
                

            }

            // Return the ChartBase object that was created.
            mMyChartBase =  cbase;
        } //    End BuildChartDataForKPI


        private List<ChartSeries> BuildChartSeriesList(List<ChartDataSetRow> pChartDataSetRowList)
        {
            List<ChartSeries> ChartSeriesList = new List<ChartSeries>();

            ChartSeries thisChartSeries = new ChartSeries();
            thisChartSeries.SeriesElementList = new List<SeriesElement>();

            List<SeriesElement> thisSeriesElementList = new List<SeriesElement>();

            String seriesName = "";
            String elemName = "";
            Double elemValue = 0.0;
            String lastSeriesName = "";

            foreach(ChartDataSetRow cRow in pChartDataSetRowList)
            {
                // Place the database data in local variables for processing.
                seriesName = cRow.SeriesName;
                elemName = cRow.ElemName;
                elemValue = cRow.ElemValue;

                if (lastSeriesName == "")
                {
                    // We haven't started a series object yet.

                    thisChartSeries.Name = seriesName;
                    lastSeriesName = seriesName;
                }

                if (lastSeriesName == seriesName)
                {
                    // We are still in the same Chart Series.  No need to make a new object.
                }
                else
                {
                    lastSeriesName = seriesName;

                    // We have a new series and need a new Chart Series object.

                    // Take the last series and add it to the List of Chart Series

                    ChartSeriesList.Add(thisChartSeries);
                    int a = ChartSeriesList.Count;

                    // Set the last Chart Series object to null to destroy it.

                    thisChartSeries = null;

                    // Create a new Chart Series object.
                    thisChartSeries = new ChartSeries();
                    thisChartSeries.SeriesElementList = new List<SeriesElement>();
                    thisChartSeries.Name = seriesName;
                    
                }

                // Add any elements to the existing series.
                SeriesElement sElement = new SeriesElement();

                sElement.Name = elemName;
                sElement.Value = elemValue;

                thisChartSeries.SeriesElementList.Add(sElement);
                
                sElement = null;
            } //    End foreach loop

            if (thisChartSeries.SeriesElementList.Count > 0)
            {
                // There is one last series object that needs to be added to the ChartSeriesList.
                ChartSeriesList.Add(thisChartSeries);
            }

            // -------------------------------------------------------------------------------------
            return ChartSeriesList;

        } //    End BuildChartSeriesObjects


    } //    End ChartBaseHandler
}
