using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities
{
    public class Customer(Gender gender, int age, string city, decimal deposit, bool isNewCustomer, int id = default, DateTime lastCampaignSentTime = default) 
        : BaseEntity<int>(id)
    {
        public Gender Gender { get; private set; } = gender;
        public int Age { get; private set; } = age;
        public string City { get; private set; } = city;
        public decimal Deposit { get; private set; } = deposit;
        public bool IsNewCustomer { get; private set; } = isNewCustomer;
        public DateTime LastCampaignSentTime { get; private set; } = lastCampaignSentTime;
    }
}