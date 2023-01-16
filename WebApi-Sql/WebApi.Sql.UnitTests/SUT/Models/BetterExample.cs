using WebApi.Sql.UnitTests.SUT.Interfaces;

namespace WebApi.Sql.UnitTests.SUT.Models
{
    public class BetterExample
    {
        private readonly IRepository _repo;
        private readonly IErrorHandler _errorHandler;

        public BetterExample(IRepository repo, IErrorHandler errorHandler)
        {
            _repo = repo;
            _errorHandler = errorHandler;
        }

        public CharacterStatusEnum GetCustomerStatus(int id)
        {
            Customer character;
            try
            {
                character = _repo.Find(id);
            }
            catch (Exception ex)
            {
                _errorHandler.HandleIt();
                //_errorHandler.HandleIt();
                throw;
            }
            switch (character.Name)
            {
                case "Fred":
                case "Barney":
                    return CharacterStatusEnum.Father;
                case "Betty":
                case "Wilma":
                    return CharacterStatusEnum.Mother;
                case "Pebbles":
                case "BamBam":
                    return CharacterStatusEnum.Child;
                case "Slate":
                    return CharacterStatusEnum.Boss;
                default:
                    throw new Exception("Unable to locate status");
            }
        }
    }
}
