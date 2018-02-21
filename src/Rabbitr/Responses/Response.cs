using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Rabbitr.Responses
{
    public class Response
    {
        public Response()
        {
            Errors = new List<Error>();
        }

        [JsonConstructor]
        protected Response(List<Error> errors)
        {
            Errors = errors ?? new List<Error>();
        }

        protected Response(params Error[] errors)
        {
            Errors = errors?.ToList() ?? new List<Error>();
        }

        public List<Error> Errors {get;}
    }
}