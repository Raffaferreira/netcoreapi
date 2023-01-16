namespace WebApi.Sql.UnitTests.SUT.Interfaces
{
    public interface ILogger
    {
        public void Error(string message);
        public void Debug(string message);
    }
}
