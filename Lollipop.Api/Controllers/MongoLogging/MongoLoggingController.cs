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
        /// 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="pivotId"></param>
        /// <param name="GetAfterPivotId"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(MongoLoggingController.GetLogs))]
        // return code
        public async Task<ActionResult<GetLogsResponse>> GetLogs(string collectionName, string? pivotId, bool GetAfterPivotId = false, int limit = 10)
        {
            try
            {
                var request = new GetLogsRequest
                {
                    CollectionName = collectionName,
                    Limit = limit,
                    PivotId = pivotId,
                    GetAfterPivotId = GetAfterPivotId
                };
                return Ok(await _loggingService.GetLogs(request));
            }
            catch (Exception ex)
            {
                // handle exception
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route(nameof(MongoLoggingController.DeleteCollection))]
        // return code
        public async Task<ActionResult> DeleteCollection(string collectionName)
        {
            try
            {
                await _loggingService.DeleteCollection(collectionName);
                return Ok();
            }
            catch (Exception ex)
            {
                // handle exception
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route(nameof(MongoLoggingController.InsertLogs))]
        // return code
        public async Task<ActionResult> InsertLogs(InsertLogsRequest request)
        {
            try
            {
                await _loggingService.InsertLogs(request);
                return Ok();
            }
            catch (Exception ex)
            {
                // handle exception
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route(nameof(MongoLoggingController.GetCollectionNames))]
        // return code
        public async Task<ActionResult<IEnumerable<string>>> GetCollectionNames()
        {
            try
            {
                return Ok(await _loggingService.GetCollectionNames());
            }
            catch (Exception ex)
            {
                // handle exception
                return BadRequest(ex.Message);
            }
        }
    }
}
