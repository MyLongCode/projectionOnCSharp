using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {

        public Guid Guid { get; }

        public string Name { get; set; }


        string inn;
        public string Inn { 
            get { return inn; } 
            set 
            {
                if (value.Length != 10 || !value.All(z => char.IsDigit(z)))
                    throw new ArgumentException();
                this.inn = value;
            } 
        }

        DateTime establishDate;

        public DateTime EstablishDate { get; set; }

        public TimeSpan ActiveTimeSpan => DateTime.Now - establishDate;

        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();
            var amount = DataBase.Transactions().Where(z => z.EnterpriseGuid == Guid).Sum(a => a.Amount);
            return amount;
        }
    }
}
