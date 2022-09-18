using Microsoft.AspNetCore.Mvc;

namespace Park4U.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class Park4UController : ControllerBase
    {
        private static readonly List<List<string>> list = new()
        {
            new List<string>{"1", "2022-09-11T18:30:21.0283923Z", "30", "AA 10 000", "example1@example.com", "+45 11 11 11 11"},
            new List<string>{"1", "2022-09-15T20:30:21.0283923Z", "500", "AA 10 000", "example1@example.com", "+45 11 11 11 11"},
            new List<string>{"1", "2022-09-11T18:30:21.0283923Z", "30", "AA 10 000", "example1@example.com", "+45 11 11 11 11"},
            new List<string>{"1", "2022-09-11T20:30:22.0283923Z", "30", "AA 20 000", "example2@example.com", "+45 22 22 22 22"},
            new List<string>{"2", "2022-09-11T20:30:23.0283923Z", "30", "AA 30 000", "example3@example.com", "+45 33 33 33 33"},
            new List<string>{"2", "2022-09-12T18:30:24.0283923Z", "1000", "AA 40 000", "example4@example.com", "+45 44 44 44 44"}
        };
        public static List<List<string>> ParkingData = list;


        [HttpPost("RegisterParking")]

        public object RegisterParking(string ParkingSpaceId, string ParkingTimeMinutes, string RegistrationNumber, string EMailAddress, string PhoneNumber)
        {
            // Limitation: Does not take into account if car and parking spot combination already exists (i.e., not possible to extend parking)

            var registration = new List<string> {
                ParkingSpaceId,
                DateTime.UtcNow.ToString(),
                ParkingTimeMinutes,
                RegistrationNumber,
                EMailAddress,
                PhoneNumber
            };

            ParkingData.Add(registration);

            return new ParkingRegistration
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
            int RemainingMinutes = 0, PaidMinutes = 0;

            DateTime StartTime = Convert.ToDateTime("2000-01-01T00:00:00.0000000Z"), EndTime, CurrentTime = DateTime.UtcNow;

            bool ParkingValid = false;

            var result = ParkingData.FindAll(x => x[0] == ParkingSpaceId && x[3] == RegistrationNumber);

            if (result.Count != 0)
            {
                foreach(var item in result)
                {
                    if (Convert.ToDateTime(item[1]) > StartTime)
                    {
                        StartTime = Convert.ToDateTime(item[1]);

                        PaidMinutes = Convert.ToInt32(item[2]);
                    }
                }

                EndTime = StartTime.AddMinutes(PaidMinutes);

                TimeSpan TimeDifference = EndTime.Subtract(CurrentTime);

                RemainingMinutes = Convert.ToInt32(TimeDifference.TotalMinutes);

                if(RemainingMinutes > 0)
                {
                    ParkingValid = true;
                }
            }

            return new ParkingVerification
            {
                ParkingSpaceId = ParkingSpaceId,
                RegistrationNumber = RegistrationNumber,
                ParkingValid = ParkingValid,
                RemainingMinutes = RemainingMinutes
            };
        }


        [HttpPost("DeleteRecords")]

        public object DeleteRecords(string? RegistrationNumber)
        {
            var CountPre = ParkingData.Count;

            ParkingData.RemoveAll(x => x[3] == RegistrationNumber);

            var CountPost = ParkingData.Count;

            var RecordsDeleted = CountPre - CountPost;

            return new ParkingDeletionConfirmation
            {
                RegistrationNumber = RegistrationNumber,
                RecordsDeleted = RecordsDeleted
            };
        }
    }
}