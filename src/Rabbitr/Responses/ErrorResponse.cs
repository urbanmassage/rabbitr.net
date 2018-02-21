using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rabbitr.Responses
{
    public class ErrorResponse : Response
    {
        [JsonConstructor]
        public ErrorResponse(List<Error> errors)
            : base(errors)
        {
        }

        public ErrorResponse(params Error[] errors)
            : base(errors)
        {
        }
    }
}