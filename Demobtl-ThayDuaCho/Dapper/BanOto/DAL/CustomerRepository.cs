using DAL.Helper;
using Model;
using System.Data;

namespace DAL
{
    public partial class CustomerRepository : ICustomerRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public CustomerRepository(IDatabaseHelper dbHelper) => _dbHelper = dbHelper;
        public bool Create(CustomerModel model)
        {
            _dbHelper.Execute("sp_customer_create", new
            {
                model.customer_email,
                model.customer_password
            }, CommandType.StoredProcedure);
            return true;
        }
    }
}
