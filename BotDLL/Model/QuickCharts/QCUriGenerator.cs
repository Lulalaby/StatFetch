﻿using BotDLL.Model.Objects;

namespace BotDLL.Model.QuickCharts
{
    public class QCUriGenerator
    {
        public Uri? QCUri { get; set; }
        private static string token = "";
        private static bool virgin = true;
        public static QCUriGenerator CreateObj(ServerInfo serverInfoObj)
        {
            if (virgin)
            {
                Connections connections = Connections.GetConnections();
#pragma warning disable CS8601 // Mögliche Nullverweiszuweisung.
                token = connections.QuickChartApi;
#if DEBUG
                token = connections.QuickChartApiDebug;
#endif
#pragma warning restore CS8601 // Mögliche Nullverweiszuweisung.
                virgin = false;
            }
            
            QCUriGenerator QCUriObj = new QCUriGenerator();

            if (serverInfoObj != null)
            {
                string quickChartString = $"https://quickchart.io/chart/render/{token}?title={serverInfoObj.Name.Replace(" ", "%20")}";

                serverInfoObj = MonthStatistics.Read(serverInfoObj);

                string lables = "&labels=";
                string data1 = "&data1=";
                string data2 = "&data2=";

                int i = 0;

                foreach (MonthStatistics monthStatisticsItem in serverInfoObj.MonthStatisticsList)
                {
                    if(i < 30)
                    {
                        lables += $"{monthStatisticsItem.Date.ToShortDateString()},";
                        data1 += $"{monthStatisticsItem.MaxPlayers},";
                    }
                    else if (i < 60)
                    {
                        data2 += $"{monthStatisticsItem.MaxPlayers},";
                    }

                    /*if (monthStatisticsItem.Date.Month == DateTime.Now.Month)
                    {
                        lables += $"{monthStatisticsItem.Date.ToShortDateString()},";
                        data1 += $"{monthStatisticsItem.MaxPlayers},";
                    }
                    else if (monthStatisticsItem.Date.Month == DateTime.Now.AddMonths(-1).Month)
                    {
                        data2 += $"{monthStatisticsItem.MaxPlayers},";
                    }*/

                    i++;
                }

                lables = lables.TrimEnd(',');
                data1 = data1.TrimEnd(',');
                quickChartString += lables += data1 += data2;
                //Q1,Q2,Q3,Q4
                //50,40,30,20

                QCUriObj.QCUri = new Uri(quickChartString);
            }
            else
                QCUriObj.QCUri = new Uri("https://quickchart.io/chart/render/{token}");

            return QCUriObj;
        }
    }
}