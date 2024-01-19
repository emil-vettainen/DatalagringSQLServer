namespace Shared.Interfaces
{
    public interface IErrorLogger
    {
        void ErrorLog(string message, string method);
    }
}