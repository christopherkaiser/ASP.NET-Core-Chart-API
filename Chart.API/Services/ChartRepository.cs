using Chart.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chart.API.Services
{
    public class ChartRepository : IChartRepository
    {
        private ChartContext _context;
        public ChartRepository(ChartContext context)
        {
            _context = context;
        }

        public bool ChartExists(int chartId)
        {
            return _context.Charts.Any(c => c.Id == chartId);
        }

        public IEnumerable<Entities.Chart> GetCharts()
        {
            return _context.Charts.OrderBy(c => c.Name).ToList();
        }

        public Entities.Chart GetChart(int chartId)
        {
            return _context.Charts.Include(c => c.ValuePairs)
                .Where(c => c.Id == chartId).FirstOrDefault();
        }

        public void AddChart(Entities.Chart chart)
        {
            _context.Charts.Add(chart);
        }

        public void DeleteChart(Entities.Chart chart)
        {
            _context.Charts.Remove(chart);
        }

        public IEnumerable<ValuePair> GetValuePairsForChart(int chartId)
        {
            return _context.ValuePairs
                           .Where(p => p.ChartId == chartId).ToList();
        }

        public ValuePair GetValuePairForChart(int chartId, int valuePairId)
        {
            return _context.ValuePairs
               .Where(p => p.ChartId == chartId && p.Id == valuePairId).FirstOrDefault();
        }

        public void AddValuePairForChart(int chartId, ValuePair valuePair)
        {
            var chart = GetChart(chartId);
            chart.ValuePairs.Add(valuePair);
        }
        
        public void DeleteValuePair(ValuePair valuePair)
        {
            _context.ValuePairs.Remove(valuePair);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

    }
}
