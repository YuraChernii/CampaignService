using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities
{
    public class Customer : BaseEntity<int>
    {
        private Customer() { }

        public Customer(Gender gender, int age, string city, decimal deposit, bool isNewCustomer, int id = default, DateTime lastCampaignSentTime = default)
        {
            Id = id;
            Gender = gender;
            Age = age;
            City = city;
            Deposit = deposit;
            IsNewCustomer = isNewCustomer;
            LastCampaignSentTime = lastCampaignSentTime;
        }

        public Gender Gender { get; private set; }
        public int Age { get; private set; }
        public string City { get; private set; }
        public decimal Deposit { get; private set; }
        public bool IsNewCustomer { get; private set; }
        public DateTime LastCampaignSentTime { get; private set; }
    }
}