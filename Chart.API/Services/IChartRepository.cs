using Chart.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chart.API.Services
{
    public interface IChartRepository
    {
        bool ChartExists(int chartId);
        IEnumerable<Entities.Chart> GetCharts();
        Entities.Chart GetChart(int chartId);
        void AddChart(Entities.Chart chart);
        void DeleteChart(Entities.Chart chart);
        IEnumerable<ValuePair> GetValuePairsForChart(int chartId);
        ValuePair GetValuePairForChart(int chartId, int valuePairId);
        void AddValuePairForChart(int chartId, ValuePair valuePair);
        void DeleteValuePair(ValuePair valuePair);
        bool Save();
    }
}
