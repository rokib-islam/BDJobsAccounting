using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;

namespace AccountingSystem.BLL
{
    public class ReportManager : IReportManager
    {
        private readonly IReportRepository _repository;
        public ReportManager(IReportRepository repository) //: base(repository)
        {
            _repository = repository;
        }
        //public  Users GetUsers(string userName, string password)
        //{
        //    return  _repository.GetUsers(userName, password);
        //}
    }
}
