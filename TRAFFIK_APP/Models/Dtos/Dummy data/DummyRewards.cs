using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRAFFIK_APP.Models.Dtos.Dummy_data
{
    public static class DummyRewards
    {
        public static List<RewardItemDto> GetSampleRewards() => new()
        {
            new RewardItemDto
            {
                Id = 1,
                Name = "10% Off Next Service",
                Description = "Get a 10% discount on your next vehicle service appointment.",
                Cost = 100,
                ImageUrl = "https://example.com/images/reward1.png"
            },

            new RewardItemDto
            {
                Id = 2,
                Name = "Free Car Wash",
                Description = "Enjoy a complimentary car wash at our service center.",
                Cost = 150,
                ImageUrl = "https://example.com/images/reward2.png"
            },

            new RewardItemDto
            {
                Id = 3,
                Name = "Free Oil Change",
                Description = "Redeem this reward for a free oil change service.",
                Cost = 300,
                ImageUrl = "https://example.com/images/reward3.png"
            },  

            new RewardItemDto
            {
                Id = 4,
                Name = "Free Tire Rotation",
                Description = "Get a free tire rotation service to keep your tires in top shape.",
                Cost = 250,
                ImageUrl = "https://example.com/images/reward4.png"
            },

            new RewardItemDto
            {
                Id = 5,
                Name = "15% Off Next Service",
                Description = "Get a 15% discount on your next vehicle service appointment.",
                Cost = 200,
                ImageUrl = "https://example.com/images/reward5.png"
            }

        };
    }
}
