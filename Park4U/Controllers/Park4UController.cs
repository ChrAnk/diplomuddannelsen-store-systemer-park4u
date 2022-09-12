using Microsoft.AspNetCore.Mvc;

namespace Park4U.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class Park4UController : ControllerBase
    {
        public List<List<string>> ParkingData = new()
        {
            new List<string>{"1", "2022-09-11T18:30:21.0283923Z", "30", "AA 10 000", "example1@example.com", "+45 11 11 11 11"},
            new List<string>{"1", "2022-09-11T20:30:22.0283923Z", "30", "AA 20 000", "example2@example.com", "+45 22 22 22 22"},
            new List<string>{"2", "2022-09-11T20:30:23.0283923Z", "30", "AA 30 000", "example3@example.com", "+45 33 33 33 33"},
            new List<string>{"2", "2022-09-12T18:30:24.0283923Z", "500", "AA 40 000", "example4@example.com", "+45 44 44 44 44"}
        };


        [HttpPost("RegisterParking")]

        public object RegisterParking(int ParkingSpaceId, int ParkingTimeMinutes, string RegistrationNumber, string EMailAddress, string PhoneNumber)
        {
            // How to handle prolonging time
            // => Add time to existing registration?

            var registration = new List<string> {
                ParkingSpaceId.ToString(),
                DateTime.UtcNow.ToString(),
                ParkingTimeMinutes.ToString(),
                RegistrationNumber,
                EMailAddress,
                PhoneNumber
            };

            ParkingData.Add(registration);

            return new RegisterParking
            {
                ParkingStart = DateTime.UtcNow,
                ParkingTimeMinutes = ParkingTimeMinutes,
                RegistrationNumber = RegistrationNumber,
                EMailAddress = EMailAddress,
                PhoneNumber = PhoneNumber,
                ParkingSpaceId = ParkingSpaceId
            };
        }


        [HttpGet("CheckMeter")]

        public object CheckMeter(string? ParkingSpaceId, string? RegistrationNumber)
        {
            // To be done:
            // * Does not currently handle multiple instances of the same car at the same parking space
            //   => Maybe use FindAll when getting result, but requires finding the latest only
            // * Not sure that dates calculate correctly

            int RemainingMinutes = 0, PaidMinutes = 0;

            DateTime StartTime, EndTime, CurrentTime = DateTime.UtcNow;

            var result = ParkingData.Find(x => x[0] == ParkingSpaceId && x[3] == RegistrationNumber);

            if (result != null)
            {
                StartTime = Convert.ToDateTime(result[1]);

                PaidMinutes = Convert.ToInt32(result[2]);

                EndTime = StartTime.AddMinutes(PaidMinutes);

                TimeSpan TimeDifference = EndTime.Subtract(CurrentTime);

                RemainingMinutes = Convert.ToInt32(TimeDifference.TotalMinutes);

                if(RemainingMinutes < 0)
                {
                    RemainingMinutes = 0;
                }
            }
            else
            {
                StartTime = Convert.ToDateTime("2000-01-01T00:00:00.0000000Z");
                EndTime = Convert.ToDateTime("2000-01-01T00:00:00.0000000Z");
                RemainingMinutes = 0;
            }

            return new CheckParking
            {
                ParkingSpaceId = ParkingSpaceId,
                RegistrationNumber = RegistrationNumber,
                RemainingMinutes = RemainingMinutes,
                StartTime = StartTime,
                EndTime = EndTime
            };
        }


        [HttpPost("DeleteRecords")]

        public object DeleteRecords(string? RegistrationNumber)
        {
            var CountPre = ParkingData.Count;

            for(int i = 0; i < ParkingData.Count; i++)
            {
                ParkingData.RemoveAll(x => x[3] == RegistrationNumber);
            }

            var CountPost = ParkingData.Count;

            var RecordsDeleted = CountPre - CountPost;

            return new DeleteRecords
            {
                RegistrationNumber = RegistrationNumber,
                RecordsDeleted = RecordsDeleted
            };
        }
    }
}