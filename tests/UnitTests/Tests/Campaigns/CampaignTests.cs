using Core.Entities;
using Core.Enums;

namespace Campaigns
{
    public class CampaignTests
    {
        [Fact]
        public void DoesCustomerMatchCondition_MaleCondition_ReturnsTrueForMaleCustomer()
        {
            Campaign campaign = new(CampaignCondition.Male, DateTime.UtcNow, 1);
            Customer customer = new(Gender.Male, 30, "New York", 200, false);

            bool matchesCondition = campaign.DoesCustomerMatchCondition(customer);

            Assert.True(matchesCondition);
        }

        [Fact]
        public void DoesCustomerMatchCondition_AgeAbove45Condition_ReturnsTrueForCustomerAbove45()
        {
            Campaign campaign = new (CampaignCondition.AgeAbove45, DateTime.UtcNow, 1);
            Customer customer = new (Gender.Female, 50, "Los Angeles", 150, true);

            bool matchesCondition = campaign.DoesCustomerMatchCondition(customer);

            Assert.True(matchesCondition);
        }

        [Fact]
        public void DoesCustomerMatchCondition_CityNewYorkCondition_ReturnsTrueForCustomerInNewYork()
        {
            Campaign campaign = new (CampaignCondition.CityNewYork, DateTime.UtcNow, 1);
            Customer customerInNY = new (Gender.Female, 40, "New York", 300, false);
            Customer customerNotInNY = new (Gender.Male, 25, "Chicago", 100, true);

            Assert.True(campaign.DoesCustomerMatchCondition(customerInNY));
            Assert.False(campaign.DoesCustomerMatchCondition(customerNotInNY));
        }

        [Fact]
        public void DoesCustomerMatchCondition_DepositAbove100Condition_ReturnsTrueForCustomerWithDepositAbove100()
        {
            Campaign campaign = new (CampaignCondition.DepositAbove100, DateTime.UtcNow, 1);
            Customer customerHighDeposit = new (Gender.Male, 35, "Dallas", 200, false);
            Customer customerLowDeposit = new (Gender.Female, 28, "Miami", 50, true);

            Assert.True(campaign.DoesCustomerMatchCondition(customerHighDeposit));
            Assert.False(campaign.DoesCustomerMatchCondition(customerLowDeposit));
        }

        [Fact]
        public void DoesCustomerMatchCondition_IsNewCustomerCondition_ReturnsTrueForNewCustomer()
        {
            Campaign campaign = new (CampaignCondition.IsNewCustomer, DateTime.UtcNow, 1);
            Customer newCustomer = new (Gender.Female, 22, "Seattle", 80, true);
            Customer existingCustomer = new (Gender.Male, 40, "San Francisco", 150, false);

            Assert.True(campaign.DoesCustomerMatchCondition(newCustomer));
            Assert.False(campaign.DoesCustomerMatchCondition(existingCustomer));
        }

        [Fact]
        public void DoesCustomerMatchCondition_UnsupportedCondition_ReturnsFalse()
        {
            Campaign campaign = new ((CampaignCondition)99, DateTime.UtcNow, 1);
            Customer customer = new (Gender.Female, 30, "Boston", 120, false);

            bool matchesCondition = campaign.DoesCustomerMatchCondition(customer);

            Assert.False(matchesCondition);
        }
    }
}