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
        /// <param name="filterModel.group"></param>
        /// <param name="filterModel.logLevels"></param>
        /// <param name="filterModel.startDate"></param>
        /// <param name="filterModel.endDate"></param>
        /// <param name="filterModel.latest"></param>
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
        /// <param name="collectionName"></param>
        /// <param name="limit"></param>
        /// <param name="filterModel.group"></param>
        /// <param name="filterModel.logLevels"></param>
        /// <param name="filterModel.startDate"></param>
        /// <param name="filterModel.endDate"></param>
        /// <param name="filterModel.pivotId"></param>
        /// <param name="filterModel.getAfterPivotId"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="logs.id"></param>
        /// <param name="logs.createdDate"></param>
        /// <param name="logs.logLevel"></param>
        /// <param name="logs.message"></param>
        /// <param name="logs.stackTrace"></param>
        /// <param name="logs.source"></param>
        /// <param name="logs.group"></param>
        /// <param name="logs.code"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
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
