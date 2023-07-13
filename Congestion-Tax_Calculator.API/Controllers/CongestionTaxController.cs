using Congestion_Tax_Calculator.Application.Contracts.Persistence;
using Congestion_Tax_Calculator.Application.Features.CongestionTaxCalculator.CalculateTaxByDayVehicle.Queries;
using Congestion_Tax_Calculator.Application.Features.CongestionTaxCalculator.CalculateTotalVehicleTax.Queries;
using Congestion_Tax_Calculator.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Congestion_Tax_Calculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongestionTaxController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        private readonly IMediator _mediatR;
        public CongestionTaxController(IMediator mediatR, IVehicleRepository vehicleRepository)
        {
            _mediatR = mediatR;
            _vehicleRepository = vehicleRepository;
        }

        // GET: api/<CongestionTaxController>
        [HttpGet]
        [Route("GetVehicleDailyTax/{v_id}/{date}")]
        public async Task<int> Get(int v_id,DateTime date)
        {
            var getTax = new GetTax();
            getTax.vehicle = await _vehicleRepository.GetVehicleWithType(v_id);
            getTax.date = date;
            var result = await _mediatR.Send(getTax);
            return result;
        }

        // GET: api/<CongestionTaxController>
        [HttpGet]
        [Route("GetVehicleTotalTax/{v_id}")]
        public async Task<int> Get(int v_id)
        {
            GetVehicleTotalTax totalTax = new GetVehicleTotalTax();
            totalTax.VehicleId = v_id;
            var result = await _mediatR.Send(totalTax);
            return result;
        }


    }
}
