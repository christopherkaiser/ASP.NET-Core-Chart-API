using AutoMapper;
using Chart.API.Models;
using Chart.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chart.API.Controllers
{
    [Route("api/charts")]
    public class ValuePairController : Controller
    {
        private IChartRepository _chartRepository;


        public ValuePairController(IChartRepository chartRepository)
        {
            _chartRepository = chartRepository;
        }

        [HttpGet("{chartId}/valuepairs")]
        public IActionResult GetValuePairs(int chartId)
        {
            try
            {
                if (!_chartRepository.ChartExists(chartId))
                {
                    return NotFound();
                }

                var valuePairsForChart = _chartRepository.GetValuePairsForChart(chartId);
                var valuePairsForChartResults =
                                   Mapper.Map<IEnumerable<ValuePairDto>>(valuePairsForChart);

                return Ok(valuePairsForChartResults);
            }
            catch
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{chartId}/valuepairs/{id}", Name = "GetValuePair")]
        public IActionResult GetValuePair(int chartId, int id)
        {
            if (!_chartRepository.ChartExists(chartId))
            {
                return NotFound();
            }

            var valuePair = _chartRepository.GetValuePairForChart(chartId, id);

            if (valuePair == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<ValuePairDto>(valuePair);
            return Ok(result);
        }

        [HttpPost("{chartId}/valuepairs")]
        public IActionResult CreateValuePair(int chartId,
            [FromBody] ValuePairForCreationDto valuePair)
        {
            if (valuePair == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_chartRepository.ChartExists(chartId))
            {
                return NotFound();
            }

            var mapValuePair = Mapper.Map<Entities.ValuePair>(valuePair);

            _chartRepository.AddValuePairForChart(chartId, mapValuePair);

            if (!_chartRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdValuePairToReturn = Mapper.Map<ValuePairDto>(mapValuePair);

            return CreatedAtRoute("GetValuePair", new
            { chartId, id = createdValuePairToReturn.Id }, createdValuePairToReturn);
        }

        [HttpPut("{chartId}/valuepairs/{id}")]
        public IActionResult UpdateValuePair(int chartId, int id,
            [FromBody] ValuePairForUpdateDto valuePair)
        {
            if (valuePair == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_chartRepository.ChartExists(chartId))
            {
                return NotFound();
            }

            var valuePairEntity = _chartRepository.GetValuePairForChart(chartId, id);
            if (valuePairEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(valuePair, valuePairEntity);

            if (!_chartRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }


        [HttpPatch("{chartId}/valuepairs/{id}")]
        public IActionResult PartiallyUpdateValuePair(int chartId, int id,
            [FromBody] JsonPatchDocument<ValuePairForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_chartRepository.ChartExists(chartId))
            {
                return NotFound();
            }

            var valuePairEntity = _chartRepository.GetValuePairForChart(chartId, id);
            if (valuePairEntity == null)
            {
                return NotFound();
            }

            var valuePairToPatch = Mapper.Map<ValuePairForUpdateDto>(valuePairEntity);

            patchDoc.ApplyTo(valuePairToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(valuePairToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(valuePairToPatch, valuePairEntity);

            if (!_chartRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{chartId}/valuepairs/{id}")]
        public IActionResult DeleteValuePair(int chartId, int id)
        {
            if (!_chartRepository.ChartExists(chartId))
            {
                return NotFound();
            }

            var valuePairEntity = _chartRepository.GetValuePairForChart(chartId, id);
            if (valuePairEntity == null)
            {
                return NotFound();
            }

            _chartRepository.DeleteValuePair(valuePairEntity);

            if (!_chartRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
