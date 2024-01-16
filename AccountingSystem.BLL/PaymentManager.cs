using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;

namespace AccountingSystem.BLL
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IPaymentRepository _repository;
        public PaymentManager(IPaymentRepository repository) //: base(repository)
        {
            _repository = repository;
        }
        //public  Users GetUsers(string userName, string password)
        //{
        //    return  _repository.GetUsers(userName, password);
        //}
    }
}
