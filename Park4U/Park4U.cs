namespace Park4U
{
    public class RegisterParking
    {
        public int ParkingSpaceId { get; set; }

        public DateTime ParkingStart { get; set; }

        public int ParkingTimeMinutes { get; set; }

        public string? RegistrationNumber { get; set; }

        public string? EMailAddress { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class CheckParking
    {
        public string? ParkingSpaceId { get; set; }

        public string? RegistrationNumber { get; set; }

        public int RemainingMinutes { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class DeleteRecords
    {
        public string? RegistrationNumber { get; set; }

        public int RecordsDeleted { get; set; }
    }
}