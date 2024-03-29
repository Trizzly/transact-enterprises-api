using api_enterprise.Models;
using api_enterprise.Models.ResourceModels;
using AutoMapper;
using enterprises.Models.ResourceModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace api_enterprise.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]

    public class EnterpriseController : ControllerBase
    {
        private readonly transactContext _context;
        private readonly ILogger<EnterpriseController> _logger;
        private readonly IMapper _mapper;

        public EnterpriseController(transactContext context, ILogger<EnterpriseController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtains the enterprise by the id.")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(204, "No data found.")]
        [SwaggerResponse(500, "An error occured while obtaining the data.")]
        public ActionResult<EnterpriseResult> GetEnterpiseFromID([Required][SwaggerParameter("Identifier of the enterprise to get.")] int id)
        {
            _logger.LogInformation("Acc�s au endpoint GetEnterpriseFromID.");
            try
            {
                var enterprise = _context.Enterprises
                                 .Where(e => e.RegionId == id)
                                 .Select(e => new EnterpriseResult
                                 {
                                     Id = e.Id,
                                     Summary = e.Summary,
                                     Description = e.Description,
                                     Region = e.Region.RegionName,
                                     Country = e.Country.CountryName,
                                     Price = e.Price,
                                     Member = e.Member.MemberName
                                 }).FirstOrDefault();

                if (enterprise != null)
                    return Ok(enterprise);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while obtaining data : {ex}");
                return StatusCode(500, "An error occured while fetching the data.");
            }
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Obtains the enterprises matching the passed parameters.")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<EnterpriseGetResult>))]
        [SwaggerResponse(204, "No data found.")]
        [SwaggerResponse(500, "An error occured while obtaining the data.")]
        public ActionResult<IEnumerable<EnterpriseGetResult>> GetEnterpriseFromRegionID([FromQuery] EnterpriseGet researchInfo)
        {
            _logger.LogInformation("Access to the endpoint GetEnterpriseFromRegionID.");

            try
            {
                var enterprises = _context.Enterprises
                                 .Select(e => new EnterpriseGetResult
                                 {
                                     Id = e.Id,
                                     Summary = e.Summary,
                                     RegionID = e.RegionId,
                                     Price = e.Price
                                 })
                                .Where(s => (s.Summary.Contains(researchInfo.Summary) || researchInfo.Summary == null)
                                && (s.RegionID == researchInfo.RegionID || researchInfo.RegionID == 0)
                                && (s.Price == researchInfo.Price || researchInfo.Price == 0))
                                .OrderBy(e => e.Id);

                if (enterprises.Count() == 0)
                    return NoContent();

                return Ok(enterprises);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while obtaining data : {ex}");
                return StatusCode(500, "An error occured while fetching the data.");
            }
        }


        //get par mots-cl�s

        //creation d'annonce 

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new enterprise to be listed.")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "An incoming parameter is invalid.")]
        [SwaggerResponse(500, "An error occured while attempting to create an enterprise.")]
        public ActionResult Create([FromBody] EnterprisePost enterprise)
        {
            _logger.LogInformation("Acc�s au endpoint Create.");
            try
            {
                ValidateEnterpriseExists(enterprise);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Enterprise newEnterprise = _mapper.Map<Enterprise>(enterprise);

                _context.Enterprises.Add(newEnterprise);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while posting data : {ex}");
                return StatusCode(500, "An error occured while posting the data.");
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Modifies an existing enterprise.")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "An incoming parameter is invalid.")]
        [SwaggerResponse(404, "No data found. Impossible to modify.")]
        [SwaggerResponse(500, "An error occured while attempting to modify an enterprise.")]
        public ActionResult Update([FromBody] EnterprisePut enterprise)
        {
            _logger.LogInformation("Acc�s au endpoint Update.");

            try
            {
                var enterpriseToModify = _context.Enterprises.Find(enterprise.Id);

                if (enterpriseToModify == null)
                    return NotFound();

                ValidateEnterpriseExistsUnderDifferentID(enterprise);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                enterpriseToModify = ModifyExistingEnterprise(enterpriseToModify, enterprise);

                _context.Enterprises.Update(enterpriseToModify);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while modifying data : {ex}");
                return StatusCode(500, "An error occured while modifying the data.");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes an existing enterprise.")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "No data found. Impossible to delete.")]
        [SwaggerResponse(500, "An error occured while attempting to delete data.")]
        public ActionResult Delete([SwaggerParameter("Unique Enterprisen ID.")][Required] int id)
        {
            _logger.LogInformation("Acc�s au endpoint DeleteEnterprise.");

            try
            {
                var enterprise = _context.Enterprises.Find(id);
                if (enterprise == null)
                    return NotFound();

                //Dont forget to remove from favorites if implemented
                _context.Remove(enterprise);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting data : {ex}");
                return StatusCode(500, "An error occured while deleting the data.");
            }
        }

        #region private functions

        private void ValidateEnterpriseExists(EnterprisePost enterprise)
        {
            if (_context.Enterprises.Any(e => e.Summary == enterprise.Summary))
                ModelState.AddModelError("Summary", "Une entreprise sous ce nom existe d�j�.");
        }

        private void ValidateEnterpriseExistsUnderDifferentID(EnterprisePut enterprise)
        {
            if (_context.Enterprises.Any(e => e.Summary == enterprise.Summary && e.Id != enterprise.Id))
                ModelState.AddModelError("Summary", "Une entreprise sous ce nom existe d�j�.");
        }

        private Enterprise ModifyExistingEnterprise(Enterprise enterpriseToModify, EnterprisePut enterprise)
        {
            enterpriseToModify.Summary = enterprise.Summary;
            enterpriseToModify.Description = enterprise.Description;
            enterpriseToModify.Price = enterprise.Price;
            enterpriseToModify.Justification = enterprise.Justification;
            enterpriseToModify.VendorFinancing = enterprise.VendorFinancing;
            enterpriseToModify.VendorImplication = enterprise.VendorImplication;

            return enterpriseToModify;
        }

        #endregion
    }
}