using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rabbitr.Responses
{
    public class Response<T> : Response
    {
        protected Response(T data)
        {
            Data = data;
        }

        [JsonConstructor]
        protected Response(List<Error> errors) : base(errors)
        {
        }

        protected Response(params Error[] errors) : base(errors)
        {
        }

        public T Data {get;}
    }
}