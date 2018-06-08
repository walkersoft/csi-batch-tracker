using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class BatchOperator
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public BatchOperator(string firstName, string lastName)
        {
            CheckIfNameIsEmpty(firstName);
            CheckIfNameIsEmpty(lastName);
            FirstName = firstName;
            LastName = lastName;
        }

        void CheckIfNameIsEmpty(string name)
        {
            if (name.Length == 0)
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
