using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlastService.Public.Controllers
{
    [Route("api/v0.3")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IHttpClientFactory clientFactory, ILogger<ProjectController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        #region ROUTES

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("projects")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<string>>> GetProjects()
        {
            var client = _clientFactory.CreateClient();
            var targetURI = new Uri(String.Format("https://localhost:61901/api/v0.3/projects"));

            try
            {
                using var response = await client.GetAsync(targetURI);
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var responseMessage = await reader.ReadToEndAsync();
                //var blastPatterns = JsonConvert.DeserializeObject<List<BlastPattern>>(responseMessage);
                _logger.LogInformation("Got all projects");
                return Ok(responseMessage); // Ok(null) is equivalent to NoContent
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Get a project given its id
        /// </summary>
        /// <returns></returns>
        [HttpGet("projects/{projectId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<string>>> GetProject(Guid projectId)
        {
            var client = _clientFactory.CreateClient();
            var targetURI = new Uri(String.Format("https://localhost:61901/api/v0.3/projects/{0}", projectId));

            try
            {
                using var response = await client.GetAsync(targetURI);
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var responseMessage = await reader.ReadToEndAsync();
                //var blastPatterns = JsonConvert.DeserializeObject<List<BlastPattern>>(responseMessage);
                _logger.LogInformation("Got project with id {0}", projectId);
                return Ok(responseMessage); // Ok(null) is equivalent to NoContent
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Get all patterns for a project given its id
        /// </summary>
        /// <returns></returns>
        [HttpGet("projects/{projectId}/patterns")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<string>>> GetPatterns(Guid projectId)
        {
            var client = _clientFactory.CreateClient();
            var targetURI = new Uri(String.Format("https://localhost:61901/api/v0.3/projects/{0}/patterns", projectId));

            try
            {
                using var response = await client.GetAsync(targetURI);
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var responseMessage = await reader.ReadToEndAsync();
                //var blastPatterns = JsonConvert.DeserializeObject<List<BlastPattern>>(responseMessage);
                _logger.LogInformation("Got all patterns for project id {0}", projectId);
                return Ok(responseMessage); // Ok(null) is equivalent to NoContent
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Get a pattern for project given its id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="patternId"></param>
        /// <returns></returns>
        [HttpGet("projects/{projectId}/patterns/{patternId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<string>> GetPattern(Guid projectId, Guid patternId)
        {
            var client = _clientFactory.CreateClient();
            var targetURI = new Uri(String.Format("https://localhost:61901/api/v0.3/projects/{0}/patterns/{1}", projectId, patternId));

            try
            {
                using var response = await client.GetAsync(targetURI);
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var responseMessage = await reader.ReadToEndAsync();
                //var blastPattern = JsonConvert.DeserializeObject<BlastPattern>(responseMessage);
                var pattern = responseMessage;
                if (pattern == null)
                {
                    _logger.LogError("Pattern with id {0} does not exist", patternId);
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation("Got pattern with id: {id}", patternId);
                    return Ok(pattern);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return new StatusCodeResult(500);
            }
        }
        #endregion
    }
}
