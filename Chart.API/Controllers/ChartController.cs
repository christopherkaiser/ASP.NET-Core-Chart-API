using AutoMapper;
using Chart.API.Models;
using Chart.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chart.API.Controllers
{
    [Route("api/charts")]
    public class ChartController : Controller
    {
        private IChartRepository _chartRepository;

        public ChartController(IChartRepository chartRepository)
        {
            _chartRepository = chartRepository;
        }

        [HttpGet()]
        public IActionResult GetCharts()
        {
            var chartEntities = _chartRepository.GetCharts();
            var results = Mapper.Map<IEnumerable<ChartDto>>(chartEntities);

            return Ok(results);
        }

        [HttpGet("{id}", Name = "GetChart")]
        public IActionResult GetChart(int id)
        {
            var chart = _chartRepository.GetChart(id);

            if (chart == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<ChartDto>(chart);
            return Ok(result);
        }

        [HttpPost()]
        public IActionResult PostChart([FromBody] ChartForCreationDto chart)
        {
            if (chart == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapChart = Mapper.Map<Entities.Chart>(chart);

            _chartRepository.AddChart(mapChart);

            if (!_chartRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdChartToReturn = Mapper.Map<ChartDto>(mapChart);

            return CreatedAtRoute("GetChart", new
            { id = createdChartToReturn.Id }, createdChartToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateChart(int id,
            [FromBody] ChartForUpdateDto chart)
        {
            if (chart == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_chartRepository.ChartExists(id))
            {
                return NotFound();
            }

            var chartEntity = _chartRepository.GetChart(id);
            if (chartEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(chart, chartEntity);

            if (!_chartRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateValuePair(int id,
            [FromBody] JsonPatchDocument<ChartForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_chartRepository.ChartExists(id))
            {
                return NotFound();
            }

            var chartEntity = _chartRepository.GetChart(id);
            if (chartEntity == null)
            {
                return NotFound();
            }

            var chartToPatch = Mapper.Map<ChartForUpdateDto>(chartEntity);

            patchDoc.ApplyTo(chartToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(chartToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(chartToPatch, chartEntity);

            if (!_chartRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteChart(int id)
        {
            if (!_chartRepository.ChartExists(id))
            {
                return NotFound();
            }

            var chartEntity = _chartRepository.GetChart(id);
            if (chartEntity == null)
            {
                return NotFound();
            }

            _chartRepository.DeleteChart(chartEntity);

            if (!_chartRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
