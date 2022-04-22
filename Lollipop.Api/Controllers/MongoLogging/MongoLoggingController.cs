using Lollipop.Helpers;
using Lollipop.Helpers.Extensions;
using Lollipop.Models.Common;
using Lollipop.Models.Requests.MongoLogging;
using Lollipop.Models.Responses.MongoLogging;
using Lollipop.Services.MongoLogging.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Lollipop.Api.Controllers.MongoLogging
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoLoggingController : ControllerBase
    {
        private readonly IMongoLoggingService _loggingService;

        public MongoLoggingController(IMongoLoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        /// <summary>
        /// Get Latest or Oldest logs.
        /// </summary>
        /// <param name="collectionName">Collection name.</param>
        /// <param name="limit">Maximum logs to retrieve.</param>
        /// <param name="filterModel.group">Group that logs belong to.</param>
        /// <param name="filterModel.logLevels">Critical level of the logs.</param>
        /// <param name="filterModel.startDate">Logs range from.</param>
        /// <param name="filterModel.endDate">Logs range to.</param>
        /// <param name="filterModel.latest">Get by latest logs or oldest log.</param>
        /// <returns>List of logs along with total collection size and count.</returns>
        [HttpPost]
        [Route(nameof(MongoLoggingController.GetOutermostLogs))]
        [ProducesResponseType(typeof(GetLogsResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetLogsResponse>> GetOutermostLogs(GetOutermostLogsRequest request)
        {
            try
            {
                return Ok(await _loggingService.GetOutermostLogs(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Get Latest or Oldest logs.
        /// </summary>
        /// <param name="collectionName">Collection name.</param>
        /// <param name="limit">Maximum logs to retrieve.</param>
        /// <param name="filterModel.group">Group that logs belong to.</param>
        /// <param name="filterModel.logLevels">Critical level of the logs.</param>
        /// <param name="filterModel.startDate">Logs range from.</param>
        /// <param name="filterModel.endDate">Logs range to.</param>
        /// <param name="filterModel.pivotId">Id of the pivot log.</param>
        /// <param name="filterModel.getAfterPivotId">Get logs after the pivot log or before the pivot log.</param>
        /// <returns>List of logs along with total collection size and count.</returns>
        [HttpPost]
        [Route(nameof(MongoLoggingController.GetNearbyLogs))]
        [ProducesResponseType(typeof(GetLogsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetLogsResponse>> GetNearbyLogs(GetNearbyLogsRequest request)
        {
            try
            {
                return Ok(await _loggingService.GetNearbyLogs(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Delete log collection by name.
        /// </summary>
        /// <param name="collectionName">Name of the collection to be deleted.</param>
        [HttpDelete]
        [Route(nameof(MongoLoggingController.DeleteCollection))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCollection(string collectionName)
        {
            try
            {
                await _loggingService.DeleteCollection(collectionName);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Insert logs into collection.
        /// </summary>
        /// <param name="logs.id">Not required, auto generated!</param>
        /// <param name="logs.createdDate">Not required, auto generated!</param>
        /// <param name="logs.logLevel">Critical level of the logs.</param>
        /// <param name="logs.message">Message of the log.</param>
        /// <param name="logs.stackTrace">Exception stack trace if any.</param>
        /// <param name="logs.source">Exception source if any.</param>
        /// <param name="logs.group">Group that logs belong to.</param>
        /// <param name="logs.code">Exception code.</param>
        /// <param name="collectionName">Name of the collection.</param>
        [HttpPost]
        [Route(nameof(MongoLoggingController.InsertLogs))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> InsertLogs(InsertLogsRequest request)
        {
            try
            {
                await _loggingService.InsertLogs(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Get all collection names.
        /// </summary>
        /// <returns>List of string for collection names.</returns>
        [HttpGet]
        [Route(nameof(MongoLoggingController.GetCollectionNames))]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<string>>> GetCollectionNames()
        {
            try
            {
                return Ok(await _loggingService.GetCollectionNames());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToSimpleError());
            }
        }
    }
}
