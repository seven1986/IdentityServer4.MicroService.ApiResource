using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace IdentityServer4.MicroService.ApiResource.Controllers
{
    [Route("api/[controller]")]
    //[ServiceFilter(typeof(ApiTracker.ApiTracker), IsReusable = true)]
    public class ValuesController : ControllerBase
    {
        public readonly IStringLocalizer l;

        public ValuesController(
           IStringLocalizer<ValuesController> localizer
          )
        {
            l = localizer;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { l["值1"], l["值2"] };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return l["值"];
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
