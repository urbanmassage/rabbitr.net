namespace Rabbitr.Messages
{
    public class RpcResponseMessage<T>
    {
        public RpcResponseMessage(T response, string error)
        {
            Response = response;
            IsError = !string.IsNullOrWhiteSpace(error);
            Error = error;
        }

        public T Response {get;}

        public bool IsError {get;}

        public string Error {get;}
    }
}