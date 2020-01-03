using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BlastService.Private.Models;
using BlastService.Private.ModelContract;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using BlastService.Private.ModelContract.Validation;
using Npgsql;
namespace BlastService.Private.Controllers
{
    [Route("api/v0.3")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ProjectContext context, ILogger<ProjectController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Routes

        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <returns></returns>
        [HttpGet("projects")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetProjects()
        {
            var projectDbs = await _context.Projects.ToListAsync();
            var responses = new List<ProjectResponse>();

            foreach (var projectDb in projectDbs)
            {
                var response = new ProjectResponse(projectDb);
                responses.Add(response);
            }

            _logger.LogInformation("Got all projects");
            return Ok(responses); // Could be an empty list.
        }

        /// <summary>
        /// Get a project given its id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("projects/{projectId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<ProjectResponse>> GetProject(Guid projectId)
        {
            if (!ProjectExists(projectId))
            {
                _logger.LogError("Project with id {0} does not exist", projectId);
                return NotFound();
            }

            var projectDb = await _context.Projects.FirstAsync(p => p.Id == projectId);
            var response = new ProjectResponse(projectDb);
            _logger.LogInformation("Got project with id {0}", projectId);
            return Ok(response); // Could be an empty list.
        }

        /// <summary>
        /// Get all patterns for project given its id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("projects/{projectId}/patterns")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<PatternResponse>>> GetPatterns(Guid projectId)
        {
            if (!ProjectExists(projectId))
            {
                _logger.LogError("Project with id {0} does not exist", projectId);
                return NotFound();
            }

            var patternDbs = await GetProjectPatterns(projectId);
            var responses = new List<PatternResponse>();
            if (patternDbs != null)
            {
                foreach (var patternDb in patternDbs)
                {
                    var response = new PatternResponse(patternDb);
                    responses.Add(response);
                }
            }

            _logger.LogInformation("Got all patterns for project id {0}", projectId);
            return Ok(responses); // Could be an empty list.
        }

        /// <summary>
        /// Get a pattern for a project given its id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="patternId"></param>
        /// <returns></returns>
        [HttpGet("projects/{projectId}/patterns/{patternId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<PatternResponse>> GetPattern(Guid projectId, Guid patternId)
        {
            if (!ProjectExists(projectId))
            {
                _logger.LogError("Project with id {0} does not exist", projectId);
                return NotFound();
            }

            var patternDbs = await GetProjectPatterns(projectId);
            if (patternDbs != null)
            {
                var patternDb = patternDbs.FirstOrDefault(p => p.Id == patternId);
                if (patternDb == null)
                {
                    _logger.LogError("Pattern with id {0} does not exist", patternId);
                    return NotFound();
                }

                var response = new PatternResponse(patternDb);
                _logger.LogInformation("Got pattern with id {0}", patternId);
                return Ok(response);
            }
            else
            {
                _logger.LogError("Pattern with id {0} does not exist", patternId);
                return NotFound();
            }
        }

        /// <summary>
        /// Add a new project.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("projects")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<PatternResponse>> AddProject(ProjectRequest request)
        {
            // Validate project using validator
            var validator = RequestValidatorFactory.CreateValidator();
            if (!validator.Validate(request, out var results))
            {
                _logger.LogError(String.Format("{0} error(s) detected in request.", results.ErrorCount));                return BadRequest();
            }

            var projectId = request.NameBasedProperties.BaseProperties.Id;
            if (ProjectExists(projectId))
            {
                _logger.LogError("Project with id {0} exists", projectId);
                return BadRequest();
            }

            try
            {
                var projectDb = new ProjectDb(request);

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Projects.Add(projectDb);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (System.Exception ex) // Database exceptions
                    {
                        await transaction.RollbackAsync();

                        _logger.LogError(ex, string.Empty);
                        return new StatusCodeResult(500);
                    }
                }

                var response = new ProjectResponse(projectDb);
                _logger.LogInformation("Project was created with id {0}" + projectId);
                return CreatedAtAction(nameof(GetProject), new { projectId = projectId }, response);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Add a new pattern for a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("projects/{projectId}/patterns")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<PatternResponse>> AddPattern(Guid projectId, PatternRequest request)
        {
            if (!ProjectExists(projectId))
            {
                _logger.LogError("Project with id {0} does not exist", projectId);
                return NotFound();
            }

            // Validate pattern using validator
            var validator = RequestValidatorFactory.CreateValidator();
            if (!validator.Validate(request, out var results))
            {
                _logger.LogError(String.Format("{0} error(s) detected in request.", results.ErrorCount));                return BadRequest();
            }

            var patternId = request.NameBasedProperties.BaseProperties.Id;
            if (PatternExists(patternId))
            {
                _logger.LogError("Pattern with id {0} Exists");
                return NotFound();
            }

            try
            {
                var patternDb = new PatternDb(projectId, request);

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {

                        _context.Patterns.Add(patternDb);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (System.Exception ex) // Database exception
                    {
                        await transaction.RollbackAsync();

                        _logger.LogError(ex, string.Empty);
                        return new StatusCodeResult(500);
                    }
                }

                var response = new PatternResponse(patternDb);
                _logger.LogInformation("Pattern was created with id {0}", patternId);
                return CreatedAtAction(nameof(GetPattern), new { projectId = projectId, patternId = patternId }, response);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Deletes an entire project.
        /// </summary>
        // TODO: Needs to be transcational to roll back.
        [HttpDelete("projects/{projectId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteProject(Guid projectId)
        {
            if (!ProjectExists(projectId))
            {
                _logger.LogError("Project with id {0} does not exist.");
                return NotFound();
            }

            var command = @"CALL ""DeleteProject"" ('" + projectId + "')";

            try
            { 
                await _context.Database.ExecuteSqlRawAsync(command);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return new StatusCodeResult(500);
            }

            _logger.LogInformation("Project was deleted.");
            return NoContent();
        }
        #endregion

        #region Private Methods
        private bool ProjectExists(Guid projectId)
        {
            return _context.Projects.Any(e => e.Id == projectId);
        }

        private bool PatternExists(Guid patternId)
        {
            return _context.Patterns.Any(e => e.Id == patternId);
        }

        private async Task<IEnumerable<PatternDb>> GetProjectPatterns(Guid projectId)
        {
            var patternDbs =  _context.Patterns
                .Include(p => p.DrillHoles)
                    .ThenInclude(h => h.ChargeProfile)
                .Include(p => p.DrillHoles)
                    .ThenInclude(h => h.ActualFragmentation)
                .Include(p => p.BlastHoles)
                    .ThenInclude(h => h.ChargeProfile)
                .Include(p => p.BlastHoles)
                    .ThenInclude(h => h.ActualFragmentation)
                .Include(p => p.ActualFragmentation)
                .Include(p => p.DesignFragmentation)
                .Where(p => p.ProjectId == projectId)
                .AsEnumerable();

            await _context.Fragments.ToListAsync();

            return patternDbs;
        }

        private async Task<PatternDb> GetAPattern(Guid patternId)
        {
            var patternDb = await _context.Patterns
                .Include(p => p.DrillHoles)
                    .ThenInclude(h => h.ChargeProfile)
                .Include(p => p.DrillHoles)
                    .ThenInclude(h => h.ActualFragmentation)
                .Include(p => p.BlastHoles)
                    .ThenInclude(h => h.ChargeProfile)
                .Include(p => p.BlastHoles)
                    .ThenInclude(h => h.ActualFragmentation)
                .Include(p => p.ActualFragmentation)
                .Where(p => p.Id == patternId)
                .SingleOrDefaultAsync();

            // Calling these populates entities properly for patterns
            await _context.Fragments.FindAsync(patternId);

            return patternDb;
        }

        private async Task<List<HoleDb>> GetPatternHoles(Guid patternId)
        {
            var holeDbs = await _context.Holes.ToListAsync();
            var releventHoleDbs = holeDbs.Where(h => h.DrillPatternId == patternId || h.BlastPatternId == patternId).ToList();
            // Calling these populates entities properly for holes
            await _context.Fragments.ToListAsync();
            return releventHoleDbs;
        }
        #endregion
    }
}
