using api_enterprise.Models;
using api_enterprise.Models.ResourceModels;
using Microsoft.AspNetCore.Mvc;
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

        public EnterpriseController(transactContext context, ILogger<EnterpriseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Obtient l'entreprise selon l'identifiant passé en paramètre.")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(204, "Aucune donnée trouvée.")]
        [SwaggerResponse(500, "Une erreur est survenue au moment de l'obtention des données.")]
        public ActionResult<EnterpriseResult> GetEnterpriseFromRegionID([Required] [SwaggerParameter("Region identifier")] int idRegion)
        {
            _logger.LogInformation("Accés au endpoint GetEnterpriseFromRegionID.");

            try
            {
                var enterprise = _context.Enterprises
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
                return Ok(enterprise);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erreur lors de l'obtention des données : {ex}");
                return StatusCode(500, "Une erreur est survenue au moment de l'obtention des données.");
            }
        }
    }
}