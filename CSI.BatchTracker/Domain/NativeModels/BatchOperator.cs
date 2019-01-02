namespace CSI.BatchTracker.Domain.NativeModels
{
    public class BatchOperator
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public BatchOperator(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string GetInitials()
        {
            return string.Format("{0}{1}", FirstName.Substring(0, 1), LastName.Substring(0, 1));
        }
    }
}
