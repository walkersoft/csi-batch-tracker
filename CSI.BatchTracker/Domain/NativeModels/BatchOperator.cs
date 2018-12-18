using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class BatchOperator
    {
        private string firstName;
        private string lastName;

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                CheckIfNameIsEmpty(value);
                firstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                CheckIfNameIsEmpty(value);
                lastName = value;
            }
        }

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

        void CheckIfNameIsEmpty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Dispense Operator name field(s) cannot be empty.");
            }
        }

        public string GetInitials()
        {
            return string.Format("{0}{1}", FirstName.Substring(0, 1), LastName.Substring(0, 1));
        }
    }
}
