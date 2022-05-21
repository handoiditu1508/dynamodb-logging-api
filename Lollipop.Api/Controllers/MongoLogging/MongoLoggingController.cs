using Lollipop.Api.Attributes;
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
    [ApiKeyAuthenticate]
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
        /// <returns>List of logs along with total collection size and count.</returns>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     {
        ///        "collectionName": "Dev_Main",// Collection name.
        ///        "limit": 10,
        ///        "filterModel": {
        ///           "group": "SYSTEM",// Group that logs belong to.
        ///           "logLevels": [0, 3, 5],// 0 - Trace, 1 - Debug, 2 - Information, 3 - Warning, 4 - Error, 5 - Critical, 6 - None
        ///           "startDate": "2000-5-15",// Logs range from.
        ///           "endDate": "2000-5-16",// Logs range to.
        ///           "latest": true// Get by latest logs or oldest log.
        ///        }
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route(nameof(MongoLoggingController.GetOutermostLogs))]
        [ProducesResponseType(typeof(GetLogsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetLogsResponse>> GetOutermostLogs(GetOutermostLogsRequest request)
        {
            try
            {
                return Ok(await _loggingService.GetOutermostLogs(request));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Get Latest or Oldest logs.
        /// </summary>
        /// <returns>List of logs along with total collection size and count.</returns>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     {
        ///        "collectionName": "Dev_Main",// Collection name.
        ///        "limit": 10,
        ///        "filterModel": {
        ///           "group": "SYSTEM",// Group that logs belong to.
        ///           "logLevels": [0, 3, 5],// 0 - Trace, 1 - Debug, 2 - Information, 3 - Warning, 4 - Error, 5 - Critical, 6 - None
        ///           "startDate": "2000-5-15",// Logs range from.
        ///           "endDate": "2000-5-16",// Logs range to.
        ///           "pivotId": "abcdefg12345",// Id of the pivot log.
        ///           "getAfterPivotId": true// Get logs after the pivot log or before the pivot log.
        ///        }
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route(nameof(MongoLoggingController.GetNearbyLogs))]
        [ProducesResponseType(typeof(GetLogsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetLogsResponse>> GetNearbyLogs(GetNearbyLogsRequest request)
        {
            try
            {
                return Ok(await _loggingService.GetNearbyLogs(request));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Delete log collection by name.
        /// </summary>
        /// <param name="collectionName">Name of the collection to be deleted.</param>
        [HttpDelete]
        [Route(nameof(MongoLoggingController.DeleteCollection) + "/{collectionName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Insert logs into collection.
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     {
        ///        "collectionName": "Dev_Main",// Name of the collection.
        ///        "logs": [
        ///           {
        ///              "id": null,// Not required, auto generated!
        ///              "createdDate": null,// Not required, auto generated!
        ///              "logLevel": 3,// 0 - Trace, 1 - Debug, 2 - Information, 3 - Warning, 4 - Error, 5 - Critical, 6 - None
        ///              "message": "Unexpected error.",// Message of the log.
        ///              "stackTrace": "string",// Exception stack trace if any.
        ///              "source": "string",// Exception source if any.
        ///              "group": "SYSTEM",// Group that logs belong to.
        ///              "code": "SYSTEM_001"// Exception code.
        ///           }
        ///        ]
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route(nameof(MongoLoggingController.InsertLogs))]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Insert log into collection.
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     {
        ///        "collectionName": "Dev_Main",// Name of the collection.
        ///        "log": {
        ///           "id": null,// Not required, auto generated!
        ///           "createdDate": null,// Not required, auto generated!
        ///           "logLevel": 3,// 0 - Trace, 1 - Debug, 2 - Information, 3 - Warning, 4 - Error, 5 - Critical, 6 - None
        ///           "message": "Unexpected error.",// Message of the log.
        ///           "stackTrace": "string",// Exception stack trace if any.
        ///           "source": "string",// Exception source if any.
        ///           "group": "SYSTEM",// Group that logs belong to.
        ///           "code": "SYSTEM_001"// Exception code.
        ///        }
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route(nameof(MongoLoggingController.InsertLog))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> InsertLog(InsertLogRequest request)
        {
            try
            {
                await _loggingService.InsertLog(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Get all collection names.
        /// </summary>
        /// <returns>List of string for collection names.</returns>
        [HttpGet]
        [Route(nameof(MongoLoggingController.GetCollectionNames))]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<string>>> GetCollectionNames()
        {
            try
            {
                return Ok(await _loggingService.GetCollectionNames());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }
    }
}
