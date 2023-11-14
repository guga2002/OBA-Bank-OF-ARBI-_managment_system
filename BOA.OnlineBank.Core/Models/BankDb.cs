using Bank_Managment_System.Models._2Fa;
using Bank_Managment_System.Models.Aunthification;
using Bank_Managment_System.Models.SystemModels;
using Microsoft.EntityFrameworkCore;

namespace Bank_Managment_System.Models
{
    public class BankDb : DbContext
    {
        public BankDb(DbContextOptions<BankDb> ops) : base(ops)
        {

        }

        // aq davwert modelis  tablebs
        public virtual DbSet<Operator> _operators { get; set; }
        public virtual DbSet<User> _users { get; set; }
        public virtual DbSet<Manager> _managers { get; set; }
        public virtual DbSet<Contact> _contacts { get; set; }
        public virtual DbSet<BankAccount> _Accounts { get; set; }
        public virtual DbSet<Card> _cards { get; set; }
        public virtual DbSet<Transaction> _transactions { get; set; }
        public virtual DbSet<Error> _errors { get; set; }
        public virtual DbSet<Log> _Logs { get; set; }
        public virtual DbSet<ValuteCurse> _valuteCourse { get; set; }
        public virtual DbSet<_2StepAunt> stepaunth{get;set;}
        public virtual DbSet<CookieforOperator> cookieforoperator { get; set; }
        public virtual DbSet<CookieforUser> cookieforuser { get; set; }
        public virtual DbSet<Cookieformanager> cookieformanager { get; set; }

        public virtual DbSet<ManagerAuthification> Manageraunth { get; set; }

        public virtual DbSet<OperatorAunthification> operatoraunth { get; set; }
        public virtual DbSet<UserAunthification> useraunth { get; set; }


    }
}
