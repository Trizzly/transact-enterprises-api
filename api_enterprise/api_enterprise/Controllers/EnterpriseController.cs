using api_enterprise.Models;
using api_enterprise.Models.ResourceModels;
using AutoMapper;
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

        [HttpGet("{Id}")]
        [SwaggerOperation(Summary = "Obtient l'entreprise selon l'identifiant passé en paramètre.")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(204, "Aucune donnée trouvée.")]
        [SwaggerResponse(500, "Une erreur est survenue au moment de l'obtention des données.")]
        public ActionResult<EnterpriseResult> GetEnterpiseFromID([Required][SwaggerParameter("Identifier of the enterprise to get.")] int idEnterprise)
        {
            _logger.LogInformation("Accès au endpoint GetEnterpriseFromID.");
            try
            {
                var enterprise = _context.Enterprises
                                 .Where(e => e.RegionId == idEnterprise)
                                 .Select(e => new EnterpriseResult
                                 {
                                     Id = e.Id,
                                     Summary = e.Summary,
                                     Description = e.Description,
                                     Region = e.Region.RegionName,
                                     Country = e.Country.CountryName,
                                     Price = e.Price,
                                     Member = e.Member.MemberName
                                 });
                var enterprise1 = enterprise.FirstOrDefault();

                if (enterprise1 != null)
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
        [SwaggerOperation(Summary = "Obtient l'entreprise selon l'identifiant passé en paramètre.")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(204, "Aucune donnée trouvée.")]
        [SwaggerResponse(500, "Une erreur est survenue au moment de l'obtention des données.")]
        public ActionResult<EnterpriseResult> GetEnterpriseFromRegionID([Required] [SwaggerParameter("Region identifier")] int idRegion)
        {
            _logger.LogInformation("Accès au endpoint GetEnterpriseFromRegionID.");

            try
            {
                var enterprises = _context.Enterprises
                                 .Where(e=> e.RegionId == idRegion)
                                 .Select(e=> new EnterpriseResult
                                 {
                                     Id = e.Id,
                                     Summary = e.Summary,
                                     Description = e.Description,
                                     Region = e.Region.RegionName,
                                     Country = e.Country.CountryName,
                                     Price = e.Price,
                                     Member = e.Member.MemberName
                                 })
                                .OrderBy(s => s.Summary)
                                .ToList();

                if (enterprises != null)
                    return Ok(enterprises);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while obtaining data : {ex}");
                return StatusCode(500, "An error occured while fetching the data.");
            }
        }


        //get par mots-clés

        //creation d'annonce 

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new enterprise to be listed.")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "An incoming parameter is invalid.")]
        [SwaggerResponse(500, "An error occured while attempting to create an enterprise.")]
        public ActionResult Create([FromBody] EnterprisePost enterprise)
        {
            _logger.LogInformation("Accès au endpoint Create.");
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
            _logger.LogInformation("Accès au endpoint Update.");

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

        [HttpDelete("{Id}")]
        [SwaggerOperation(Summary = "Deletes an existing enterprise.")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "No data found. Impossible to delete.")]
        [SwaggerResponse(500, "An error occured while attempting to delete data.")]
        public ActionResult Delete([SwaggerParameter("Unique Enterprisen ID.")][Required] int idEnterprise)
        {
            _logger.LogInformation("Accès au endpoint DeleteEnterprise.");

            try
            {
                var enterprise = _context.Enterprises.Find(idEnterprise);
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
                ModelState.AddModelError("Summary", "Une entreprise sous ce nom existe déjà.");
        }

        private void ValidateEnterpriseExistsUnderDifferentID(EnterprisePut enterprise)
        {
            if (_context.Enterprises.Any(e => e.Summary == enterprise.Summary && e.Id != enterprise.Id))
                ModelState.AddModelError("Summary", "Une entreprise sous ce nom existe déjà.");
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