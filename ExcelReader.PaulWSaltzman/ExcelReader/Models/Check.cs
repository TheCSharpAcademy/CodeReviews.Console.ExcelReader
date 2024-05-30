using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.Models
{
    public class Check
    {
        public Check(string accountNo, int checkNo, decimal amount, DateOnly date, bool canceled)
        {
            this.AccountNo = accountNo;
            this.CheckNo = checkNo;
            this.Amount = amount;
            this.Date = date;
            this.Canceled = canceled;
        }

        [Key]
        public int CheckKid { get; set; }

        public string AccountNo { get; set; }
        public int CheckNo { get; set; }
        public decimal Amount { get; set; }
        public DateOnly Date {  get; set; }
        public bool Canceled {  get; set; }
    }
}
