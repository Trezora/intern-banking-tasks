namespace BankingApp.Domain.Customers
{
    public class Customer
    {
        public Guid CustomerId { get; private set; }
        public string FullName { get; private set; } = string.Empty;
        public string EmailAddress { get; private set; } = string.Empty;
        public DateTime DateOfBirth { get; private set; }

        public Customer(string FullName, string EmailAddress, DateTime DateOfBirth)
        {
            this.CustomerId = new Guid();
            this.FullName = FullName;
            this.EmailAddress = EmailAddress;   
            this.DateOfBirth = DateOfBirth;
        }

        /*
         * Returns age according to the customer's brithday was this year or wasn't
        */
        public int GetAge()
        {
            var today = DateTime.Today;   
            var age = today.Year - DateOfBirth.Year;
            
            return HadBirthDayThisYear(today) ? age : age - 1;
        }

        private bool HadBirthDayThisYear(DateTime today)
        {
            return today.Month > DateOfBirth.Month || (today.Month == DateOfBirth.Month && today.Day >= DateOfBirth.Day);
        }
        public string GetCustomerSummary()
        {
            return $"Customer Summary:\n" +
                   $"- ID: {CustomerId}\n" +
                   $"- Name: {FullName}\n" +
                   $"- Email: {EmailAddress}\n" +
                   $"- Date of Birth: {DateOfBirth:yyyy-MM-dd}\n" +
                   $"- Age: {GetAge()}";
        }

    }
}