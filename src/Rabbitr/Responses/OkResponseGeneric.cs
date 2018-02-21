namespace Rabbitr.Responses
{
    public class OkResponseGeneric<T> : Response<T>
    {
        public OkResponseGeneric(T data) : base(data)
        {
        }
    }
}